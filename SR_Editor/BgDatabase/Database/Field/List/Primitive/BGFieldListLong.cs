/*
<copyright file="BGFieldListLong.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Field with list of longs value
    /// </summary>
    [FieldDescriptor(Name = "listLong", Folder = "List/Primitive", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerListLong")]
    public partial class BGFieldListLong : BGFieldCachedStructListA<long>
    {
        public const ushort CodeType = 18;

        /// <inheritdoc/>
        public override ushort TypeCode => CodeType;

        /// <inheritdoc/>
        protected override int ValueSize => BGFieldLong.SizeOfTheValue;

        //for new field
        public BGFieldListLong(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldListLong(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
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
        protected override long ValueFromString(string value) => BGFieldLong.ValueFromString(value);

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc/>
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldListLong(meta, id, name);

        //================================================================================================
        //                                              Value
        //================================================================================================
        /// <inheritdoc/>
        protected override bool AreEqual(long myValue, long myValue2) => myValue == myValue2;
    }
}