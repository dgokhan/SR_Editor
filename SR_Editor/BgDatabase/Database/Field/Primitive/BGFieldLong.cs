/*
<copyright file="BGFieldLong.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Globalization;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// long Field 
    /// </summary>
    [FieldDescriptor(Name = "long", Folder = "Primitive", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerLong")]
    public partial class BGFieldLong : BGFieldCachedStructA<long>, BGBinaryBulkLoaderStruct
    {
        public const ushort CodeType = 32;

        /// <inheritdoc/>
        public override ushort TypeCode => CodeType;

        public const int SizeOfTheValue = 8;

        /// <inheritdoc/>
        protected override int ValueSize => SizeOfTheValue;

        /// <inheritdoc/>
        public override bool CanBeUsedAsKey => true;

        //for new field
        public BGFieldLong(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldLong(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Serialization
        //================================================================================================
        /// <inheritdoc />
        public override byte[] ToBytes(int entityIndex) => ValueToBytes(this[entityIndex]);

        /// <inheritdoc />
        public override void FromBytes(int entityIndex, ArraySegment<byte> segment) => this[entityIndex] = ValueFromBytes(segment);

        /// <inheritdoc />
        public void FromBytes(BGBinaryBulkRequestStruct request)
        {
            var array = request.Array;
            var offset = request.Offset;
            var entitiesCount = request.EntitiesCount;
            for (var i = 0; i < entitiesCount; i++)
            {
                var startIndex = offset + (SizeOfTheValue * i);
                StoreItems[i] = ((long)array[startIndex + 7] << 56) | ((long)array[startIndex + 6] << 48) | ((long)array[startIndex + 5] << 40) | ((long)array[startIndex + 4] << 32) |
                                ((long)array[startIndex + 3] << 24) | ((long)array[startIndex + 2] << 16) | ((long)array[startIndex + 1] << 8) | (long)array[startIndex];
            }
        }

        /// <inheritdoc />
        public override string ToString(int entityIndex) => ValueToString(this[entityIndex]);

        /// <inheritdoc />
        public override void FromString(int entityIndex, string value) => this[entityIndex] = ValueFromString(value);

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc/>
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldLong(meta, id, name);


        //================================================================================================
        //                                              Static
        //================================================================================================
        public static byte[] ValueToBytes(long value)
        {
            var array = new byte[SizeOfTheValue];
            array[0] = (byte)value;
            array[1] = (byte)(((ulong)value >> 8) & 0xFF);
            array[2] = (byte)(((ulong)value >> 16) & 0xFF);
            array[3] = (byte)(((ulong)value >> 24) & 0xFF);
            array[4] = (byte)(((ulong)value >> 32) & 0xFF);
            array[5] = (byte)(((ulong)value >> 40) & 0xFF);
            array[6] = (byte)(((ulong)value >> 48) & 0xFF);
            array[7] = (byte)(((ulong)value >> 56) & 0xFF);
            return array;
        }

        public static long ValueFromBytes(ArraySegment<byte> segment)
        {
            if (segment.Count != SizeOfTheValue) return 0;

            var array = segment.Array;
            var offset = segment.Offset;
            return ((long)array[offset + 7] << 56) | ((long)array[offset + 6] << 48) | ((long)array[offset + 5] << 40) | ((long)array[offset + 4] << 32) |
                   ((long)array[offset + 3] << 24) | ((long)array[offset + 2] << 16) | ((long)array[offset + 1] << 8) | (long)array[offset];
        }

        public static string ValueToString(long l) => l.ToString(CultureInfo.InvariantCulture);

        public static long ValueFromString(string value) => string.IsNullOrEmpty(value) ? 0 : long.Parse(value, NumberStyles.Any, CultureInfo.InvariantCulture);
    }
}