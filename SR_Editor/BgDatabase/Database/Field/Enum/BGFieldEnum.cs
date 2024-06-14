/*
<copyright file="BGFieldEnum.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// enum Field with int underlying type 
    /// </summary>
    [FieldDescriptor(Name = "enum", Folder = "Enum", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerEnum")]
    public class BGFieldEnum : BGFieldEnumA<int>, BGBinaryBulkLoaderStruct
    {
        public const ushort CodeType = 9;
        /// <inheritdoc/>
        public override ushort TypeCode => CodeType;

        //================================================================================================
        //                                              Fields
        //================================================================================================
        /// <inheritdoc/>
        protected override int ValueSize => BGFieldInt.SizeOfTheValue;

        //================================================================================================
        //                                              Constructors
        //================================================================================================
        public BGFieldEnum(BGMetaEntity meta, string name, Type enumType) : base(meta, name, enumType)
        {
        }

        internal BGFieldEnum(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc/>
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldEnum(meta, id, name);

        //================================================================================================
        //                                              Value
        //================================================================================================
        /// <inheritdoc/>
        protected override Enum StoredValueToEnum(int value) => (Enum)Enum.ToObject(EnumType, value);

        /// <inheritdoc/>
        protected override int EnumToStoredValue(Enum value) => Convert.ToInt32(value);

        //================================================================================================
        //                                              Serialization
        //================================================================================================
        /// <inheritdoc />
        public override byte[] ToBytes(int entityIndex) => BGFieldInt.ValueToBytes(GetStoredValue(entityIndex));

        /// <inheritdoc />
        public override void FromBytes(int entityIndex, ArraySegment<byte> segment)
        {
            if (segment.Count != BGFieldInt.SizeOfTheValue) return;
            SetStoredValue(entityIndex, BGFieldInt.ValueFromBytes(segment));
        }

        /// <inheritdoc />
        public void FromBytes(BGBinaryBulkRequestStruct request)
        {
            var array = request.Array;
            var offset = request.Offset;
            var entitiesCount = request.EntitiesCount;
            for (var i = 0; i < entitiesCount; i++)
            {
                var startIndex = offset + (BGFieldInt.SizeOfTheValue * i);
                StoreItems[i] = (array[startIndex + 3] << 24) | (array[startIndex + 2] << 16) | (array[startIndex + 1] << 8) | array[startIndex];
            }
        }
        //================================================================================================
        //                                              Utilities
        //================================================================================================
    }
}