/*
<copyright file="BGFieldGuid.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Guid Field 
    /// </summary>
    [FieldDescriptor(Name = "guid", Folder = "Primitive", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerGuid")]
    public partial class BGFieldGuid : BGFieldCachedStructA<Guid>, BGBinaryBulkLoaderStruct
    {
        public const ushort CodeType = 30;

        /// <inheritdoc/>
        public override ushort TypeCode => CodeType;

        public const int SizeOfTheValue = 16;

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
        public BGFieldGuid(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldGuid(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
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
            var tempArray = new byte[SizeOfTheValue];
            for (var i = 0; i < entitiesCount; i++)
            {
                var startIndex = offset + (SizeOfTheValue * i);
                Buffer.BlockCopy(array, startIndex, tempArray, 0, SizeOfTheValue);
                StoreItems[i] = new Guid(tempArray);
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
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldGuid(meta, id, name);

        //================================================================================================
        //                                              Static
        //================================================================================================

        public static byte[] ValueToBytes(Guid guid) => guid.ToByteArray();

        public static Guid ValueFromBytes(ArraySegment<byte> segment) => segment.Count != SizeOfTheValue ? Guid.Empty : new Guid(BGUtil.ToArray(segment));

        public static string ValueToString(Guid guid) => guid.ToString();

        public static Guid ValueFromString(string value) => string.IsNullOrEmpty(value) ? Guid.Empty : new Guid(value);
    }
}