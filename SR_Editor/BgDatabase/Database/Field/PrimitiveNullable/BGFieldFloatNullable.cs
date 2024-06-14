/*
<copyright file="BGFieldFloatNullable.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// nullable float Field 
    /// </summary>
    [FieldDescriptor(Name = "float?", Folder = "Primitive Nullable", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerFloatNullable")]
    public partial class BGFieldFloatNullable : BGFieldCachedStructNullableA<float>
    {
        public const ushort CodeType = 38;

        /// <inheritdoc/>
        public override ushort TypeCode => CodeType;

        //for new field
        public BGFieldFloatNullable(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldFloatNullable(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        /// <inheritdoc/>
        protected override int ValueSize => BGFieldFloat.SizeOfTheValue;

        //================================================================================================
        //                                              Serialization
        //================================================================================================
        /// <inheritdoc/>
        protected override byte[] ValueToBytes(float value) => BGFieldFloat.ValueToBytes(value);

        /// <inheritdoc/>
        protected override float ValueFromBytes(ArraySegment<byte> segment) => BGFieldFloat.ValueFromBytes(segment);
        protected override float ValueFromBytes(byte[] array, int offset) => BitConverter.ToSingle(array, offset);
        /// <inheritdoc/>
        protected override string ValueToString(float value) => BGFieldFloat.ValueToString(value);

        /// <inheritdoc/>
        protected override float? ValueFromString(string value)
        {
            try
            {
                return BGFieldFloat.ValueFromString(value);
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
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldFloatNullable(meta, id, name);
    }
}