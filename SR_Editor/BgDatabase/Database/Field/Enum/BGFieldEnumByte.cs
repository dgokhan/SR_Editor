/*
<copyright file="BGFieldEnumByte.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// enum Field with byte underlying type 
    /// </summary>
    [FieldDescriptor(Name = "enumByte", Folder = "Enum", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerEnumByte")]
    public class BGFieldEnumByte : BGFieldEnumA<byte>, BGBinaryBulkLoaderStruct
    {
        public const ushort CodeType = 10;
        /// <inheritdoc/>
        public override ushort TypeCode => CodeType;

        //================================================================================================
        //                                              Fields
        //================================================================================================
        /// <inheritdoc/>
        protected override int ValueSize => 1;

        //================================================================================================
        //                                              Constructors
        //================================================================================================
        public BGFieldEnumByte(BGMetaEntity meta, string name, Type enumType) : base(meta, name, enumType)
        {
        }

        internal BGFieldEnumByte(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc/>
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldEnumByte(meta, id, name);

        //================================================================================================
        //                                              Value
        //================================================================================================
        /// <inheritdoc/>
        protected override Enum StoredValueToEnum(byte value) => (Enum)Enum.ToObject(EnumType, value);

        /// <inheritdoc/>
        protected override byte EnumToStoredValue(Enum value) => Convert.ToByte(value);

        //================================================================================================
        //                                              Serialization
        //================================================================================================
        /// <inheritdoc />
        public override byte[] ToBytes(int entityIndex) => new[] { GetStoredValue(entityIndex) };

        /// <inheritdoc />
        public override void FromBytes(int entityIndex, ArraySegment<byte> segment)
        {
            if (segment.Count != ValueSize) return;
            var array = segment.Array;
            var offset = segment.Offset;
            var result = array[offset];
            SetStoredValue(entityIndex, result);
        }
        
        /// <inheritdoc />
        public void FromBytes(BGBinaryBulkRequestStruct request)
        {
            var array = request.Array;
            var offset = request.Offset;
            var entitiesCount = request.EntitiesCount;
            for (var i = 0; i < entitiesCount; i++) StoreItems[i] = array[offset + i];
        }
        //================================================================================================
        //                                              Utilities
        //================================================================================================
    }
}