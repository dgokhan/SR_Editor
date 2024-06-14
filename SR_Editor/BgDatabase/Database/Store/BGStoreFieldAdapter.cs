using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// This is used only to keep backward compatibility
    /// </summary>
    public struct BGStoreFieldAdapter<T, TStoreType> : BGStoreFieldI<TStoreType>
    {
        private readonly BGFieldCachedA<T, TStoreType> field;

        public BGStoreFieldAdapter(BGFieldCachedA<T, TStoreType> field) : this() => this.field = field;

        public TStoreType this[int index]
        {
            get => field.StoreGet(index);
            set => field.StoreSet(index, value);
        }

        public void ForEachKey(Action<int> action) => field.StoreForEachKey(action);

        public void ForEachKeyValue(Action<int, TStoreType> action) => field.StoreForEachKeyValue(action);

        public TStoreType[] CopyRawValues() => field.StoreCopyRawValues();

        public int Count => field.StoreCount;

        public int MinSize
        {
            set => field.StoreMinSize = value;
        }

        public int MinCapacity
        {
            set => field.StoreMinCapacity = value;
        }

        public TStoreType Get(int index) => this[index];

        public void DeleteAt(int index) => field.StoreDeleteAt(index);

        public void Clear() => field.StoreClear();

        public void Add(TStoreType item) => field.StoreAdd(item);

        public void Swap(int index1, int index2) => field.StoreSwap(index1, index2);

        public void MoveValues(int fromIndex, int toIndex, int numberOfElements) => field.StoreMoveValues(fromIndex, toIndex, numberOfElements);
    }
}