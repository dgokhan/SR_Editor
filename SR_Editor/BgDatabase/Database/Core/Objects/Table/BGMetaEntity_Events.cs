using System;

namespace BansheeGz.BGDatabase
{
    public partial class BGMetaEntity
    {

        private BGIdDictionary<BGEventsDelegatesHolder<BGEventArgsEntity>> entityId2DeleteEntityListener;
        private BGIdDictionary<BGEventsDelegatesHolder<BGEventArgsEntityUpdated>> entityId2UpdateEntityListener;

        public event EventHandler<BGEventArgsAnyEntity> AnyEntityAdded;
        public event EventHandler<BGEventArgsAnyEntityBeforeAdded> AnyEntityBeforeAdded;
        public event EventHandler<BGEventArgsAnyEntity> AnyEntityDeleted;
        public event EventHandler<BGEventArgsAnyEntity> AnyEntityBeforeDeleted;
        public event EventHandler<BGEventArgsAnyEntityUpdated> AnyEntityUpdated;

        public event EventHandler<BGEventArgsAnyEntityUpdated> AnyEntityBeforeUpdated;
        public event EventHandler<BGEventArgsEntitiesOrder> EntitiesOrderChanged;

        //fire meta changed event (is it used??)
        protected bool FieldChanged<T>(ref T oldValue, T newValue)
        {
            var oldValueNull = oldValue == null;
            var newValueNull = newValue == null;
            if (oldValueNull && newValueNull) return false;

            if (oldValueNull == newValueNull && oldValue.Equals(newValue)) return false;

            oldValue = newValue;
            Repo.Events.MetaWasChanged(this);
            return true;
        }

        /// <summary>
        /// add listener to row value changed event
        /// </summary>
        public void AddEntityUpdatedListener(BGId entityId, EventHandler<BGEventArgsEntityUpdated> handler)
        {
            entityId2UpdateEntityListener = entityId2UpdateEntityListener ?? new BGIdDictionary<BGEventsDelegatesHolder<BGEventArgsEntityUpdated>>();
            if (!entityId2UpdateEntityListener.TryGetValue(entityId, out var holder))
            {
                holder = new BGEventsDelegatesHolder<BGEventArgsEntityUpdated>();
                entityId2UpdateEntityListener.Add(entityId, holder);
            }

            holder.Handler += handler;
        }

        /// <summary>
        /// add listener to row deleted event
        /// </summary>
        public void AddEntityDeletedListener(BGId entityId, EventHandler<BGEventArgsEntity> handler)
        {
            entityId2DeleteEntityListener = entityId2DeleteEntityListener ?? new BGIdDictionary<BGEventsDelegatesHolder<BGEventArgsEntity>>();
            if (!entityId2DeleteEntityListener.TryGetValue(entityId, out var holder))
            {
                holder = new BGEventsDelegatesHolder<BGEventArgsEntity>();
                entityId2DeleteEntityListener.Add(entityId, holder);
            }

            holder.Handler += handler;
        }

        /// <summary>
        /// remove listener from row value changed event
        /// </summary>
        public void RemoveEntityUpdatedListener(BGId entityId, EventHandler<BGEventArgsEntityUpdated> handler)
        {
            if (entityId2UpdateEntityListener == null) return;
            if (!entityId2UpdateEntityListener.TryGetValue(entityId, out var holder)) return;
            holder.Handler -= handler;
        }

        /// <summary>
        /// remove listener to row deleted event
        /// </summary>
        public void RemoveEntityDeletedListener(BGId entityId, EventHandler<BGEventArgsEntity> handler)
        {
            if (entityId2DeleteEntityListener == null) return;
            if (!entityId2DeleteEntityListener.TryGetValue(entityId, out var holder)) return;
            holder.Handler -= handler;
        }


        internal void FireValueChanged(BGField field, BGEntity entity, bool nested = false)
        {
            if (!nested && events.ConsumeOnChange(Id)) return;
            if (entityId2UpdateEntityListener != null && entityId2UpdateEntityListener.TryGetValue(entity.Id, out var delegates))
            {
                if (delegates.Handler != null)
                    using (var args = BGEventArgsEntityUpdated.GetInstance(entity, field.Id))
                        delegates.Handler.Invoke(this, args);
            }

            if (AnyEntityUpdated != null)
                using (var args = BGEventArgsAnyEntityUpdated.GetInstance(entity, field.Id))
                    AnyEntityUpdated.Invoke(this, args);

            if (!nested) events.FireAnyChange();
        }

        internal void FireValueChanged<T>(BGField<T> field, BGEntity entity, T oldValue, T newValue)
        {
            if (events.ConsumeOnChange(Id)) return;
            if (entityId2UpdateEntityListener != null && entityId2UpdateEntityListener.TryGetValue(entity.Id, out var delegates))
            {
                if (delegates.Handler != null)
                    using (var args = BGEventArgsEntityUpdatedWithValue<T>.GetInstance(entity, field, oldValue, newValue))
                        delegates.Handler.Invoke(this, args);
            }

            if (AnyEntityUpdated != null)
                using (var args = BGEventArgsAnyEntityUpdatedWithValue<T>.GetInstance(entity, field, oldValue, newValue))
                    AnyEntityUpdated.Invoke(this, args);
        }

        internal void FireBeforeValueChanged<T>(BGField<T> field, BGEntity entity, T oldValue, T newValue)
        {
            if (events.ConsumeOnChange(Id)) return;

            if (AnyEntityBeforeUpdated != null)
                using (var args = BGEventArgsAnyEntityUpdatedWithValue<T>.GetInstance(entity, field, oldValue, newValue))
                    AnyEntityBeforeUpdated.Invoke(this, args);
        }

        internal void FireStoredValueChanged<T, TStoreType>(BGFieldCachedA<T, TStoreType> field, BGEntity entity, TStoreType oldValue, TStoreType newValue, bool nested = false)
        {
            if (!nested && events.ConsumeOnChange(Id)) return;
            if (entityId2UpdateEntityListener != null && entityId2UpdateEntityListener.TryGetValue(entity.Id, out var delegates))
            {
                if (delegates.Handler != null)
                    using (var args = BGEventArgsEntityUpdatedWithValue<T, TStoreType>.GetInstance(entity, field, oldValue, newValue))
                        delegates.Handler.Invoke(this, args);
            }

            if (AnyEntityUpdated != null)
                using (var args = BGEventArgsAnyEntityUpdatedWithValue<T, TStoreType>.GetInstance(entity, field, oldValue, newValue))
                    AnyEntityUpdated.Invoke(this, args);

            if (!nested) events.FireAnyChange();
        }

        internal void FireEntityBeforeDelete(BGEntity entity)
        {
            if (events.ConsumeOnEntityDelete(Id)) return;
            if (AnyEntityBeforeDeleted != null)
                using (var args = BGEventArgsAnyEntity.GetInstance(entity))
                    AnyEntityBeforeDeleted.Invoke(this, args);
        }

        internal void FireEntityDeleted(BGEntity entity)
        {
            if (events.ConsumeOnEntityDelete(Id)) return;
            if (entityId2DeleteEntityListener != null && entityId2DeleteEntityListener.TryGetValue(entity.Id, out var delegates))
            {
                if (delegates.Handler != null)
                    using (var args = BGEventArgsEntity.GetInstance(entity))
                        delegates.Handler.Invoke(this, args);
            }

            if (AnyEntityDeleted != null)
                using (var args = BGEventArgsAnyEntity.GetInstance(entity))
                    AnyEntityDeleted.Invoke(this, args);

            events.FireAnyChange();
        }

        internal void FireEntityAdded(BGEntity entity)
        {
            if (events.ConsumeOnEntityAdded(Id)) return;

            if (AnyEntityAdded != null)
                using (var args = BGEventArgsAnyEntity.GetInstance(entity))
                    AnyEntityAdded.Invoke(this, args);

            events.FireAnyChange();
        }

        internal void FireEntityBeforeAdded()
        {
            if (events.ConsumeOnEntityAdded(Id)) return;
            if (AnyEntityBeforeAdded != null)
                using (var args = BGEventArgsAnyEntityBeforeAdded.GetInstance(this))
                    AnyEntityBeforeAdded.Invoke(this, args);
        }

        internal void FireEntitiesOrderChanged()
        {
            if (events.ConsumeOnEntitiesOrderChanged(Id)) return;
            if (EntitiesOrderChanged != null)
                using (var args = BGEventArgsEntitiesOrder.GetInstance(this))
                    EntitiesOrderChanged.Invoke(this, args);

            events.FireAnyChange();
        }


        internal void TransferEventsTo(BGEventsHolder eventsHolder)
        {
            if (AnyEntityAdded != null)
            {
                eventsHolder.AddOnAnyEntityAddedListeners(Id, AnyEntityAdded.GetInvocationList());
                AnyEntityAdded = null;
            }

            if (AnyEntityBeforeAdded != null)
            {
                eventsHolder.AddOnAnyEntityBeforeAddedListeners(Id, AnyEntityBeforeAdded.GetInvocationList());
                AnyEntityBeforeAdded = null;
            }

            if (AnyEntityUpdated != null)
            {
                eventsHolder.AddOnAnyEntityUpdatedListeners(Id, AnyEntityUpdated.GetInvocationList());
                AnyEntityUpdated = null;
            }

            if (AnyEntityBeforeUpdated != null)
            {
                eventsHolder.AddOnAnyEntityBeforeUpdatedListeners(Id, AnyEntityBeforeUpdated.GetInvocationList());
                AnyEntityBeforeUpdated = null;
            }

            if (AnyEntityDeleted != null)
            {
                eventsHolder.AddOnAnyEntityDeletedListeners(Id, AnyEntityDeleted.GetInvocationList());
                AnyEntityDeleted = null;
            }

            if (EntitiesOrderChanged != null)
            {
                eventsHolder.AddOnEntitiesOrderChangedListeners(Id, EntitiesOrderChanged.GetInvocationList());
                EntitiesOrderChanged = null;
            }

            if (entityId2UpdateEntityListener?.Count > 0) eventsHolder.AddOnEntityUpdatedListeners(Id, entityId2UpdateEntityListener);
            if (entityId2DeleteEntityListener?.Count > 0) eventsHolder.AddOnEntityDeletedListeners(Id, entityId2DeleteEntityListener);
        }

        internal void TransferEventsFrom(BGEventsHolder eventsHolder)
        {
            var delegates = eventsHolder.GetOnAnyEntityAddedListeners(Id);
            if (delegates != null && delegates.Length > 0)
            {
                foreach (var @delegate in delegates) AnyEntityAdded += (EventHandler<BGEventArgsAnyEntity>)@delegate;
            }

            delegates = eventsHolder.GetOnAnyEntityBeforeAddedListeners(Id);
            if (delegates != null && delegates.Length > 0)
            {
                foreach (var @delegate in delegates) AnyEntityBeforeAdded += (EventHandler<BGEventArgsAnyEntityBeforeAdded>)@delegate;
            }

            delegates = eventsHolder.GetOnAnyEntityUpdatedListeners(Id);
            if (delegates != null && delegates.Length > 0)
            {
                foreach (var @delegate in delegates) AnyEntityUpdated += (EventHandler<BGEventArgsAnyEntityUpdated>)@delegate;
            }

            delegates = eventsHolder.GetOnAnyEntityBeforeUpdatedListeners(Id);
            if (delegates != null && delegates.Length > 0)
            {
                foreach (var @delegate in delegates) AnyEntityBeforeUpdated += (EventHandler<BGEventArgsAnyEntityUpdated>)@delegate;
            }

            delegates = eventsHolder.GetOnAnyEntityDeletedListeners(Id);
            if (delegates != null && delegates.Length > 0)
            {
                foreach (var @delegate in delegates) AnyEntityDeleted += (EventHandler<BGEventArgsAnyEntity>)@delegate;
            }

            delegates = eventsHolder.GetOnEntitiesOrderChangedListeners(Id);
            if (delegates != null && delegates.Length > 0)
            {
                foreach (var @delegate in delegates) EntitiesOrderChanged += (EventHandler<BGEventArgsEntitiesOrder>)@delegate;
            }

            entityId2UpdateEntityListener = eventsHolder.GetOnEntityUpdatedListeners(Id);

            entityId2DeleteEntityListener = eventsHolder.GetOnEntityDeletedListeners(Id);
        }
    }
}