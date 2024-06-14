/*
<copyright file="BGFieldVector4Nullable.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// nullable vector4 Field 
    /// </summary>
    [FieldDescriptor(Name = "vector4?", Folder = "Unity Primitive Nullable", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerVector4Nullable")]
    public class BGFieldVector4Nullable : BGFieldCachedStructNullableA<Vector4>
    {
        public const ushort CodeType = 75;

        /// <inheritdoc />
        public override ushort TypeCode => CodeType;

        //for new field
        public BGFieldVector4Nullable(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldVector4Nullable(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        /// <inheritdoc />
        protected override int ValueSize => BGFieldVector4.SizeOfTheValue;

        //================================================================================================
        //                                              Serialization
        //================================================================================================
        /// <inheritdoc />
        protected override byte[] ValueToBytes(Vector4 value) => BGFieldVector4.ValueToBytes(value);

        /// <inheritdoc />
        protected override Vector4 ValueFromBytes(ArraySegment<byte> segment) => BGFieldVector4.ValueFromBytes(segment);

        protected override Vector4 ValueFromBytes(byte[] array, int offset) => new Vector4
        (
            BitConverter.ToSingle(array, offset),
            BitConverter.ToSingle(array, offset + 4),
            BitConverter.ToSingle(array, offset + 8),
            BitConverter.ToSingle(array, offset + 12)
        );

        /// <inheritdoc />
        protected override string ValueToString(Vector4 value) => BGFieldVector4.ValueToString(value);

        /// <inheritdoc />
        protected override Vector4? ValueFromString(string value)
        {
            try
            {
                return BGFieldVector4.ValueFromString(value);
            }
            catch
            {
                return null;
            }
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc />
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldVector4Nullable(meta, id, name);
    }
}