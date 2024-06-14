/*
<copyright file="BGFieldIntNullable.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// nullable int Field 
    /// </summary>
    [FieldDescriptor(Name = "int?", Folder = "Primitive Nullable", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerIntNullable")]
    public partial class BGFieldIntNullable : BGFieldCachedStructNullableA<int>
    {
        public const ushort CodeType = 40;

        /// <inheritdoc/>
        public override ushort TypeCode => CodeType;

        //================================================================================================
        //                                              Properties
        //================================================================================================
        /// <inheritdoc/>
        protected override int ValueSize => BGFieldInt.SizeOfTheValue;

        /// <inheritdoc/>
        public override bool CanBeUsedAsKey => true;

        //================================================================================================
        //                                              Constructors
        //================================================================================================
        //for new field
        public BGFieldIntNullable(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldIntNullable(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
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
        protected override int? ValueFromString(string value)
        {
            try
            {
                return BGFieldInt.ValueFromString(value);
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
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldIntNullable(meta, id, name);
    }
}