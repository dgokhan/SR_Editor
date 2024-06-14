/*
<copyright file="BGArrayStore.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// array based objects store
    /// </summary>
    public class BGArrayStore<T>
    {
        protected T[] Items = Array.Empty<T>();

        /// <summary>
        /// objects count
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Capacity
        /// </summary>
        public int MinSize
        {
            set
            {
                if (Count >= value) return;
                MinCapacity = value;
                Count = value;
            }
        }

        /// <summary>
        /// Capacity
        /// </summary>
        public int MinCapacity
        {
            set
            {
                if (Items.Length >= value) return;
                var newCapacity = Items.Length == 0 ? 4 : Items.Length * 2;
                if (newCapacity < value) newCapacity = value;
                var newItems = new T[newCapacity];
                if (Count > 0) Array.Copy(Items, 0, newItems, 0, Count);
                Items = newItems;
            }
        }

        /// <summary>
        /// Get object by its index
        /// </summary>
        public T Get(int index)
        {
            if (index >= Count) throw new Exception("Index is out of bounds, greater or equal to maxIndex, " + index + ">=" + Count);
            return Items[index];
        }

        /// <summary>
        /// delete an object at specified index
        /// </summary>
        public void DeleteAt(int index)
        {
            if (Count <= index) return;

            Count--;
            var length = Count - index;
            if (length > 0) Array.Copy(Items, index + 1, Items, index, length);
            Items[Count] = default;
        }

        /// <summary>
        /// Clear internal storage
        /// </summary>
        public void Clear()
        {
            Items = Array.Empty<T>();
            Count = 0;
        }

        /// <summary>
        /// add new object to the store
        /// </summary>
        public void Add(T item)
        {
            MinCapacity = Count + 1;
            Items[Count] = item;
            Count++;
        }

        /// <summary>
        /// swap 2 objects
        /// </summary>
        public void Swap(int index1, int index2) => (Items[index1], Items[index2]) = (Items[index2], Items[index1]);

        /// <summary>
        /// move the objects 
        /// </summary>
        public void MoveValues(int fromIndex, int toIndex, int numberOfElements)
        {
            var temp = new T[numberOfElements];
            Array.Copy(Items, fromIndex, temp, 0, numberOfElements);
            if (fromIndex > toIndex)
            {
                if (toIndex + numberOfElements < fromIndex) Array.Copy(Items, toIndex, Items, toIndex + numberOfElements, fromIndex - toIndex);
                else
                {
                    var toMove = fromIndex - toIndex;
                    Array.Copy(Items, toIndex, Items, fromIndex + numberOfElements - toMove, toMove);
                }
            }
            else
            {
                if (fromIndex + numberOfElements <= toIndex) Array.Copy(Items, fromIndex + numberOfElements, Items, fromIndex, toIndex - fromIndex);
                else Array.Copy(Items, fromIndex + numberOfElements, Items, fromIndex, toIndex - fromIndex);
            }

            Array.Copy(temp, 0, Items, toIndex, numberOfElements);
        }

/*
        protected internal void Insert(int numberOfRows, int atRow)
        {
            var newCount = Count + numberOfRows;

            var newItems = new T[newCount];

            if (atRow > 0) Array.Copy(Items, 0, newItems, 0, atRow);

            if (atRow < Count) Array.Copy(Items, atRow, newItems, atRow + numberOfRows, Count - atRow);

            Count = newCount;
            Items = newItems;
        }
*/
        /*
         Use MinSize!!!
        public void Resize(int count)
        {
            if (Count == count) return;
            if (count < 0) throw new BGException("Can not resize array: new count should be more than zero, new count=$", count);
            var oldItems = Items;
            var oldCount = Count;
            Items = new T[count];
            Count = count;
            if (oldCount > 0 && count > 0)
            {
                Array.Copy(oldItems, 0, Items, 0, oldCount < count ? oldCount : count);
            }
        }
    */
    }
}