/*
<copyright file="BGMetaPartitionModelProvider.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Factory for generating partitioning model
    /// </summary>
    public class BGMetaPartitionModelProvider
    {
        private static List<BGMetaPartitionModelProviderDelegate> delegates = null;

        private static List<BGMetaPartitionModelProviderDelegate> Delegates
        {
            get
            {
                if (delegates != null) return delegates;
                var implementations = BGUtil.GetAllImplementations(typeof(BGMetaPartitionModelProviderDelegate));
                var list = new List<BGMetaPartitionModelProviderDelegate>();
                if (implementations != null)
                    foreach (var implementation in implementations)
                    {
                        var provider = (BGMetaPartitionModelProviderDelegate)Activator.CreateInstance(implementation);
                        list.Add(provider);
                    }

                delegates = list;
                return delegates;
            }
        }

        /// <summary>
        /// Create partitioning model for provided table 
        /// </summary>
        public virtual BGMetaPartitionModelA Get(BGMetaEntity meta)
        {
            //check delegates first
            var delegates = Delegates;
            foreach (var providerDelegate in delegates)
            {
                var delegateResult = providerDelegate.Get(meta);
                if (delegateResult != null) return delegateResult;
            }

            //check if table supports partitioning
            if (!BGAddonPartition.SupportPartitioningField(meta)) return null;

            if (meta is BGMetaNested)
            {
                //special case for nested tables
                var chain = new List<BGFieldRelationSingle>();
                var currentMeta = meta;
                while (currentMeta is BGMetaNested nested)
                {
                    var relation = nested.OwnerRelation;
                    chain.Add(relation);
                    currentMeta = relation.To;
                }

                for (var i = chain.Count - 1; i >= 0; i--)
                {
                    var relation = chain[i];
                    var model = GetDefault(relation.To);
                    if (model == null) continue;

                    var chainToDelegate = new BGFieldRelationSingle[i + 1];
                    for (var j = 0; j <= i; j++) chainToDelegate[j] = chain[j];
                    return new BGMetaPartitionModelNested(chainToDelegate, model);
                }
            }

            //get default partitioning model
            return GetDefault(meta);
        }

        //get default partitioning model
        public static BGMetaPartitionModelDefault GetDefault(BGMetaEntity meta)
        {
            var field = GetPartitionField(meta);
            return field != null ? new BGMetaPartitionModelDefault(field) : null;
        }

        private static BGField GetPartitionField(BGMetaEntity meta)
        {
            var field = meta.GetField(BGAddonPartition.PartitionFieldName, false);
            if (field == null) return null;

            var targetField = false;
            switch (field)
            {
                case BGFieldByte _:
                case BGFieldShort _:
                case BGFieldInt _:
                case BGFieldByteNullable _:
                case BGFieldShortNullable _:
                case BGFieldIntNullable _:
                    targetField = true;
                    break;
                case BGFieldRelationSingle single:
                {
                    if (string.Equals(single.RelatedMeta.Name, BGAddonPartition.PartitionMetaName)
                        && string.Equals(single.Name, BGAddonPartition.PartitionFieldName)) targetField = true;
                    break;
                }
            }
            return targetField ? field : null;
        }

        /// <summary>
        /// custom partitioning model provider  
        /// </summary>
        public interface BGMetaPartitionModelProviderDelegate
        {
            /// <summary>
            /// get  partitioning model for provided table
            /// </summary>
            BGMetaPartitionModelA Get(BGMetaEntity meta);
        }

        /// <summary>
        /// Iterate tables, which is not partitioned
        /// </summary>
        public void ForEachNotPartitionedMeta(BGRepo repo, Action<BGMetaEntity> action)
        {
            repo.ForEachMeta(meta =>
            {
                var partitionedMetaModel = Get(meta);
                if (partitionedMetaModel != null) return;
                action(meta);
            });
        }

        /// <summary>
        /// Iterate all partitioned tables with default partitioning model
        /// </summary>
        public void ForEachModelWithField(BGRepo repo, Action<BGMetaPartitionModelA.FieldOwner> action)
        {
            repo.ForEachMeta(meta =>
            {
                var partitionedMetaModel = Get(meta);
                if (!(partitionedMetaModel is BGMetaPartitionModelA.FieldOwner owner)) return;
                action(owner);
            });
        }

        /// <summary>
        /// Iterate all partitioned root tables 
        /// </summary>
        public void ForEachRootModel(BGRepo repo, Action<BGMetaPartitionModelI> action)
        {
            repo.ForEachMeta(meta =>
            {
                var metaModel = Get(meta);
                if (metaModel == null || !metaModel.IsRoot) return;
                action(metaModel);
            });
        }
    }
}