/*
<copyright file="BGFieldCachedStructNullableA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Basic class for a field with nullable struct value type
    /// </summary>
    public abstract partial class BGFieldCachedStructNullableA<T> : BGFieldCachedA<T?>, BGStructNullableI, BGBinaryBulkLoaderClass where T : struct
    {
        protected abstract int ValueSize { get; }

        protected BGFieldCachedStructNullableA(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        protected BGFieldCachedStructNullableA(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Value
        //================================================================================================
        /// <inheritdoc />
        public override T? this[int entityIndex]
        {
            set
            {
                if (events.On)
                {
                    var oldValue = this[entityIndex];
                    if (Nullable.Equals(oldValue, value)) return;
                    var entity = Meta[entityIndex];
                    FireBeforeValueChanged(entity, oldValue, value);
                    StoreSet(entityIndex, value);
                    FireValueChanged(entity, oldValue, value);
                }
                else StoreSet(entityIndex, value);
            }
        }

        //================================================================================================
        //                                              Methods
        //================================================================================================
/*
        /// <inheritdoc />
        public override void CopyValue(BGField fieldFrom, BGId entityId)
        {
            var cached = (BGFieldCachedStructNullableA<T>) fieldFrom;
            T otherValue;
            if (cached.Store.TryGetValue(entityId, out otherValue)) Store[entityId] = otherValue;
            else ClearValueNoEvent(entityId);
        }
*/

        //================================================================================================
        //                                              Serialization
        //================================================================================================
        /// <inheritdoc />
        public override byte[] ToBytes(int entityIndex)
        {
            var value = StoreItems[entityIndex];
            return value == null ? null : ValueToBytes(value.Value);
        }


        /// <inheritdoc />
        public override void FromBytes(int entityIndex, ArraySegment<byte> segment)
        {
            if (segment.Count == ValueSize) this[entityIndex] = ValueFromBytes(segment);
            else ClearValueNoEvent(entityIndex);
        }

        /// <inheritdoc />
        public void FromBytes(BGBinaryBulkRequestClass request)
        {
            var array = request.Array;
            var requests = request.CellRequests;
            var length = requests.Length;
            if (BitConverter.IsLittleEndian)
            {
                for (var i = 0; i < length; i++)
                {
                    var cellRequest = requests[i];
                    try
                    {
                        StoreItems[cellRequest.EntityIndex] = ValueFromBytes(array, cellRequest.Offset);
                    }
                    catch (Exception e)
                    {
                        request.OnError?.Invoke(e);
                    }
                }
            }
            else
            {
                //this code is probably will never be used cause Unity does not support any BigEndian platform!
                for (var i = 0; i < length; i++)
                {
                    var cellRequest = requests[i];
                    FromBytes(cellRequest.EntityIndex, new ArraySegment<byte>(array, cellRequest.Offset, ValueSize));
                }
            }
        }

        protected abstract T ValueFromBytes(byte[] array, int offset);

        /// <inheritdoc />
        public override string ToString(int entityIndex)
        {
            var value = StoreItems[entityIndex];
            return value == null ? "" : ValueToString(value.Value);
        }

        /// <inheritdoc />
        public override void FromString(int entityIndex, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                var valueFromString = ValueFromString(value);
                if (valueFromString.HasValue) this[entityIndex] = valueFromString.Value;
                else ClearValueNoEvent(entityIndex);
            }
            else ClearValueNoEvent(entityIndex);
        }

        /// <summary>
        /// convert T value to byte array 
        /// </summary>
        protected abstract byte[] ValueToBytes(T value);
        /// <summary>
        /// restore T value from byte array 
        /// </summary>
        protected abstract T ValueFromBytes(ArraySegment<byte> segment);
        /// <summary>
        /// convert T value to string
        /// </summary>
        protected abstract string ValueToString(T value);
        /// <summary>
        /// restore T value from string
        /// </summary>
        protected abstract T? ValueFromString(string value);
    }
}