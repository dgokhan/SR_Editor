/*
<copyright file="BGRepoDeltaUpdated.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    //wrapper class for deleted members
    internal class BGRepoDeltaUpdated
    {
        private readonly BGRepo updated = new BGRepo();
        private readonly Dictionary<BGId, MetaUpdated> metaId2Updated = new Dictionary<BGId, MetaUpdated>();

        //================================================================================================
        //                                              Methods
        //================================================================================================
        /// <summary>
        /// Find any matching members and store them in internal state
        /// </summary>
        public void Match(BGRepo repo, BGRepo targetRepo)
        {
            updated.Clear();
            metaId2Updated.Clear();

            BGRepoDeltaUtils.ForEachMatchingMeta(repo, targetRepo, (meta, targetMeta) =>
            {
                var fields = new List<BGField>();
                var targetFields = new List<BGField>();
                BGRepoDeltaUtils.ForEachMatchingField(meta, targetMeta, (field, targetField) =>
                {
                    fields.Add(field);
                    targetFields.Add(targetField);
                });
                if (fields.Count == 0) return;

                BGRepoDeltaUtils.ForEachMatchingEntity(meta, targetMeta, (entity, targetEntity) =>
                {
                    for (var i = 0; i < fields.Count; i++)
                    {
                        var field = fields[i];
                        var targetField = targetFields[i];

                        if (field.AreStoredValuesEqual(targetField, entity.Index, targetEntity.Index)) continue;


                        //need to add info
                        var myField = EnsureField(field);
                        var myEntity = EnsureEntity(myField.Meta, targetEntity.Id);
                        myField.CopyValue(targetField, targetEntity.Id, targetEntity.Index, myEntity.Id);

                        var metaUpdate = MetaUpdated.Ensure(meta.Id, metaId2Updated);
                        metaUpdate.Add(myField.Id, myEntity.Id);
                    }
                });
            });
        }

        private BGEntity EnsureEntity(BGMetaEntity meta, BGId entityId)
        {
            var entity = meta.GetEntity(entityId);
            if (entity != null) return entity;
            entity = meta.NewEntity(entityId);
            return entity;
        }

        private BGField EnsureField(BGField field)
        {
            var myMeta = updated.GetMeta(field.MetaId);
            if (myMeta == null) myMeta = BGRepoDeltaUtils.CreateMeta(updated, field.Meta);

            var myField = myMeta.GetField(field.Id, false);
            if (myField == null) myField = BGRepoDeltaUtils.CreateField(myMeta, field);
            return myField;
        }

        /// <summary>
        /// Apply the changes to the repo
        /// </summary>
        public void ApplyTo(BGRepo repo, BGModdingRepoProtection repoProtection)
        {
            foreach (var pair in metaId2Updated)
            {
                var metaId = pair.Key;
                var metaUpdated = pair.Value;

                var fromMeta = updated[metaId];
                if (fromMeta == null) continue;
                var toMeta = repo[metaId];
                if (toMeta == null) continue;

                metaUpdated.ForEach((fieldId, entityIds) =>
                {
                    if (entityIds.Count == 0) return;

                    var fromField = fromMeta.GetField(fieldId, false);
                    if (fromField == null) return;
                    var toField = toMeta.GetField(fieldId, false);
                    if (toField == null) return;

                    foreach (var entityId in entityIds)
                    {
                        var fromEntity = fromMeta.GetEntity(entityId);
                        if (fromEntity == null) continue;
                        var toEntity = toMeta.GetEntity(entityId);
                        if (toEntity == null) continue;
                        if (repoProtection != null && repoProtection.IsEditDisabled(metaId, fieldId, toEntity.Id)) continue;
                        toField.CopyValue(fromField, fromEntity.Id, fromEntity.Index, toEntity.Id);
                    }
                });
            }
        }


        //================================================================================================
        //                                              Binary
        //================================================================================================
        private const int LastVersion = 1;

        public void ToBinary(BGBinaryWriter builder)
        {
            builder.AddInt(LastVersion);
            builder.AddByteArray(updated.Save());
            builder.AddArray(() =>
            {
                foreach (var metaUpdated in metaId2Updated)
                {
                    builder.AddId(metaUpdated.Key);
                    metaUpdated.Value.ToBinary(builder);
                }
            }, metaId2Updated.Count);
        }

        public void FromBinary(BGBinaryReader reader)
        {
            updated.Clear();
            metaId2Updated.Clear();

            var version = reader.ReadInt();
            switch (version)
            {
                case 1:
                {
                    updated.Load(BGRepoDeltaUtils.ToArray(reader.ReadByteArray()));
                    reader.ReadArray(() =>
                    {
                        var metaId = reader.ReadId();
                        var metaUpdated = new MetaUpdated();
                        metaId2Updated[metaId] = metaUpdated;
                        metaUpdated.FromBinary(reader);
                    });
                    break;
                }
                default:
                {
                    throw new BGException("Can not read repo delta deleted from binary array: unsupported version $", version);
                }
            }
        }

        //================================================================================================
        //                                              Nested
        //================================================================================================
        private class MetaUpdated
        {
            private readonly Dictionary<BGId, List<BGId>> fieldId2EntityIds = new Dictionary<BGId, List<BGId>>();

            public void ForEach(Action<BGId, List<BGId>> action)
            {
                foreach (var pair in fieldId2EntityIds) action(pair.Key, pair.Value);
            }

            public void Add(BGId fieldId, BGId entityId)
            {
                if (!fieldId2EntityIds.TryGetValue(fieldId, out var list))
                {
                    list = new List<BGId>();
                    fieldId2EntityIds[fieldId] = list;
                }

                list.Add(entityId);
            }

            public static MetaUpdated Ensure(BGId metaId, Dictionary<BGId, MetaUpdated> id2Updated)
            {
                if (id2Updated.TryGetValue(metaId, out var metaUpdate)) return metaUpdate;

                metaUpdate = new MetaUpdated();
                id2Updated[metaId] = metaUpdate;
                return metaUpdate;
            }

            internal void FromBinary(BGBinaryReader reader)
            {
                fieldId2EntityIds.Clear();

                reader.ReadArray(() =>
                {
                    var fieldId = reader.ReadId();
                    var entityIdList = new List<BGId>();
                    fieldId2EntityIds[fieldId] = entityIdList;
                    reader.ReadArray(() =>
                    {
                        entityIdList.Add(reader.ReadId());
                    });
                });
            }

            internal void ToBinary(BGBinaryWriter builder)
            {
                builder.AddArray(() =>
                {
                    foreach (var fieldId2EntityId in fieldId2EntityIds)
                    {
                        builder.AddId(fieldId2EntityId.Key);
                        builder.AddArray(() =>
                        {
                            foreach (var entityId in fieldId2EntityId.Value) builder.AddId(entityId);
                        }, fieldId2EntityId.Value.Count);
                    }
                }, fieldId2EntityIds.Count);
            }
        }
    }
}