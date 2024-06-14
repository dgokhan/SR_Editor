/*
<copyright file="BGEntityListReference.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Reference to database rows with custom Editor GUI
    /// </summary>
    [Serializable]
    public class BGEntityListReference : BGMetaReference
    {
        //the list of entities IDs
        [SerializeField] private List<string> entityIds;

        /// <summary>
        /// Retrieve the rows, using their IDs
        /// </summary>
        public List<BGEntity> GetEntities()
        {
            if (entityIds == null || entityIds.Count == 0) return null;
            var meta = Meta;
            if (meta == null) return null;

            var result = new List<BGEntity>();
            foreach (var entityId in entityIds)
            {
                var entity = meta.GetEntity(BGId.Parse(entityId));
                if (entity == null) continue;
                result.Add(entity);
            }

            return result;
        }

        /// <summary>
        /// Assign the referenced rows
        /// </summary>
        public void SetEntities(List<BGEntity> entities)
        {
            if (entities == null || entities.Count == 0) Reset();
            else
            {
                var targetMetaId = MetaIdConstraint;
                var constrained = !targetMetaId.IsEmpty;
                if (!constrained) targetMetaId = entities[0].MetaId;
                var result = new List<string>();
                foreach (var entity in entities)
                {
                    if (targetMetaId != entity.MetaId)
                        throw new Exception("Can not assign entities, cause meta is wrong for one of the entities. Meta ids mismatch "
                                            + entity.Meta.Id + "!=" + targetMetaId);
                    result.Add(entity.Id.ToString());
                }

                entityIds = result;
            }
        }

        /// <summary>
        /// Clear internal state
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            entityIds = null;
        }
    }

    /// <summary>
    /// Reference to database rows with additional row class reference (T)
    /// Different row classes are used by CodeGen addon generated classes (each table has it's own class, inherited from BGEntity)
    /// </summary>
    [Serializable]
    public abstract class BGEntityListReference<T> : BGEntityListReference where T : BGEntity
    {
        /// <summary>
        /// Retrieve/assign the list of referenced entities with correct BGEntity subclass = T
        /// </summary>
        public List<T> Entities
        {
            get
            {
                var result = new List<T>();
                var list = GetEntities();
                if (list == null || list.Count == 0) return null;
                foreach (var entity in list) result.Add((T)entity);
                return result;
            }
            set
            {
                if (value == null || value.Count == 0) SetEntities(null);
                else
                {
                    var result = new List<BGEntity>();
                    foreach (var entity in value) result.Add(entity);
                    SetEntities(result);
                }
            }
        }

        /// <inheritdoc/>
        public override BGId MetaIdConstraint => TargetMetaId;

        /// <summary>
        /// Referenced table
        /// </summary>
        public abstract BGId TargetMetaId { get; }

        /// <summary>
        /// implicit cast to generic List with correct row BGEntity subclass 
        /// </summary>
        public static implicit operator List<T>(BGEntityListReference<T> reference) => reference.Entities;
    }
}