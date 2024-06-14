/*
<copyright file="BGFieldListInt.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Field with list of ints value
    /// </summary>
    [FieldDescriptor(Name = "listInt", Folder = "List/Primitive", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerListInt")]
    public partial class BGFieldListInt : BGFieldCachedStructListA<int>
    {
        public const ushort CodeType = 17;

        /// <inheritdoc/>
        public override ushort TypeCode => CodeType;

        /// <inheritdoc/>
        protected override int ValueSize => BGFieldInt.SizeOfTheValue;

        //for new field
        public BGFieldListInt(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldListInt(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Serialization
        //================================================================================================
        /// <inheritdoc/>
        protected override byte[] ValueToBytes(int value) => BGFieldInt.ValueToBytes(value);

        /// <inheritdoc/>
        protected override int ValueFromBytes(ArraySegment<byte> segment) => BGFieldInt.ValueFromBytes(segment);

        protected override int ValueFromBytes(byte[] array, int offset) => (array[offset + 3] << 24) | (array[offset + 2] << 16) | (array[offset + 1] << 8) | array[offset];

        /// <inheritdoc/>
        protected override string ValueToString(int value) => BGFieldInt.ValueToString(value);

        /// <inheritdoc/>
        protected override int ValueFromString(string value) => BGFieldInt.ValueFromString(value);

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc/>
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldListInt(meta, id, name);

        //================================================================================================
        //                                              Value
        //================================================================================================
        /// <inheritdoc/>
        protected override bool AreEqual(int myValue, int myValue2) => myValue == myValue2;
    }
}