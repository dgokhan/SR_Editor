/*
<copyright file="BGRepoEvents.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Repo events. Turned off by default
    /// </summary>
    public partial class BGRepoEvents
    {
        private BGEventArgsBatch batchEvent;

        public event EventHandler<BGEventArgsAnyChange> OnAnyChange;
        public event EventHandler<BGEventArgsBatch> OnBatchUpdate;
        public event EventHandler<BGEventArgsMeta> OnRepoStructureChange;

        //events & handlers

        // private readonly BGIdDictionary<DelegatesHolder<BGEventArgsField>> id2FieldListener = new BGIdDictionary<DelegatesHolder<BGEventArgsField>>();

        /*
        private readonly BGIdDictionary<BGEventsDelegatesHolder<BGEventArgsEntity>> entityId2DeleteEntityListener = new BGIdDictionary<BGEventsDelegatesHolder<BGEventArgsEntity>>();
        private readonly BGIdDictionary<BGEventsDelegatesHolder<BGEventArgsEntityUpdated>> entityId2UpdateEntityListener = new BGIdDictionary<BGEventsDelegatesHolder<BGEventArgsEntityUpdated>>();

        private readonly BGIdDictionary<BGEventsDelegatesHolder<BGEventArgsAnyEntity>> metaId2AddAnyEntityListener = new BGIdDictionary<BGEventsDelegatesHolder<BGEventArgsAnyEntity>>();
        private readonly BGIdDictionary<BGEventsDelegatesHolder<BGEventArgsAnyEntity>> metaId2DeleteAnyEntityListener = new BGIdDictionary<BGEventsDelegatesHolder<BGEventArgsAnyEntity>>();
        private readonly BGIdDictionary<BGEventsDelegatesHolder<BGEventArgsAnyEntityUpdated>> metaId2UpdateAnyEntityListener = new BGIdDictionary<BGEventsDelegatesHolder<BGEventArgsAnyEntityUpdated>>();

        private readonly BGIdDictionary<BGEventsDelegatesHolder<BGEventArgsEntitiesOrder>> id2EntitiesOrderListener = new BGIdDictionary<BGEventsDelegatesHolder<BGEventArgsEntitiesOrder>>();
        */

        private bool on;

        /// <summary>
        /// turn on/off
        /// </summary>
        public bool On
        {
            get => on;
            set => on = value;
        }

        private readonly BGRepo repo;

        //================================================================================================
        //                                              Constructor
        //================================================================================================
        public BGRepoEvents(BGRepo repo) => this.repo = repo;

        //================================================================================================
        //                                              Any change
        //================================================================================================

        public void FireAnyChange()
        {
            if (!on) return;

            if (batchEvent != null) return;

            if (OnAnyChange == null) return;

            using (var e = BGEventArgsAnyChange.GetInstance(repo))
            {
                try
                {
                    OnAnyChange(this, e);
                }
                catch (Exception ex)
                {
                }
            }
        }

        //================================================================================================
        //                                              Addon
        //================================================================================================
        public void FireAddonChange()
        {
            FireAnyChange();
        }

        //================================================================================================
        //                                              Field
        //================================================================================================
        //this is fallback method- no optimization needed
        [Obsolete("Use BGField.ValueChanged event instead")]
        public void AddFieldListener(BGId fieldId, EventHandler<BGEventArgsField> handler)
        {
            var field = repo.GetField(fieldId);
            if (field == null) return;
            field.ValueChanged += handler;
            // AddListener(id2FieldListener, fieldId, handler);
        }

        //this is fallback method- no optimization needed
        [Obsolete("Use BGField.ValueChanged event instead")]
        public void RemoveFieldListener(BGId fieldId, EventHandler<BGEventArgsField> handler)
        {
            var field = repo.GetField(fieldId);
            if (field == null) return;
            field.ValueChanged -= handler;
            // RemoveListener(id2FieldListener, fieldId, handler);
        }

        //this is fallback method- no optimization needed
        [Obsolete("Use BGField.FireValueChanged event instead")]
        public void FieldWasChanged(BGId metaId, BGId fieldId, BGId entityId)
        {
            if (!on) return;

            if (batchEvent != null) batchEvent.AddMetaWithUpdatedEntities(metaId);
            else FireFieldChanged(metaId, fieldId, entityId);
        }

        //this is fallback method- no optimization needed
        [Obsolete("Use BGField.FireValueChanged event instead")]
        public void FireFieldChanged(BGId metaId, BGId fieldId, BGId entityId)
        {
            BGMetaEntity meta = null;
            BGEntity entity = null;

            /*
if (id2FieldListener.TryGetValue(fieldId, out var fieldListeners))
    if (EnsureEntity(metaId, entityId, ref meta, ref entity))
        Fire(fieldListeners, BGEventArgsField.GetInstance(entity, fieldId));
        */

            if (EnsureEntity(metaId, entityId, ref meta, ref entity))
            {
                var field = meta.GetField(fieldId, false);
                field?.FireValueChanged(entity);

                /*
                if (entityId2UpdateEntityListener.TryGetValue(entityId, out var entityListeners))
                    Fire(entityListeners, BGEventArgsEntityUpdated.GetInstance(entity, fieldId));

                if (metaId2UpdateAnyEntityListener.TryGetValue(metaId, out var entityAnyListeners))
                    Fire(entityAnyListeners, BGEventArgsAnyEntityUpdated.GetInstance(entity, fieldId));
            */
            }


            FireAnyChange();
        }

        //this is fallback method- no optimization needed
        private bool EnsureEntity(BGId metaId, BGId entityId, ref BGMetaEntity meta, ref BGEntity entity)
        {
            if (entity != null) return true;

            meta = repo[metaId];
            if (meta == null) return false;
            entity = meta[entityId];
            if (entity == null) return false;

            return true;
        }

        internal bool ConsumeOnChange(BGId metaId)
        {
            if (!on) return true;

            if (batchEvent != null)
            {
                batchEvent.AddMetaWithUpdatedEntities(metaId);
                return true;
            }

            return false;
        }

        //================================================================================================
        //                                              Entity
        //================================================================================================
        //this is fallback method- no optimization needed
        [Obsolete("Use BGMetaEntity.AddEntityUpdatedListener method instead")]
        public void AddEntityUpdatedListener(BGId entityId, EventHandler<BGEventArgsEntityUpdated> handler)
        {
            var entity = repo.GetEntity(entityId);
            entity?.Meta.AddEntityUpdatedListener(entityId, handler);
            // AddListener(entityId2UpdateEntityListener, entityId, handler);
        }

        //this is fallback method- no optimization needed
        [Obsolete("Use BGMetaEntity.AddEntityDeletedListener method instead")]
        public void AddEntityDeletedListener(BGId entityId, EventHandler<BGEventArgsEntity> handler)
        {
            var entity = repo.GetEntity(entityId);
            entity?.Meta.AddEntityDeletedListener(entityId, handler);
            // AddListener(entityId2DeleteEntityListener, entityId, handler);
        }

        //this is fallback method- no optimization needed
        [Obsolete("Use BGMetaEntity.RemoveEntityUpdatedListener method instead")]
        public void RemoveEntityUpdatedListener(BGId entityId, EventHandler<BGEventArgsEntityUpdated> handler)
        {
            var entity = repo.GetEntity(entityId);
            entity?.Meta.RemoveEntityUpdatedListener(entityId, handler);
            // RemoveListener(entityId2UpdateEntityListener, entityId, handler);
        }

        //this is fallback method- no optimization needed
        [Obsolete("Use BGMetaEntity.RemoveEntityDeletedListener method instead")]
        public void RemoveEntityDeletedListener(BGId entityId, EventHandler<BGEventArgsEntity> handler)
        {
            var entity = repo.GetEntity(entityId);
            entity?.Meta.RemoveEntityDeletedListener(entityId, handler);
            // RemoveListener(entityId2DeleteEntityListener, entityId, handler);
        }

        //================================================================================================
        //                                              Meta
        //================================================================================================

        //this is fallback method- no optimization needed
        [Obsolete("Use BGMetaEntity.EntitiesOrderChanged event instead")]
        public void AddEntitiesOrderListener(BGId metaId, EventHandler<BGEventArgsEntitiesOrder> handler)
        {
            var meta = repo.GetMeta(metaId);
            if (meta != null) meta.EntitiesOrderChanged += handler;

            // AddListener(id2EntitiesOrderListener, metaId, handler);
        }

        //this is fallback method- no optimization needed
        [Obsolete("Use BGMetaEntity.AnyEntityUpdated event instead")]
        public void AddAnyEntityUpdatedListener(BGId metaId, EventHandler<BGEventArgsAnyEntityUpdated> handler)
        {
            var meta = repo.GetMeta(metaId);
            if (meta != null) meta.AnyEntityUpdated += handler;

            // AddListener(metaId2UpdateAnyEntityListener, metaId, handler);
        }

        //this is fallback method- no optimization needed
        [Obsolete("Use BGMetaEntity.AnyEntityAdded event instead")]
        public void AddAnyEntityAddedListener(BGId metaId, EventHandler<BGEventArgsAnyEntity> handler)
        {
            var meta = repo.GetMeta(metaId);
            if (meta != null) meta.AnyEntityAdded += handler;

            // AddListener(metaId2AddAnyEntityListener, metaId, handler);
        }

        //this is fallback method- no optimization needed
        [Obsolete("Use BGMetaEntity.AnyEntityDeleted event instead")]
        public void AddAnyEntityDeletedListener(BGId metaId, EventHandler<BGEventArgsAnyEntity> handler)
        {
            var meta = repo.GetMeta(metaId);
            if (meta != null) meta.AnyEntityDeleted += handler;

            // AddListener(metaId2DeleteAnyEntityListener, metaId, handler);
        }

        //this is fallback method- no optimization needed
        [Obsolete("Use BGMetaEntity.EntitiesOrderChanged event instead")]
        public void RemoveEntitiesOrderListener(BGId metaId, EventHandler<BGEventArgsEntitiesOrder> handler)
        {
            var meta = repo.GetMeta(metaId);
            if (meta != null) meta.EntitiesOrderChanged -= handler;

            // RemoveListener(id2EntitiesOrderListener, metaId, handler);
        }

        //this is fallback method- no optimization needed
        [Obsolete("Use BGMetaEntity.AnyEntityUpdated event instead")]
        public void RemoveAnyEntityUpdatedListener(BGId metaId, EventHandler<BGEventArgsAnyEntityUpdated> handler)
        {
            var meta = repo.GetMeta(metaId);
            if (meta != null) meta.AnyEntityUpdated -= handler;

            // RemoveListener(metaId2UpdateAnyEntityListener, metaId, handler);
        }

        //this is fallback method- no optimization needed
        [Obsolete("Use BGMetaEntity.AnyEntityAdded event instead")]
        public void RemoveAnyEntityAddedListener(BGId metaId, EventHandler<BGEventArgsAnyEntity> handler)
        {
            var meta = repo.GetMeta(metaId);
            if (meta != null) meta.AnyEntityAdded -= handler;

            // RemoveListener(metaId2AddAnyEntityListener, metaId, handler);
        }

        //this is fallback method- no optimization needed
        [Obsolete("Use BGMetaEntity.AnyEntityDeleted event instead")]
        public void RemoveAnyEntityDeletedListener(BGId metaId, EventHandler<BGEventArgsAnyEntity> handler)
        {
            var meta = repo.GetMeta(metaId);
            if (meta != null) meta.AnyEntityDeleted -= handler;

            // RemoveListener(metaId2DeleteAnyEntityListener, metaId, handler);
        }

        //this is fallback method- no optimization needed
        [Obsolete("Use BGMetaEntity.FireEntityAdded method instead")]
        public void EntityWasAdded(BGEntity entity)
        {
            if (!on) return;

            if (batchEvent != null) batchEvent.AddMetaWithAddedEntities(entity.MetaId);
            else FireEntityAdded(entity);
        }

        //this is fallback method- no optimization needed
        [Obsolete("Use BGMetaEntity.FireEntityDeleted method instead")]
        public void EntityWasDeleted(BGEntity entity)
        {
            if (!on) return;

            if (batchEvent != null) batchEvent.AddMetaWithDeletedEntities(entity.MetaId);
            else FireEntityDeleted(entity);
        }

        //this is fallback method- no optimization needed
        [Obsolete("Use BGMetaEntity.FireEntityDeleted method instead")]
        public void FireEntityDeleted(BGEntity entity)
        {
            entity.Meta.FireEntityDeleted(entity);
            /*
            if (entityId2DeleteEntityListener.TryGetValue(entity.Id, out var deleteEntityListeners)) Fire(deleteEntityListeners, BGEventArgsEntity.GetInstance(entity));

            if (metaId2DeleteAnyEntityListener.TryGetValue(entity.MetaId, out var deleteAnyEntityListeners)) Fire(deleteAnyEntityListeners, BGEventArgsAnyEntity.GetInstance(entity));

            FireAnyChange();
            */
        }

        //this is fallback method- no optimization needed
        [Obsolete("Use BGMetaEntity.FireEntityAdded method instead")]
        public void FireEntityAdded(BGEntity entity)
        {
            entity.Meta.FireEntityAdded(entity);
            /*
            if (metaId2AddAnyEntityListener.TryGetValue(entity.MetaId, out var addEntityListeners)) Fire(addEntityListeners, BGEventArgsAnyEntity.GetInstance(entity));

            FireAnyChange();
        */
        }

        //================================================================================================
        //                                              Meta
        //================================================================================================
        public void AddRepoStructureListener(EventHandler<BGEventArgsMeta> handler)
        {
            OnRepoStructureChange += handler;
        }

        public void RemoveRepoStructureListener(BGId metaId, EventHandler<BGEventArgsMeta> handler)
        {
            OnRepoStructureChange -= handler;
        }

        public void MetaWasChanged(BGMetaEntity meta)
        {
            FireRepoEvent(meta, BGEventArgsMeta.OperationEnum.Update);
        }

        public void MetaWasAdded(BGMetaEntity meta)
        {
            FireRepoEvent(meta, BGEventArgsMeta.OperationEnum.Add);
        }

        public void MetaWasDeleted(BGMetaEntity meta)
        {
            FireRepoEvent(meta, BGEventArgsMeta.OperationEnum.Delete);
        }

        public void FireRepoEvent(BGMetaEntity meta, BGEventArgsMeta.OperationEnum operation)
        {
            if (!on) return;

            if (batchEvent != null) batchEvent.StructureChange = true;
            else
            {
                if (OnRepoStructureChange != null)
                    using (var args = BGEventArgsMeta.GetInstance(operation, meta))
                        Fire(OnRepoStructureChange, args);
                FireAnyChange();
            }
        }

        internal bool ConsumeOnEntityDelete(BGId metaId)
        {
            if (!on) return true;

            if (batchEvent != null)
            {
                batchEvent.AddMetaWithDeletedEntities(metaId);
                return true;
            }

            return false;
        }

        internal bool ConsumeOnEntityAdded(BGId metaId)
        {
            if (!on) return true;

            if (batchEvent != null)
            {
                batchEvent.AddMetaWithAddedEntities(metaId);
                return true;
            }

            return false;
        }

        internal bool ConsumeOnEntitiesOrderChanged(BGId metaId)
        {
            if (!on) return true;

            if (batchEvent != null)
            {
                batchEvent.AddMetaEntitiesOrderChanged(metaId);
                return true;
            }

            return false;
        }

        [Obsolete("Use BGMetaEntity.FireEntitiesOrderChanged event instead")]
        public void EntitiesOrderWasChanged(BGMetaEntity meta)
        {
            if (!on) return;

            if (batchEvent != null) batchEvent.AddMetaEntitiesOrderChanged(meta.Id);
            else
            {
                // if (id2EntitiesOrderListener.TryGetValue(meta.Id, out var listeners)) Fire(listeners, BGEventArgsEntitiesOrder.GetInstance(meta));
                meta.FireEntitiesOrderChanged();
                FireAnyChange();
            }
        }


        //================================================================================================
        //                                              Batch
        //================================================================================================

        public bool IsInBatch => batchEvent != null;

        public BGEventArgsBatch EnsureBatch()
        {
            if (batchEvent == null) batchEvent = BGEventArgsBatch.GetInstance(repo);
            return batchEvent;
        }
        public void ClearBatch()
        {
            batchEvent?.Dispose();
            batchEvent = null;
        }

        public void Batch(Action action)
        {
            var fireEvent = batchEvent == null && on;
            if (fireEvent) batchEvent = BGEventArgsBatch.GetInstance(repo);

            try
            {
                action();
            }
            finally
            {
                if (fireEvent) FireBatchEvent();
            }
        }

        public void FireBatchEvent()
        {
            if (batchEvent == null) return;
            try
            {
                if (OnBatchUpdate != null && !batchEvent.IsEmpty) OnBatchUpdate(this, batchEvent);
            }
            catch (Exception e)
            {
            }
            finally
            {
                ClearBatch();
                FireAnyChange();
            }
        }

        public void FireFullChange()
        {
            batchEvent = BGEventArgsBatch.GetInstance(repo);
            batchEvent.EverythingChanged = true;
            FireBatchEvent();
        }

        public void WithEventsDisabled(Action action)
        {
            var oldEnabled = On;
            try
            {
                On = false;
                action();
            }
            finally
            {
                On = oldEnabled;
            }
        }

        //================================================================================================
        //                                              Merge
        //================================================================================================

/*
        public void TransferTo(BGRepoEvents events)
        {
            foreach (var pair in id2FieldListener)
            {
                events.AddFieldListener(pair.Key, pair.Value);
            }
            foreach (var pair in entityId2EntityListener)
            {
                events.AddEntityListener(pair.Key, pair.Value);
            }
            foreach (var pair in metaId2AnyEntityListener)
            {
                events.AddMetaListener(pair.Key, pair.Value);
            }

            if (OnRepoStructureChange != null)
            {
                foreach (var @delegate in OnRepoStructureChange.GetInvocationList())
                {
                    events.OnRepoStructureChange += (EventHandler<BGEventArgsMeta>) @delegate;
                }
            }

            if (OnBatchUpdate != null)
            {
                var invocationList = OnBatchUpdate.GetInvocationList();
                foreach (var @delegate in invocationList)
                {
                    events.OnBatchUpdate += (EventHandler<BGEventArgsBatch>) @delegate;
                }
            }
            if (OnAnyChange != null)
            {
                var invocationList = OnAnyChange.GetInvocationList();
                foreach (var @delegate in invocationList)
                {
                    events.OnAnyChange += (EventHandler) @delegate;
                }
            }
        }
*/

        //================================================================================================
        //                                              Clear
        //================================================================================================

        //can we remove it?
        public void Clear()
        {
            // id2FieldListener.Clear();

            /*
            entityId2UpdateEntityListener.Clear();
            entityId2DeleteEntityListener.Clear();

            metaId2AddAnyEntityListener.Clear();
            metaId2UpdateAnyEntityListener.Clear();
            metaId2DeleteAnyEntityListener.Clear();

            id2EntitiesOrderListener.Clear();
            */

            OnRepoStructureChange = null;
            OnBatchUpdate = null;
            OnAnyChange = null;
        }

        //================================================================================================
        //                                              Private
        //================================================================================================

        private void Fire<T>(BGEventsDelegatesHolder<T> holder, T e) where T : BGEventArgsA
        {
            var handler = holder?.Handler;
            Fire(handler, e);
        }

        private void Fire<T>(EventHandler<T> handler, T e) where T : BGEventArgsA
        {
            if (handler == null) return;

            try
            {
                handler(this, e);
            }
            catch (Exception ex)
            {
            }
            /*
            finally
            {
                e.Dispose();
            }
        */
        }

        private static void RemoveListener<T>(BGIdDictionary<BGEventsDelegatesHolder<T>> dictionary, BGId id, EventHandler<T> handler) where T : EventArgs
        {
            if (!dictionary.TryGetValue(id, out var holder)) return;
            holder.Handler -= handler;
            //creating and deleting DelegatesHolder constantly is not the best idea- better leave an empty object 
            // if (holder.Handler == null) dictionary.Remove(id);
        }

        private static void AddListener<T>(BGIdDictionary<BGEventsDelegatesHolder<T>> dictionary, BGId id, EventHandler<T> handler) where T : EventArgs
        {
            if (dictionary.TryGetValue(id, out var holder)) holder.Handler += handler;
            else
            {
                holder = new BGEventsDelegatesHolder<T>();
                holder.Handler += handler;
                dictionary.Add(id, holder);
            }
        }

        //================================================================================================
        //                                              view
        //================================================================================================
        public void ViewWasAdded(BGMetaView view) => FireViewEvent(view, BGEventArgsMeta.OperationEnum.Add);

        public void ViewWasDeleted(BGMetaView view) => FireViewEvent(view, BGEventArgsMeta.OperationEnum.Delete);

        public void ViewWasChanged(BGMetaView view) => FireViewEvent(view, BGEventArgsMeta.OperationEnum.Update);

        public void FireViewEvent(BGMetaView view, BGEventArgsMeta.OperationEnum operation)
        {
            if (!on) return;

            if (batchEvent != null) batchEvent.StructureChange = true;
            else
            {
                if (OnRepoStructureChange != null)
                    using (var args = BGEventArgsMeta.GetInstance(operation, view))
                        Fire(OnRepoStructureChange, args);

                FireAnyChange();
            }
        }
    }
}