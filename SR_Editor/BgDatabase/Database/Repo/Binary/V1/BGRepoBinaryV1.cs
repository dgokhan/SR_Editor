/*
<copyright file="BGRepoBinaryV1.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// version 1 reader/writer
    /// </summary>
    public partial class BGRepoBinaryV1 : BGRepo.RepoReaderI, BGRepo.RepoWriterI
    {
        private const int MyVersion = 1;

        //================================== READ
        public BGRepo Read(byte[] dataBytes)
        {
            var binder = new BGBinaryReader(dataBytes);

            var version = binder.ReadInt();
            var repo = new BGRepo { BinaryFormatVersion = version };

            binder.ReadArray(() =>
            {
                var type = binder.ReadString();
                var config = binder.ReadString();
                repo.Addons.Add(BGAddon.Create(type, config));
            });

            ReadMetas(binder, repo);

            return repo;
        }

        private static void ReadMetas(BGBinaryReader binder, BGRepo repo)
        {
            binder.ReadArray(() =>
            {
                //meta
                var id = binder.ReadId();
                var name = binder.ReadString();
                var type = binder.ReadString();
                var config = binder.ReadString();
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
                    var fieldConfig = binder.ReadString();
                    var fieldSystem = binder.ReadBool();
                    var fieldAddon = binder.ReadString();
                    var fieldDefaultValue = binder.ReadString();
                    var fieldRequired = binder.ReadBool();

                    var field = BGField.Create(meta, fieldType, fieldId, fieldName, fieldConfig, fieldSystem, fieldAddon, fieldDefaultValue, fieldRequired);
                    var entitiesCount = meta.CountEntities;

                    var fieldValues = binder.ReadByteArray();
                    if (fieldValues.Count > 0)
                    {
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
                                    }

                                    cursor = valueEnd;
                                }
                        }
                    }
                });
            });
        }

        //================================== SAVE
        public byte[] Write(BGRepo repo)
        {
            var builder = new BGBinaryWriter();
            builder.AddInt(MyVersion);
            builder.AddArray(() =>
            {
                repo.Addons.ForEachAddon(addon =>
                {
                    builder.AddString(addon.GetType().FullName);
                    builder.AddString(addon.ConfigToString());
                });
            }, repo.Addons.Count);

            builder.AddArray(() =>
            {
                repo.ForEachMeta(meta =>
                {
                    //meta
                    builder.AddId(meta.Id);
                    builder.AddString(meta.Name);
                    builder.AddString(meta.GetType().FullName);
                    builder.AddString(meta.ConfigToString());
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
                            builder.AddString(field.GetType().FullName);
                            builder.AddString(field.ConfigToString());
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
                                var fieldBuilder = new FieldBuilder(builder);
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

            return builder.ToArray();
        }

        internal class FieldBuilder
        {
            //array with serialized fields values
            private byte[] fieldValues = new byte[64];
            //array with index+end values. index is a entity index and end is a pointer to value end
            private byte[] indexesValues = new byte[64];

            private readonly BGBinaryWriter builder;

            private int count;
            private int cursor;

            //all field values byte count
            private int fieldSize;
            //all index+end byte count
            private int indexesSize;

            public FieldBuilder(BGBinaryWriter builder)
            {
                this.builder = builder;
            }

            public void Add(int entityIndex, byte[] fieldValue)
            {
                //------ensure capacity
                //indexes
                var startIndexesSize = indexesSize;
                // 8 = sizeof(entityIndex) + sizeof(end) 
                indexesSize += 8;
                if (indexesValues.Length < indexesSize)
                {
                    //array resize if needed
                    var newSize = indexesValues.Length * 2;
                    if (newSize < indexesSize) newSize = indexesSize;
                    var newArray = new byte[newSize];
                    Buffer.BlockCopy(indexesValues, 0, newArray, 0, startIndexesSize);
                    indexesValues = newArray;
                }

                //fields
                var fieldValueLength = fieldValue.Length;
                var startFieldsSize = fieldSize;
                fieldSize += fieldValueLength;
                if (fieldValues.Length < fieldSize)
                {
                    //array resize if needed
                    var newSize = fieldValues.Length * 2;
                    if (newSize < fieldSize) newSize = fieldSize;
                    var newArray = new byte[newSize];
                    Buffer.BlockCopy(fieldValues, 0, newArray, 0, startFieldsSize);
                    fieldValues = newArray;
                }


                //------indexes
                count++;
                var entityIndexArray = BGFieldInt.ValueToBytes(entityIndex);
                indexesValues[startIndexesSize] = entityIndexArray[0];
                indexesValues[startIndexesSize + 1] = entityIndexArray[1];
                indexesValues[startIndexesSize + 2] = entityIndexArray[2];
                indexesValues[startIndexesSize + 3] = entityIndexArray[3];

                var ends = cursor + fieldValueLength;
                var endsArray = BGFieldInt.ValueToBytes(ends);
                indexesValues[startIndexesSize + 4] = endsArray[0];
                indexesValues[startIndexesSize + 5] = endsArray[1];
                indexesValues[startIndexesSize + 6] = endsArray[2];
                indexesValues[startIndexesSize + 7] = endsArray[3];
                cursor = ends;

                //------field values
                if (fieldValueLength < 16)
                    for (var i = 0; i < fieldValueLength; i++)
                        fieldValues[startFieldsSize + i] = fieldValue[i];
                else Buffer.BlockCopy(fieldValue, 0, fieldValues, startFieldsSize, fieldValueLength);
            }

            public void Finish()
            {
                //--------------- counts
                //byte array overall bytes count
                builder.AddInt(4 + indexesSize + fieldSize);

                //fields values count
                builder.AddInt(count);

                if (indexesSize > 0)
                {
                    var targetArray = new byte[indexesSize];
                    Buffer.BlockCopy(indexesValues, 0, targetArray, 0, indexesSize);
                    builder.AddBytesRaw(targetArray);
                }

                if (fieldSize > 0)
                {
                    var targetArray = new byte[fieldSize];
                    Buffer.BlockCopy(fieldValues, 0, targetArray, 0, fieldSize);
                    builder.AddBytesRaw(targetArray);
                }
            }
        }
    }
}