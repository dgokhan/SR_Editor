/*
<copyright file="BGStoreField.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Storage for field data 
    /// </summary>
    public partial class BGStoreField<T> : BGArrayStore<T>, BGStoreFieldI<T>
    {
        /// <summary>
        /// get value by entity id
        /// </summary>
        public T this[int index]
        {
            get
            {
                if (index >= Count) throw new Exception("Index is out of bounds, greater or equal to maxIndex, " + index + ">=" + Count);
                return Items[index];
            }
            set
            {
                if (index >= Count) throw new Exception("Index is out of bounds, greater or equal to maxIndex, " + index + ">=" + Count);
                Items[index] = value;
            }
        }

        /// <summary>
        /// invoke action for each value
        /// </summary>
        public void ForEachKey(Action<int> action)
        {
            var count = Count;
            var defaultValue = default(T);
            var comparer = EqualityComparer<T>.Default;
            for (var i = 0; i < count; i++)
            {
                var item = Items[i];
                if (comparer.Equals(item, defaultValue)) continue;
                action(i);
            }
        }

        /// <summary>
        /// invoke action for each value
        /// </summary>
        public void ForEachKeyValue(Action<int, T> action)
        {
            var count = Count;
            for (var i = 0; i < count; i++) action(i, Items[i]);
        }

        /// <summary>
        /// copy values to array
        /// </summary>
        public T[] CopyRawValues()
        {
            var copy = new T[Count];
            Array.Copy(Items, copy, Count);
            return copy;
        }
    }
}