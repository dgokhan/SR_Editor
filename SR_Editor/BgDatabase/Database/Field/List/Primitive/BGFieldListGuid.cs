/*
<copyright file="BGFieldListGuid.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Field with list of Guids values
    /// </summary>
    [FieldDescriptor(Name = "listGuid", Folder = "List/Primitive", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerListGuid")]
    public partial class BGFieldListGuid : BGFieldCachedStructListA<Guid>
    {
        public const ushort CodeType = 16;

        /// <inheritdoc/>
        public override ushort TypeCode => CodeType;

        /// <inheritdoc/>
        protected override int ValueSize => BGFieldGuid.SizeOfTheValue;

        //for new field
        public BGFieldListGuid(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldListGuid(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Serialization
        //================================================================================================
        /// <inheritdoc/>
        protected override byte[] ValueToBytes(Guid value) => BGFieldGuid.ValueToBytes(value);

        /// <inheritdoc/>
        protected override Guid ValueFromBytes(ArraySegment<byte> segment) => BGFieldGuid.ValueFromBytes(segment);
        protected override Guid ValueFromBytes(byte[] array, int offset)
        {
            var tempArray = new byte[BGFieldGuid.SizeOfTheValue];
            Buffer.BlockCopy(array, offset, tempArray, 0, BGFieldGuid.SizeOfTheValue);
            return new Guid(tempArray);
        }
        /// <inheritdoc/>
        protected override string ValueToString(Guid value) => BGFieldGuid.ValueToString(value);

        /// <inheritdoc/>
        protected override Guid ValueFromString(string value) => BGFieldGuid.ValueFromString(value);

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc/>
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldListGuid(meta, id, name);

        //================================================================================================
        //                                              Value
        //================================================================================================
        /// <inheritdoc/>
        protected override bool AreEqual(Guid myValue, Guid myValue2) => myValue == myValue2;
    }
}