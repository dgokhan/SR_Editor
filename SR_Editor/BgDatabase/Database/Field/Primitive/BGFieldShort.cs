/*
<copyright file="BGFieldShort.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Globalization;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// short Field 
    /// </summary>
    [FieldDescriptor(Name = "short", Folder = "Primitive", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerShort")]
    public class BGFieldShort : BGFieldCachedStructA<short>, BGBinaryBulkLoaderStruct
    {
        public const ushort CodeType = 33;

        /// <inheritdoc/>
        public override ushort TypeCode => CodeType;

        public const int SizeOfTheValue = 2;

        //================================================================================================
        //                                              Properties
        //================================================================================================
        /// <inheritdoc/>
        protected override int ValueSize => SizeOfTheValue;

        /// <inheritdoc/>
        public override bool CanBeUsedAsKey => true;

        //================================================================================================
        //                                              Constructors
        //================================================================================================
        //for new field
        public BGFieldShort(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldShort(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
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
                StoreItems[i] = (short)((array[startIndex + 1] << 8) | array[startIndex]);
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
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldShort(meta, id, name);


        //================================================================================================
        //                                              Static
        //================================================================================================

        public static byte[] ValueToBytes(short value)
        {
            var array = new byte[SizeOfTheValue];
            array[0] = (byte)value;
            array[1] = (byte)(((uint)value >> 8) & 0xFF);
            return array;
        }

        public static short ValueFromBytes(ArraySegment<byte> segment)
        {
            if (segment.Count != SizeOfTheValue) return 0;

            var array = segment.Array;
            var offset = segment.Offset;
            return (short)((array[offset + 1] << 8) | array[offset]);
        }

        public static string ValueToString(short i) => i.ToString(CultureInfo.InvariantCulture);

        public static short ValueFromString(string value) => string.IsNullOrEmpty(value) ? (short)0 : short.Parse(value, NumberStyles.Any, CultureInfo.InvariantCulture);
    }
}