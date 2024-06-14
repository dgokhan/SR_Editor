/*
<copyright file="BGEntityReference.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Reference to the database row with custom Editor GUI
    /// </summary>
    [Serializable]
    public class BGEntityReference : BGMetaReference
    {
        //referenced entity ID
        [SerializeField] private string entityId;

        private BGEntity entity;

        public BGEntityReference()
        {
        }

        public BGEntityReference(BGEntity entity) => SetEntity(entity);


        /// <summary>
        /// Retrieve the row using table and row IDs 
        /// </summary>
        public BGEntity GetEntity()
        {
            if (entity != null && entity.Meta != null && !entity.Meta.IsDeleted && entity.Id == BGId.Parse(entityId)) return entity;

            var meta = Meta;
            if (meta == null) return null;
            entity = meta.GetEntity(BGId.Parse(entityId));
            return entity;
        }

        /// <summary>
        /// Reference a new row  
        /// </summary>
        public void SetEntity(BGEntity entity)
        {
            if (entity == null) Reset();
            else
            {
                var metaConstraintId = MetaIdConstraint;
                if (!metaConstraintId.IsEmpty && entity.MetaId != metaConstraintId)
                    throw new Exception("Can not assign entity, cause meta is wrong. Ids mismatch "
                                        + entity.Meta.Id + "!=" + metaConstraintId);
                metaId = entity.MetaId.ToString();
                entityId = entity.Id.ToString();
                this.entity = entity;
            }
        }

        /// <summary>
        /// Clear internal state
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            entityId = null;
            entity = null;
        }

        protected bool Equals(BGEntityReference other) => base.Equals(other) && entityId == other.entityId;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BGEntityReference)obj);
        }

        public override int GetHashCode()
        {
            unchecked { return (base.GetHashCode() * 397) ^ (entityId != null ? entityId.GetHashCode() : 0); }
        }
    }

    /// <summary>
    /// Reference to the database row with additional row class reference (T)
    /// Different row classes are used by CodeGen addon generated classes (each table has it's own class, inherited from BGEntity)
    /// </summary>
    [Serializable]
    public abstract class BGEntityReference<T> : BGEntityReference where T : BGEntity
    {
        /// <summary>
        /// Retrieve/assign the row with correct BGEntity subclass
        /// </summary>
        public T Entity
        {
            get => (T)GetEntity();
            set => SetEntity(value);
        }

        /// <inheritdoc/> 
        public override BGId MetaIdConstraint => TargetMetaId;

        /// <summary>
        /// Referenced table
        /// </summary>
        public abstract BGId TargetMetaId { get; }

        /// <summary>
        /// implicit cast to the entity with correct entity type 
        /// </summary>
        public static implicit operator T(BGEntityReference<T> reference) => reference.Entity;
    }
}