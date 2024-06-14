/*
<copyright file="BGFieldVector2Nullable.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// nullable vector2 Field 
    /// </summary>
    [FieldDescriptor(Name = "vector2?", Folder = "Unity Primitive Nullable", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerVector2Nullable")]
    public partial class BGFieldVector2Nullable : BGFieldCachedStructNullableA<Vector2>
    {
        public const ushort CodeType = 73;

        /// <inheritdoc />
        public override ushort TypeCode => CodeType;

        //for new field
        public BGFieldVector2Nullable(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldVector2Nullable(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        /// <inheritdoc />
        protected override int ValueSize => BGFieldVector2.SizeOfTheValue;

        //================================================================================================
        //                                              Serialization
        //================================================================================================
        /// <inheritdoc />
        protected override byte[] ValueToBytes(Vector2 value) => BGFieldVector2.ValueToBytes(value);

        /// <inheritdoc />
        protected override Vector2 ValueFromBytes(ArraySegment<byte> segment) => BGFieldVector2.ValueFromBytes(segment);

        protected override Vector2 ValueFromBytes(byte[] array, int offset) => new Vector2
        (
            BitConverter.ToSingle(array, offset),
            BitConverter.ToSingle(array, offset + 4)
        );

        /// <inheritdoc />
        protected override string ValueToString(Vector2 value) => BGFieldVector2.ValueToString(value);

        /// <inheritdoc />
        protected override Vector2? ValueFromString(string value)
        {
            try
            {
                return BGFieldVector2.ValueFromString(value);
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
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldVector2Nullable(meta, id, name);
    }
}