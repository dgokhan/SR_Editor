/*
<copyright file="BGMetaPartitionModelNested.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


namespace BansheeGz.BGDatabase
{
    public class BGMetaPartitionModelNested : BGMetaPartitionModelA
    {
        private readonly BGFieldRelationSingle[] chainToDelegate;
        private readonly BGMetaPartitionModelDefault modelDelegate;

        public BGMetaPartitionModelNested(BGFieldRelationSingle[] chainToDelegate, BGMetaPartitionModelDefault modelDelegate)
            : base(chainToDelegate[0].Meta)
        {
            this.chainToDelegate = chainToDelegate;
            this.modelDelegate = modelDelegate;
        }


        /// <inheritdoc />
        public override bool IsRoot => false;

        /// <inheritdoc />
        public override int? GetPartitionIndex(BGEntity entity)
        {
            var delegateEntity = entity;
            foreach (var relation in chainToDelegate)
            {
                delegateEntity = relation[delegateEntity.Index];
                if (delegateEntity == null) return null;
            }

            if (delegateEntity == null) return null;

            return modelDelegate.GetPartitionIndex(delegateEntity);
        }
    }
}