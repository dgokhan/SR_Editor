/*
<copyright file="BGFieldDecimal.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


using System;
using System.Globalization;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// decimal Field 
    /// </summary>
    [FieldDescriptor(Name = "decimal", Folder = "Primitive", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerDecimal")]
    public class BGFieldDecimal : BGFieldCachedStructA<decimal>, BGBinaryBulkLoaderStruct
    {
        public const ushort CodeType = 27;

        /// <inheritdoc/>
        public override ushort TypeCode => CodeType;

        public const int SizeOfTheValue = 16;

        /// <inheritdoc/>
        protected override int ValueSize => SizeOfTheValue;

        /// <inheritdoc/>
        public override bool CanBeUsedAsKey => true;

        //for new field
        public BGFieldDecimal(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldDecimal(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
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
            var intValues = new int[4];
            for (var i = 0; i < entitiesCount; i++)
            {
                var startIndex = offset + (SizeOfTheValue * i);

                intValues[0] = (array[startIndex + 3] << 24) | (array[startIndex + 2] << 16) | (array[startIndex + 1] << 8) | array[startIndex];
                intValues[1] = (array[startIndex + 7] << 24) | (array[startIndex + 6] << 16) | (array[startIndex + 5] << 8) | array[startIndex + 4];
                intValues[2] = (array[startIndex + 11] << 24) | (array[startIndex + 10] << 16) | (array[startIndex + 9] << 8) | array[startIndex + 8];
                intValues[3] = (array[startIndex + 15] << 24) | (array[startIndex + 14] << 16) | (array[startIndex + 13] << 8) | array[startIndex + 12];
                StoreItems[i] =  new decimal(intValues);
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
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldDecimal(meta, id, name);


        //================================================================================================
        //                                              Static
        //================================================================================================

        public static byte[] ValueToBytes(decimal value)
        {
            var result = new byte[16];
            var intValues = decimal.GetBits(value);
            Buffer.BlockCopy(BGFieldInt.ValueToBytes(intValues[0]), 0, result, 0, 4);
            Buffer.BlockCopy(BGFieldInt.ValueToBytes(intValues[1]), 0, result, 4, 4);
            Buffer.BlockCopy(BGFieldInt.ValueToBytes(intValues[2]), 0, result, 8, 4);
            Buffer.BlockCopy(BGFieldInt.ValueToBytes(intValues[3]), 0, result, 12, 4);
            return result;
        }

        public static decimal ValueFromBytes(ArraySegment<byte> segment)
        {
            var result = 0;
            if (segment.Count != SizeOfTheValue) return result;

            var array = segment.Array;
            var offset = segment.Offset;
            var intValues = new int[4];

            intValues[0] = BGFieldInt.ValueFromBytes(new ArraySegment<byte>(array, offset + 0, 4));
            intValues[1] = BGFieldInt.ValueFromBytes(new ArraySegment<byte>(array, offset + 4, 4));
            intValues[2] = BGFieldInt.ValueFromBytes(new ArraySegment<byte>(array, offset + 8, 4));
            intValues[3] = BGFieldInt.ValueFromBytes(new ArraySegment<byte>(array, offset + 12, 4));
            return new decimal(intValues);
        }

        public static string ValueToString(decimal f) => f.ToString(CultureInfo.InvariantCulture);

        public static decimal ValueFromString(string value) => string.IsNullOrEmpty(value) ? 0 : decimal.Parse(value.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture);
    }
}