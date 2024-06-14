/*
<copyright file="BGFieldLongNullable.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// nullable long Field 
    /// </summary>
    [FieldDescriptor(Name = "long?", Folder = "Primitive Nullable", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerLongNullable")]
    public partial class BGFieldLongNullable : BGFieldCachedStructNullableA<long>
    {
        public const ushort CodeType = 41;

        /// <inheritdoc/>
        public override ushort TypeCode => CodeType;

        /// <inheritdoc/>
        public override bool CanBeUsedAsKey => true;

        /// <inheritdoc/>
        protected override int ValueSize => BGFieldLong.SizeOfTheValue;

        //for new field
        public BGFieldLongNullable(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldLongNullable(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Serialization
        //================================================================================================
        /// <inheritdoc/>
        protected override byte[] ValueToBytes(long value) => BGFieldLong.ValueToBytes(value);

        /// <inheritdoc/>
        protected override long ValueFromBytes(ArraySegment<byte> segment) => BGFieldLong.ValueFromBytes(segment);

        protected override long ValueFromBytes(byte[] array, int offset) => ((long)array[offset + 7] << 56) | 
                                                                            ((long)array[offset + 6] << 48) | 
                                                                            ((long)array[offset + 5] << 40) |
                                                                            ((long)array[offset + 4] << 32) |
                                                                            ((long)array[offset + 3] << 24) | 
                                                                            ((long)array[offset + 2] << 16) | 
                                                                            ((long)array[offset + 1] << 8) | 
                                                                            (long)array[offset];

        /// <inheritdoc/>
        protected override string ValueToString(long value) => BGFieldLong.ValueToString(value);

        /// <inheritdoc/>
        protected override long? ValueFromString(string value)
        {
            try
            {
                return BGFieldLong.ValueFromString(value);
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
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldLongNullable(meta, id, name);
    }
}