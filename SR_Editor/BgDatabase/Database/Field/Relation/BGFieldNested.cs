/*
<copyright file="BGFieldNested.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// nested field. Nested is very special field, cause it creates a new meta (BGMetaNested)
    /// </summary>
    [FieldDescriptor(Name = "nested", Folder = "Relation", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerNested")]
    public partial class BGFieldNested : BGField<List<BGEntity>>, BGRelationI, BGFieldWithCustomConfigI
    {
        public const ushort CodeType = 44;
        public override ushort TypeCode => CodeType;

        protected  BGId ownerRelationId;
        protected BGId nestedMetaId;

        /// <summary>
        /// relation field id, which is used to link to owner entity
        /// </summary>
        public BGId OwnerRelationId => ownerRelationId;

        /// <summary>
        /// relation field, which is used to link to owner entity
        /// </summary>
        public BGFieldRelationSingle OwnerRelation => NestedMeta.GetFieldAs<BGFieldRelationSingle>(ownerRelationId);

        /// <summary>
        /// related nested meta id
        /// </summary>
        public BGId NestedMetaId => nestedMetaId;

        /// <inheritdoc />
        public BGId ToId => nestedMetaId;

        /// <summary>
        /// related nested meta
        /// </summary>
        public BGMetaNested NestedMeta => Meta.Repo.GetMeta<BGMetaNested>(nestedMetaId);

        /// <inheritdoc />
        public override bool System
        {
            get => base.System;
            set
            {
                base.System = value;
                var meta = NestedMeta;
                if (meta != null) meta.System = value;
            }
        }

        /// <inheritdoc />
        public override bool ReadOnly => true;

        /// <inheritdoc/>
        public override bool StoredValueIsTheSameAsValueType => false;

        /// <inheritdoc/>
        public override bool EmptyContent => true;

        //================================================================================================
        //                                              Constructors
        //================================================================================================
        //for new fields
        public BGFieldNested(BGMetaEntity meta, string name) : base(meta, name)
        {
            if (meta.Repo.HasMeta(name))
            {
                Unregister();
                throw new BGException("Meta with name ($) already exists!", name);
            }

            var nestedMeta = CreateNestedMeta(meta, name);
            nestedMetaId = nestedMeta.Id;
            ownerRelationId = nestedMeta.OwnerRelationId;
        }

        //for existing fields
        protected internal BGFieldNested(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        protected virtual BGMetaNested CreateNestedMeta(BGMetaEntity meta, string name) => new BGMetaNested(meta.Repo, name, meta);

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc/>
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldNested(meta, id, name);

        //================================================================================================
        //                                              Methods
        //================================================================================================
        /// <inheritdoc />
        public override void Delete()
        {
            Unregister();
            NestedMeta.Delete();
            base.Delete();
        }

        //================================================================================================
        //                                              Callbacks
        //================================================================================================

        /// <inheritdoc />
        public override void OnEntityDelete(BGEntity entity)
        {
            var nested = OwnerRelation.GetRelatedIn(entity.Id);
            if (BGUtil.IsEmpty(nested)) return;

            foreach (var obj in nested) obj.Delete();
        }

        //================================================================================================
        //                                              Relation
        //================================================================================================
        /// <inheritdoc />
        public BGMetaEntity RelatedMeta => NestedMeta;

        /// <inheritdoc />
        public List<BGEntity> GetRelatedIn(BGId entityId, List<BGEntity> result = null)
        {
            var entity = Meta[entityId];
            if (entity == null) return null;
            return NestedMeta.GetNested(entity, result);
        }

        /// <inheritdoc />
        public List<BGEntity> GetRelatedIn(HashSet<BGId> entityIds, List<BGEntity> result = null)
        {
            if (entityIds == null || entityIds.Count == 0) return result;
            var metaNested = NestedMeta;
            if (metaNested == null) return result;
            var relation = metaNested.OwnerRelation;
            if (relation == null) return result;
            return relation.GetRelatedIn(entityIds, result);
        }

        /// <inheritdoc />
        public void ClearToValue(BGId id)
        {
            //this method does not make sense for nested field, cause it's readonnly
        }

        /// <inheritdoc />
        public void ClearToValue(HashSet<BGId> entityIds)
        {
            //this method does not make sense for nested field, cause it's readonnly
        }

        //================================================================================================
        //                                              Value
        //================================================================================================
        /// <inheritdoc />
        public override void CopyValue(BGField fromField, BGId fromEntityId, int fromEntityIndex, BGId toEntityId)
        {
        }

        /// <inheritdoc />
        public override void ClearValue(int entityIndex)
        {
            //field is readonly
        }

        /// <inheritdoc />
        public override void OnDelete()
        {
            //field is readonly
        }

        /// <inheritdoc />
        public override void ClearValues()
        {
        }

        /// <inheritdoc />
        public override void ForEachValue(Action<int> action)
        {
            //field is readonly
        }

        /// <inheritdoc />
        public override List<BGEntity> this[BGId id]
        {
            get => GetRelatedIn(id);
            set { }
        }

        /// <inheritdoc />
        public override List<BGEntity> this[int entityIndex]
        {
            get => GetRelatedIn(Meta[entityIndex].Id);
            set { }
        }

        /// <inheritdoc />
        public override void Swap(int entityIndex1, int entityIndex2)
        {
            //have no sense
        }

        /// <inheritdoc />
        public override void MoveEntitiesValues(int fromIndex, int toIndex, int numberOfValues)
        {
            //have no sense
        }

        /*
        public override BGField Duplicate(BGMetaEntity meta)
        {
            var clone = base.Duplicate(meta) as BGFieldNested;
            var cloneNested = NestedMeta.Duplicate(BGUtil.DuplicateMetaName(NestedMeta), meta.Id);
            clone.nestedMetaId = cloneNested.Id; 
            clone.ownerRelationId = cloneNested.OwnerRelationId; 
            return clone;
        }
        */

        /// <inheritdoc/>
        public override void DuplicateValue(BGId fromEntityId, int fromEntityIndex, BGId toEntityId)
        {
            if (fromEntityIndex == -1 || IsDeleted) return;
            var index = Meta.FindEntityIndex(toEntityId);
            if (index == -1) return;

            var ownerRelation = NestedMeta.OwnerRelation;
            var entities = ownerRelation.GetRelatedIn(fromEntityId);
            if (entities == null || entities.Count == 0) return;

            for (var i = 0; i < entities.Count; i++)
            {
                var entity = entities[i];
                var copy = entity.Duplicate();
                ownerRelation.SetStoredValue(copy.Index, toEntityId);
            }
        }

        /// <inheritdoc/>
        public override bool AreStoredValuesEqual(BGField field, int myEntityIndex, int otherEntityIndex) => true;

        //================================================================================================
        //                                              Configuration
        //================================================================================================
        /// <inheritdoc />
        public override string ConfigToString()
        {
            return JsonUtility.ToJson(new JsonConfig { NestedMetaId = nestedMetaId.ToString(), OwnerRelationId = ownerRelationId.ToString() });
        }

        /// <inheritdoc />
        public override void ConfigFromString(string config)
        {
            var jsonSettings = JsonUtility.FromJson<JsonConfig>(config);
            nestedMetaId = new BGId(jsonSettings.NestedMetaId);
            ownerRelationId = new BGId(jsonSettings.OwnerRelationId);
        }

        [Serializable]
        private struct JsonConfig
        {
            public string NestedMetaId;
            public string OwnerRelationId;
        }

        /// <inheritdoc />
        public override byte[] ConfigToBytes()
        {
            var writer = new BGBinaryWriter(36);
            //version
            writer.AddInt(1);
            //NestedMetaId
            writer.AddId(nestedMetaId);
            //OwnerRelationId
            writer.AddId(ownerRelationId);

            return writer.ToArray();
        }

        /// <inheritdoc />
        public override void ConfigFromBytes(ArraySegment<byte> config)
        {
            var reader = new BGBinaryReader(config);
            var version = reader.ReadInt();
            switch (version)
            {
                case 1:
                {
                    nestedMetaId = reader.ReadId();
                    ownerRelationId = reader.ReadId();
                    break;
                }
                default:
                {
                    throw new BGException("Unknown version: $", version);
                }
            }
        }

        //================================================================================================
        //                                              Serialization
        //================================================================================================
        /// <inheritdoc />
        public override byte[] ToBytes(int entityIndex)
        {
            return Array.Empty<byte>();
        }

        /// <inheritdoc />
        public override void FromBytes(int entityIndex, ArraySegment<byte> segment)
        {
            //field does not store any value
        }

        /// <inheritdoc />
        public override string ToString(int entityIndex)
        {
            return "";
        }

        /// <inheritdoc />
        public override void FromString(int entityIndex, string value)
        {
        }
    }
}