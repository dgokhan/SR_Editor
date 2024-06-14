/*
<copyright file="BGIndexOperatorBetween.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// class for BETWEEN operator
    /// </summary>
    public class BGIndexOperatorBetween<T> : BGIndexOperatorRange<T> where T : IComparable<T>
    {
        public T From;
        public T To;
        public bool LowBoundInclusive;
        public bool UpperBoundInclusive;
        
        public BGIndexOperatorBetween(T from, T to, bool lowBoundInclusive, bool upperBoundInclusive)
        {
            this.From = from;
            this.To = to;
            this.LowBoundInclusive = lowBoundInclusive;
            this.UpperBoundInclusive = upperBoundInclusive;
        }

        /// <inheritdoc/>
        protected override BGIndexStorageItem<T> GetKeyFrom(out bool pooled, out bool inclusive)
        {
            pooled = true;
            inclusive = LowBoundInclusive;
            var key = BGIndexStorageItem<T>.Pool.Get();
            key.key = From;
            return key;

        }

        /// <inheritdoc/>
        protected override BGIndexStorageItem<T> GetKeyTo(out bool pooled, out bool inclusive)
        {
            pooled = true;
            inclusive = UpperBoundInclusive;
            var key = BGIndexStorageItem<T>.Pool.Get();
            key.key = To;
            return key;
        }
    }
}