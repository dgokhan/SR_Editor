/*
<copyright file="BGIndexStorageItem.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Class to be used for storing indexes (key+target entity)
    /// </summary>
    public class BGIndexStorageItem<T> : IComparable<BGIndexStorageItem<T>> where T : IComparable<T>
    {
        public static readonly BGObjectPool<BGIndexStorageItem<T>> Pool = new BGObjectPool<BGIndexStorageItem<T>>(
            () => new BGIndexStorageItem<T>(default, null), item =>
            {
                item.key = default(T);
                item.entity = null;
            });

        public static readonly BGIndexStorageItem<T> Eternity = new BGIndexStorageItem<T>(default, null);
        public static readonly BGIndexStorageItem<T> EternityMinus = new BGIndexStorageItem<T>(default, null);
        
        private static readonly bool IsString = typeof(T) == typeof(string);
        
        public T key;
        public BGEntity entity;

        public BGIndexStorageItem(T key, BGEntity entity)
        {
            this.key = key;
            this.entity = entity;
        }

        /// <inheritdoc/>
        public int CompareTo(BGIndexStorageItem<T> other)
        {
            if (this == Eternity) return 1;
            if (other == Eternity) return -1;
            if (this == EternityMinus) return -1;
            if (other == EternityMinus) return 1;
            
            if (IsString)
            {
                var key1Null = key == null;
                var key2Null = other.key == null;
                if (key1Null && key2Null)
                {
                    if (entity == null || other.entity == null) return 0;
                    return entity.Index.CompareTo(other.entity.Index);
                }
                if (key1Null) return -1;
                if (key2Null) return 1;
                // var compareTo = key.CompareTo(other.key);
                var compareTo = string.Compare((string)(object)key, (string)(object)other.key, BGIndex.DefaultStringComparison);
                if (compareTo != 0) return compareTo;
                if (entity == null || other.entity == null) return 0;
                return entity.Index.CompareTo(other.entity.Index);
            }
            else
            {
                var compareTo = key.CompareTo(other.key);
                if (compareTo != 0) return compareTo;
                if (entity == null || other.entity == null) return 0;
                return entity.Index.CompareTo(other.entity.Index);
            }
        }
    }
}