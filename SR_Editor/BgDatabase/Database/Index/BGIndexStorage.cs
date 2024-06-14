/*
<copyright file="BGIndexStorage.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    //internal index values storage
    internal abstract class BGIndexStorage
    {
        protected bool dirty;
        public readonly BGIndex dbIndex;

        public abstract int Count { get; }
        public BGIndexStorage(BGIndex dbIndex) => this.dbIndex = dbIndex;
        
        public void MarkDirty() => dirty = true;
    }

    //internal index values storage for fields with T value type
    internal class BGIndexStorage<T> : BGIndexStorage where T : IComparable<T>
    {
        public readonly BGField<T> typedField;

        // private readonly SortedSet<BGIndexStorageItem<T>> store = new SortedSet<BGIndexStorageItem<T>>();
        // private readonly OrderedBag<BGIndexStorageItem<T>> store = new OrderedBag<BGIndexStorageItem<T>>();
        //indexed values in btree 
        private readonly SortedSet<BGIndexStorageItem<T>> store = new SortedSet<BGIndexStorageItem<T>>();


        public BGIndexStorage(BGIndex dbIndex, BGField<T> typedField) : base(dbIndex)
        {
            this.typedField = typedField;
            AttachListeners();
            Build();
        }


        //=================================================================================================================
        //                      Events
        //=================================================================================================================
        private void AttachListeners()
        {
            dbIndex.OnUnload += Dispose;
            dbIndex.Meta.Repo.Events.OnBatchUpdate += BatchListener;
            dbIndex.Meta.AnyEntityAdded += EntityAddedListener;
            dbIndex.Meta.AnyEntityBeforeDeleted += EntityBeforeDeletedListener;
            dbIndex.Meta.EntitiesOrderChanged += EntityOrderChangedListener;
            dbIndex.Field.ValueChanged += FieldValueListener;
        }

        private void Dispose(BGObject obj)
        {
            dbIndex.OnUnload -= Dispose;
            dbIndex.Meta.Repo.Events.OnBatchUpdate -= BatchListener;
            dbIndex.Meta.AnyEntityAdded -= EntityAddedListener;
            dbIndex.Meta.AnyEntityBeforeDeleted -= EntityBeforeDeletedListener;
            dbIndex.Meta.EntitiesOrderChanged -= EntityOrderChangedListener;
            dbIndex.Field.ValueChanged -= FieldValueListener;
        }

        private void BatchListener(object sender, BGEventArgsBatch e)
        {
            if (dirty) return;
            var metaId = dbIndex.Meta.Id;
            if (!e.WasEntitiesAdded(metaId) && !e.WasEntitiesDeleted(metaId) && !e.WasEntitiesUpdated(metaId) && !e.WasEntitiesOrderChanged(metaId)) return;
            dirty = true;
        }

        private void EntityOrderChangedListener(object sender, BGEventArgsEntitiesOrder e) => dirty = true;

        private void EntityBeforeDeletedListener(object sender, BGEventArgsAnyEntity e)
        {
            if (dirty) return;
            if (!Remove(e.Entity)) dirty = true;
        }


        private void EntityAddedListener(object sender, BGEventArgsAnyEntity e)
        {
            if (dirty) return;
            var entity = e.Entity;
            var key = GetKey(entity.Index);
            Add(key, entity);
        }

        private void FieldValueListener(object sender, BGEventArgsField e)
        {
            dirty = true;
            //if event contains old and new values
            if (e is BGEventArgsFieldWithValue valueEvent)
            {
                //get field
                var eventField = valueEvent.GetField();
                var oldValue = valueEvent.GetOldValue();
                var newValue = valueEvent.GetNewValue();

                if (Remove((T)oldValue, e.Entity))
                {
                    Add((T)newValue, e.Entity);
                    dirty = false;
                }
            }
        }

        //=================================================================================================================
        //                      Methods
        //=================================================================================================================
        //build internal storage 
        private void Build()
        {
            store.Clear();
            typedField.Meta.ForEachEntity(entity => Add(typedField[entity.Index], entity));
        }

        //get the index key for entity with provided index
        private T GetKey(int entityIndex) => typedField[entityIndex];

        //get the index key for provided entity
        private T GetKey(BGEntity entity) => typedField[entity.Index];

        //add entity data to the index
        public void Add(BGEntity entity) => Add(GetKey(entity), entity);

        //add entity data to the index
        internal void Add(T key, BGEntity entity) => store.Add(new BGIndexStorageItem<T>(key, entity));

        //remove entity data from the index
        private bool Remove(BGEntity entity) => Remove(GetKey(entity), entity);

        //remove entity data from the index
        private bool Remove(T key, BGEntity entity) => store.Remove(new BGIndexStorageItem<T>(key, entity));

        //get the range using from and to as bounds
        internal void GetRange<TEntity>(List<TEntity> result, BGIndexStorageItem<T> from, BGIndexStorageItem<T> to, bool fromInclusive, bool toInclusive) where TEntity : BGEntity
        {
            /*
            var set = store.Range(from,fromInclusive,  to, toInclusive);
            // for (var i = 0; i < set.Count; i++)
            // {
            //     var item = set[i];
            //     result.Add((TEntity)item.entity);
            // }
            foreach (var item in set) result.Add((TEntity)item.entity);
            */

            var started = fromInclusive || BGIndexStorageItem<T>.EternityMinus == from;
            using (var enumerator = store.GetViewBetween(from, to).GetEnumerator())
            {
                if (!started)
                {
                    while (enumerator.MoveNext())
                    {
                        var current = enumerator.Current;
                        if (Equals(current.key, from.key)) continue;
                        started = true;
                        result.Add((TEntity)current.entity);
                        break;
                    }
                }

                if (started)
                {
                    while (enumerator.MoveNext())
                    {
                        var current = enumerator.Current;
                        result.Add((TEntity)current.entity);
                    }
                }
            }

            if (!toInclusive && BGIndexStorageItem<T>.Eternity != to)
            {
                for (var i = result.Count - 1; i >= 0; i--)
                {
                    var entity = result[i];
                    var value = typedField[entity.Index];
                    if (!Equals(value, to.key)) break;
                    result.RemoveAt(i);
                }
            }
        }

        public override int Count => store.Count;
        public T Min => store.Min.key;

        public T Max => store.Max.key;
    }
}