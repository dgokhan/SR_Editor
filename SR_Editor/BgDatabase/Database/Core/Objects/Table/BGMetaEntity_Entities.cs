using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    public partial class BGMetaEntity
    {
        //rows store
        protected readonly BGStoreMeta Store = new BGStoreMeta();

        //do not remove!
        internal BGId NewEntityId
        {
            get
            {
                var entityId = BGId.NewId;
                while (HasEntity(entityId)) entityId = BGId.NewId;
                return entityId;
            }
        }

        /// <summary>
        /// get the very first entity
        /// </summary>
        public BGEntity EntityFirst
        {
            get
            {
                if (LazyLoader != null) LazyLoad();
                return Store.Count == 0 ? null : Store[0];
            }
        }

        /// <summary>
        /// get the entity by it's id
        /// </summary>
        public BGEntity this[BGId entityId]
        {
            get
            {
                if (LazyLoader != null) LazyLoad();
                return Store[entityId];
            }
        }

        /// <summary>
        /// get the entity by it's index
        /// </summary>
        public BGEntity this[int index]
        {
            get
            {
                if (LazyLoader != null) LazyLoad();
                return Store[index];
            }
        }

        /// <summary>
        /// get the entity by it's name
        /// </summary>
        public BGEntity this[string entityName]
        {
            get
            {
                if (LazyLoader != null) LazyLoad();
                return Store[entityName];
            }
        }

        /// <summary>
        /// number of entities
        /// </summary>
        public int CountEntities
        {
            get
            {
                if (LazyLoader != null) LazyLoad();
                return Store.Count;
            }
        }

        //ensure required capacity for rows storage
        internal int EntitiesCapacity
        {
            set => Store.MinCapacity = value;
        }

        /// <summary>
        /// Batch delete for multiple entities.
        /// </summary>
        public void DeleteEntities(ICollection<BGEntity> entities)
        {
            if (LazyLoader != null) LazyLoad();
            if (entities == null || entities.Count == 0) return;


            foreach (var entity in entities)
            {
                if (entity == null) throw new BGException("One of the entities, submitted for removal, is null");
                if (entity.Meta != this) throw new BGException("One of the entities, submitted for removal, does not belong to this meta");
                if (entity.IsDeleted) throw new BGException("One of the entities, submitted for removal, already deleted");
            }

            InvalidateNameCache();
            var toRemove = new List<BGEntity>(entities);
            //in desc order
            toRemove.Sort((e1, e2) => e2.Index.CompareTo(e1.Index));

            var allIds = new HashSet<BGId>();
            Exception ex = null;
            var entitiesCount = entities.Count;
            for (var i = 0; i < entitiesCount; i++)
            {
                var entity = toRemove[i];
                //duplicates handling
                if (!allIds.Add(entity.Id)) continue;
                Unregister(entity, false);
                entity.Unload();

                BGUtil.Catch(ref ex, () =>
                {
                    FireEntityDeleted(entity);
                });

                entity.Index = -1;
            }

            var reverseRelations = RelationsInbound;
            if (reverseRelations.Count > 0)
                foreach (var reverseRelation in reverseRelations)
                {
//                    var objects = reverseRelation.GetRelatedIn(allIds);
//                    if (BGUtil.IsEmpty(objects)) continue;
                    var relation = reverseRelation;
                    BGUtil.Catch(ref ex, () =>
                    {
                        relation.ClearToValue(allIds);
                    });
                }

            if (ex != null) throw ex;
        }

        /// <summary>
        /// Get entity by its id   
        /// </summary>
        public BGEntity GetEntity(BGId entityId)
        {
            if (LazyLoader != null) LazyLoad();
            return Store[entityId];
        }

        /// <summary>
        /// Get entity by its name   
        /// </summary>
        public BGEntity GetEntity(string entityName)
        {
            if (LazyLoader != null) LazyLoad();
            return Store[entityName];
        }

        /// <summary>
        /// Get entity by index   
        /// </summary>
        public BGEntity GetEntity(int index)
        {
            if (LazyLoader != null) LazyLoad();
            return Store[index];
        }


        /// <summary>
        /// Iterate all entities, which comply to the filter and apply the action to each of them   
        /// </summary>
        public void ForEachEntity(Action<BGEntity> action, Predicate<BGEntity> filter = null, Comparison<BGEntity> sort = null)
        {
            if (LazyLoader != null) LazyLoad();
            if (sort == null)
            {
                var storeCount = Store.Count;
                if (filter == null)
                {
                    //do not change to foreach
                    for (var i = 0; i < storeCount; i++)
                    {
                        var entity = Store[i];
                        action(entity);
                    }
                }
                else
                {
                    //do not change to foreach
                    for (var i = 0; i < storeCount; i++)
                    {
                        var entity = Store[i];
                        if (!filter(entity)) continue;
                        action(entity);
                    }
                }
            }
            else
            {
                var pool = BGListPoolDefault<BGEntity>.I;
                var list = pool.Get();
                try
                {
                    if (filter == null) Store.ToList(list);
                    else
                    {
                        var storeCount = Store.Count;
                        //do not change to foreach
                        for (var i = 0; i < storeCount; i++)
                        {
                            var entity = Store[i];
                            if (!filter(entity)) continue;
                            list.Add(entity);
                        }
                    }

                    list.Sort(sort);
                    var count = list.Count;
                    //do not change to foreach
                    for (var i = 0; i < count; i++) action(list[i]);
                }
                finally
                {
                    pool.Return(list);
                }
            }
        }

        /// <summary>
        /// Find the first entity, which comply to the filter   
        /// </summary>
        public BGEntity FindEntity(Predicate<BGEntity> filter)
        {
            if (LazyLoader != null) LazyLoad();
            if (filter == null) return CountEntities == 0 ? null : GetEntity(0);
            var storeCount = Store.Count;
            for (var i = 0; i < storeCount; i++)
            {
                var entity = Store[i];
                if (filter(entity)) return entity;
            }

            return null;
        }

        /// <summary>
        /// Find all entities, which comply to the filter. Pass in your own list as result to get rid of Garbage Collection   
        /// </summary>
        public List<BGEntity> FindEntities(Predicate<BGEntity> filter, List<BGEntity> result = null, Comparison<BGEntity> sort = null)
        {
            if (LazyLoader != null) LazyLoad();
            if (result == null) result = new List<BGEntity>();
            else result.Clear();

            var storeCount = Store.Count;
            if (filter == null) Store.ToList(result);
            else
            {
                for (var i = 0; i < storeCount; i++)
                {
                    var entity = Store[i];
                    if (filter(entity)) result.Add(entity);
                }
            }

            if (sort != null) result.Sort(sort);
            return result;
        }

        /// <summary>
        /// Do not call this method   
        /// </summary>
        public virtual void OnEntityCreate(BGEntity entity)
        {
            ForEachField(field => field.OnEntityCreate(entity));
            try
            {
                if (Controller is BGControllerOnEntityAdd onAddController) onAddController.OnEntityAdd(this, entity);
            }
            catch (Exception e)
            {
            }
        }

        /// <summary>
        /// Do not call this method   
        /// </summary>
        // no need to check lazy load here
        internal void OnEntityNameChange(int entityIndex, string oldName, string newName) => Store.OnEntityNameChange(entityIndex, oldName, newName);

        /// <summary>
        /// Do not call this method   
        /// </summary>
        internal void InvalidateNameCache()
        {
            if (LazyLoader != null) LazyLoad();
            Store.InvalidateNameCache();
        }


        /// <summary>
        /// Does this meta have an entity with specified id?   
        /// </summary>
        public bool HasEntity(BGId entityId)
        {
            if (LazyLoader != null) LazyLoad();
            return Store.ContainsKey(entityId);
        }

        /// <summary>
        /// Fill the list with all entities   
        /// </summary>
        public List<BGEntity> EntitiesToList(List<BGEntity> result = null)
        {
            if (LazyLoader != null) LazyLoad();
            return Store.ToList(result);
        }

        /// <summary>
        /// Find first entity index, which comply to specified filter   
        /// </summary>
        public int FindEntityIndex(Predicate<BGEntity> filter)
        {
            if (LazyLoader != null) LazyLoad();
            for (var i = 0; i < Store.Count; i++)
            {
                var entity = Store[i];
                if (filter(entity)) return i;
            }

            return -1;
        }

        /// <summary>
        /// Find entity index by its id   
        /// </summary>
        public int FindEntityIndex(BGId id)
        {
            var entity = GetEntity(id);
            return entity?.Index ?? -1;
        }

        /// <summary>
        /// Find entity id by its index   
        /// </summary>
        public BGId FindEntityId(int index)
        {
            var entity = GetEntity(index);
            return entity?.Id ?? BGId.Empty;
        }

        /// <summary>
        /// Remove all entities   
        /// </summary>
        public void ClearEntities()
        {
            if (LazyLoader != null) LazyLoad();
            //mark entities deleted
            var storeCount = Store.Count;
            for (var i = storeCount - 1; i >= 0; i--) Store[i].Unload();

            ForEachField(field => field.ClearValues());
            Store.Clear();
        }

        //register (add) row
        internal void Register(BGEntity entity)
        {
            if (LazyLoader != null) LazyLoad();
            Store.Add(entity);
            for (var i = 0; i < fields.Count; i++) fields[i].OnEntityAdd(entity);
        }

        //unregister (remove) row
        internal void Unregister(BGEntity entity, bool clearRelations = true)
        {
            if (LazyLoader != null) LazyLoad();
            Store.Remove(entity);

            Exception ex = null;

            for (var i = 0; i < fields.Count; i++)
            {
                var field = fields[i];
                BGUtil.Catch(ref ex, () => field.OnEntityDelete(entity));
            }

            if (clearRelations)
            {
                var reverseRelations = RelationsInbound;
                if (reverseRelations.Count > 0)
                    foreach (var reverseRelation in reverseRelations)
                    {
                        var relation = reverseRelation;
                        /*
                        var objects = relation.GetRelatedIn(Id);
                        if (BGUtil.IsEmpty(objects)) continue;
                        */

                        BGUtil.Catch(ref ex, () => relation.ClearToValue(entity.Id));
                    }
            }

            if (ex != null) throw ex;
        }

        /// <summary>
        /// Swap physical order for 2 entities   
        /// </summary>
        public void SwapEntities(int entityIndex1, int entityIndex2)
        {
            if (entityIndex1 == entityIndex2) return;

            var count = CountEntities;
            if (entityIndex1 < 0 || entityIndex2 < 0 || entityIndex1 >= count || entityIndex2 >= count) throw new BGException("Invalid entity indexes for swap: $ and $ ", entityIndex1, entityIndex2);

            Store[entityIndex1].Index = entityIndex2;
            Store[entityIndex2].Index = entityIndex1;
            Store.Swap(entityIndex1, entityIndex2);
            for (var i = 0; i < fields.Count; i++) fields[i].Swap(entityIndex1, entityIndex2);

            FireEntitiesOrderChanged();
        }

        /// <summary>
        /// Change physical order by moving "numberOfEntities" entities from fromIndex to toIndex 
        /// </summary>
        public void MoveEntities(int fromIndex, int toIndex, int numberOfEntities)
        {
            //check input parameters
            if (fromIndex == toIndex) return;
            var count = CountEntities;
            if (numberOfEntities <= 0) throw new BGException("Invalid numberOfEntities: $. It should be more than 0", numberOfEntities);
            if (fromIndex < 0) throw new BGException("Invalid fromIndex: $. It should be equal or more than 0", fromIndex);
            if (fromIndex >= count) throw new BGException("Invalid fromIndex: $. It should be less than number of entities $", fromIndex, count);
            if (fromIndex + numberOfEntities > count)
                throw new BGException("Invalid fromIndex: $. fromIndex + numberOfEntities($) should not exceed the number of entities $", fromIndex, numberOfEntities, count);
            if (toIndex < 0) throw new BGException("Invalid toIndex: $. It should be equal or more than 0", toIndex);
            if (toIndex >= count) throw new BGException("Invalid toIndex: $. It should be less than number of entities $", toIndex, count);
            if (toIndex + numberOfEntities > count)
                throw new BGException("Invalid toIndex: $. toIndex + numberOfEntities($) should not exceed the number of entities $", toIndex, numberOfEntities, count);

            //move physical values
            Store.MoveValues(fromIndex, toIndex, numberOfEntities);

            //reassign indexes
            for (var i = Math.Min(fromIndex, toIndex); i < count; i++) Store[i].Index = i;

            //reassign fields values
            var fieldsCount = fields.Count;
            for (var i = 0; i < fieldsCount; i++) fields[i].MoveEntitiesValues(fromIndex, toIndex, numberOfEntities);

            //fire event
            Repo.Events.FireAnyChange();
        }

        //================================================================================================
        //                                              New entity
        //================================================================================================

        private BGEntity.EntityFactory factory;
        private bool factoryRetrieved;

        /// <summary>
        /// Create a new entity   
        /// </summary>
        public BGEntity NewEntity()
        {
            FireEntityBeforeAdded();
            if (!factoryRetrieved) InitFactory();
            var result = factory == null ? new BGEntity(this) : factory.NewEntity(this);
            FireEntityAdded(result);
            return result;
        }

        /// <summary>
        /// Create a new entity with id   
        /// </summary>
        public BGEntity NewEntity(BGId entityId)
        {
            FireEntityBeforeAdded();
            if (!factoryRetrieved) InitFactory();
            var result = factory == null ? new BGEntity(this, entityId) : factory.NewEntity(this, entityId);
            FireEntityAdded(result);
            return result;
        }

        /// <summary>
        /// Create a new entity using information from context   
        /// </summary>
        public BGEntity NewEntity(NewEntityContext context)
        {
            FireEntityBeforeAdded();
            if (context == null) return NewEntity();

            if (!factoryRetrieved) InitFactory();
            BGEntity result;
            if (context.EntityId != null) result = factory == null ? new BGEntity(this, context.EntityId.Value) : factory.NewEntity(this, context.EntityId.Value);
            else result = factory == null ? new BGEntity(this) : factory.NewEntity(this);

            if (context.Callback != null)
            {
                var oldOn = events.On;
                events.On = false;
                try
                {
                    context.Callback(result);
                }
                catch (Exception e)
                {
                }
                finally
                {
                    events.On = oldOn;
                }
            }

            FireEntityAdded(result);
            return result;
        }

        //try to instantiate code generated entities factory
        private void InitFactory()
        {
            factoryRetrieved = true;
            var codeGenAddon = Repo.Addons.Get<BGAddonCodeGen>();
            if (codeGenAddon == null) return;

            var factoryTypeName = codeGenAddon.GetEntityFactoryTypeWithPackage(Name);
            var factoryType = BGUtil.GetType(factoryTypeName, true);
            if (factoryType == null) return;
            factory = Activator.CreateInstance(factoryType) as BGEntity.EntityFactory;
        }

        /// <summary>
        /// Context to use while creating a row
        /// </summary>
        public class NewEntityContext
        {
            /// <summary>
            /// Entity ID if present
            /// </summary>
            public readonly BGId? EntityId;

            /// <summary>
            /// Callback to use before new entity event is fired. Can be used for initializing row's fields
            /// </summary>
            public readonly Action<BGEntity> Callback;

            public NewEntityContext(Action<BGEntity> callback)
            {
                Callback = callback;
            }

            public NewEntityContext(BGId entityId, Action<BGEntity> callback)
            {
                EntityId = entityId;
                Callback = callback;
            }
        }
    }
}