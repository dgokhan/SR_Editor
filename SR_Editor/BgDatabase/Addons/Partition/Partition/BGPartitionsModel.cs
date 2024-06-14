/*
<copyright file="BGPartitionsModel.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Data container for partitioning
    /// </summary>
    public class BGPartitionsModel
    {
        private readonly BGMetaEntity partitionMeta;
        private readonly BGPartitionModelDefault[] partitions;
        private readonly BGPartitionModelMain main;
        private Dictionary<BGId, BGPartitionModelDefault> id2partition;

        public BGPartitionModelA Main => main;

        public BGRepo Repo => partitionMeta.Repo;

        public int PartitionsCount => partitions.Length;

        public BGPartitionModelDefault[] Partitions => partitions;

        public void ForEach(Action<BGPartitionModelDefault> action)
        {
            foreach (var partition in partitions) action(partition);
        }

        public BGPartitionsModel(BGRepo repo)
        {
            partitionMeta = repo.GetMeta(BGAddonPartition.PartitionMetaName);
            main = new BGPartitionModelMain();
            partitions = new BGPartitionModelDefault[partitionMeta.CountEntities];
            for (var i = 0; i < partitionMeta.CountEntities; i++) partitions[i] = new BGPartitionModelDefault(partitionMeta.GetEntity(i));
        }

        public BGPartitionModelDefault Get(int index)
        {
            // if (index < 0) throw new Exception("Can not get partition model: index=" + index);
            // if (index >= partitions.Length) throw new Exception("Can not get partition model: index=" + index);
            if (index < 0) return null;
            if (index >= partitions.Length) return null;
            return partitions[index];
        }

        public BGPartitionModelDefault Get(BGId id)
        {
            if (id2partition == null) InitIdDictionary();
            var model = BGUtil.Get(id2partition, id);
            // if (model == null) throw new Exception("Can not get partition model: id=" + id);
            return model;
        }

        private void InitIdDictionary()
        {
            id2partition = new Dictionary<BGId, BGPartitionModelDefault>(partitionMeta.CountEntities);
            partitionMeta.ForEachEntity(entity => id2partition[entity.MetaId] = Get(entity.Index));
        }
    }
}