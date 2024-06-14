/*
<copyright file="BGMTFieldManyTablesRelationMultiple.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


using System.Collections.Generic;
using System.Linq;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Multi-threaded field for many multiple relation
    /// </summary>
    public class BGMTFieldManyTablesRelationMultiple : BGMTFieldCached<List<BGMTEntity>, List<BGRowRef>>
    {
        private readonly List<BGId> relatedMetaIds = new List<BGId>();

        public List<BGId> RelatedMetaIds => relatedMetaIds;

        private bool allowDuplicates;


        internal BGMTFieldManyTablesRelationMultiple(BGField field) : base(field)
        {
            var relation = (BGFieldManyRelationsMultiple)field;
            relatedMetaIds.AddRange(relation.ToIds);
            allowDuplicates = relation.AllowDuplicates;
        }

        internal BGMTFieldManyTablesRelationMultiple(BGMTMeta meta, BGMTFieldManyTablesRelationMultiple otherField) : base(meta, otherField)
        {
            relatedMetaIds.AddRange(otherField.RelatedMetaIds);
            allowDuplicates = otherField.allowDuplicates;
        }

        internal override BGMTField DeepClone(BGMTMeta meta)
        {
            return new BGMTFieldManyTablesRelationMultiple(meta, this);
        }


        protected internal override List<BGMTEntity> this[int entityIndex]
        {
            get
            {
                var list = GetStoredValue(entityIndex);
                if (list == null || list.Count == 0) return null;

                List<BGMTEntity> result = null;
                for (var i = 0; i < list.Count; i++)
                {
                    var tuple = list[i];
                    var relatedMeta = Meta.Repo[tuple.MetaId];
                    var relatedEntity = relatedMeta?[tuple.EntityId];
                    if (relatedEntity == null) continue;

                    result = result ?? new List<BGMTEntity>();
                    result.Add(relatedEntity.Value);
                }

                return result;
            }
            set
            {
                if (value == null || value.Count == 0) SetStoredValue(entityIndex, null);
                else
                {
                    var list = new List<BGRowRef>();
                    for (var i = 0; i < value.Count; i++)
                    {
                        var entity = value[i];

                        var found = false;
                        for (var j = 0; j < relatedMetaIds.Count; j++)
                        {
                            if (relatedMetaIds[j] != entity.Meta.Id) continue;
                            found = true;
                            break;
                        }

                        if (!found)
                            throw new BGException("Can not set value: Entity's meta does not match related metas. meta with id=$ and name=$ is not allowed",
                                entity.Meta.Id, entity.Meta.Name);

                        list.Add(new BGRowRef(entity.Meta.Id, entity.Id));
                    }

                    if (!allowDuplicates) list = list.Distinct().ToList();
                    SetStoredValue(entityIndex, list);
                }
            }
        }

        public override void CopyTo(BGField field, BGEntity entity, BGMTEntity fromEntity)
        {
            var relation = (BGFieldManyRelationsMultiple)field;
            var storedValue = GetStoredValue(fromEntity.Index);
            List<BGRowRef> result;
            if (storedValue == null || storedValue.Count == 0) result = null;
            else result = new List<BGRowRef>(storedValue);

            relation.SetStoredValue(entity.Index, result);
        }
    }
}