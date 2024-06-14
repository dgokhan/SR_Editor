/*
<copyright file="BGMTFieldNested.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Multi-threaded field for nested 
    /// </summary>
    public class BGMTFieldNested : BGMTField<List<BGMTEntity>>
    {
        private readonly BGId relatedMetaId;
        private readonly BGId relationId;

        public BGId RelatedMetaId => relatedMetaId;

        public BGId RelationId => relationId;

        private BGMTMeta RelatedMeta
        {
            get
            {
                var relatedMeta = Meta.Repo[relatedMetaId];
                if (relatedMeta == null) throw new BGException("Can not find related meta with id '$'", relatedMetaId);
                return relatedMeta;
            }
        }

        internal BGMTFieldNested(BGField field) : base(field.Id, field.Name)
        {
            var nestedField = (BGFieldNested)field;
            relatedMetaId = nestedField.NestedMeta.Id;
            relationId = nestedField.OwnerRelationId;
        }

        internal BGMTFieldNested(BGMTMeta meta, BGMTField<List<BGMTEntity>> otherField) : base(meta, otherField)
        {
        }

        internal override BGMTField DeepClone(BGMTMeta meta)
        {
            return new BGMTFieldNested(meta, this);
        }

        internal override void ResizeTo(int newCount)
        {
        }

        internal override void RemoveRange(int @from, int count)
        {
        }

        public override void CopyTo(BGField field, BGEntity entity, BGMTEntity fromEntity)
        {
        }

        protected internal override List<BGMTEntity> this[int entityIndex]
        {
            get
            {
                List<BGMTEntity> result = null;

                var relatedMeta = RelatedMeta;
                var relationField = relatedMeta.GetField(relationId) as BGMTFieldRelationSingle;
                if (relationField == null) throw new BGException("Can not find relation field with id '$' at meta '$'", relationId, relatedMeta.Name);

                var entityId = Meta.GetEntityId(entityIndex);
                if (entityId.IsEmpty) return result;

                var count = relatedMeta.CountEntities;
                for (var i = 0; i < count; i++)
                {
                    var relatedId = relationField.GetStoredValue(i);
                    if (relatedId != entityId) continue;
                    result = result ?? new List<BGMTEntity>();
                    result.Add(new BGMTEntity(relatedMeta, i));
                }

                return result;
            }
            set { }
        }
    }
}