/*
<copyright file="BGFieldDictionaryBasedA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// base abstract field that holds values cached in the dictionary.
    /// Currently is used by calculated fields only
    /// </summary>
    public abstract class BGFieldDictionaryBasedA<T, TStoreType> : BGField<T>, BGStorable<TStoreType>
    {
        protected readonly Dictionary<BGId, TStoreType> storage = new Dictionary<BGId, TStoreType>();

        protected BGFieldDictionaryBasedA(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        protected BGFieldDictionaryBasedA(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
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
                var e = Meta.GetEntity(entityId);
                if (e == null) throw new BGException("Can not get entity with Id=$", entityId);
                storage.TryGetValue(entityId, out var value);
                return Convert(e, value);
            }
            set
            {
                var e = Meta.GetEntity(entityId);
                if (e == null) throw new BGException("Can not get entity with Id=$", entityId);
                Set(e, value);
            }
        }
        
        /// <inheritdoc/>
        public override T this[int index]
        {
            get
            {
                var e = Meta.GetEntity(index);
                if (e == null) throw new BGException("Can not get entity with index=$", index);

                storage.TryGetValue(e.Id, out var value);
                return Convert(e, value);
            }
            set
            {
                var e = Meta.GetEntity(index);
                if (e == null) throw new BGException("Can not get entity with index=$", index);
                Set(e, value);
            }
        }

        private void Set(BGEntity e, T value)
        {
            if (!storage.TryGetValue(e.Id, out var oldStoreValue)) oldStoreValue = default;
            var newStoreValue = Convert(e, value);
            storage[e.Id] = newStoreValue;
            FireStoredValueChanged(e, oldStoreValue, newStoreValue);
        }

        /// <inheritdoc/>
        public void SetStoredValue(int entityIndex, TStoreType value)
        {
            var e = Meta.GetEntity(entityIndex);
            if (e == null) throw new BGException("Can not get entity with index=$", entityIndex);
            if (!storage.TryGetValue(e.Id, out var oldStoreValue)) oldStoreValue = default;
            storage[e.Id] = value;
            FireStoredValueChanged(e, oldStoreValue, value);
        }

        /// <inheritdoc/>
        public TStoreType GetStoredValue(int entityIndex)
        {
            var e = Meta.GetEntity(entityIndex);
            if (e == null) throw new BGException("Can not get entity with index=$", entityIndex);
            if (storage.TryGetValue(e.Id, out var value)) return value;
            return default;
        }

        /*
        /// <inheritdoc/>
        public TStoreType[] CopyRawValues() => throw new Exception("not implemented");
        */

        /// <inheritdoc />
        public override bool AreStoredValuesEqual(BGField field, int myEntityIndex, int otherEntityIndex)
        {
            if (!(field is BGFieldDictionaryBasedA<T, TStoreType> typed)) return false;


            var e1 = Meta.GetEntity(myEntityIndex);
            var e2 = typed.Meta.GetEntity(otherEntityIndex);
            var value1 = default(TStoreType);
            if (storage.TryGetValue(e1.Id, out var v1)) value1 = v1;
            var value2 = default(TStoreType);
            if (typed.storage.TryGetValue(e2.Id, out var v2)) value2 = v2;
            return AreStoredValuesEqual(value1, value2);
        }

        //are stored values equal
        protected virtual bool AreStoredValuesEqual(TStoreType myValue, TStoreType otherValue) => Equals(myValue, otherValue);
        
        /// <inheritdoc/>
        public override void MoveEntitiesValues(int fromIndex, int toIndex, int numberOfValues)
        {
            //no need
        }

        /// <inheritdoc />
        public override void Swap(int entityIndex1, int entityIndex2)
        {
            //no need
        }


        /// <inheritdoc />
        public override void CopyValue(BGField fromField, BGId fromEntityId, int fromEntityIndex, BGId toEntityId)
        {
            if (!(fromField is BGFieldDictionaryBasedA<T, TStoreType> fromTyped)) return;

            if (fromEntityIndex == -1 || fromTyped.IsDeleted) return;

            var toEntity = Meta.GetEntity(toEntityId);
            if (toEntity == null) return;

            var fromValue = default(TStoreType);
            if (fromTyped.storage.TryGetValue(fromEntityId, out var value)) fromValue = value;

            if (fromValue == null) storage.Remove(toEntityId);
            else
            {
                if (fromValue.GetType().IsValueType) storage[toEntityId] = fromValue;
                else
                    switch (fromValue)
                    {
                        case BGFieldDictionaryClonebleValueI valueI:
                            storage[toEntityId] = (TStoreType)valueI.CloneTo(toEntity);
                            break;
                        case ICloneable cloneable:
                            storage[toEntityId] = (TStoreType)cloneable.Clone();
                            break;
                        default:
                            //gentle reminder
                            throw new Exception("Can not copy value cause the value is not cloneable and not a struct");
                    }
            }
        }

        /// <inheritdoc />
        public override void DuplicateValue(BGId fromEntityId, int fromEntityIndex, BGId toEntityId) => CopyValue(this, fromEntityId, fromEntityIndex, toEntityId);

        /// <inheritdoc />
        public override void ClearValue(int entityIndex)
        {
            var e = Meta.GetEntity(entityIndex);
            if (events.On)
            {
                var hadValue = storage.TryGetValue(e.Id, out var oldValue);
                if (hadValue)
                {
                    ClearValueNoEvent(e.Id);
                    FireStoredValueChanged(Meta[entityIndex], oldValue, default);
                }
            }
            else ClearValueNoEvent(e.Id);
        }

        //clear value for row with ID=id without firing event 
        private void ClearValueNoEvent(BGId id) => storage.Remove(id);

        /// <inheritdoc/>
        public override void ClearValues() => storage.Clear();

        /// <inheritdoc/>
        public override void OnDelete() => ClearValues();

        /// <inheritdoc />
        public override void ForEachValue(Action<int> action)
        {
            foreach (var id in storage.Keys)
            {
                var entity = Meta.GetEntity(id);
                if (entity == null) continue;
                action(entity.Index);
            }
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
        public override void OnEntityDelete(BGEntity entity)
        {
            storage.Remove(entity.Id);
        }

        /// <summary>
        /// Fire value changed event
        /// </summary>
        public void FireStoredValueChanged(BGEntity entity, TStoreType oldValue, TStoreType newValue)
        {
            if (events.ConsumeOnChange(MetaId)) return;
            using (var eventArgs = BGEventArgsFieldWithValue<T, TStoreType>.GetInstance(entity, this, oldValue, newValue)) FireValueChanged(eventArgs);

            Meta.FireValueChanged(this, entity, true);
            events.FireAnyChange();
        }

        //================================================================================================
        //                                              Synchronization
        //================================================================================================
        /// <inheritdoc />
        public override byte[] ToBytes(int entityIndex)
        {
            var value = GetStoredValue(entityIndex);
            if (value == null) return null;
            return ValueToBytes(value);
        }


        /// <inheritdoc />
        public override void FromBytes(int entityIndex, ArraySegment<byte> segment) => SetStoredValue(entityIndex, ValueFromBytes(entityIndex, segment));

        /// <inheritdoc />
        public override string ToString(int entityIndex)
        {
            var value = GetStoredValue(entityIndex);
            if (value == null) return null;
            return ValueToString(value);
        }

        /// <inheritdoc />
        public override void FromString(int entityIndex, string value) => SetStoredValue(entityIndex, ValueFromString(entityIndex, value));

        //================================================================================================
        //                                              abstract
        //================================================================================================

        /// <summary>
        /// convert T value to stored value TStoreType
        /// </summary>
        protected abstract TStoreType Convert(BGEntity entity, T value);

        /// <summary>
        /// convert stored value TStoreType to T value  
        /// </summary>
        protected abstract T Convert(BGEntity entity, TStoreType value);

        /// <summary>
        /// convert stored value TStoreType to byte array
        /// </summary>
        protected abstract byte[] ValueToBytes(TStoreType value);

        /// <summary>
        /// restore stored value TStoreType from byte array
        /// </summary>
        protected abstract TStoreType ValueFromBytes(int entityIndex, ArraySegment<byte> segment);

        /// <summary>
        /// convert stored value TStoreType to string
        /// </summary>
        protected abstract string ValueToString(TStoreType value);

        /// <summary>
        /// restore stored value TStoreType from string
        /// </summary>
        protected abstract TStoreType ValueFromString(int entityIndex, string value);
    }

    /// <summary>
    /// Cloneable field value type
    /// </summary>
    public interface BGFieldDictionaryClonebleValueI
    {
        /// <summary>
        /// Clone the field value to provided entity 
        /// </summary>
        object CloneTo(BGEntity e);
    }
}