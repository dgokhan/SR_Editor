/*
<copyright file="BGFieldReferenceA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// abstract Field for referencing object(s) from unity scene 
    /// </summary>
    public abstract partial class BGFieldReferenceA<T> : BGFieldCachedA<T, BGId>, BGSceneObjectReferenceI, BGBinaryBulkLoaderStruct
    {
        private const int Size = BGFieldId.Size;

        /// <inheritdoc />
        public override int ConstantSize => Size;

        /// <inheritdoc />
        public override bool StoredValueIsTheSameAsValueType => false;


        protected BGFieldReferenceA(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        protected BGFieldReferenceA(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }


        //================================================================================================
        //                                              Synchronization
        //================================================================================================
        /// <inheritdoc />
        public override byte[] ToBytes(int entityIndex) => GetStoredValue(entityIndex).ToByteArray();

        /// <inheritdoc />
        public override void FromBytes(int entityIndex, ArraySegment<byte> segment) => StoreItems[entityIndex] = new BGId(segment.Array, segment.Offset);

        /// <inheritdoc />
        public void FromBytes(BGBinaryBulkRequestStruct request)
        {
            var array = request.Array;
            var offset = request.Offset;
            var entitiesCount = request.EntitiesCount;
            for (var i = 0; i < entitiesCount; i++) StoreItems[i] = new BGId(array, offset + (Size * i));
        }

        /// <inheritdoc />
        public override string ToString(int entityIndex) => GetStoredValue(entityIndex).ToString();

        /// <inheritdoc />
        public override void FromString(int entityIndex, string value) => StoreItems[entityIndex] = new BGId(value);
    }
}