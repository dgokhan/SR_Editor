/*
<copyright file="BGFieldId.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Field for BGId
    /// </summary>
    [FieldDescriptor(Name = "id", Folder = "Special", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerId")]
    public partial class BGFieldId : BGFieldCachedStructA<BGId>, BGBinaryBulkLoaderStruct
    {
        public const ushort CodeType = 48;
        /// <inheritdoc/>
        public override ushort TypeCode => CodeType;

        public const int Size = 16;

        /// <inheritdoc/>
        protected override int ValueSize => Size;

        //for new field
        public BGFieldId(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldId(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
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
            for (var i = 0; i < entitiesCount; i++) StoreItems[i] = new BGId(array, offset + (Size * i));
        }

        /// <inheritdoc />
        public override string ToString(int entityIndex) => ValueToString(this[entityIndex]);

        /// <inheritdoc />
        public override void FromString(int entityIndex, string value) => this[entityIndex] = ValueFromString(value);

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc/>
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldId(meta, id, name);

        //================================================================================================
        //                                              Static
        //================================================================================================

        public static byte[] ValueToBytes(BGId id) => id.ToByteArray();

        public static BGId ValueFromBytes(ArraySegment<byte> segment)
        {
            if (segment.Count != Size) return BGId.Empty;
            return new BGId(segment.Array, segment.Offset);
        }

        public static string ValueToString(BGId id) => id.ToString();

        public static BGId ValueFromString(string value) => string.IsNullOrEmpty(value) ? BGId.Empty : new BGId(value);
    }
}