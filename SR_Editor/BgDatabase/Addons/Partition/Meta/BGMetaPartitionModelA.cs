/*
<copyright file="BGMetaPartitionModelA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Abstract table partitioning model
    /// </summary>
    public abstract class BGMetaPartitionModelA : BGMetaPartitionModelI
    {
        private readonly BGMetaEntity meta;

        /// <inheritdoc />
        public BGMetaEntity Meta => meta;

        protected BGMetaPartitionModelA(BGMetaEntity meta)
        {
            this.meta = meta;
        }

        /// <inheritdoc />
        public virtual bool IsRoot => true;

        /// <inheritdoc />
        public abstract int? GetPartitionIndex(BGEntity entity);

        /// <summary>
        /// Table, supporting partitioning
        /// </summary>
        public interface FieldOwner : BGMetaPartitionModelI
        {
            /// <summary>
            /// The type of partitioning field 
            /// </summary>
            BGPartitionFieldTypeEnum FieldType { get; }
        }
    }
}