/*
<copyright file="BGRelationI.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// single table relation interface
    /// </summary>
    public partial interface BGRelationI : BGAbstractRelationI
    {
        /// <summary>
        /// related meta
        /// </summary>
        BGMetaEntity RelatedMeta { get; }

        /// <summary>
        /// related meta ID
        /// </summary>
        BGId ToId { get; }
    }
}