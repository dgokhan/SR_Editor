/*
<copyright file="BGFieldColorNullable.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// nullable color Field 
    /// </summary>
    [FieldDescriptor(Name = "color?", Folder = "Unity Primitive Nullable", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerColorNullable")]
    public class BGFieldColorNullable : BGFieldCachedStructNullableA<Color>
    {
        public const ushort CodeType = 71;

        /// <inheritdoc />
        public override ushort TypeCode => CodeType;

        //for new field
        public BGFieldColorNullable(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldColorNullable(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        /// <inheritdoc />
        protected override int ValueSize => BGFieldColor.SizeOfTheValue;

        //================================================================================================
        //                                              Serialization
        //================================================================================================
        /// <inheritdoc />
        protected override byte[] ValueToBytes(Color value) => BGFieldColor.ValueToBytes(value);

        /// <inheritdoc />
        protected override Color ValueFromBytes(ArraySegment<byte> segment) => BGFieldColor.ValueFromBytes(segment);

        protected override Color ValueFromBytes(byte[] array, int offset) => new Color
        (
            BitConverter.ToSingle(array, offset),
            BitConverter.ToSingle(array, offset + 4),
            BitConverter.ToSingle(array, offset + 8),
            BitConverter.ToSingle(array, offset + 12)
        );

        /// <inheritdoc />
        protected override string ValueToString(Color value) => BGFieldColor.ValueToString(value);

        /// <inheritdoc />
        protected override Color? ValueFromString(string value)
        {
            try
            {
                return BGFieldColor.ValueFromString(value);
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
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldColorNullable(meta, id, name);
    }
}