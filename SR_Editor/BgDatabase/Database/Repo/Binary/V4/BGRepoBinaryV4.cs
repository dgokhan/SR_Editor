/*
<copyright file="BGRepoBinaryV4.cs" company="BansheeGz">
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
    /// version 4 reader/writer
    /// Changes:
    /// 1) Fields & metas binary reading/writing now moved to corresponding classes and have version
    /// 2) Unique Id is added to recognize binary array as being valid database content
    /// </summary>
    public class BGRepoBinaryV4 : BGRepo.RepoReaderI, BGRepo.RepoWriterI
    {
        private const int MyVersion = 4;
        private static readonly BGId UniqueId = new BGId(5208396773359933591, 14401663668042484142);

        //==============================================================
        //                                                READ
        //==============================================================
        public BGRepo Read(byte[] dataBytes)
        {
            var binder = new BGBinaryReader(dataBytes);

            var version = binder.ReadInt();
            var repo = new BGRepo { BinaryFormatVersion = version };

            //!!!!!!!!!!!!!! v4 changes
            if (dataBytes.Length < 21) return repo;
            if (binder.ReadId() != UniqueId) throw new Exception("Provided binary array is not a valid BGDatabase content!");

            binder.ReadArray(() =>
            {
                //!!!!!!!!!!!!!! v4 changes 
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

            ReadMetas(binder, repo,  multithreaded);

            return repo;
        }

        private static void ReadMetas(BGBinaryReader binder, BGRepo repo, bool multithreaded)
        {

            BGMultiThreadedLoader multiThreadedLoader = null;
            if (multithreaded) multiThreadedLoader = new BGMultiThreadedLoader();

            binder.ReadArray(() =>
            {
                //meta
                //!!!!!!!!!!!!!! v4 changes
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
                    //!!!!!!!!!!!!!! v4 changes
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

            //!!!!!!!!!!!!!! v4 changes
            builder.AddId(UniqueId);

            builder.AddArray(() =>
            {
                repo.Addons.ForEachAddon(addon =>
                {
                    //!!!!!!!!!!!!!! v4 changes
                    BGAddon.ToBinary(builder, addon);
                });
            }, repo.Addons.Count);

            var settingsAddon = repo.Addons.Get<BGAddonSettings>();
            var zipped = settingsAddon != null && settingsAddon.ZippedContent;
            var addonsEndAt = builder.Count;

            builder.AddArray(() =>
            {
                repo.ForEachMeta(meta =>
                {
                    //meta
                    //!!!!!!!!!!!!!! v4 changes
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
                            //!!!!!!!!!!!!!! v4 changes
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

            return result;
        }
    }
}