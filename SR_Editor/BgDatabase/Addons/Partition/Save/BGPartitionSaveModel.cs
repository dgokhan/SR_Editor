/*
<copyright file="BGPartitionSaveModel.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Saving model with all the files path/data
    /// </summary>
    public class BGPartitionSaveModel
    {
        private readonly Dictionary<string, byte[]> path2content = new Dictionary<string, byte[]>();

        /// <summary>
        /// add File data (path, content)
        /// </summary>
        public void Add(string key, byte[] content)
        {
            path2content[key] = content;
        }

        /// <summary>
        /// Get File data by its path
        /// </summary>
        public byte[] Get(string key)
        {
            return BGUtil.Get(path2content, key);
        }

        /// <summary>
        /// Iterate all saved data (path, content)
        /// </summary>
        public void ForEach(Action<string, byte[]> action)
        {
            foreach (var pair in path2content) action(pair.Key, pair.Value);
        }
    }
}