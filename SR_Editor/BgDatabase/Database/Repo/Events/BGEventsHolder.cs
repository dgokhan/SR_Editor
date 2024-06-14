/*
<copyright file="BGEventsHolder.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Data container for all event listeners.
    /// Can be used to transfer listeners between different databases
    /// </summary>
    public class BGEventsHolder
    {
        private readonly BGIdDictionary<Delegate[]> fieldListeners = new BGIdDictionary<Delegate[]>();
        private readonly BGIdDictionary<Delegate[]> fieldBeforeListeners = new BGIdDictionary<Delegate[]>();

        private readonly BGIdDictionary<BGIdDictionary<BGEventsDelegatesHolder<BGEventArgsEntity>>> entityId2DeleteEntityListener 
            = new BGIdDictionary<BGIdDictionary<BGEventsDelegatesHolder<BGEventArgsEntity>>>();
        
        private readonly BGIdDictionary<BGIdDictionary<BGEventsDelegatesHolder<BGEventArgsEntityUpdated>>> entityId2UpdateEntityListener 
            = new BGIdDictionary<BGIdDictionary<BGEventsDelegatesHolder<BGEventArgsEntityUpdated>>>();

        private readonly BGIdDictionary<Delegate[]> metaId2AddAnyEntityListener = new BGIdDictionary<Delegate[]>();
        private readonly BGIdDictionary<Delegate[]> metaId2BeforeAddAnyEntityListener = new BGIdDictionary<Delegate[]>();
        private readonly BGIdDictionary<Delegate[]> metaId2DeleteAnyEntityListener = new BGIdDictionary<Delegate[]>();
        private readonly BGIdDictionary<Delegate[]> metaId2UpdateAnyEntityListener = new BGIdDictionary<Delegate[]>();
        private readonly BGIdDictionary<Delegate[]> metaId2BeforeUpdateAnyEntityListener = new BGIdDictionary<Delegate[]>();
        private readonly BGIdDictionary<Delegate[]> metaId2EntitiesOrderListener = new BGIdDictionary<Delegate[]>();

        internal void AddOnFieldValueChangedListeners(BGId fieldId, Delegate[] handlers) => fieldListeners.Add(fieldId, handlers);
        internal void AddOnBeforeFieldValueChangedListeners(BGId fieldId, Delegate[] handlers) => fieldBeforeListeners.Add(fieldId, handlers);

        public void AddOnAnyEntityAddedListeners(BGId metaId, Delegate[] handlers) => metaId2AddAnyEntityListener.Add(metaId, handlers);
        public void AddOnAnyEntityBeforeAddedListeners(BGId metaId, Delegate[] handlers) => metaId2BeforeAddAnyEntityListener.Add(metaId, handlers);

        public void AddOnAnyEntityUpdatedListeners(BGId metaId, Delegate[] handlers) => metaId2UpdateAnyEntityListener.Add(metaId, handlers);
        public void AddOnAnyEntityBeforeUpdatedListeners(BGId metaId, Delegate[] handlers) => metaId2BeforeUpdateAnyEntityListener.Add(metaId, handlers);

        public void AddOnAnyEntityDeletedListeners(BGId metaId, Delegate[] handlers) => metaId2DeleteAnyEntityListener.Add(metaId, handlers);

        public void AddOnEntitiesOrderChangedListeners(BGId metaId, Delegate[] handlers) => metaId2EntitiesOrderListener.Add(metaId, handlers);

        public void AddOnEntityUpdatedListeners(BGId entityId, BGIdDictionary<BGEventsDelegatesHolder<BGEventArgsEntityUpdated>> handlers) => entityId2UpdateEntityListener.Add(entityId, handlers);

        public void AddOnEntityDeletedListeners(BGId entityId, BGIdDictionary<BGEventsDelegatesHolder<BGEventArgsEntity>> handlers) => entityId2DeleteEntityListener.Add(entityId, handlers);

        public void TransferEventsFrom(BGMetaEntity meta) => meta.TransferEventsTo(this);

        public void TransferEventsFrom(BGField field) => field.TransferEventsTo(this);

        public void TransferEventsTo(BGMetaEntity meta) => meta.TransferEventsFrom(this);

        public void TransferEventsTo(BGField field) => field.TransferEventsFrom(this);

        public Delegate[] GetOnFieldValueChangedListeners(BGId fieldId) => !fieldListeners.TryGetValue(fieldId, out var result) ? null : result;
        public Delegate[] GetOnFieldBeforeValueChangedListeners(BGId fieldId) => !fieldBeforeListeners.TryGetValue(fieldId, out var result) ? null : result;

        public Delegate[] GetOnAnyEntityAddedListeners(BGId metaId) => metaId2AddAnyEntityListener.TryGetValue(metaId, out var handlers) ? handlers : null;
        public Delegate[] GetOnAnyEntityBeforeAddedListeners(BGId metaId) => metaId2BeforeAddAnyEntityListener.TryGetValue(metaId, out var handlers) ? handlers : null;

        public Delegate[] GetOnAnyEntityUpdatedListeners(BGId metaId) => metaId2UpdateAnyEntityListener.TryGetValue(metaId, out var handlers) ? handlers : null;
        public Delegate[] GetOnAnyEntityBeforeUpdatedListeners(BGId metaId) => metaId2BeforeUpdateAnyEntityListener.TryGetValue(metaId, out var handlers) ? handlers : null;

        public Delegate[] GetOnAnyEntityDeletedListeners(BGId metaId) => metaId2DeleteAnyEntityListener.TryGetValue(metaId, out var handlers) ? handlers : null;

        public Delegate[] GetOnEntitiesOrderChangedListeners(BGId metaId) => metaId2EntitiesOrderListener.TryGetValue(metaId, out var handlers) ? handlers : null;

        public BGIdDictionary<BGEventsDelegatesHolder<BGEventArgsEntityUpdated>> GetOnEntityUpdatedListeners(BGId metaId)
        {
            if (entityId2UpdateEntityListener.Count == 0) return null;
            if (!entityId2UpdateEntityListener.TryGetValue(metaId, out var holder)) return null;
            return holder;
        }
        public BGIdDictionary<BGEventsDelegatesHolder<BGEventArgsEntity>> GetOnEntityDeletedListeners(BGId metaId)
        {
            if (entityId2DeleteEntityListener.Count == 0) return null;
            if (!entityId2DeleteEntityListener.TryGetValue(metaId, out var holder)) return null;
            return holder;
        }

        private class BGListenersList
        {
            public readonly BGId Id;
            public readonly Delegate[] delegates;

            public BGListenersList(BGId Id, Delegate[] delegates)
            {
                this.Id = Id;
                this.delegates = delegates;
            }
        }

    }
}