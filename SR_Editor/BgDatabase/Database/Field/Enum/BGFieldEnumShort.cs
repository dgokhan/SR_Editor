/*
<copyright file="BGFieldEnumShort.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// enum Field with short underlying type 
    /// </summary>
    [FieldDescriptor(Name = "enumShort", Folder = "Enum", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerEnumShort")]
    public class BGFieldEnumShort : BGFieldEnumA<short>, BGBinaryBulkLoaderStruct
    {
        public const ushort CodeType = 12;
        /// <inheritdoc/>
        public override ushort TypeCode => CodeType;

        //================================================================================================
        //                                              Fields
        //================================================================================================
        /// <inheritdoc/>
        protected override int ValueSize => BGFieldShort.SizeOfTheValue;

        //================================================================================================
        //                                              Constructors
        //================================================================================================
        public BGFieldEnumShort(BGMetaEntity meta, string name, Type enumType) : base(meta, name, enumType)
        {
        }

        internal BGFieldEnumShort(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc/>
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldEnumShort(meta, id, name);

        //================================================================================================
        //                                              Value
        //================================================================================================
        /// <inheritdoc/>
        protected override Enum StoredValueToEnum(short value) => (Enum)Enum.ToObject(EnumType, value);

        /// <inheritdoc/>
        protected override short EnumToStoredValue(Enum value) => Convert.ToInt16(value);

        //================================================================================================
        //                                              Serialization
        //================================================================================================
        /// <inheritdoc />
        public override byte[] ToBytes(int entityIndex)
        {
            var value = GetStoredValue(entityIndex);
            var array = new byte[ValueSize];
            array[0] = (byte)(value & 255);
            array[1] = (byte)(value >> 8);
            return array;
        }

        /// <inheritdoc />
        public override void FromBytes(int entityIndex, ArraySegment<byte> segment)
        {
            if (segment.Count != ValueSize) return;

            var array = segment.Array;
            var offset = segment.Offset;
            var value = (short)((array[offset + 1] << 8) + array[offset]);
            SetStoredValue(entityIndex, value);
        }
        
        /// <inheritdoc />
        public void FromBytes(BGBinaryBulkRequestStruct request)
        {
            var array = request.Array;
            var offset = request.Offset;
            var entitiesCount = request.EntitiesCount;
            for (var i = 0; i < entitiesCount; i++)
            {
                var startIndex = offset + (BGFieldShort.SizeOfTheValue * i);
                StoreItems[i] = (short)((array[startIndex + 1] << 8) | array[startIndex]);
            }
        }
        //================================================================================================
        //                                              Utilities
        //================================================================================================
    }
}