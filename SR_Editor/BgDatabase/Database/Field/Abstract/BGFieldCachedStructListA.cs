/*
<copyright file="BGFieldCachedStructListA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Basic class for a field with list value type, holding struct values
    /// T is the type of the struct (list element value)
    /// </summary>
    public abstract partial class BGFieldCachedStructListA<T> : BGFieldCachedListA<T>, BGBinaryBulkLoaderClass where T : struct
    {
        /// <summary>
        /// how much bytes T value takes 
        /// </summary>
        protected abstract int ValueSize { get; }

        protected BGFieldCachedStructListA(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        protected BGFieldCachedStructListA(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Serialization
        //================================================================================================
        /// <inheritdoc />
        public override byte[] ToBytes(int entityIndex)
        {
            var value = this[entityIndex];
            if (BGUtil.IsEmpty(value)) return null;

            var valueSize = ValueSize;

            var result = new byte[value.Count * valueSize];
            for (var i = 0; i < value.Count; i++) Buffer.BlockCopy(ValueToBytes(value[i]), 0, result, i * valueSize, valueSize);
            return result;
        }

        /// <inheritdoc />
        public override void FromBytes(int entityIndex, ArraySegment<byte> segment)
        {
            var segmentCount = segment.Count;
            if (segmentCount == 0) ClearValueNoEvent(entityIndex);
            else
            {
                var valueSize = ValueSize;
                if (segmentCount % valueSize != 0) throw new BGException("Can not convert byte array to value. Wrong byte array size $. Should be dividable by $", segmentCount, valueSize);

                //ensure the list
                var myValue = EnsureValueCleared(this, entityIndex, segmentCount / valueSize);

                //set values
                for (var i = 0; i < segmentCount; i = i + valueSize) myValue.Add(ValueFromBytes(new ArraySegment<byte>(segment.Array, segment.Offset + i, valueSize)));
            }
        }

        /// <inheritdoc />
        public void FromBytes(BGBinaryBulkRequestClass request)
        {
            var array = request.Array;
            var requests = request.CellRequests;
            var length = requests.Length;
            for (var i = 0; i < length; i++)
            {
                var cellRequest = requests[i];
                var entityIndex = cellRequest.EntityIndex;
                var offset = cellRequest.Offset;
                try
                {
                    var valueSize = ValueSize;
                    //remove? too expensive
                    if (cellRequest.Count % valueSize != 0) throw new BGException("Can not convert byte array to value. Wrong byte array size $. Should be dividable by $", cellRequest.Count, valueSize);
                    var count = cellRequest.Count / valueSize;
                    if (count == 0) StoreItems[entityIndex] = default;
                    else
                    {
                        var list = StoreItems[entityIndex];
                        if (list == null) list = new List<T>(count);
                        else
                        {
                            list.Clear();
                            if (list.Capacity < count) list.Capacity = count;
                        }

                        StoreItems[entityIndex] = list;

                        var upperLimit = valueSize * count;
                        if (BitConverter.IsLittleEndian)
                        {
                            for (var cursor = 0; cursor < upperLimit; cursor += valueSize) list.Add(ValueFromBytes(array, offset + cursor));
                        }
                        else
                        {
                            //this code is probably will never be used cause Unity does not support any BigEndian platform!
                            for (var cursor = BGFieldInt.SizeOfTheValue; cursor < upperLimit; cursor += valueSize) list.Add(ValueFromBytes(new ArraySegment<byte>(array, cursor, valueSize)));
                        }
                    }
                }
                catch (Exception e)
                {
                    request.OnError?.Invoke(e);
                }
            }
        }

        protected abstract T ValueFromBytes(byte[] array, int offset);

        /// <inheritdoc />
        public override string ToString(int entityIndex)
        {
            var value = this[entityIndex];
            if (BGUtil.IsEmpty(value)) return null;

            var separator = StringValueSeparator[0];
            var result = "";
            for (var i = 0; i < value.Count; i++)
            {
                var val = value[i];
                if (i != 0) result += separator;
                result += ValueToString(val);
            }

            return result;
        }

        /// <inheritdoc />
        public override void FromString(int entityIndex, string value)
        {
            if (string.IsNullOrEmpty(value)) ClearValueNoEvent(entityIndex);
            else
            {
                var parts = value.Split(StringValueSeparator, StringSplitOptions.RemoveEmptyEntries);
                //ensure the list
                var myValue = EnsureValueCleared(this, entityIndex, parts.Length);

                foreach (var part in parts) myValue.Add(ValueFromString(part));
            }
        }


        //================================================================================================
        //                                              Value
        //================================================================================================
        /// <inheritdoc />
        public override List<T> this[int entityIndex]
        {
            set
            {
                if (events.On)
                {
                    var oldValue = this[entityIndex];
                    if (BGUtil.ListsValuesEqual(value, oldValue)) return;
                    var entity = Meta[entityIndex];
                    FireBeforeValueChanged(entity, oldValue, value);
                    StoreSet(entityIndex, value);
                    FireValueChanged(entity, oldValue, value);
                }
                else StoreSet(entityIndex, value);
            }
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
        protected abstract T ValueFromString(string value);
    }
}