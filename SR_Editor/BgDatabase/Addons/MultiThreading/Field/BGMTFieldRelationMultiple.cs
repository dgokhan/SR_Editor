/*
<copyright file="BGMTFieldRelationMultiple.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System.Collections.Generic;
using System.Linq;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Multi-threaded field for multiple relation
    /// </summary>
    public class BGMTFieldRelationMultiple : BGMTFieldCached<List<BGMTEntity>, List<BGId>>
    {
        private readonly BGId relatedMetaId;

        public BGId RelatedMetaId => relatedMetaId;

        private bool allowDuplicates;

        private BGMTMeta RelatedMeta
        {
            get
            {
                var relatedMeta = Meta.Repo[relatedMetaId];
                if (relatedMeta == null) throw new BGException("Can not find related meta with id '$'", relatedMetaId);
                return relatedMeta;
            }
        }

        internal BGMTFieldRelationMultiple(BGField field) : base(field)
        {
            var relation = (BGFieldRelationMultiple)field;
            relatedMetaId = relation.RelatedMeta.Id;
            allowDuplicates = relation.AllowDuplicates;
        }

        internal BGMTFieldRelationMultiple(BGMTMeta meta, BGMTFieldRelationMultiple otherField) : base(meta, otherField)
        {
            relatedMetaId = otherField.relatedMetaId;
            allowDuplicates = otherField.allowDuplicates;
        }

        internal override BGMTField DeepClone(BGMTMeta meta)
        {
            return new BGMTFieldRelationMultiple(meta, this);
        }


        protected internal override List<BGMTEntity> this[int entityIndex]
        {
            get
            {
                var list = GetStoredValue(entityIndex);
                if (list == null || list.Count == 0) return null;

                var relatedMeta = RelatedMeta;
                if (relatedMeta == null) return null;

                List<BGMTEntity> result = null;
                for (var i = 0; i < list.Count; i++)
                {
                    var bgId = list[i];
                    var relatedEntity = relatedMeta[bgId];
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
                    var relatedMeta = RelatedMeta;
                    if (relatedMeta == null) SetStoredValue(entityIndex, null);
                    else
                    {
                        var list = new List<BGId>();
                        for (var i = 0; i < value.Count; i++)
                        {
                            var entity = value[i];

                            if (relatedMeta.Id != entity.Meta.Id)
                                throw new BGException("Can not set value: Entity's meta does not match related meta. expected: $, found $", relatedMeta.Name, entity.Meta.Name);

                            list.Add(entity.Id);
                        }

                        if (!allowDuplicates) list = list.Distinct().ToList();
                        SetStoredValue(entityIndex, list);
                    }
                }
            }
        }

        public override void CopyTo(BGField field, BGEntity entity, BGMTEntity fromEntity)
        {
            var relation = (BGFieldRelationMultiple)field;
            var storedValue = GetStoredValue(fromEntity.Index);
            List<BGId> result;
            if (storedValue == null || storedValue.Count == 0) result = null;
            else result = new List<BGId>(storedValue);

            relation.SetStoredValue(entity.Index, result);
        }
    }
}