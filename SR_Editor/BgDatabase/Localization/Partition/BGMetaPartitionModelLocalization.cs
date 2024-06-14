/*
<copyright file="BGMetaPartitionModelLocalization.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// for main repo localization meta!
    /// </summary>
    public class BGMetaPartitionModelLocalization : BGMetaPartitionModelA
    {
        private readonly List<BGMetaPartitionModelLocalized.BGMetaPartitionLocalizedItem> relations;

        public BGMetaPartitionModelLocalization(List<BGMetaPartitionModelLocalized.BGMetaPartitionLocalizedItem> relations) : base(relations[0].relation.RelatedMeta)
        {
            this.relations = relations;
        }

        public override int? GetPartitionIndex(BGEntity entity)
        {
            foreach (var relation in relations)
            {
                var owners = relation.relation.GetRelatedIn(entity.Id);
                if (owners == null || owners.Count == 0) continue;
                var firstOwner = owners[0];
                return relation.metaModel.GetPartitionIndex(firstOwner);
            }

            return null;
        }
    }
}