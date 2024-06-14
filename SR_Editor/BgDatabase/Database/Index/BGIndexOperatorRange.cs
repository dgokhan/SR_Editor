/*
<copyright file="BGIndexOperatorRange.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// abstract class for range scan operator
    /// </summary>
    public abstract class BGIndexOperatorRange<T> : BGIndexOperator where T : IComparable<T>
    {
        /// <summary>
        /// get lower bound value
        /// </summary>
        protected abstract BGIndexStorageItem<T> GetKeyFrom(out bool pooled, out bool inclusive);
        /// <summary>
        /// get upper bound value
        /// </summary>
        protected abstract BGIndexStorageItem<T> GetKeyTo(out bool pooled, out bool inclusive);
        
        /// <inheritdoc/>
        internal override void GetResult<T1>(List<T1> result, BGIndexStorage storage)
        {
            var typedStorage = (BGIndexStorage<T>)storage;
            var key1 = GetKeyFrom(out var pooled1, out var inclusive1);
            var key2 = GetKeyTo(out var pooled2, out var inclusive2);
            try
            {
                typedStorage.GetRange(result, key1, key2, inclusive1, inclusive2);
            }
            finally
            {
                if (pooled1) BGIndexStorageItem<T>.Pool.Return(key1);
                if (pooled2) BGIndexStorageItem<T>.Pool.Return(key2);
            }
        }
    }
}