/*
<copyright file="BGFieldListDouble.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Field with list of doubles values
    /// </summary>
    [FieldDescriptor(Name = "listDouble", Folder = "List/Primitive", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerListDouble")]
    public partial class BGFieldListDouble : BGFieldCachedStructListA<double>
    {
        public const ushort CodeType = 14;

        /// <inheritdoc/>
        public override ushort TypeCode => CodeType;

        /// <inheritdoc/>
        protected override int ValueSize => BGFieldDouble.SizeOfTheValue;

        //for new field
        public BGFieldListDouble(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldListDouble(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

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
        protected override double ValueFromString(string value) => BGFieldDouble.ValueFromString(value);

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc/>
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldListDouble(meta, id, name);

        //================================================================================================
        //                                              Value
        //================================================================================================
        /// <inheritdoc/>
        protected override bool AreEqual(double myValue, double myValue2) => Math.Abs(myValue - myValue2) < 0.00001;
    }
}