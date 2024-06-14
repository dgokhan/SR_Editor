/*
<copyright file="BGIndexOperatorGreaterOrEqual.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// class for GREATER OR EQUAL operator
    /// </summary>
    public class BGIndexOperatorGreaterOrEqual<T> : BGIndexOperatorRange<T> where T : IComparable<T>
    {
        public T Value;
        public BGIndexOperatorGreaterOrEqual(T value) => this.Value = value;

        /// <inheritdoc/>
        protected override BGIndexStorageItem<T> GetKeyFrom(out bool pooled, out bool inclusive)
        {
            pooled = true;
            inclusive = true;
            var key = BGIndexStorageItem<T>.Pool.Get();
            key.key = Value;
            return key;

        }

        /// <inheritdoc/>
        protected override BGIndexStorageItem<T> GetKeyTo(out bool pooled, out bool inclusive)
        {
            pooled = false;
            inclusive = false;
            return BGIndexStorageItem<T>.Eternity;
        }
    }
}