/*
<copyright file="BGMTMeta.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Readonly Multi-threaded table
    /// </summary>
    public class BGMTMeta : BGObjectI
    {
        //================================================================================================
        //                                              Static
        //================================================================================================


        //================================================================================================
        //                                              Fields
        //================================================================================================

        private readonly BGId id;
        private readonly string name;
        private readonly int index;

        protected BGIdDictionary<BGMTField> id2Field;
        protected Dictionary<string, BGMTField> name2Field;
        protected BGMTField[] fields;
        protected List<BGId> entityIds;
        protected BGIdDictionary<int> entityId2Index;

        public BGId Id => id;

        public string Name => name;

        public int Index => index;

        public int CountEntities => entityIds.Count;

        public BGMTRepo Repo { get; internal set; }

        public int CountFields => fields.Length;

        //================================================================================================
        //                                              Constructor
        //================================================================================================
        /// <summary>
        /// initial create 
        /// </summary>
        internal BGMTMeta(BGMetaEntity meta, int index)
        {
            id = meta.Id;
            name = meta.Name;
            this.index = index;

            //----------------------------- entities
            entityIds = new List<BGId>(meta.CountEntities);
            entityId2Index = new BGIdDictionary<int>(meta.CountEntities);
            meta.ForEachEntity(entity =>
            {
                entityId2Index.Add(entity.Id, entityIds.Count);
                entityIds.Add(entity.Id);
            });

            //----------------------------- fields
            var i = 0;
            id2Field = new BGIdDictionary<BGMTField>();
            name2Field = new Dictionary<string, BGMTField>();
            var fieldsList = new List<BGMTField>();
            meta.ForEachField(field =>
            {
                var newField = BGMTFieldFactory.Create(this, field);
                if (newField == null) return;

                fieldsList.Add(newField);
                newField.Index = i++;
                id2Field[newField.Id] = newField;
                name2Field[newField.Name] = newField;
            });

            fields = fieldsList.ToArray();
        }

        /// <summary>
        /// Create copy on update 
        /// </summary>
        protected internal BGMTMeta(BGMTMeta meta)
        {
            id = meta.Id;
            name = meta.Name;
            index = meta.Index;

            entityIds = meta.entityIds;
            entityId2Index = meta.entityId2Index;

            fields = meta.fields;
            id2Field = meta.id2Field;
            name2Field = meta.name2Field;
            if (fields != null)
                for (var i = 0; i < fields.Length; i++)
                    fields[i].Meta = this;
        }

        //================================================================================================
        //                                              Fields
        //================================================================================================
        public void ForEachField(Action<BGMTField> action)
        {
            for (var i = 0; i < fields.Length; i++) action(fields[i]);
        }

        public BGMTField GetField(int fieldIndex, bool errorIfNotFound = true)
        {
            try
            {
                return fields[fieldIndex];
            }
            catch (Exception e)
            {
                if (errorIfNotFound) throw new BGException("Can not find field with index $, error: $", fieldIndex, e.Message);
                return null;
            }
        }

        public BGMTField GetField(string fieldName, bool errorIfNotFound = true)
        {
            if (name2Field.TryGetValue(fieldName, out var result)) return result;
            if (errorIfNotFound) throw new BGException("Can not find field with name $", fieldName);
            return null;
        }

        public BGMTField GetField(BGId fieldId, bool errorIfNotFound = true)
        {
            if (id2Field.TryGetValue(fieldId, out var result)) return result;
            if (errorIfNotFound) throw new BGException("Can not find field with id $", fieldId);
            return null;
        }

        public BGMTField<T> GetField<T>(int fieldIndex, bool errorIfNotFound = true)
        {
            try
            {
                var result = fields[fieldIndex] as BGMTField<T>;
                if (result == null && errorIfNotFound) throw new BGException("Field '$' can not be cast to BGMTField<$>", fields[fieldIndex].Name, typeof(T).FullName);
                return result;
            }
            catch (Exception e)
            {
                if (errorIfNotFound) throw new BGException("Can not find field with index $, error: $", fieldIndex, e.Message);
                return null;
            }
        }

        public BGMTField<T> GetField<T>(string fieldName, bool errorIfNotFound = true)
        {
            if (name2Field.TryGetValue(fieldName, out var field))
            {
                var result = (BGMTField<T>)field;
                if (result == null && errorIfNotFound) throw new BGException("Field '$' can not be cast to BGMTField<$>", field.Name, typeof(T).FullName);
                return result;
            }

            if (errorIfNotFound) throw new BGException("Can not find field with name $", fieldName);
            return null;
        }

        public BGMTField<T> GetField<T>(BGId fieldId, bool errorIfNotFound = true)
        {
            if (id2Field.TryGetValue(fieldId, out var field))
            {
                var result = (BGMTField<T>)field;
                if (result == null && errorIfNotFound) throw new BGException("Field '$' can not be cast to BGMTField<$>", field.Name, typeof(T).FullName);
                return result;
            }

            if (errorIfNotFound) throw new BGException("Can not find field with id $", fieldId);
            return null;
        }


        //================================================================================================
        //                                              Entities
        //================================================================================================
        internal BGId GetEntityId(int entityIndex)
        {
            if (entityIndex < 0 || entityIds.Count <= entityIndex) return BGId.Empty;
            return entityIds[entityIndex];
        }

        public BGMTEntity? this[int entityIndex]
        {
            get
            {
                if (entityIndex < 0 || entityIds.Count <= entityIndex) return null;
                return new BGMTEntity(this, entityIndex);
            }
        }

        public BGMTEntity? this[BGId entityId]
        {
            get
            {
                if (!entityId2Index.TryGetValue(entityId, out var entityIndex)) return null;
                return new BGMTEntity(this, entityIndex);
            }
        }


        //================================================================================================
        //                                              Copy
        //================================================================================================

        //================================================================================================
        //                                              Query
        //================================================================================================
        /// <summary>
        /// Iterate all entities, which comply to the filter, sorted by sort delegate
        /// </summary>
        public void ForEachEntity(Action<BGMTEntity> action, Predicate<BGMTEntity> filter = null, Comparison<BGMTEntity> sort = null)
        {
            var count = CountEntities;
            if (count == 0) return;

            if (sort == null)
                for (var i = 0; i < count; i++)
                {
                    var entity = new BGMTEntity(this, i);
                    if (filter == null || filter(entity)) action(entity);
                }
            else
            {
                //what about pool of lists to remove GC?
                var list = new List<BGMTEntity>();
                for (var i = 0; i < count; i++)
                {
                    var entity = new BGMTEntity(this, i);
                    if (filter != null && !filter(entity)) continue;
                    list.Add(entity);
                }

                list.Sort(sort);
                var sortedCount = list.Count;
                for (var i = 0; i < sortedCount; i++) action(list[i]);
            }
        }

        /// <summary>
        /// Find the first entity, which comply to the filter   
        /// </summary>
        public BGMTEntity? FindEntity(Predicate<BGMTEntity> filter)
        {
            var count = CountEntities;
            if (count == 0) return null;

            for (var i = 0; i < count; i++)
            {
                var entity = new BGMTEntity(this, i);
                if (filter(entity)) return entity;
            }

            return null;
        }

        /// <summary>
        /// Find all entities, which comply to the filter. Pass in your own list as result to get rid of Garbage Collection   
        /// </summary>
        public List<BGMTEntity> FindEntities(Predicate<BGMTEntity> filter, List<BGMTEntity> result = null, Comparison<BGMTEntity> sort = null)
        {
            if (result == null) result = new List<BGMTEntity>();
            else result.Clear();

            var count = CountEntities;
            if (count == 0) return result;

            for (var i = 0; i < count; i++)
            {
                var entity = new BGMTEntity(this, i);
                if (filter == null || filter(entity)) result.Add(entity);
            }

            if (sort != null) result.Sort(sort);
            return result;
        }

        //================================================================================================
        //                                              Write
        //================================================================================================
        protected internal virtual void Set<T>(int fieldIndex, int entityIndex, T value)
        {
            ReadOnlyError();
        }

        protected internal virtual void Delete(int entityIndex)
        {
            ReadOnlyError();
        }

        protected internal virtual bool IsDeleted(int entityIndex)
        {
            ReadOnlyError();
            return false;
        }

        protected internal virtual void ApplyDelete()
        {
            ReadOnlyError();
        }


        protected internal virtual void Dispose()
        {
            id2Field = null;
            name2Field = null;
            fields = null;
            entityIds = null;
            entityId2Index = null;
        }

        /// <summary>
        /// Creates new entities, returns index of the first added entity
        /// </summary>
        public virtual int NewEntities(int numberOfEntities = 1)
        {
            ReadOnlyError();
            return -1;
        }

        private static void ReadOnlyError()
        {
            throw new BGException("You can not change data in read-only transaction. To change the data create write transaction.");
        }
    }
}