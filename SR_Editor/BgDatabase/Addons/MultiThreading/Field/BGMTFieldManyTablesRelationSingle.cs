/*
<copyright file="BGMTFieldManyTablesRelationSingle.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Multi-threaded field for many single relation
    /// </summary>
    public class BGMTFieldManyTablesRelationSingle : BGMTFieldCached<BGMTEntity?, BGRowRef>
    {
        private readonly List<BGId> relatedMetaIds = new List<BGId>();

        public List<BGId> RelatedMetaIds => relatedMetaIds;

        /*
        private List<BGMTMeta> RelatedMetas
        {
            get
            {
                var result = new List<BGMTMeta>();
                foreach (var metaId in relatedMetaIds)
                {
                    var relatedMeta = Meta.Repo[metaId];
                    if(relatedMeta==null)  throw new BGException("Can not find related meta with id '$'", metaId);
                    result.Add(relatedMeta);
                }
                return result;
            }
        }
        */


        internal BGMTFieldManyTablesRelationSingle(BGField field) : base(field)
        {
            var relation = (BGFieldManyRelationsSingle)field;
            relatedMetaIds.AddRange(relation.ToIds);
        }

        internal BGMTFieldManyTablesRelationSingle(BGMTMeta meta, BGMTFieldManyTablesRelationSingle otherField) : base(meta, otherField)
        {
            relatedMetaIds.AddRange(otherField.RelatedMetaIds);
        }

        internal override BGMTField DeepClone(BGMTMeta meta)
        {
            return new BGMTFieldManyTablesRelationSingle(meta, this);
        }

        protected internal override BGMTEntity? this[int entityIndex]
        {
            get
            {
                var tuple = GetStoredValue(entityIndex);
                if (tuple == null) return null;

                var relatedMeta = Meta.Repo[tuple.MetaId];
                return relatedMeta?[tuple.EntityId];
            }
            set
            {
                if (value == null) SetStoredValue(entityIndex, null);
                else
                {
                    var relatedMeta = Meta.Repo[value.Value.Meta.Id];
                    if (relatedMeta == null) return;

                    var found = false;
                    for (var i = 0; i < relatedMetaIds.Count; i++)
                    {
                        if (relatedMetaIds[i] != relatedMeta.Id) continue;
                        found = true;
                        break;
                    }

                    if (!found)
                        throw new BGException("Can not set value: Entity's meta does not match related metas. meta with id=$ and name=$ is not allowed",
                            value.Value.Meta.Id, value.Value.Meta.Name);

                    var relatedId = value.Value.Meta.GetEntityId(value.Value.Index);
                    SetStoredValue(entityIndex, new BGRowRef(value.Value.Meta.Id, relatedId));
                }
            }
        }

        public override void CopyTo(BGField field, BGEntity entity, BGMTEntity fromEntity)
        {
            var relation = (BGFieldManyRelationsSingle)field;
            relation.SetStoredValue(entity.Index, GetStoredValue(fromEntity.Index));
        }
    }
}