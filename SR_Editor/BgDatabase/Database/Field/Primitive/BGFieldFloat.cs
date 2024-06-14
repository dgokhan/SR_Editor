/*
<copyright file="BGFieldFloat.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Globalization;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// float Field 
    /// </summary>
    [FieldDescriptor(Name = "float", Folder = "Primitive", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerFloat")]
    public partial class BGFieldFloat : BGFieldCachedStructA<float>, BGBinaryBulkLoaderStruct
    {
        public const ushort CodeType = 29;
        /// <inheritdoc/>
        public override ushort TypeCode => CodeType;

        public const int SizeOfTheValue = 4;

        /// <inheritdoc/>
        protected override int ValueSize => SizeOfTheValue;

        //for new field
        public BGFieldFloat(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldFloat(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        /// <inheritdoc/>
        protected override bool AreStoredValuesEqual(float myValue, float otherValue) => Mathf.Approximately(myValue, otherValue);

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
                    StoreItems[i] = BitConverter.ToSingle(array, startIndex);
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
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldFloat(meta, id, name);


        //================================================================================================
        //                                              Static
        //================================================================================================

        public static byte[] ValueToBytes(float value)
        {
            var array = BitConverter.GetBytes(value);
            if (!BitConverter.IsLittleEndian) Array.Reverse(array);
            return array;
        }

        public static float ValueFromBytes(ArraySegment<byte> segment)
        {
            if (segment.Count != SizeOfTheValue) return 0;
            if (BitConverter.IsLittleEndian) return BitConverter.ToSingle(segment.Array, segment.Offset);

            //big endian architecture
            var tempArray = new byte[SizeOfTheValue];
            var array = segment.Array;
            var offset = segment.Offset;
            for (var i = 0; i < SizeOfTheValue; i++) tempArray[i] = array[offset + i];
            Array.Reverse(tempArray);

            return BitConverter.ToSingle(tempArray, 0);
        }

        public static string ValueToString(float f)
        {
            return f.ToString("G9", CultureInfo.InvariantCulture);
        }

        public static float ValueFromString(string value)
        {
            return string.IsNullOrEmpty(value) ? 0 : float.Parse(value.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture);
        }

/*
        [StructLayout(LayoutKind.Explicit)]
        private struct UIntFloat
        {
            [FieldOffset(0)] public float FloatValue;

            [FieldOffset(0)] public uint IntValue;
        }
*/
    }
}