/*
<copyright file="BGFieldGuidNullable.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;


namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// nullable guid Field 
    /// </summary>
    [FieldDescriptor(Name = "guid?", Folder = "Primitive Nullable", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerGuidNullable")]
    public partial class BGFieldGuidNullable : BGFieldCachedStructNullableA<Guid>
    {
        public const ushort CodeType = 39;

        /// <inheritdoc/>
        public override ushort TypeCode => CodeType;

        //================================================================================================
        //                                              Properties
        //================================================================================================
        /// <inheritdoc/>
        public override bool CanBeUsedAsKey => true;

        /// <inheritdoc/>
        protected override int ValueSize => BGFieldGuid.SizeOfTheValue;

        //================================================================================================
        //                                              Constructors
        //================================================================================================
        //for new field
        public BGFieldGuidNullable(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldGuidNullable(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
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
        protected override Guid? ValueFromString(string value)
        {
            try
            {
                return BGFieldGuid.ValueFromString(value);
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
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldGuidNullable(meta, id, name);
    }
}