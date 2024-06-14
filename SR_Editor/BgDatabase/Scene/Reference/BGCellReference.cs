/*
<copyright file="BGCellReference.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Reference to the database cell with custom Editor GUI
    /// </summary>
    [Serializable]
    public class BGCellReference : BGFieldReference
    {
        public object Value
        {
            get => GetField().GetValue(GetEntity().Index);
            set => GetField().SetValue(GetEntity().Index, value);
        }

        //COPIED FROM BGEntityReference
        //referenced entity ID
        [SerializeField] private string entityId;

        private BGEntity entity;

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

        protected bool Equals(BGCellReference other) => base.Equals(other) && entityId == other.entityId;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BGCellReference)obj);
        }

        public override int GetHashCode()
        {
            unchecked { return (base.GetHashCode() * 397) ^ (entityId != null ? entityId.GetHashCode() : 0); }
        }
    }

    /// <summary>
    /// Reference to the database cell with custom Editor GUI and cell type T generic parameter
    /// </summary>
    [Serializable]
    public class BGCellReference<T> : BGCellReference
    {
        public T ValueCasted
        {
            get => (T)GetField().GetValue(GetEntity().Index);
            set => GetField().SetValue(GetEntity().Index, value);
        }
    }
}