/*
<copyright file="BGStoreMeta.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Storage for entities
    /// </summary>
    public partial class BGStoreMeta : BGArrayStore<BGEntity>
    {
        private BGIdDictionary<BGEntity> id2Entity;
        private Dictionary<string, BGEntity> name2Entity;

        /// <summary>
        /// get entity by id
        /// </summary>
        public BGEntity this[BGId id]
        {
            get
            {
                if (id2Entity == null) InitDictionaryById();
                if (!id2Entity.TryGetValue(id, out var result)) return null;
                return result;
            }
        }

        /// <summary>
        /// get entity by index
        /// </summary>
        public BGEntity this[int index]
        {
            get
            {
                if (index >= Count) throw new Exception("Index is out of bounds, greater or equal to maxIndex, " + index + ">=" + Count);
                return Items[index];
            }
        }

        /// <summary>
        /// get entity by name
        /// </summary>
        public BGEntity this[string name]
        {
            get
            {
                if (name2Entity == null) InitDictionaryByName();
                if (!name2Entity.TryGetValue(name, out var result)) return null;
                return result;
            }
        }

        private void InitDictionaryById()
        {
            //this is super costly operation 
            var size = Count;
            id2Entity = new BGIdDictionary<BGEntity>(size);

            for (var i = 0; i < size; i++)
            {
                var entity = Items[i];
                id2Entity[entity.Id] = entity;
            }
        }

        private void InitDictionaryByName()
        {
            //this is super costly operation 
            var size = Count;
            name2Entity = new Dictionary<string, BGEntity>(size);

            for (var i = size - 1; i >= 0; i--)
            {
                var entity = Items[i];
                if (string.IsNullOrEmpty(entity.Name)) continue;
                name2Entity[entity.Name] = entity;
            }
        }

        /// <summary>
        /// add entity
        /// </summary>
        public new void Add(BGEntity entity)
        {
            entity.Index = Count;
            base.Add(entity);

            id2Entity?.Add(entity.Id, entity);
            /*
             //who added this?? entity.Name throws exception if name2Entity!=null !!!
            if (name2Entity != null && !string.IsNullOrEmpty(entity.Name))
            {
                //this should never happen
//                Debug.Log("This should never happen");
                var entityName = entity.Name;
                if (!name2Entity.ContainsKey(entityName)) name2Entity[entityName] = entity;
            }
        */
        }

        /// <summary>
        /// remove entity
        /// </summary>
        public void Remove(BGEntity entity)
        {
            var index = entity.Index;
            if (index != -1)
            {
                id2Entity?.Remove(entity.Id);
                if (name2Entity != null)
                {
                    var entityName = entity.Name;
                    if (!string.IsNullOrEmpty(entityName))
                    {
                        var entityUnderName = BGUtil.Get(name2Entity, entityName);
                        if (entityUnderName != null && entityUnderName.Id == entity.Id)
                        {
                            var nextEntity = FindNext(index + 1, entityName);
                            if (nextEntity != null) name2Entity[entityName] = nextEntity;
                            else name2Entity.Remove(entityName);
                        }
                    }
                }

                //remove
                DeleteAt(index);
                var size = Count;
                for (var i = index; i < size; i++) Items[i].Index = i;
            }
        }

        /// <summary>
        /// clear all data about entities
        /// </summary>
        public new void Clear()
        {
            base.Clear();
            if (id2Entity != null)
            {
                id2Entity.Clear();
                id2Entity = null;
            }

            if (name2Entity != null)
            {
                name2Entity.Clear();
                name2Entity = null;
            }
        }

        /// <summary>
        /// copy entities data to list
        /// </summary>
        public List<BGEntity> ToList(List<BGEntity> result = null)
        {
            var size = Count;

            if (result == null) result = new List<BGEntity>(size);
            else result.Clear();

            for (var i = 0; i < size; i++) result.Add(Items[i]);
            return result;
        }

        /// <summary>
        /// if entity with id exists?
        /// </summary>
        public bool ContainsKey(BGId entityId)
        {
            if (id2Entity == null) InitDictionaryById();
            return id2Entity.ContainsKey(entityId);
        }

        /// <summary>
        /// On entity name change callback. This method should not be called apart from entity name changed event 
        /// </summary>
        internal void OnEntityNameChange(int entityIndex, string oldName, string newName)
        {
            if (name2Entity == null || string.IsNullOrEmpty(oldName) && string.IsNullOrEmpty(newName) || string.Equals(oldName, newName)) return;

            //this is pretty costly function !! Any idea how to improve?

            var entity = Items[entityIndex];
            var entityId = entity.Id;

            //============== Deal with old name
            if (!string.IsNullOrEmpty(oldName))
            {
                var entityUnderOldName = BGUtil.Get(name2Entity, oldName);
                if (entityUnderOldName != null && entityUnderOldName.Id == entityId)
                {
                    //entity was found and it was indeed the current entity 
                    var nextEntity = FindNext(entityIndex + 1, oldName);
                    if (nextEntity != null) name2Entity[oldName] = nextEntity;
                    else name2Entity.Remove(oldName);
                }
            }


            //============== Deal with new name
            if (!string.IsNullOrEmpty(newName))
            {
                var entityWithNewName = BGUtil.Get(name2Entity, newName);
                if (entityWithNewName == null || entityWithNewName.Id == entityId) name2Entity[newName] = entity;
                else if (entityIndex < entityWithNewName.Index) name2Entity[newName] = entity;
            }
        }

        private BGEntity FindNext(int startIndex, string name)
        {
            var size = Count;
            for (var i = startIndex; i < size; i++)
            {
                var e = Items[i];
                if (string.IsNullOrEmpty(e.Name) || !string.Equals(e.Name, name)) continue;
                return e;
            }

            return null;
        }

        internal void InvalidateNameCache()
        {
            name2Entity = null;
        }
    }
}