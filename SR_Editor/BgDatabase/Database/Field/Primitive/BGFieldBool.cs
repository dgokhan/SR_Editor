/*
<copyright file="BGFieldBool.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// bool Field 
    /// </summary>
    [FieldDescriptor(Name = "bool", Folder = "Primitive", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerBool")]
    public partial class BGFieldBool : BGFieldCachedStructA<bool>, BGBinaryBulkLoaderStruct
    {
        public const ushort CodeType = 25;

        /// <inheritdoc/>
        public override ushort TypeCode => CodeType;

        public const int SizeOfTheValue = 1;

        /// <inheritdoc/>
        protected override int ValueSize => SizeOfTheValue;

        /// <inheritdoc/>
        public override bool CanBeUsedAsKey => true;

        //for new field
        public BGFieldBool(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldBool(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
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
            for (var i = 0; i < entitiesCount; i++) StoreItems[i] = array[offset + i] != 0;
        }

        /// <inheritdoc />
        public override string ToString(int entityIndex) => ValueToString(this[entityIndex]);

        /// <inheritdoc />
        public override void FromString(int entityIndex, string value) => this[entityIndex] = ValueFromString(value);

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc/>
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldBool(meta, id, name);


        //================================================================================================
        //                                              Static
        //================================================================================================
        public static byte[] ValueToBytes(bool value) => new[] { value ? (byte)1 : (byte)0 };

        public static bool ValueFromBytes(ArraySegment<byte> segment) => segment.Count == SizeOfTheValue && segment.Array[segment.Offset] != (byte)0;

        public static string ValueToString(bool b) => b ? "1" : "0";

        public static bool ValueFromString(string value) => "1".Equals(value);
    }
}