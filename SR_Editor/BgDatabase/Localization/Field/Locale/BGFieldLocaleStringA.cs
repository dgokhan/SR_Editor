/*
<copyright file="BGFieldLocaleStringA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    public abstract class BGFieldLocaleStringA : BGFieldLocaleA<string>, BGStorableString
    {
        private BGStoreField<string> Store;

        public BGFieldLocaleStringA(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        protected BGFieldLocaleStringA(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        protected void EnsureStoreOnWrite()
        {
            if (Store != null) return;
            EnsureStore();
            Load();
        }

        protected bool EnsureStoreOnRead()
        {
            if (Store != null) return true;
            if (!IsMainRepo) return false;
            EnsureStore();
            Load();
            return true;
        }

        public override void EnsureStore()
        {
            if (Store != null) return;
            Store = new BGStoreField<string> { MinSize = Meta.CountEntities };
        }

        public override void DestroyStore()
        {
            if (Store == null) return;
            Store.Clear();
            Store = null;
        }

        //for main repo fields only
        private void Load()
        {
            if (!IsMainRepo) return;
            try
            {
                BGLocalizationReposCache.Load(this);
            }
            catch
            {
                Store = null;
                throw;
            }
            // Meta.Repo.Events.FireAnyChange();
        }

        //================================================================================================
        //                                              COPY PASTE (BGFieldCached)
        //================================================================================================
        /// <inheritdoc />
        public override string this[BGId entityId]
        {
            get
            {
                //this is slow version with +1 dict lookup to find entity index
                var index = Meta.FindEntityIndex(entityId);
                if (index == -1)
                {
                    Debug.LogException(new BGException("Can not find entity with specified id=$, meta=$, field=$. Default value returned. ", entityId, MetaName, Name));
                    return null;
                }

                return this[index];
            }
            set
            {
                //this is slow version with +1 dict lookup to find entity index
                var index = Meta.FindEntityIndex(entityId);
                if (index == -1)
                {
                    Debug.LogException(new BGException("Can not find entity with specified id=$, meta=$, field=$. Setting value is skipped. ", entityId, MetaName, Name));
                    return;
                }

                this[index] = value;
            }
        }

        public override string this[int entityIndex]
        {
            get
            {
                if (!EnsureStoreOnRead()) return null;
                return Store[entityIndex];
            }
            set
            {
                EnsureStoreOnWrite();
                if (events.On)
                {
                    var oldValue = this[entityIndex];
                    if (string.Equals(oldValue, value)) return;
                    var entity = Meta[entityIndex];
                    FireBeforeValueChanged(entity, oldValue, value);
                    Store[entityIndex] = value;
                    FireValueChanged(entity, oldValue, value);
                }
                else Store[entityIndex] = value;
            }
        }


        /// <inheritdoc />
        public override void ForEachValue(Action<int> action)
        {
            if (!EnsureStoreOnRead()) return;

            // Store.ForEachKey(action);

            //this is copy/paste from Store.ForEachValue to get rid of empty strings
            var count = Store.Count;
            for (var i = 0; i < count; i++)
            {
                var item = Store[i];
                if (string.IsNullOrEmpty(item)) continue;
                action(i);
            }
        }


        public override void OnDelete()
        {
            ClearValues();
        }

        public override void ClearValues()
        {
            Store?.Clear();
        }

        /// <inheritdoc />
        public override void ClearValue(int entityIndex)
        {
            EnsureStoreOnWrite();
            if (events.On)
            {
                var oldValue = Store[entityIndex];
                if (oldValue != null) FireBeforeValueChanged(Meta[entityIndex], oldValue, null);
                ClearValueNoEvent(entityIndex);
                if (oldValue != null) FireValueChanged(Meta[entityIndex], oldValue, null);
            }
            else ClearValueNoEvent(entityIndex);
        }

        //clear value with no event
        protected void ClearValueNoEvent(int entityIndex)
        {
            EnsureStoreOnWrite();
            Store[entityIndex] = null;
        }

        /// <inheritdoc/>
        [Obsolete("Use CloneTo(BGCloneContextField context) instead")]
        public override BGField CloneTo(BGMetaEntity meta, bool copyValues) => CloneTo(new BGCloneContextField(meta, copyValues));

        /// <inheritdoc/>
        public override BGField CloneTo(BGCloneContextField context)
        {
            var clone = base.CloneTo(context);
            context.OnAfterFieldCreated?.Invoke(clone);
            if (context.copyValues)
                Meta.ForEachEntity(entity =>
                {
                    clone.CopyValue(this, entity.Id, entity.Index, entity.Id);
                });

            return clone;
        }

        /// <summary>
        /// Get stored value.  
        /// </summary>
        public string GetStoredValue(int entityIndex)
        {
            if (!EnsureStoreOnRead()) return null;
            return Store[entityIndex];
        }

        /// <summary>
        /// Set raw stored value.  
        /// </summary>
        public virtual void SetStoredValue(int entityIndex, string value)
        {
            EnsureStoreOnWrite();
            if (events.On)
            {
                var oldValue = GetStoredValue(entityIndex);
                if (Equals(oldValue, value)) return;
                var entity = Meta[entityIndex];
                FireBeforeValueChanged(entity, oldValue, value);
                Store[entityIndex] = value;
                FireValueChanged(entity, oldValue, value);
            }
            else Store[entityIndex] = value;
        }

        /// <inheritdoc />
        public override void CopyValue(BGField fromField, BGId fromEntityId, int fromEntityIndex, BGId toEntityId)
        {
            EnsureStoreOnWrite();
            if (fromEntityIndex == -1 || fromField.IsDeleted) return;
            var index = Meta.FindEntityIndex(toEntityId);
            if (index == -1) return;

            Store[index] = ((BGField<string>)fromField)[fromEntityIndex];
        }


        /// <inheritdoc />
        public override void DuplicateValue(BGId fromEntityId, int fromEntityIndex, BGId toEntityId)
        {
            CopyValue(this, fromEntityId, fromEntityIndex, toEntityId);
        }


        /// <inheritdoc />
        public override void Swap(int entityIndex1, int entityIndex2)
        {
            EnsureStoreOnWrite();
            Store.Swap(entityIndex1, entityIndex2);
        }

        /// <inheritdoc />
        public override void MoveEntitiesValues(int fromIndex, int toIndex, int numberOfValues)
        {
            EnsureStoreOnWrite();
            //check input parameters
            if (fromIndex == toIndex) return;
            var count = Store.Count;
            if (numberOfValues <= 0) throw new BGException("Invalid numberOfEntities: $. It should be more than 0", numberOfValues);
            if (fromIndex < 0) throw new BGException("Invalid fromIndex: $. It should be equal or more than 0", fromIndex);
            if (fromIndex >= count) throw new BGException("Invalid fromIndex: $. It should be less than number of entities $", fromIndex, count);
            if (fromIndex + numberOfValues > count) throw new BGException("Invalid fromIndex: $. fromIndex + numberOfEntities should not exceed the number of entities $", fromIndex, count);

            if (toIndex < 0) throw new BGException("Invalid toIndex: $. It should be equal or more than 0", toIndex);
            if (toIndex >= count) throw new BGException("Invalid toIndex: $. It should be less than number of entities $", toIndex, count);
            if (toIndex + numberOfValues > count) throw new BGException("Invalid toIndex: $. toIndex + numberOfEntities should not exceed the number of entities $", toIndex, count);


            Store.MoveValues(fromIndex, toIndex, numberOfValues);
        }

        /// <inheritdoc />
        public override bool AreStoredValuesEqual(BGField field, int myEntityIndex, int otherEntityIndex)
        {
            var typed = field as BGFieldLocaleStringA;
            if (typed == null) return false;
            EnsureStoreOnRead();
            typed.EnsureStoreOnRead();
            if (Store == null && typed.Store == null) return true;
            if (Store == null || typed.Store == null) return false;

            var myValue = Store[myEntityIndex];
            var otherValue = typed.Store[otherEntityIndex];
            return AreStoredValuesEqual(myValue, otherValue);
        }

        protected virtual bool AreStoredValuesEqual(string myValue, string otherValue)
        {
            return myValue == otherValue;
        }

        /// <inheritdoc />
        public override void OnEntityAdd(BGEntity entity)
        {
            if (Store == null) return;
            Store.MinSize = Meta.CountEntities;
        }

        /// <inheritdoc />
        public override void OnEntityDelete(BGEntity entity)
        {
            Store?.DeleteAt(entity.Index);
        }

        /// <inheritdoc />
        public override void OnCreate()
        {
            if (Store == null) return;
            Store.MinSize = Meta.CountEntities;
        }

        public string[] CopyRawValues()
        {
            return Store?.CopyRawValues();
        }

        public override byte[] ToBytes(int entityIndex)
        {
            return BGFieldStringA.ValueToBytes(this[entityIndex]);
        }

        public override void FromBytes(int entityIndex, ArraySegment<byte> segment)
        {
            this[entityIndex] = BGFieldStringA.ValueFromBytes(segment);
        }

        public override string ToString(int entityIndex)
        {
            return BGFieldStringA.ValueToString(this[entityIndex]);
        }

        public override void FromString(int entityIndex, string value)
        {
            this[entityIndex] = BGFieldStringA.ValueFromString(value);
        }
    }
}