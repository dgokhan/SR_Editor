/*
<copyright file="BGFieldCachedEnumA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Basic class for a field with enum value type
    /// T is enum type
    /// </summary>
    public abstract partial class BGFieldCachedEnumA<T> : BGFieldCachedStructA<T>, BGBinaryBulkLoaderStruct where T : struct, IComparable, IConvertible, IFormattable
    {
        private const int Size = 4;

        protected override int ValueSize => Size;
        /*
        public override bool CanBeUsedAsKey
        {
            get
            {
                return true;
            }
        }
        */


        private readonly Type enumType = typeof(T);

        public BGFieldCachedEnumA(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        public BGFieldCachedEnumA(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Serialization
        //================================================================================================
        /// <inheritdoc />
        public override byte[] ToBytes(int entityIndex) => BGFieldInt.ValueToBytes(Convert.ToInt32(this[entityIndex]));

        /// <inheritdoc />
        public override void FromBytes(int entityIndex, ArraySegment<byte> segment)
        {
            if (segment.Count != Size) return;
            this[entityIndex] = (T)(object)BGFieldInt.ValueFromBytes(segment);
        }

        /// <inheritdoc />
        public void FromBytes(BGBinaryBulkRequestStruct request)
        {
            var array = request.Array;
            var offset = request.Offset;
            var entitiesCount = request.EntitiesCount;
            for (var i = 0; i < entitiesCount; i++)
            {
                var startIndex = offset + (Size * i);
                StoreItems[i] = (T)(object) ((array[startIndex + 3] << 24) | (array[startIndex + 2] << 16) | (array[startIndex + 1] << 8) | array[startIndex]);
            }
        }

        /// <inheritdoc />
        public override string ToString(int entityIndex) => Enum.GetName(enumType, this[entityIndex]);

        /// <inheritdoc />
        public override void FromString(int entityIndex, string value)
        {
            T val;

            if (string.IsNullOrEmpty(value)) val = default;
            else
                try
                {
                    val = (T)Enum.Parse(enumType, value);
                    if (!Enum.IsDefined(enumType, val)) val = default;
                }
                catch
                {
                    val = default;
                }

            this[entityIndex] = val;
        }
    }
}