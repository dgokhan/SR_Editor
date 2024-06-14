/*
<copyright file="BGFieldCachedA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;


namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// class that holds values cached (in data container) however the type of the field can be different from cached value in the storage
    /// </summary>
    public abstract partial class BGFieldCachedA<T, TStoreType> : BGField<T>, BGStorageI<TStoreType>
    {
        //This is for backward compatibility only!
        protected BGStoreFieldI<TStoreType> Store => new BGStoreFieldAdapter<T, TStoreType>(this);

        protected BGFieldCachedA(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        protected BGFieldCachedA(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Value
        //================================================================================================
        /// <inheritdoc />
        public override T this[BGId entityId]
        {
            get
            {
                //this is slow version with +1 dict lookup to find entity index
                var index = Meta.FindEntityIndex(entityId);
                if (index == -1)
                {
                    return default;
                }

                return this[index];
            }
            set
            {
                //this is slow version with +1 dict lookup to find entity index
                var index = Meta.FindEntityIndex(entityId);
                if (index == -1)
                {
                    return;
                }

                this[index] = value;
            }
        }

        /// <inheritdoc />
        public override void ForEachValue(Action<int> action) => StoreForEachKey(action);

        /// <inheritdoc/>
        public override void OnDelete() => ClearValues();

        /// <inheritdoc/>
        public override void ClearValues() => StoreClear();

        /// <inheritdoc />
        public override void ClearValue(int entityIndex)
        {
            if (events.On)
            {
                if (entityIndex >= StoreCount) ThrowIndexOutOfBoundOnWrite(entityIndex);
                var oldValue = StoreItems[entityIndex];
                var hadValue = !EqualityComparer<TStoreType>.Default.Equals(oldValue, default);
                StoreItems[entityIndex] = default;
                if (hadValue) FireStoredValueChanged(Meta[entityIndex], oldValue, default);
            }
            else ClearValueNoEvent(entityIndex);
        }

        //clear value with no event
        protected void ClearValueNoEvent(int entityIndex)
        {
            if (entityIndex >= StoreCount) ThrowIndexOutOfBoundOnWrite(entityIndex);
            StoreItems[entityIndex] = default;
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


        /// <inheritdoc/>
        public TStoreType GetStoredValue(int index)
        {
            if (index >= StoreCount) ThrowIndexOutOfBoundOnRead(index);
            return StoreItems[index];
        }

        /// <inheritdoc/>
        public virtual void SetStoredValue(int entityIndex, TStoreType value)
        {
            if (events.On)
            {
                var oldValue = GetStoredValue(entityIndex);
                if (Equals(oldValue, value)) return;
                StoreItems[entityIndex] = value;
                FireStoredValueChanged(Meta[entityIndex], oldValue, value);
            }
            else
            {
                if (entityIndex >= StoreCount) ThrowIndexOutOfBoundOnWrite(entityIndex);
                StoreItems[entityIndex] = value;
            }
        }

        /// <inheritdoc />
        public override void CopyValue(BGField fromField, BGId fromEntityId, int fromEntityIndex, BGId toEntityId)
        {
            if (fromEntityIndex == -1 || fromField.IsDeleted) return;
            var index = Meta.FindEntityIndex(toEntityId);
            if (index == -1) return;

            StoreItems[index] = ((BGStorable<TStoreType>)fromField).GetStoredValue(fromEntityIndex);
        }

        /// <inheritdoc />
        public override void DuplicateValue(BGId fromEntityId, int fromEntityIndex, BGId toEntityId) => CopyValue(this, fromEntityId, fromEntityIndex, toEntityId);


        /// <inheritdoc />
        public override void Swap(int entityIndex1, int entityIndex2) => StoreSwap(entityIndex1, entityIndex2);

        /// <inheritdoc />
        public override void MoveEntitiesValues(int fromIndex, int toIndex, int numberOfValues)
        {
            //check input parameters
            if (fromIndex == toIndex) return;
            var count = StoreCount;
            if (numberOfValues <= 0) throw new BGException("Invalid numberOfEntities: $. It should be more than 0", numberOfValues);
            if (fromIndex < 0) throw new BGException("Invalid fromIndex: $. It should be equal or more than 0", fromIndex);
            if (fromIndex >= count) throw new BGException("Invalid fromIndex: $. It should be less than number of entities $", fromIndex, count);
            if (fromIndex + numberOfValues > count) throw new BGException("Invalid fromIndex: $. fromIndex + numberOfEntities should not exceed the number of entities $", fromIndex, count);

            if (toIndex < 0) throw new BGException("Invalid toIndex: $. It should be equal or more than 0", toIndex);
            if (toIndex >= count) throw new BGException("Invalid toIndex: $. It should be less than number of entities $", toIndex, count);
            if (toIndex + numberOfValues > count) throw new BGException("Invalid toIndex: $. toIndex + numberOfEntities should not exceed the number of entities $", toIndex, count);


            StoreMoveValues(fromIndex, toIndex, numberOfValues);
        }

        /// <inheritdoc />
        public override bool AreStoredValuesEqual(BGField field, int myEntityIndex, int otherEntityIndex)
        {
            if (!(field is BGFieldCachedA<T, TStoreType> typed)) return false;
            
            if (myEntityIndex >= StoreCount) ThrowIndexOutOfBoundOnRead(myEntityIndex);
            if (otherEntityIndex >= typed.StoreCount) typed.ThrowIndexOutOfBoundOnRead(otherEntityIndex);
            
            var myValue = StoreItems[myEntityIndex];
            var otherValue = typed.StoreItems[otherEntityIndex];
            return AreStoredValuesEqual(myValue, otherValue);
        }

        protected virtual bool AreStoredValuesEqual(TStoreType myValue, TStoreType otherValue) => Equals(myValue, otherValue);

        //================================================================================================
        //                                              Callbacks
        //================================================================================================
        /// <inheritdoc />
        public override void OnEntityAdd(BGEntity entity) => StoreMinSize = Meta.CountEntities;

        /// <inheritdoc />
        public override void OnEntityDelete(BGEntity entity) => StoreDeleteAt(entity.Index);

        /// <inheritdoc />
        public override void OnCreate() => StoreMinSize = Meta.CountEntities;

        //================================================================================================
        //                                              BGStorageI
        //================================================================================================
        /// <inheritdoc/>
        public TStoreType[] CopyRawValues() => StoreCopyRawValues();

        //================================================================================================
        //                                              Events
        //================================================================================================
        /// <summary>
        /// Fire stored value changed event
        /// </summary>
        public void FireStoredValueChanged(BGEntity entity, TStoreType oldValue, TStoreType newValue)
        {
            if (events.ConsumeOnChange(MetaId)) return;
            using (var eventArgs = BGEventArgsFieldWithValue<T, TStoreType>.GetInstance(entity, this, oldValue, newValue)) FireValueChanged(eventArgs);

            Meta.FireStoredValueChanged(this, entity, oldValue, newValue, true);
            events.FireAnyChange();
        }

        //================================================================================================
        //               THIS SECTION IS copied from BGArrayStore & BGStoreField
        //               This is micro-optimization
        //================================================================================================
        protected TStoreType[] StoreItems = Array.Empty<TStoreType>();

        /// <summary>
        /// objects count
        /// </summary>
        protected internal int StoreCount;

        /// <summary>
        /// Capacity
        /// </summary>
        protected internal int StoreMinSize
        {
            set
            {
                if (StoreCount >= value) return;
                StoreMinCapacity = value;
                StoreCount = value;
            }
        }

        /// <summary>
        /// Capacity
        /// </summary>
        protected internal int StoreMinCapacity
        {
            set
            {
                if (StoreItems.Length >= value) return;
                var newCapacity = StoreItems.Length == 0 ? 4 : StoreItems.Length * 2;
                if (newCapacity < value) newCapacity = value;
                var newItems = new TStoreType[newCapacity];
                if (StoreCount > 0) Array.Copy(StoreItems, 0, newItems, 0, StoreCount);
                StoreItems = newItems;
            }
        }

        /// <summary>
        /// Get object by its index
        /// </summary>
        protected internal TStoreType StoreGet(int index)
        {
            if (index >= StoreCount) ThrowIndexOutOfBoundOnRead(index);
            return StoreItems[index];
        }

        /// <summary>
        /// Set object by its index
        /// </summary>
        protected internal void StoreSet(int index, TStoreType value)
        {
            if (index >= StoreCount) ThrowIndexOutOfBoundOnWrite(index);
            StoreItems[index] = value;
        }

        protected void ThrowIndexOutOfBoundOnRead(int index)
        {
            if (IsDeleted) throw new Exception($"An attempt to read value from field [{Name}], which was deleted or unloaded. Field can be unloaded when database is reloaded");
            throw new Exception($"Index is out of bounds while trying to read a value from field [{FullName}], it's greater or equal to maxIndex, " + index + ">=" + StoreCount);
        }

        protected void ThrowIndexOutOfBoundOnWrite(int index)
        {
            if (IsDeleted) throw new Exception($"An attempt to set value to field [{Name}], which was deleted or unloaded. Field can be unloaded when database is reloaded");
            throw new Exception($"Index is out of bounds while trying to set a value to field [{FullName}], it's greater or equal to maxIndex, " + index + ">=" + StoreCount);
        }

        /// <summary>
        /// delete an object at specified index
        /// </summary>
        protected internal void StoreDeleteAt(int index)
        {
            if (StoreCount <= index) return;

            StoreCount--;
            var length = StoreCount - index;
            if (length > 0) Array.Copy(StoreItems, index + 1, StoreItems, index, length);
            StoreItems[StoreCount] = default;
        }

        /// <summary>
        /// Clear internal storage
        /// </summary>
        protected internal void StoreClear()
        {
            StoreItems = Array.Empty<TStoreType>();
            StoreCount = 0;
        }

        /// <summary>
        /// add new object to the store
        /// </summary>
        protected internal void StoreAdd(TStoreType item)
        {
            StoreMinCapacity = StoreCount + 1;
            StoreItems[StoreCount] = item;
            StoreCount++;
        }

        /// <summary>
        /// swap 2 objects
        /// </summary>
        protected internal void StoreSwap(int index1, int index2) => (StoreItems[index1], StoreItems[index2]) = (StoreItems[index2], StoreItems[index1]);

        /// <summary>
        /// move the objects 
        /// </summary>
        protected internal void StoreMoveValues(int fromIndex, int toIndex, int numberOfElements)
        {
            var temp = new TStoreType[numberOfElements];
            Array.Copy(StoreItems, fromIndex, temp, 0, numberOfElements);
            if (fromIndex > toIndex)
            {
                if (toIndex + numberOfElements < fromIndex) Array.Copy(StoreItems, toIndex, StoreItems, toIndex + numberOfElements, fromIndex - toIndex);
                else
                {
                    var toMove = fromIndex - toIndex;
                    Array.Copy(StoreItems, toIndex, StoreItems, fromIndex + numberOfElements - toMove, toMove);
                }
            }
            else
            {
                if (fromIndex + numberOfElements <= toIndex) Array.Copy(StoreItems, fromIndex + numberOfElements, StoreItems, fromIndex, toIndex - fromIndex);
                else Array.Copy(StoreItems, fromIndex + numberOfElements, StoreItems, fromIndex, toIndex - fromIndex);
            }

            Array.Copy(temp, 0, StoreItems, toIndex, numberOfElements);
        }

        /// <summary>
        /// invoke action for each value
        /// </summary>
        protected internal void StoreForEachKey(Action<int> action)
        {
            var count = StoreCount;
            var defaultValue = default(TStoreType);
            var comparer = EqualityComparer<TStoreType>.Default;
            for (var i = 0; i < count; i++)
            {
                var item = StoreItems[i];
                if (comparer.Equals(item, defaultValue)) continue;
                action(i);
            }
        }

        /// <summary>
        /// invoke action for each value
        /// </summary>
        protected internal void StoreForEachKeyValue(Action<int, TStoreType> action)
        {
            var count = StoreCount;
            for (var i = 0; i < count; i++) action(i, StoreItems[i]);
        }

        /// <summary>
        /// copy values to array
        /// </summary>
        protected internal TStoreType[] StoreCopyRawValues()
        {
            var copy = new TStoreType[StoreCount];
            Array.Copy(StoreItems, copy, StoreCount);
            return copy;
        }
        //================================================================================================
        //                  micro-optimization section ends here
        //================================================================================================

    }

    /// <summary>
    /// class that holds values cached (in data container) and value type and container type are the same
    /// </summary>
    public abstract partial class BGFieldCachedA<T> : BGFieldCachedA<T, T>
    {
        protected BGFieldCachedA(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        protected BGFieldCachedA(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Value
        //================================================================================================
        /// <inheritdoc />
        public override T this[int index]
        {
            get
            {
                //Micro-optimization, copied from base class
                if (index >= StoreCount) ThrowIndexOutOfBoundOnRead(index);
                return StoreItems[index];
            }
        }

        /// <inheritdoc />
        public override void ClearValue(int entityIndex)
        {
            if (events.On)
            {
                var oldValue = this[entityIndex];
                var hadValue = !EqualityComparer<T>.Default.Equals(oldValue, default);
                if (hadValue) FireBeforeValueChanged(Meta[entityIndex], oldValue, default);
                ClearValueNoEvent(entityIndex);
                if (hadValue) FireValueChanged(Meta[entityIndex], oldValue, default);
            }
            else ClearValueNoEvent(entityIndex);
        }
    }
}