/*
<copyright file="BGRepoBinaryV2.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// version 2 reader/writer
    /// Changes:
    /// 1) Objects (fields, metas and addons) now use binary serialization for storing configs (vs json)
    /// 2) Types now use AssemblyQualifiedName (vs FullName)
    /// </summary>
    public partial class BGRepoBinaryV2 : BGRepo.RepoReaderI, BGRepo.RepoWriterI
    {
        private const int MyVersion = 2;

        //==============================================================
        //                                                READ
        //==============================================================
        public BGRepo Read(byte[] dataBytes)
        {
            var binder = new BGBinaryReader(dataBytes);

            var version = binder.ReadInt();
            var repo = new BGRepo { BinaryFormatVersion = version };

            binder.ReadArray(() =>
            {
                var type = binder.ReadString();
                var config = binder.ReadByteArray();
                repo.Addons.Add(BGAddon.Create(type, config));
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

            ActionsListRunner[] loaders = null;
            var currentLoader = 0;
            if (multithreaded)
            {
                var loadersCount = 1;
                loaders = new ActionsListRunner[loadersCount];
                for (var i = 0; i < loadersCount; i++) loaders[i] = new ActionsListRunner();
            }

            binder.ReadArray(() =>
            {
                //meta
                var id = binder.ReadId();
                var name = binder.ReadString();
                var type = binder.ReadString();
                var config = binder.ReadByteArray();
                var system = binder.ReadBool();
                var addon = binder.ReadString();
                var nameUnique = binder.ReadBool();
                var singleton = binder.ReadBool();
                var nameEmpty = binder.ReadBool();
                var meta = BGMetaEntity.Create(repo, type, id, name, config, system, addon, nameUnique, singleton, nameEmpty);

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
                    var fieldId = binder.ReadId();
                    var fieldName = binder.ReadString();
                    var fieldType = binder.ReadString();
                    var fieldConfig = binder.ReadByteArray();
                    var fieldSystem = binder.ReadBool();
                    var fieldAddon = binder.ReadString();
                    var fieldDefaultValue = binder.ReadString();
                    var fieldRequired = binder.ReadBool();

                    var field = BGField.Create(meta, fieldType, fieldId, fieldName, fieldConfig, fieldSystem, fieldAddon, fieldDefaultValue, fieldRequired);
                    var entitiesCount = meta.CountEntities;

                    var fieldValues = binder.ReadByteArray();
                    if (fieldValues.Count > 0)
                    {
                        if (multithreaded)
                        {
                            var threadLoader = loaders[currentLoader];
                            threadLoader.AddAction(() => ReadFieldValues(fieldValues, field, entitiesCount, threadLoader.AddException));
                            currentLoader++;
                            if (currentLoader == loaders.Length) currentLoader = 0;
                        }
                        else
                            ReadFieldValues(fieldValues, field, entitiesCount, e =>
                            {
                            });
                    }
                });
            });

            if (multithreaded)
            {
                var threads = new Thread[loaders.Length];
                for (var i = 0; i < threads.Length; i++)
                {
                    var runner = loaders[i];
                    if (!runner.HasActions) continue;

                    var thread = new Thread(runner.Go);
                    thread.Start();
                    threads[i] = thread;
                }

                for (var i = 0; i < threads.Length; i++)
                {
                    var thread = threads[i];
                    if (thread == null) continue;
                    thread.Join();
                    loaders[i].PrintExceptions();
                }
            }
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

        private class ActionsListRunner
        {
            private readonly List<Action> actions = new List<Action>();
            private List<Exception> errorsList;

            public bool HasActions => actions.Count > 0;

            public void AddException(Exception e)
            {
                errorsList = errorsList ?? new List<Exception>();
                errorsList.Add(e);
            }

            public void AddAction(Action action)
            {
                actions.Add(action);
            }

            public void Go()
            {
                for (var i = 0; i < actions.Count; i++) actions[i]();
            }

            public void PrintExceptions()
            {
                if (errorsList == null) return;
            }
        }

        //==============================================================
        //                                                SAVE
        //==============================================================
        public byte[] Write(BGRepo repo)
        {
            var builder = new BGBinaryWriter();
            builder.AddInt(MyVersion);
            builder.AddArray(() =>
            {
                repo.Addons.ForEachAddon(addon =>
                {
                    builder.AddString(addon.GetType().AssemblyQualifiedName);
                    builder.AddByteArray(addon.ConfigToBytes());
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
                    builder.AddId(meta.Id);
                    builder.AddString(meta.Name);
                    builder.AddString(meta.GetType().AssemblyQualifiedName);
                    builder.AddByteArray(meta.ConfigToBytes());
                    builder.AddBool(meta.System);
                    builder.AddString(meta.Addon);
                    builder.AddBool(meta.UniqueName);
                    builder.AddBool(meta.Singleton);
                    builder.AddBool(meta.EmptyName);

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
                            builder.AddId(field.Id);
                            builder.AddString(field.Name);
                            builder.AddString(field.GetType().AssemblyQualifiedName);
                            builder.AddByteArray(field.ConfigToBytes());
                            builder.AddBool(field.System);
                            builder.AddString(field.Addon);
                            builder.AddString(field.DefaultValue);
                            builder.AddBool(field.Required);

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