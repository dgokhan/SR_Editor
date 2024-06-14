/*
<copyright file="BGKeyStorage.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Data container for storing key's internal data
    /// </summary>
    internal class BGKeyStorage
    {
        private const int MaxListSize = 16;

        private readonly Dictionary<BGKeyStorageKeyI, object> key2Value = new Dictionary<BGKeyStorageKeyI, object>(new KeyComparer());

        private readonly BGKey dbKey;
        private readonly BGField[] fields;
        private bool dirty;

        public int KeysLength => fields.Length;

        public BGKeyStorage(BGKey dbKey, BGField[] fields)
        {
            if (fields == null || fields.Length == 0) throw new Exception("Can not create keys storage: fields are null!");
            this.dbKey = dbKey;
            this.fields = fields;

            AttachListeners();
            Build();
        }

        //=================================================================================================================
        //                      Events
        //=================================================================================================================
        private void AttachListeners()
        {
            dbKey.OnUnload += Dispose;
            dbKey.Meta.Repo.Events.OnBatchUpdate += BatchListener;
            dbKey.Meta.AnyEntityAdded += EntityAddedListener;
            dbKey.Meta.AnyEntityBeforeDeleted += EntityBeforeDeletedListener;
            dbKey.Meta.EntitiesOrderChanged += EntityOrderChangedListener;
            for (var i = 0; i < fields.Length; i++) fields[i].ValueChanged += FieldValueListener;
        }

        private void Dispose(BGObject obj)
        {
            dbKey.OnUnload -= Dispose;
            dbKey.Meta.Repo.Events.OnBatchUpdate -= BatchListener;
            dbKey.Meta.AnyEntityAdded -= EntityAddedListener;
            dbKey.Meta.AnyEntityBeforeDeleted -= EntityBeforeDeletedListener;
            dbKey.Meta.EntitiesOrderChanged -= EntityOrderChangedListener;
            for (var i = 0; i < fields.Length; i++) fields[i].ValueChanged -= FieldValueListener;
        }

        private void BatchListener(object sender, BGEventArgsBatch e)
        {
            if (dirty) return;
            var metaId = dbKey.Meta.Id;
            if (!e.WasEntitiesAdded(metaId) && !e.WasEntitiesDeleted(metaId) && !e.WasEntitiesUpdated(metaId) && !e.WasEntitiesOrderChanged(metaId)) return;
            dirty = true;
        }

        private void EntityOrderChangedListener(object sender, BGEventArgsEntitiesOrder e)
        {
            dirty = true;
        }

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
                var targetIndex = -1;
                for (var i = 0; i < fields.Length; i++)
                {
                    var field = fields[i];
                    if (!Equals(field, eventField)) continue;
                    targetIndex = i;
                    break;
                }

                if (targetIndex != -1)
                {
                    //if field is found- try to found old new locations
                    var oldValue = valueEvent.GetOldValue();
                    // var newValue = valueEvent.GetNewValue();
                    var oldValueKey = GetKey(e.Entity.Index);
                    var newValueKey = oldValueKey.Clone();
                    switch (KeysLength)
                    {
                        case 1:
                        {
                            ((BGKeyStorageKey1)oldValueKey).Value0 = oldValue;
                            break;
                        }
                        case 2:
                        {
                            var typedKey = ((BGKeyStorageKey2)oldValueKey);
                            switch (targetIndex)
                            {
                                case 0:
                                {
                                    typedKey.Value0 = oldValue;
                                    break;
                                }
                                case 1:
                                {
                                    typedKey.Value1 = oldValue;
                                    break;
                                }
                            }

                            break;
                        }
                        case 3:
                        {
                            var typedKey = ((BGKeyStorageKey3)oldValueKey);
                            switch (targetIndex)
                            {
                                case 0:
                                {
                                    typedKey.Value0 = oldValue;
                                    break;
                                }
                                case 1:
                                {
                                    typedKey.Value1 = oldValue;
                                    break;
                                }
                                case 2:
                                {
                                    typedKey.Value2 = oldValue;
                                    break;
                                }
                            }

                            break;
                        }
                        case 4:
                        {
                            var typedKey = ((BGKeyStorageKey4)oldValueKey);
                            switch (targetIndex)
                            {
                                case 0:
                                {
                                    typedKey.Value0 = oldValue;
                                    break;
                                }
                                case 1:
                                {
                                    typedKey.Value1 = oldValue;
                                    break;
                                }
                                case 2:
                                {
                                    typedKey.Value2 = oldValue;
                                    break;
                                }
                                case 3:
                                {
                                    typedKey.Value3 = oldValue;
                                    break;
                                }
                            }

                            break;
                        }
                        default:
                        {
                            var typedKey = ((BGKeyStorageKeyN)oldValueKey);
                            typedKey.Values[targetIndex] = oldValue;
                            break;
                        }
                    }

                    if (Remove(e.Entity, oldValueKey))
                    {
                        Add(newValueKey, e.Entity);
                        dirty = false;
                    }
                }
            }
        }

        //=================================================================================================================
        //                      Build
        //=================================================================================================================
        //check if key data needs to be rebuilt
        private void CheckDirty()
        {
            if (!dirty) return;
            Build();
        }

        public void MarkDirty() => dirty = true;

        //build internal data storage
        internal void Build()
        {
            key2Value.Clear();
            dirty = false;
            var meta = fields[0].Meta;
            switch (KeysLength)
            {
                case 1:
                {
                    var field0 = fields[0];
                    meta.ForEachEntity(entity =>
                    {
                        Add(new BGKeyStorageKey1(field0.GetValue(entity.Index)), entity);
                    });
                    break;
                }
                case 2:
                {
                    var field0 = fields[0];
                    var field1 = fields[1];
                    meta.ForEachEntity(entity =>
                    {
                        Add(new BGKeyStorageKey2(field0.GetValue(entity.Index), field1.GetValue(entity.Index)), entity);
                    });
                    break;
                }
                case 3:
                {
                    var field0 = fields[0];
                    var field1 = fields[1];
                    var field2 = fields[2];
                    meta.ForEachEntity(entity =>
                    {
                        Add(new BGKeyStorageKey3(field0.GetValue(entity.Index), field1.GetValue(entity.Index), field2.GetValue(entity.Index)), entity);
                    });
                    break;
                }
                case 4:
                {
                    var field0 = fields[0];
                    var field1 = fields[1];
                    var field2 = fields[2];
                    var field3 = fields[3];
                    meta.ForEachEntity(entity =>
                    {
                        Add(new BGKeyStorageKey4(field0.GetValue(entity.Index), field1.GetValue(entity.Index), field2.GetValue(entity.Index), field3.GetValue(entity.Index)), entity);
                    });
                    break;
                }
                default:
                {
                    meta.ForEachEntity(entity =>
                    {
                        var values = new object[fields.Length];
                        for (var i = 0; i < fields.Length; i++) values[i] = fields[i].GetValue(entity.Index);
                        Add(new BGKeyStorageKeyN(values), entity);
                    });
                    break;
                }
            }
        }

        //add provided row to the key's storage
        private void Add(BGKeyStorageKeyI key, BGEntity entity)
        {
            if (key2Value.TryGetValue(key, out var value))
            {
                switch (value)
                {
                    case BGEntity e:
                    {
                        var list = e.Index < entity.Index ? new List<BGEntity>() { e, entity } : new List<BGEntity>() { entity, e };
                        key2Value[key] = list;
                        break;
                    }
                    case List<BGEntity> list:
                    {
                        if (list.Count >= MaxListSize) key2Value[key] = new SortedSet<BGEntity>(list, new EntityComparer()) { entity };
                        else
                        {
                            var targetIndex = -1;
                            for (var i = 0; i < list.Count; i++)
                            {
                                if (entity.Index > list[i].Index) continue;
                                targetIndex = i;
                                break;
                            }

                            if (targetIndex == -1) list.Add(entity);
                            else list.Insert(targetIndex, entity);
                        }

                        break;
                    }
                    case SortedSet<BGEntity> set:
                    {
                        set.Add(entity);
                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), $"value is {(value == null ? "null" : value.GetType().FullName)}");
                }
            }
            else
            {
                key2Value[key] = entity;
            }
        }

        //remove provided row from key's storage
        private bool Remove(BGEntity entity) => Remove(entity, GetKey(entity.Index));

        //remove provided row from key's storage
        private bool Remove(BGEntity entity, BGKeyStorageKeyI key)
        {
            if (!key2Value.TryGetValue(key, out var value)) return false;

            switch (value)
            {
                case BGEntity e:
                {
                    if (Equals(e, entity))
                    {
                        key2Value.Remove(key);
                        return true;
                    }

                    break;
                }
                case List<BGEntity> list:
                {
                    return list.Remove(entity);
                }
                case SortedSet<BGEntity> set:
                {
                    return set.Remove(entity);
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), $"value is {(value == null ? "null" : value.GetType().FullName)}");
            }

            return false;
        }

        //=================================================================================================================
        //                      Nested classes
        //=================================================================================================================

        //comparer to be used for comparing key's values
        private class KeyComparer : IEqualityComparer<BGKeyStorageKeyI>
        {
            public bool Equals(BGKeyStorageKeyI x, BGKeyStorageKeyI y)
            {
                return x.Equals(y);
            }

            public int GetHashCode(BGKeyStorageKeyI obj)
            {
                return obj.GetHashCode();
            }
        }

        //comparer to be used for comparing rows
        private class EntityComparer : IComparer<BGEntity>
        {
            public int Compare(BGEntity x, BGEntity y)
            {
                return x.Index.CompareTo(y.Index);
            }
        }

        //=================================================================================================================
        //                      GET Entity
        //=================================================================================================================
        /// <summary>
        /// Get the first row with provided key values 
        /// </summary>
        public BGEntity GetEntity(params object[] keys)
        {
            CheckDirty();

            var pool = BGKeyStorageKeyN.Pool;
            var key = pool.Get();
            try
            {
                key.Values = keys;
                return key2Value.TryGetValue(key, out var result) ? GetFirst(result) : null;
            }
            finally
            {
                pool.Return(key);
            }
        }

        /// <summary>
        /// Get the first row with provided key value 
        /// </summary>
        public BGEntity GetEntity<T0>(T0 key0)
        {
            CheckDirty();
            var pool = BGKeyStorageKey1<T0>.Pool;
            var key = pool.Get();
            try
            {
                key.Value0 = key0;
                return key2Value.TryGetValue(key, out var result) ? GetFirst(result) : null;
            }
            finally
            {
                pool.Return(key);
            }
        }

        /// <summary>
        /// Get the first row with provided key values 
        /// </summary>
        public BGEntity GetEntity<T0, T1>(T0 key0, T1 key1)
        {
            CheckDirty();
            var pool = BGKeyStorageKey2<T0, T1>.Pool;
            var key = pool.Get();
            try
            {
                key.Value0 = key0;
                key.Value1 = key1;
                return key2Value.TryGetValue(key, out var result) ? GetFirst(result) : null;
            }
            finally
            {
                pool.Return(key);
            }
        }

        /// <summary>
        /// Get the first row with provided key values 
        /// </summary>
        public BGEntity GetEntity<T0, T1, T2>(T0 key0, T1 key1, T2 key2)
        {
            CheckDirty();
            var pool = BGKeyStorageKey3<T0, T1, T2>.Pool;
            var key = pool.Get();
            try
            {
                key.Value0 = key0;
                key.Value1 = key1;
                key.Value2 = key2;
                return key2Value.TryGetValue(key, out var result) ? GetFirst(result) : null;
            }
            finally
            {
                pool.Return(key);
            }
        }

        /// <summary>
        /// Get the first row with provided key values 
        /// </summary>
        public BGEntity GetEntity<T0, T1, T2, T3>(T0 key0, T1 key1, T2 key2, T3 key3)
        {
            CheckDirty();
            var pool = BGKeyStorageKey4<T0, T1, T2, T3>.Pool;
            var key = pool.Get();
            try
            {
                key.Value0 = key0;
                key.Value1 = key1;
                key.Value2 = key2;
                key.Value3 = key3;
                return key2Value.TryGetValue(key, out var result) ? GetFirst(result) : null;
            }
            finally
            {
                pool.Return(key);
            }
        }
        //=================================================================================================================
        //                      GET Entities
        //=================================================================================================================
        /// <summary>
        /// Get all rows with provided key values 
        /// </summary>
        public List<BGEntity> GetEntities(params object[] keys)
        {
            return GetEntities<BGEntity>(null, keys);
        }

        /// <summary>
        /// Get all rows with provided key values 
        /// </summary>
        public List<T> GetEntities<T>(List<T> result, params object[] keys) where T : BGEntity
        {
            CheckDirty();

            var pool = BGKeyStorageKeyN.Pool;
            var key = pool.Get();
            try
            {
                key.Values = keys;
                return key2Value.TryGetValue(key, out var r) ? GetList(result, r) : null;
            }
            finally
            {
                pool.Return(key);
            }
        }

        /// <summary>
        /// Get all rows with provided key value 
        /// </summary>
        public List<T> GetEntities<T, T0>(List<T> result, T0 key0) where T : BGEntity
        {
            CheckDirty();
            var pool = BGKeyStorageKey1<T0>.Pool;
            var key = pool.Get();
            try
            {
                key.Value0 = key0;
                return key2Value.TryGetValue(key, out var r) ? GetList(result, r) : null;
            }
            finally
            {
                pool.Return(key);
            }
        }

        /// <summary>
        /// Get all rows with provided key values 
        /// </summary>
        public List<T> GetEntities<T, T0, T1>(List<T> result, T0 key0, T1 key1) where T : BGEntity
        {
            CheckDirty();
            var pool = BGKeyStorageKey2<T0, T1>.Pool;
            var key = pool.Get();
            try
            {
                key.Value0 = key0;
                key.Value1 = key1;
                return key2Value.TryGetValue(key, out var r) ? GetList(result, r) : null;
            }
            finally
            {
                pool.Return(key);
            }
        }

        /// <summary>
        /// Get all rows with provided key values 
        /// </summary>
        public List<T> GetEntities<T, T0, T1, T2>(List<T> result, T0 key0, T1 key1, T2 key2) where T : BGEntity
        {
            CheckDirty();
            var pool = BGKeyStorageKey3<T0, T1, T2>.Pool;
            var key = pool.Get();
            try
            {
                key.Value0 = key0;
                key.Value1 = key1;
                key.Value2 = key2;
                return key2Value.TryGetValue(key, out var r) ? GetList(result, r) : null;
            }
            finally
            {
                pool.Return(key);
            }
        }

        /// <summary>
        /// Get all rows with provided key values 
        /// </summary>
        public List<T> GetEntities<T, T0, T1, T2, T3>(List<T> result, T0 key0, T1 key1, T2 key2, T3 key3) where T : BGEntity
        {
            CheckDirty();
            var pool = BGKeyStorageKey4<T0, T1, T2, T3>.Pool;
            var key = pool.Get();
            try
            {
                key.Value0 = key0;
                key.Value1 = key1;
                key.Value2 = key2;
                key.Value3 = key3;
                return key2Value.TryGetValue(key, out var r) ? GetList(result, r) : null;
            }
            finally
            {
                pool.Return(key);
            }
        }

        //=================================================================================================================
        //                      Misc
        //=================================================================================================================

        /// <summary>
        /// Get key value for the row with provided index 
        /// </summary>
        private BGKeyStorageKeyI GetKey(int entityIndex)
        {
            BGKeyStorageKeyI key;
            switch (KeysLength)
            {
                case 1:
                {
                    key = new BGKeyStorageKey1(fields[0].GetValue(entityIndex));
                    break;
                }
                case 2:
                {
                    key = new BGKeyStorageKey2(fields[0].GetValue(entityIndex), fields[1].GetValue(entityIndex));
                    break;
                }
                case 3:
                {
                    key = new BGKeyStorageKey3(fields[0].GetValue(entityIndex), fields[1].GetValue(entityIndex), fields[2].GetValue(entityIndex));
                    break;
                }
                case 4:
                {
                    key = new BGKeyStorageKey4(fields[0].GetValue(entityIndex), fields[1].GetValue(entityIndex), fields[2].GetValue(entityIndex), fields[3].GetValue(entityIndex));
                    break;
                }
                default:
                {
                    var values = new object[fields.Length];
                    for (var i = 0; i < fields.Length; i++) values[i] = fields[i].GetValue(entityIndex);
                    key = new BGKeyStorageKeyN(values);
                    break;
                }
            }

            return key;
        }

        // Get the first entity from result object 
        private BGEntity GetFirst(object result)
        {
            // if (result == null) return null;
            switch (result)
            {
                case BGEntity entity:
                    return entity;
                case List<BGEntity> list:
                    return list.Count == 0 ? null : list[0];
                case SortedSet<BGEntity> set:
                    return set.Min;
                // using (var enumerator = set.GetEnumerator())
                // return enumerator.MoveNext() ? enumerator.Current : null;
                default:
                    throw new ArgumentOutOfRangeException(nameof(result), $"result is {(result == null ? "null" : result.GetType().FullName)}");
            }
        }

        // Get all entities from result object
        private List<T> GetList<T>(List<T> resultList, object result) where T : BGEntity
        {
            resultList?.Clear();
            // if (result == null) return resultList;
            switch (result)
            {
                case BGEntity entity:
                    if (resultList != null) resultList.Add((T)entity);
                    else resultList = new List<T>() { (T)entity };
                    break;
                case List<BGEntity> list:
                    if (resultList == null) resultList = new List<T>();
                    //is there any faster GC-free alternative?
                    if (resultList is List<BGEntity> entities) entities.AddRange(list);
                    else for (var i = 0; i < list.Count; i++) resultList.Add((T)list[i]);
                    break;
                case SortedSet<BGEntity> set:
                    if (resultList == null) resultList = new List<T>();
                    //is there any faster GC-free alternative?
                    if (resultList is List<BGEntity> entitiesList) entitiesList.AddRange(set);
                    else foreach (var entity in set) resultList.Add((T)entity);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(result), $"result is {(result == null ? "null" : result.GetType().FullName)}");
            }

            return resultList;
        }
    }
}