/*
<copyright file="BGPartitionSaveProcessor.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.IO;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Processor for creating partitioned data (file path => content)
    /// </summary>
    public class BGPartitionSaveProcessor
    {
        /// <summary>
        /// Provide saved data model (file path => content)
        /// </summary>
        public BGPartitionSaveModel Save(BGPartitionSaveContext context)
        {

            // var partitionFilter = context.partitionFilter;
            var partitions = context.partitions;
            var basicPath = context.basicPath;
            var provider = context.provider;
            var partitionAddon = context.partitionAddon;
            partitionAddon.CheckConfig();


            //original repo
            var originalRepo = context.repo;

            //main repo
            var mainRepo = new BGRepo(originalRepo);
            mainRepo.Addons.AddFrom(originalRepo.Addons);

            //horizontal partitions
            BGPartitionWrapper[] partitionContexts = null;
            if (partitions != null)
            {
                partitionContexts = new BGPartitionWrapper[partitions.PartitionsCount];
                for (var i = 0; i < partitionContexts.Length; i++)
                {
                    var partition = partitions.Partitions[i];
                    // if (partitionFilter != null && !partitionFilter(partition)) continue;
                    partitionContexts[i] = new BGPartitionWrapper(partition);
                }
            }

            //vertical partitioning
            var verticalProvider = partitionAddon.EnabledVertical && !context.verticalDisabled ? new BGPartitionSaveVerticalProvider(originalRepo, partitionAddon) : null;

            //process
            originalRepo.ForEachMeta(meta =>
            {
                var mainMeta = verticalProvider?.GetMeta(meta.Id) ?? mainRepo.GetMeta(meta.Id);

                var metaModel = provider.Get(meta);

                if (metaModel == null || partitions==null) CopyRows(meta, mainMeta);
                else 
                    //partitioned
                    meta.ForEachEntity(entity =>
                    {
                        var index = metaModel.GetPartitionIndex(entity);
                        if (index == null) CopyRow(entity, mainMeta);
                        else
                        {
                            if (index.Value < 0 || index.Value >= partitionContexts.Length)
                            {
                                CopyRow(entity, mainMeta);
                            }
                            else
                            {
                                var saveContext = partitionContexts[index.Value];
                                if (saveContext == null) return;
                                saveContext.Process(entity);
                            }
                        }
                    });
            });

            var result = new BGPartitionSaveModel();
            result.Add(basicPath, mainRepo.Save());


            var folder = Path.GetDirectoryName(basicPath);
            var basicPathNoExt = Path.GetFileNameWithoutExtension(basicPath);
            partitions?.ForEach(partition =>
            {
                var saveContext = partitionContexts[partition.Entity.Index];
                if (saveContext == null) return;

                // var filePathNoExt = BGPartitionModelDefault.ToFilePath(basicPathNoExt, partition.Entity.Id)
                var filePathNoExt = basicPathNoExt + '_' + BGAddonPartition.PartitionFilePathKey + '_' + BGAddonPartition.ToFilePath(partition.Entity.Id);
                // var filePath = Path.ChangeExtension(filePathNoExt, "bytes");
                var filePath = filePathNoExt;
                if (folder != null) filePath = Path.Combine(folder, filePath);
                result.Add(filePath, saveContext.Repo.Save());
            });

            verticalProvider?.ForEachRepo((id, repo) =>
            {
                var filePath = basicPathNoExt + '_' + BGAddonPartition.PartitionVerticalFilePathKey + '_' + BGAddonPartition.ToFilePath(id);
                if (folder != null) filePath = Path.Combine(folder, filePath);
                result.Add(filePath, repo.Save());
            });

            return result;
        }


        //copy rows from source meta to target meta
        public static void CopyRows(BGMetaEntity from, BGMetaEntity to)
        {
            from.ForEachEntity(entity => CopyRow(entity, to));
        }

        //copy a row from source meta to target meta
        public static void CopyRow(BGEntity entity, BGMetaEntity to)
        {
            var cloneEntity = to.NewEntity(entity.Id);
            entity.Meta.ForEachField(field =>
            {
                if (field.EmptyContent) return;
                var targetField = to.GetField(field.Index);
                targetField.CopyValue(field, entity.Id, entity.Index, cloneEntity.Id);
            });
        }

        //helper class to hold partition model
        private class BGPartitionWrapper
        {
            private readonly BGRepo repo = new BGRepo();

            private BGMetaEntity cachedMeta;

            public BGRepo Repo => repo;

            private readonly BGPartitionModelDefault partition;

            public BGPartitionWrapper(BGPartitionModelDefault partition)
            {
                this.partition = partition;
            }

            public void Process(BGEntity entity)
            {
                if (cachedMeta == null || cachedMeta.Id != entity.MetaId) cachedMeta = entity.Meta.CloneTo(repo, null, null, false);

                CopyRow(entity, cachedMeta);
            }
        }
    }
}