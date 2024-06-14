/*
<copyright file="BGAbstractRelationI.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// interface to implement for relational fields
    /// </summary>
    public interface BGAbstractRelationI
    {
        /// <summary>
        /// get list of entities, which refer to entity with specified id
        /// </summary>
        List<BGEntity> GetRelatedIn(BGId entityId, List<BGEntity> result = null);

        /// <summary>
        /// get list of entities, which refer to entities with specified ids
        /// </summary>
        List<BGEntity> GetRelatedIn(HashSet<BGId> entityIds, List<BGEntity> result = null);

        /// <summary>
        /// clear all values, which use entityId as it's target 
        /// </summary>
        void ClearToValue(BGId entityId);

        /// <summary>
        /// clear all values, which use any value from entityIds as it's target 
        /// </summary>
        void ClearToValue(HashSet<BGId> entityIds);
    }
}