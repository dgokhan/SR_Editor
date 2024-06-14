/*
<copyright file="BGMetaPartitionModelI.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Abstract table partitioning model interface
    /// </summary>
    public interface BGMetaPartitionModelI
    {
        /// <summary>
        /// Return the reference to the  table
        /// </summary>
        BGMetaEntity Meta { get; }

        /// <summary>
        /// Is the meta root or nested?
        /// </summary>
        bool IsRoot { get; }
        
        /// <summary>
        /// Return partition index if row is in partition, or null if row is not partitioned 
        /// </summary>
        int? GetPartitionIndex(BGEntity entity);
    }
}