/*
<copyright file="BGRowsOrder.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// helper class for manipulating rows order 
    /// </summary>
    public class BGRowsOrder
    {
        private readonly BGLogger logger;
        private readonly BGMetaEntity meta;
        private readonly List<EntityOrderInfo> rows = new List<EntityOrderInfo>();
        private readonly Action<int, int> swap;

        public BGRowsOrder(BGLogger logger, BGMetaEntity meta, Action<int, int> swap)
        {
            this.logger = logger;
            this.meta = meta;
            this.swap = swap;
        }

        /// <summary>
        /// add row's information 
        /// </summary>
        public void Add(EntityOrderInfo entityOrderInfo) => rows.Add(entityOrderInfo);

        /// <summary>
        /// Called after all rows information is added
        /// </summary>
        public void Complete(Action finished)
        {
            try
            {
                var sortedRowsBySource = new List<EntityOrderInfo>(rows);
                sortedRowsBySource.Sort((e1, e2) => e1.SourceEntity.Index.CompareTo(e2.SourceEntity.Index));
                if (!RequireReordering(sortedRowsBySource))
                {
                    logger.AppendLine("No rows sorting is required. Sorting skipped..");
                    return;
                }

                var sortedRowsByTarget = new List<EntityOrderInfo>(rows);
                sortedRowsByTarget.Sort((e1, e2) => e1.TargetIndex.CompareTo(e2.TargetIndex));

                var swapsCount = 0;
                for (var i = sortedRowsBySource.Count - 1; i >= 0; i--)
                {
                    var infoSource = sortedRowsBySource[i];
                    var infoTarget = sortedRowsByTarget[i];

                    if (infoTarget == infoSource) continue;

                    swapsCount++;

                    swap(infoTarget.TargetIndex, infoSource.TargetIndex);

                    var notSortedIndex = sortedRowsByTarget.FindIndex(orderInfo => orderInfo == infoSource);
                    (sortedRowsByTarget[i], sortedRowsByTarget[notSortedIndex]) = (sortedRowsByTarget[notSortedIndex], sortedRowsByTarget[i]);

                    (infoTarget.TargetIndex, infoSource.TargetIndex) = (infoSource.TargetIndex, infoTarget.TargetIndex);
                }

                logger.AppendLine("Rows were sorted with $ operations ", swapsCount);
            }
            finally
            {
                finished?.Invoke();
            }
        }

        /// <summary>
        /// if provided rows information required reordering? 
        /// </summary>
        private static bool RequireReordering(List<EntityOrderInfo> sortedRows)
        {
            var max = -1;
            foreach (var info in sortedRows)
            {
                var entityIndex = info.TargetIndex;
                if (max > entityIndex) return true;
                max = entityIndex;
            }

            return false;
        }


        /// <summary>
        /// data container for a single row ordering info
        /// </summary>
        public class EntityOrderInfo
        {
            private readonly BGEntity entity;
            private readonly BGEntity sourceEntity;

            /// <summary>
            /// Target entity index
            /// </summary>
            public int TargetIndex { get; set; }

            /// <summary>
            /// target entity
            /// </summary>
            public BGEntity Entity => entity;

            /// <summary>
            /// source entity
            /// </summary>
            public BGEntity SourceEntity => sourceEntity;

            public EntityOrderInfo(BGEntity sourceEntity, BGEntity entity, int targetIndex)
            {
                this.sourceEntity = sourceEntity;
                this.entity = entity;
                TargetIndex = targetIndex;
            }

            /// <inheritdoc/>
            public override string ToString() => entity.ToString() + " - " + TargetIndex;
        }
    }
}