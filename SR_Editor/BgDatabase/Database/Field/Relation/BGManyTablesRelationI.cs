/*
<copyright file="BGManyTablesRelationI.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// interface for many tables relational field
    /// </summary>
    public interface BGManyTablesRelationI : BGAbstractRelationI
    {
        /// <summary>
        /// related metas
        /// </summary>
        List<BGMetaEntity> RelatedMetas { get; }

        List<BGId> ToIds { get; }

        /// <summary>
        /// remove related meta
        /// </summary>
        void RemoveRelatedMeta(BGMetaEntity metaEntity);


        /// <summary>
        /// add related meta
        /// </summary>
        void AddRelatedMeta(BGMetaEntity metaEntity);
    }
}