/*
<copyright file="BGFieldByte.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


using System;
using System.Globalization;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// byte Field 
    /// </summary>
    [FieldDescriptor(Name = "byte", Folder = "Primitive", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerByte")]
    public class BGFieldByte : BGFieldCachedStructA<byte>, BGBinaryBulkLoaderStruct
    {
        public const ushort CodeType = 26;

        /// <inheritdoc/>
        public override ushort TypeCode => CodeType;

        public const int SizeOfTheValue = 1;

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
        public BGFieldByte(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldByte(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
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
            for (var i = 0; i < entitiesCount; i++) StoreItems[i] = array[offset + i];
        }

        /// <inheritdoc />
        public override string ToString(int entityIndex) => ValueToString(this[entityIndex]);

        /// <inheritdoc />
        public override void FromString(int entityIndex, string value) => this[entityIndex] = ValueFromString(value);

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc/>
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldByte(meta, id, name);


        //================================================================================================
        //                                              Static
        //================================================================================================

        public static byte[] ValueToBytes(byte value)
        {
            var array = new byte[SizeOfTheValue];
            array[0] = (byte)value;
            return array;
        }

        public static byte ValueFromBytes(ArraySegment<byte> segment)
        {
            if (segment.Count != SizeOfTheValue) return 0;

            var array = segment.Array;
            var offset = segment.Offset;
            return (byte)array[offset];
        }

        public static string ValueToString(byte i) => i.ToString(CultureInfo.InvariantCulture);

        public static byte ValueFromString(string value) => string.IsNullOrEmpty(value) ? (byte)0 : byte.Parse(value, NumberStyles.Any, CultureInfo.InvariantCulture);
    }
}