/*
<copyright file="BGMetaPartitionModelLocalized.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// for loaded locale repo localization meta!
    /// </summary>
    public class BGMetaPartitionModelLocalized : BGMetaPartitionModelA
    {
        private readonly List<BGMetaPartitionLocalizedItem> relations;

        //relations- are from main repo!
        public BGMetaPartitionModelLocalized(BGMetaEntity meta, List<BGMetaPartitionLocalizedItem> relations) : base(meta)
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

        public class BGMetaPartitionLocalizedItem
        {
            public BGFieldLocalizedI relation;
            public BGMetaPartitionModelA metaModel;
        }

        public static List<BGMetaPartitionLocalizedItem> GetInboundRelations(BGMetaLocalization mainMeta, Func<BGMetaEntity, BGMetaPartitionModelA> modelProvider)
        {
            var relations = new List<BGMetaPartitionLocalizedItem>();
            var inboundRelations = mainMeta.RelationsInbound;
            for (var i = 0; i < inboundRelations.Count; i++)
            {
                var relation = inboundRelations[i];

                if (!(relation is BGFieldLocalizedI localizedI)) continue;
                var ownerMeta = ((BGField)localizedI).Meta;
                var ownerMetaPartition = modelProvider(ownerMeta);
                ;
                if (ownerMetaPartition == null) continue;
                relations.Add(new BGMetaPartitionLocalizedItem
                {
                    relation = localizedI,
                    metaModel = ownerMetaPartition
                });
            }

            return relations;
        }
    }
}