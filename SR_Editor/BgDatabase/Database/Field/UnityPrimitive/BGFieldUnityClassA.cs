/*
<copyright file="BGFieldUnityClassA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// base abstract class for Unity class value (like Gradient)
    /// </summary>
    public abstract class BGFieldUnityClassA<T> : BGFieldCachedClassA<T> where T : class, new()
    {
        protected BGFieldUnityClassA(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        protected BGFieldUnityClassA(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Serialization
        //================================================================================================
        /// <inheritdoc />
        public override byte[] ToBytes(int entityIndex)
        {
            var value = this[entityIndex];
            if (value == null) return null;
            var writer = new BGBinaryWriter(128);
            ToBytes(writer, value);
            return writer.ToArray();
        }

        /// <inheritdoc />
        public override void FromBytes(int entityIndex, ArraySegment<byte> segment)
        {
            if (segment.Count < MinValueSize) ClearValueNoEvent(entityIndex);
            else this[entityIndex] = FromBytes(segment);
        }

        /// <summary>
        /// reconstruct value from binary array 
        /// </summary>
        protected abstract T FromBytes(ArraySegment<byte> segment);
        /// <summary>
        /// write value to binary array 
        /// </summary>
        protected abstract void ToBytes(BGBinaryWriter writer, T value);
        /// <summary>
        /// Min bytes length for the value
        /// </summary>
        public abstract int MinValueSize { get; }


        /// <inheritdoc />
        public override string ToString(int entityIndex)
        {
            var value = this[entityIndex];
            if (value == null) return null;
            return ToString(value);
        }

        /// <inheritdoc />
        public override void FromString(int entityIndex, string value)
        {
            if (value == null || value.Trim().Length == 0) ClearValueNoEvent(entityIndex);
            else this[entityIndex] = FromString(value);
        }

        /// <summary>
        /// reconstruct value from string 
        /// </summary>
        protected abstract T FromString(string value);

        /// <summary>
        /// write value to string 
        /// </summary>
        protected abstract string ToString(T value);

        //================================================================================================
        //                                              Value
        //================================================================================================
        /// <inheritdoc />
        public override void CopyValue(BGField fromField, BGId fromEntityId, int fromEntityIndex, BGId toEntityId)
        {
            if (fromEntityIndex == -1 || fromField.IsDeleted) return;
            var index = Meta.FindEntityIndex(toEntityId);
            if (index == -1) return;

            var otherField = (BGField<T>)fromField;
            var otherValue = otherField[fromEntityIndex];
            if (otherValue == null) ClearValueNoEvent(index);
            else StoreSet(index, CloneValue(otherValue));
        }

        /// <summary>
        /// clone the value 
        /// </summary>
        public abstract T CloneValue(T value);

        /// <inheritdoc />
        protected override bool AreStoredValuesEqual(T myValue, T otherValue) => AreEqual(myValue, otherValue);

        /// <summary>
        /// Are two values equal?
        /// </summary>
        public abstract bool AreEqual(T myValue, T otherValue);
    }
}