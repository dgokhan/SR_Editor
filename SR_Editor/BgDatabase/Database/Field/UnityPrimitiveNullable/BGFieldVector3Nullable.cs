/*
<copyright file="BGFieldVector3Nullable.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// nullable vector3 Field 
    /// </summary>
    [FieldDescriptor(Name = "vector3?", Folder = "Unity Primitive Nullable", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerVector3Nullable")]
    public class BGFieldVector3Nullable : BGFieldCachedStructNullableA<Vector3>
    {
        public const ushort CodeType = 74;

        /// <inheritdoc />
        public override ushort TypeCode => CodeType;

        //for new field
        public BGFieldVector3Nullable(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldVector3Nullable(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        /// <inheritdoc />
        protected override int ValueSize => BGFieldVector3.SizeOfTheValue;

        //================================================================================================
        //                                              Serialization
        //================================================================================================
        /// <inheritdoc />
        protected override byte[] ValueToBytes(Vector3 value) => BGFieldVector3.ValueToBytes(value);

        /// <inheritdoc />
        protected override Vector3 ValueFromBytes(ArraySegment<byte> segment) => BGFieldVector3.ValueFromBytes(segment);

        protected override Vector3 ValueFromBytes(byte[] array, int offset) => new Vector3
        (
            BitConverter.ToSingle(array, offset),
            BitConverter.ToSingle(array, offset + 4),
            BitConverter.ToSingle(array, offset + 8)
        );

        /// <inheritdoc />
        protected override string ValueToString(Vector3 value) => BGFieldVector3.ValueToString(value);

        /// <inheritdoc />
        protected override Vector3? ValueFromString(string value)
        {
            try
            {
                return BGFieldVector3.ValueFromString(value);
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
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldVector3Nullable(meta, id, name);
    }
}