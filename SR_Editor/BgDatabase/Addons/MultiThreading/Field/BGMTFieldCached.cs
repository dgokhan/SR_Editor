/*
<copyright file="BGMTFieldCached.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Multi-threaded field storing values in list
    /// </summary>
    public class BGMTFieldCached<T> : BGMTField<T>
    {
        protected List<T> values;

        internal BGMTFieldCached(BGField field) : base(field.Id, field.Name)
        {
            MoveData(field);
        }

        private BGMTFieldCached(BGMTMeta meta, BGMTFieldCached<T> otherField) : base(meta, otherField)
        {
            values = new List<T>(otherField.values);
        }

        //!!!! this method is supposed to be called from main thread only
        protected virtual void MoveData(BGField field)
        {
            var typedField = field as BGStorageI<T>;
            if (typedField == null) throw new BGException("Can not cast to BGStorageI<T>");

            values = new List<T>(typedField.CopyRawValues());
        }

        protected internal override T this[int entityIndex]
        {
            get => values[entityIndex];
            set => values[entityIndex] = value;
        }

        internal override void ResizeTo(int newCount)
        {
            if (values.Count >= newCount) return;
            values.AddRange(new T[newCount]);
        }

        internal override void RemoveRange(int @from, int count)
        {
            values.RemoveRange(from, count);
        }

        internal override BGMTField DeepClone(BGMTMeta meta)
        {
            return new BGMTFieldCached<T>(meta, this);
        }

        public override void CopyTo(BGField field, BGEntity entity, BGMTEntity fromEntity)
        {
            var typedField = (BGField<T>)field;
            typedField[entity.Index] = this[fromEntity.Index];
        }
    }

    public abstract class BGMTFieldCached<T, TStoreValue> : BGMTField<T>, BGStorable<TStoreValue>
    {
        protected List<TStoreValue> values;


        protected internal BGMTFieldCached(BGField field) : base(field.Id, field.Name)
        {
            MoveData(field);
        }

        protected internal BGMTFieldCached(BGMTMeta meta, BGMTFieldCached<T, TStoreValue> otherField) : base(meta, otherField)
        {
            values = new List<TStoreValue>(otherField.values);
        }

        //!!!! this method is supposed to be called from main thread only
        protected virtual void MoveData(BGField field)
        {
            var typedField = field as BGStorageI<TStoreValue>;
            if (typedField == null) throw new BGException("Can not cast to BGStorageI<TStoreValue>");

            values = new List<TStoreValue>(typedField.CopyRawValues());
        }

        internal override void ResizeTo(int newCount)
        {
            if (values.Count >= newCount) return;
            values.AddRange(new TStoreValue[newCount]);
        }

        internal override void RemoveRange(int @from, int count)
        {
            values.RemoveRange(from, count);
        }

        public void SetStoredValue(int entityIndex, TStoreValue value)
        {
            values[entityIndex] = value;
        }

        public TStoreValue GetStoredValue(int entityIndex)
        {
            return values[entityIndex];
        }
    }
}