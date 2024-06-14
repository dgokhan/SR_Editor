/*
<copyright file="BGPartitionSnapshot.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Stores the number of rows before loading/unloading partition and invalidate keys/indexes after loading/unloading if rows count has changed
    /// </summary>
    public class BGPartitionSnapshot
    {
        private readonly List<Tuple<BGMetaEntity, int>> data = new List<Tuple<BGMetaEntity, int>>();
        public BGPartitionSnapshot(BGRepo repo)
        {
            var provider = new BGMetaPartitionModelProvider();
            provider.ForEachModelWithField(repo, owner => data.Add(Tuple.Create(owner.Meta, owner.Meta.CountEntities)));
        }

        public void MarkKeysAndIndexesDirty()
        {
            foreach (var tuple in data)
            {
                var meta = tuple.Item1;
                var count = tuple.Item2;
                if (meta.CountEntities == count) continue;
                meta.ForEachKey(key => key.MarkDirty());
                meta.ForEachIndex(index => index.MarkDirty());
            }
        }
    }
}