/*
<copyright file="BGMetaNested.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// meta to use when nested field is used
    /// </summary>
    [MetaDescriptor(Name = "nested", ManagerType = "BansheeGz.BGDatabase.Editor.BGMetaManagerNested", SkipInList = true)]
    public partial class BGMetaNested : BGMetaEntity
    {
        public const ushort CodeType = 2;
        //========================================================================================
        //              Fields
        //========================================================================================

        protected BGId ownerRelationId;

        /// <summary>
        /// owner meta
        /// </summary>
        public BGMetaEntity Owner => OwnerRelation.To;

        /// <summary>
        /// owner relation id
        /// </summary>
        public BGId OwnerRelationId => ownerRelationId;

        /// <summary>
        /// owner relation 
        /// </summary>
        public BGFieldRelationSingle OwnerRelation => (BGFieldRelationSingle)GetField(ownerRelationId);

        //========================================================================================
        //              Constructors
        //========================================================================================

        //for new models!
        public BGMetaNested(BGRepo repo, string name, BGMetaEntity owner) : base(repo, name)
        {
            if (owner == null)
            {
                Unregister();
                throw new BGException("Owner can not be null");
            }

            ownerRelationId = new BGFieldRelationSingle(this, owner.Name, owner) { System = true }.Id;
        }

        //for serialization of existing models!
        protected internal BGMetaNested(BGRepo repo, BGId id, string name) : base(repo, id, name)
        {
        }

        //========================================================================================
        //              Factory
        //========================================================================================
        protected override Func<BGRepo, BGId, string, BGMetaEntity> CreateMetaFactory() => (repo, id, name) => new BGMetaNested(repo, id, name);

        public override ushort TypeCode => CodeType;

        //========================================================================================
        //              Configuration
        //========================================================================================
        /// <inheritdoc />
        public override string ConfigToString()
        {
            return JsonUtility.ToJson(new JsonConfig { OwnerRelationId = ownerRelationId.ToString() });
        }

        /// <inheritdoc />
        public override void ConfigFromString(string config)
        {
            ownerRelationId = new BGId(JsonUtility.FromJson<JsonConfig>(config).OwnerRelationId);
        }

        [Serializable]
        private struct JsonConfig
        {
            public string OwnerRelationId;
        }

        /// <inheritdoc />
        public override byte[] ConfigToBytes()
        {
            var writer = new BGBinaryWriter(20);
            //version
            writer.AddInt(1);
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
                    ownerRelationId = reader.ReadId();
                    break;
                }
                default:
                {
                    throw new BGException("Unknown version: $", version);
                }
            }
        }

        //========================================================================================
        //              Utilities
        //========================================================================================
        /// <summary>
        /// Creates new entity and assign it to specified owner
        /// </summary>
        public BGEntity NewEntity(BGEntity owner)
        {
            CheckOwner(owner);
            return NewEntity(new NewEntityContext(entity => entity.Set(ownerRelationId, owner)));
        }

        /// <summary>
        /// Creates new entity with known id and assign it to specified owner
        /// </summary>
        public BGEntity NewEntity(BGEntity owner, BGId entityId)
        {
            CheckOwner(owner);
            return NewEntity(new NewEntityContext(entityId, entity => entity.Set(ownerRelationId, owner)));
        }

        /// <summary>
        /// Creates new entity and assign it to specified owner
        /// </summary>
        public BGEntity NewEntity(BGEntity owner, NewEntityContext context)
        {
            CheckOwner(owner);
            var result = NewEntity(new NewEntityContext(entity =>
            {
                entity.Set(ownerRelationId, owner);
                context?.Callback?.Invoke(entity);
            }));
            return result;
        }

        private void CheckOwner(BGEntity owner)
        {
            if (owner == null) return;
            if (owner.MetaId == OwnerRelation.ToId) return;
            
            var toMeta = OwnerRelation.To;
            if (toMeta != null) throw new BGException("Can not create a nested entity, cause the owner entity has a wrong meta, expected [$] actual [$]", toMeta.Name, owner.MetaName);
            throw new BGException("Can not create a nested entity, cause the owner entity has a wrong meta, expected meta ID [$] actual meta ID [$]", OwnerRelation.ToId, owner.MetaId);
        }

        /// <summary>
        /// Get nested list by owner
        /// </summary>
        public List<BGEntity> GetNested(BGEntity owner, List<BGEntity> result = null)
        {
            var ownerRelation = OwnerRelation;
            if (ownerRelation.To.Id != owner.Meta.Id)
                throw new BGException("Error from Nested meta $. Incorrect type of owner ($). The type should be equal to $", Name, owner.Meta.Name, ownerRelation.To.Name);

            result = result ?? new List<BGEntity>();
            result.Clear();
            ownerRelation.GetRelatedIn(owner.Id, result);
            return result;
        }

        /// <summary>
        /// get owner for nested entity
        /// </summary>
        public BGEntity GetOwner(BGEntity nestedEntity) => nestedEntity?.Get<BGEntity>(ownerRelationId);

        /*
        public override BGMetaEntity Duplicate(string newMetaName)
        {
            //we can not recreate nested here cause there is not enough information
            return Duplicate(newMetaName, () => new BGMetaRow(Repo, newMetaName));
        }
        
        public BGMetaNested Duplicate(string newMetaName, BGId parentMetaId)
        {
            var clone = base.Duplicate(newMetaName) as BGMetaNested;
            var relation = clone.GetField(OwnerRelation.Name) as BGFieldRelationSingle;
            clone.ownerRelationId = relation.Id;
            relation.toId = parentMetaId;
            return clone;
        }
    */
    }
}