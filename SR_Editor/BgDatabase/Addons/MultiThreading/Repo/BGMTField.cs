/*
<copyright file="BGMTField.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    ///  Multi-threaded field
    /// </summary>
    public abstract class BGMTField : BGObjectI
    {
        private readonly BGId id;
        private readonly string name;

        public BGId Id => id;

        public string Name => name;

        public int Index { get; internal set; }

        public abstract Type ValueType { get; }

        public BGMTMeta Meta { get; internal set; }

        protected BGMTField(BGId id, string name)
        {
            this.id = id;
            this.name = name;
        }

        protected BGMTField(BGMTMeta meta, BGMTField otherField)
        {
            id = otherField.id;
            name = otherField.name;
            Meta = meta;
            Index = otherField.Index;
        }

        public override string ToString()
        {
            return "Name: " + name + " (id=" + Id + ")";
        }


        internal abstract BGMTField DeepClone(BGMTMeta meta);
        internal abstract void ResizeTo(int newCount);
        internal abstract void RemoveRange(int @from, int count);
        public abstract void CopyTo(BGField field, BGEntity entity, BGMTEntity fromEntity);
    }

    public abstract class BGMTField<T> : BGMTField
    {
        public override Type ValueType => typeof(T);

        protected internal BGMTField(BGId id, string name) : base(id, name)
        {
        }

        protected internal BGMTField(BGMTMeta meta, BGMTField<T> otherField) : base(meta, otherField)
        {
        }

        protected internal abstract T this[int entityIndex] { get; set; }
    }
}