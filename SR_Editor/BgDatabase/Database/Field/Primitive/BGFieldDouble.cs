/*
<copyright file="BGFieldDouble.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Globalization;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// double Field 
    /// </summary>
    [FieldDescriptor(Name = "double", Folder = "Primitive", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerDouble")]
    public partial class BGFieldDouble : BGFieldCachedStructA<double>, BGBinaryBulkLoaderStruct
    {
        public const ushort CodeType = 28;
        /// <inheritdoc/>
        public override ushort TypeCode => CodeType;

        public const int SizeOfTheValue = 8;

        /// <inheritdoc/>
        protected override int ValueSize => SizeOfTheValue;

        //for new field
        public BGFieldDouble(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldDouble(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        /// <inheritdoc/>
        protected override bool AreStoredValuesEqual(double myValue, double otherValue)
        {
            //https://stackoverflow.com/questions/2411392/double-epsilon-for-equality-greater-than-less-than-less-than-or-equal-to-gre
            var epsilon = Math.Max(Math.Abs(myValue), Math.Abs(otherValue)) * 1E-15;
            return Math.Abs(myValue - otherValue) <= epsilon;
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
            if (BitConverter.IsLittleEndian)
            {
                for (var i = 0; i < entitiesCount; i++)
                {
                    var startIndex = offset + (SizeOfTheValue * i);
                    StoreItems[i] = BitConverter.ToDouble(array, startIndex);
                }                
            }
            else
            {
                for (var i = 0; i < entitiesCount; i++) FromBytes(i, new ArraySegment<byte>(array, offset + (SizeOfTheValue * i), SizeOfTheValue));
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
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldDouble(meta, id, name);

        //================================================================================================
        //                                              Static
        //================================================================================================

        public static byte[] ValueToBytes(double value)
        {
            var array = BitConverter.GetBytes(value);
            if (!BitConverter.IsLittleEndian) Array.Reverse(array);
            return array;
        }

        public static double ValueFromBytes(ArraySegment<byte> segment)
        {
            if (segment.Count != SizeOfTheValue) return 0;
            if (BitConverter.IsLittleEndian) return BitConverter.ToDouble(segment.Array, segment.Offset);

            //big endian architecture
            var tempArray = new byte[SizeOfTheValue];
            var array = segment.Array;
            var offset = segment.Offset;
            for (var i = 0; i < SizeOfTheValue; i++) tempArray[i] = array[offset + i];
            Array.Reverse(tempArray);

            return BitConverter.ToDouble(tempArray, 0);
        }

        public static string ValueToString(double d)
        {
            return d.ToString("G17", CultureInfo.InvariantCulture);
        }

        public static double ValueFromString(string value)
        {
            return string.IsNullOrEmpty(value) ? 0 : double.Parse(value, NumberStyles.Any, CultureInfo.InvariantCulture);
        }
    }
}