/*
<copyright file="BGRepoBinaryV5.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.IO;
using System.IO.Compression;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// version 5 reader/writer
    /// Changes:
    /// 1) Keys are added
    /// 2) encryption
    /// </summary>
    public class BGRepoBinaryV5 : BGRepo.RepoReaderI, BGRepo.RepoWriterI
    {
        private const int MyVersion = 5;
        private static readonly BGId UniqueId = new BGId(5208396773359933591, 14401663668042484142);
        private static readonly BGId EncryptionId = new BGId(4770294628005998460, 9299804957405062829);

        //==============================================================
        //                                                READ
        //==============================================================
        public BGRepo Read(byte[] dataBytes)
        {
            var binder = new BGBinaryReader(dataBytes);

            var version = binder.ReadInt();
            var repo = new BGRepo { BinaryFormatVersion = version };

            if (dataBytes.Length < 21) return repo;
            if (binder.ReadId() != UniqueId) throw new Exception("Provided binary array is not a valid BGDatabase content!");

            //!!!!!!Version5 change
            //encryption
            if (binder.Cursor + 16 < binder.Length)
            {
                if (binder.ReadId() == EncryptionId)
                {
                    var encryptorType = binder.ReadString();
                    var encryptorConfig = binder.ReadString();
                    if (string.IsNullOrEmpty(encryptorType)) throw new BGException("BGDatabase content is encrypted, but encryptor is not set!");
                    var encryptorClass = BGUtil.GetType(encryptorType);
                    if (encryptorClass == null) throw new BGException("BGDatabase content is encrypted, can not load encryptor class $!", encryptorType);
                    var encryptor = Activator.CreateInstance(encryptorClass) as BGEncryptor;
                    if (encryptor == null) throw new BGException("BGDatabase content is encrypted, can not create encryptor $- it does not implement BGEncryptor interface!", encryptorType);

                    var decryptedData = encryptor.Decrypt(new ArraySegment<byte>(dataBytes, binder.Cursor, binder.Length - binder.Cursor), encryptorConfig);
                    binder = new BGBinaryReader(decryptedData);
                }
                else binder.ShiftCursor(-16);
            }


            binder.ReadArray(() =>
            {
                repo.Addons.Add(BGAddon.FromBinary(binder));
            });

            var settingsAddon = repo.Addons.Get<BGAddonSettings>();
            var multithreaded = false;
            if (settingsAddon != null)
            {
                multithreaded = settingsAddon.MultiThreadedLoading;

                if (settingsAddon.ZippedContent)
                    //uncompress
                    using (var deflateStream = new DeflateStream(new MemoryStream(dataBytes, binder.Cursor, dataBytes.Length - binder.Cursor), CompressionMode.Decompress))
                    {
                        using (var outputStream = new MemoryStream())
                        {
                            CopyTo(deflateStream, outputStream);
                            binder = new BGBinaryReader(outputStream.ToArray());
                        }
                    }
            }

            ReadMetas(binder, repo, multithreaded);

            return repo;
        }

        private static void ReadMetas(BGBinaryReader binder, BGRepo repo, bool multithreaded)
        {

            BGMultiThreadedLoader multiThreadedLoader = null;
            if (multithreaded) multiThreadedLoader = new BGMultiThreadedLoader();

            binder.ReadArray(() =>
            {
                //meta
                var meta = BGMetaEntity.FromBinary(binder, repo);

                //entities
                var entityIds = binder.ReadByteArray();
                var entityCount = entityIds.Count == 0 ? 0 : entityIds.Count / 16;
                if (entityCount > 0)
                {
                    meta.EntitiesCapacity = entityCount;
                    //---objects
                    var array = entityIds.Array;
                    var offset = entityIds.Offset;
                    var idMax = offset + entityCount * 16;
                    for (var idCursor = offset; idCursor < idMax; idCursor += 16) meta.NewEntity(new BGId(array, idCursor));
                }

                //fields
                binder.ReadArray(() =>
                {
                    var field = BGField.FromBinary(binder, meta);

                    var entitiesCount = meta.CountEntities;

                    var fieldValues = binder.ReadByteArray();
                    if (fieldValues.Count > 0)
                    {
                        if (multithreaded)
                            multiThreadedLoader.AddAction(() => ReadFieldValues(fieldValues, field, entitiesCount, multiThreadedLoader.AddException), !field.SupportMultiThreadedLoading);
                        else
                            ReadFieldValues(fieldValues, field, entitiesCount, e =>
                            {

                            });
                    }
                });
                //!!!!!!!  version 5 change
                //keys
                binder.ReadArray(() =>
                {
                    var key = BGKey.FromBinary(binder, meta);
                });
            });

            multiThreadedLoader?.Load();
        }

        private static void ReadFieldValues(ArraySegment<byte> fieldValues, BGField field, int entitiesCount, Action<Exception> onError)
        {
            var meta = field.Meta;
            var array = fieldValues.Array;
            var offset = fieldValues.Offset;
            var constantSize = field.ConstantSize;
            if (constantSize > 0)
                for (var i = 0; i < entitiesCount; i++)
                {
                    var entity = meta[i];
                    field.FromBytes(entity.Index, new ArraySegment<byte>(array, offset + i * constantSize, constantSize));
                }
            else
            {
                var count = BGFieldInt.ValueFromBytes(new ArraySegment<byte>(array, offset, BGFieldInt.SizeOfTheValue));
                if (count == 0) return;
                var indexStart = offset + 4;
                var valueStart = indexStart + count * 8;
                var cursor = 0;

                if (field is BGFieldEntityName fieldEntityName)
                {
                    // This is microoptimization
                    // here we can get minor speed improvements for BGFieldEntityName field (code is copied from below)
                    // by bypassing standard slow field[entity.Id] = value; method
                    for (var i = 0; i < count; i++)
                    {
                        var startIndex = indexStart + i * 8;
                        var entityIndex = BGFieldInt.ValueFromBytes(new ArraySegment<byte>(array, startIndex, BGFieldInt.SizeOfTheValue));
                        var entity = meta[entityIndex];
                        var valueEnd = BGFieldInt.ValueFromBytes(new ArraySegment<byte>(array, startIndex + 4, BGFieldInt.SizeOfTheValue));

                        try
                        {
                            fieldEntityName.SetEntityValue(entity.Index, BGFieldStringA.ValueFromBytes(new ArraySegment<byte>(array, valueStart + cursor, valueEnd - cursor)));
                        }
                        catch (Exception e)
                        {
                            onError(e);
                        }

                        cursor = valueEnd;
                    }
                }
                else
                    for (var i = 0; i < count; i++)
                    {
                        var startIndex = indexStart + i * 8;
                        var entityIndex = BGFieldInt.ValueFromBytes(new ArraySegment<byte>(array, startIndex, BGFieldInt.SizeOfTheValue));
                        var entity = meta[entityIndex];
                        var valueEnd = BGFieldInt.ValueFromBytes(new ArraySegment<byte>(array, startIndex + 4, BGFieldInt.SizeOfTheValue));

                        try
                        {
                            field.FromBytes(entity.Index, new ArraySegment<byte>(array, valueStart + cursor, valueEnd - cursor));
                        }
                        catch (Exception e)
                        {
                            onError(e);
                        }

                        cursor = valueEnd;
                    }
            }
        }

        public static void CopyTo(Stream input, Stream output)
        {
            var buffer = new byte[64 * 1024];
            int bytesRead;
            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0) output.Write(buffer, 0, bytesRead);
        }


        //==============================================================
        //                                                SAVE
        //==============================================================
        public byte[] Write(BGRepo repo)
        {
            var builder = new BGBinaryWriter();
            builder.AddInt(MyVersion);

            builder.AddId(UniqueId);

            var uniqueIdEndAt = builder.Count;
            var settingsAddon = repo.Addons.Get<BGAddonSettings>();

            builder.AddArray(() =>
            {
                repo.Addons.ForEachAddon(addon =>
                {
                    BGAddon.ToBinary(builder, addon);
                });
            }, repo.Addons.Count);

            var zipped = settingsAddon != null && settingsAddon.ZippedContent;
            var addonsEndAt = builder.Count;

            builder.AddArray(() =>
            {
                repo.ForEachMeta(meta =>
                {
                    //meta
                    BGMetaEntity.ToBinary(builder, meta);

                    var entityCount = meta.CountEntities;

                    var entityIds = new byte[entityCount * 16];

                    //entity
                    var i = 0;
                    meta.ForEachEntity(entity =>
                    {
                        var entityId = entity.Id;
                        entityId.ToByteArray(entityIds, i << 4);
                        i++;
                    });
                    builder.AddByteArray(entityIds);

                    //fields
                    builder.AddArray(() =>
                    {
                        meta.ForEachField(field =>
                        {
                            BGField.ToBinary(builder, field);

                            var constantSize = field.ConstantSize;
                            if (constantSize > 0)
                            {
                                var fieldValues = new byte[entityCount * constantSize];
                                var cursor = 0;
                                if (constantSize < 16)
                                    meta.ForEachEntity(entity =>
                                    {
                                        var bytes = field.ToBytes(entity.Index);
                                        for (var j = 0; j < constantSize; j++) fieldValues[cursor++] = bytes[j];
                                    });
                                else
                                    meta.ForEachEntity(entity =>
                                    {
                                        Buffer.BlockCopy(field.ToBytes(entity.Index), 0, fieldValues, cursor, constantSize);
                                        cursor += constantSize;
                                    });

                                builder.AddByteArray(fieldValues);
                            }
                            else
                            {
                                var fieldBuilder = new BGRepoBinaryV1.FieldBuilder(builder);
                                if (!field.EmptyContent)
                                    field.ForEachValue(index =>
                                    {
                                        byte[] fieldValue;
                                        try
                                        {
                                            fieldValue = field.ToBytes(index);
                                        }
                                        catch (Exception)
                                        {
                                            // ignored
                                            fieldValue = null;
                                        }

                                        if (fieldValue == null || fieldValue.Length == 0) return;

                                        fieldBuilder.Add(index, fieldValue);
                                    });

                                fieldBuilder.Finish();
                            }
                        });
                    }, meta.CountFields);

                    //!!!!!!! version 5 change
                    //fields
                    builder.AddArray(() =>
                    {
                        meta.ForEachKey(key =>
                        {
                            BGKey.ToBinary(builder, key);
                        });
                    }, meta.CountKeys);
                });
            }, repo.CountMeta);

            var result = builder.ToArray();

            if (zipped)
            {
                var output = new MemoryStream();
                using (var zipStream = new DeflateStream(output, CompressionMode.Compress))
                {
                    zipStream.Write(result, addonsEndAt, result.Length - addonsEndAt);
                }

                var zippedResult = output.ToArray();
                var newResult = new byte[addonsEndAt + zippedResult.Length];
                Buffer.BlockCopy(result, 0, newResult, 0, addonsEndAt);
                Buffer.BlockCopy(zippedResult, 0, newResult, addonsEndAt, zippedResult.Length);
                result = newResult;
            }

            //!!!!!!!!!!!!!!!!!!!!!!!  Version 5 change
            //encryption
            if (settingsAddon != null && settingsAddon.Encryptor != null)
            {
                var headerArray = new byte[uniqueIdEndAt];
                Buffer.BlockCopy(result, 0, headerArray, 0, uniqueIdEndAt);

                //encryption
                var encryptor = settingsAddon.Encryptor;
                var encryptedResult = encryptor.Encrypt(new ArraySegment<byte>(result, headerArray.Length, result.Length - headerArray.Length), settingsAddon.EncryptorConfig);

                //encryptor class & config
                var encryptionWriter = new BGBinaryWriter();
                encryptionWriter.AddId(EncryptionId);
                encryptionWriter.AddString(settingsAddon.EncryptorType);
                encryptionWriter.AddString(settingsAddon.EncryptorConfig);
                var encryptionContent = encryptionWriter.ToArray();

                //copy arrays
                var newResult = new byte[headerArray.Length + encryptionContent.Length + encryptedResult.Count];
                Buffer.BlockCopy(headerArray, 0, newResult, 0, headerArray.Length);
                Buffer.BlockCopy(encryptionContent, 0, newResult, uniqueIdEndAt, encryptionContent.Length);
                Buffer.BlockCopy(encryptedResult.Array, encryptedResult.Offset, newResult, uniqueIdEndAt + encryptionContent.Length, encryptedResult.Count);
                result = newResult;
            }

            return result;
        }
    }
}