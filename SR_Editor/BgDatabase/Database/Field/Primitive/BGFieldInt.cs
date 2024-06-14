/*
<copyright file="BGFieldInt.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Globalization;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// int Field 
    /// </summary>
    [FieldDescriptor(Name = "int", Folder = "Primitive", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerInt")]
    public partial class BGFieldInt : BGFieldCachedStructA<int>, BGBinaryBulkLoaderStruct
    {
        public const ushort CodeType = 31;

        /// <inheritdoc/>
        public override ushort TypeCode => CodeType;

        public const int SizeOfTheValue = 4;

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
        public BGFieldInt(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldInt(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
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
                StoreItems[i] = (array[startIndex + 3] << 24) | (array[startIndex + 2] << 16) | (array[startIndex + 1] << 8) | array[startIndex];
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
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldInt(meta, id, name);


        //================================================================================================
        //                                              Static
        //================================================================================================

        public static byte[] ValueToBytes(int value)
        {
            var array = new byte[SizeOfTheValue];
            array[0] = (byte)value;
            array[1] = (byte)(((uint)value >> 8) & 0xFF);
            array[2] = (byte)(((uint)value >> 16) & 0xFF);
            array[3] = (byte)(((uint)value >> 24) & 0xFF);
            return array;
        }

        public static int ValueFromBytes(ArraySegment<byte> segment)
        {
            if (segment.Count != SizeOfTheValue) return 0;

            var array = segment.Array;
            var offset = segment.Offset;
            return (array[offset + 3] << 24) | (array[offset + 2] << 16) | (array[offset + 1] << 8) | array[offset];
        }

        public static string ValueToString(int i) => i.ToString(CultureInfo.InvariantCulture);

        public static int ValueFromString(string value) => string.IsNullOrEmpty(value) ? 0 : int.Parse(value, NumberStyles.Any, CultureInfo.InvariantCulture);
    }
}