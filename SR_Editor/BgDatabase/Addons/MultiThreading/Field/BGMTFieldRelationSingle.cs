/*
<copyright file="BGMTFieldRelationSingle.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Multi-threaded field for single relation
    /// </summary>
    public class BGMTFieldRelationSingle : BGMTFieldCached<BGMTEntity?, BGId>
    {
        private readonly BGId relatedMetaId;

        public BGId RelatedMetaId => relatedMetaId;

        private BGMTMeta RelatedMeta
        {
            get
            {
                var relatedMeta = Meta.Repo[relatedMetaId];
                if (relatedMeta == null) throw new BGException("Can not find related meta with id '$'", relatedMetaId);
                return relatedMeta;
            }
        }


        internal BGMTFieldRelationSingle(BGField field) : base(field)
        {
            var relation = (BGFieldRelationSingle)field;
            relatedMetaId = relation.RelatedMeta.Id;
        }

        internal BGMTFieldRelationSingle(BGMTMeta meta, BGMTFieldRelationSingle otherField) : base(meta, otherField)
        {
            relatedMetaId = otherField.relatedMetaId;
        }

        internal override BGMTField DeepClone(BGMTMeta meta)
        {
            return new BGMTFieldRelationSingle(meta, this);
        }

        protected internal override BGMTEntity? this[int entityIndex]
        {
            get
            {
                var entityId = GetStoredValue(entityIndex);
                if (entityId.IsEmpty) return null;

                var relatedMeta = RelatedMeta;
                return relatedMeta?[entityId];
            }
            set
            {
                if (value == null) SetStoredValue(entityIndex, BGId.Empty);
                else
                {
                    var relatedMeta = RelatedMeta;
                    if (relatedMeta == null) return;

                    if (relatedMeta.Id != value.Value.Meta.Id)
                        throw new BGException("Can not set value: Entity's meta does not match related meta. expected: $, found $", relatedMeta.Name, value.Value.Meta.Name);

                    var relatedId = value.Value.Meta.GetEntityId(value.Value.Index);
                    SetStoredValue(entityIndex, relatedId);
                }
            }
        }

        public override void CopyTo(BGField field, BGEntity entity, BGMTEntity fromEntity)
        {
            var relation = (BGFieldRelationSingle)field;
            relation.SetStoredValue(entity.Index, GetStoredValue(fromEntity.Index));
        }
    }
}