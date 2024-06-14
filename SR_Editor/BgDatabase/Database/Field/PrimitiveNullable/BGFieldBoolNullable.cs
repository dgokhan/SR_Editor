/*
<copyright file="BGFieldBoolNullable.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// nullable bool Field 
    /// </summary>
    [FieldDescriptor(Name = "bool?", Folder = "Primitive Nullable", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerBoolNullable")]
    public partial class BGFieldBoolNullable : BGFieldCachedStructNullableA<bool>
    {
        public const ushort CodeType = 36;

        /// <inheritdoc/>
        public override ushort TypeCode => CodeType;

        //for new field
        public BGFieldBoolNullable(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldBoolNullable(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        /// <inheritdoc/>
        protected override int ValueSize => BGFieldBool.SizeOfTheValue;

        /// <inheritdoc/>
        public override bool CanBeUsedAsKey => true;

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
        protected override bool? ValueFromString(string value)
        {
            try
            {
                return BGFieldBool.ValueFromString(value);
            }
            catch
            {
                return null;
            }
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc/>
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldBoolNullable(meta, id, name);
    }
}