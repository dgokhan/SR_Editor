/*
<copyright file="BGFieldDoubleNullable.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;


namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// nullable double Field 
    /// </summary>
    [FieldDescriptor(Name = "double?", Folder = "Primitive Nullable", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerDoubleNullable")]
    public partial class BGFieldDoubleNullable : BGFieldCachedStructNullableA<double>
    {
        public const ushort CodeType = 37;

        /// <inheritdoc/>
        public override ushort TypeCode => CodeType;

        //for new field
        public BGFieldDoubleNullable(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldDoubleNullable(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        /// <inheritdoc/>
        protected override int ValueSize => BGFieldDouble.SizeOfTheValue;

        //================================================================================================
        //                                              Serialization
        //================================================================================================
        /// <inheritdoc/>
        protected override byte[] ValueToBytes(double value) => BGFieldDouble.ValueToBytes(value);

        /// <inheritdoc/>
        protected override double ValueFromBytes(ArraySegment<byte> segment) => BGFieldDouble.ValueFromBytes(segment);

        protected override double ValueFromBytes(byte[] array, int offset) => BitConverter.ToDouble(array, offset);

        /// <inheritdoc/>
        protected override string ValueToString(double value) => BGFieldDouble.ValueToString(value);

        /// <inheritdoc/>
        protected override double? ValueFromString(string value)
        {
            try
            {
                return BGFieldDouble.ValueFromString(value);
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
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldDoubleNullable(meta, id, name);
    }
}