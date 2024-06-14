/*
<copyright file="BGFieldListBool.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Field with list of bools values
    /// </summary>
    [FieldDescriptor(Name = "listBool", Folder = "List/Primitive", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerListBool")]
    public partial class BGFieldListBool : BGFieldCachedStructListA<bool>
    {
        public const ushort CodeType = 13;

        /// <inheritdoc/>
        public override ushort TypeCode => CodeType;

        /// <inheritdoc/>
        protected override int ValueSize => BGFieldBool.SizeOfTheValue;

        //for new field
        public BGFieldListBool(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldListBool(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Serialization
        //================================================================================================
        /// <inheritdoc/>
        protected override byte[] ValueToBytes(bool value) => BGFieldBool.ValueToBytes(value);

        /// <inheritdoc/>
        protected override bool ValueFromBytes(ArraySegment<byte> segment) => BGFieldBool.ValueFromBytes(segment);
        protected override bool ValueFromBytes(byte[] array, int offset) => array[offset] != 0;

        /// <inheritdoc/>
        protected override string ValueToString(bool value) => BGFieldBool.ValueToString(value);

        /// <inheritdoc/>
        protected override bool ValueFromString(string value) => BGFieldBool.ValueFromString(value);

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc/>
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldListBool(meta, id, name);

        //================================================================================================
        //                                              Value
        //================================================================================================
        /// <inheritdoc/>
        protected override bool AreEqual(bool myValue, bool myValue2) => myValue == myValue2;
    }
}