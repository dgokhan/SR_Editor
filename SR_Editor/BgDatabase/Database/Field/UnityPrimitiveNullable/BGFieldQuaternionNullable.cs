/*
<copyright file="BGFieldQuaternionNullable.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// nullable quaternion Field 
    /// </summary>
    [FieldDescriptor(Name = "quaternion?", Folder = "Unity Primitive Nullable", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerQuaternionNullable")]
    public class BGFieldQuaternionNullable : BGFieldCachedStructNullableA<Quaternion>
    {
        public const ushort CodeType = 72;

        /// <inheritdoc />
        public override ushort TypeCode => CodeType;

        //for new field
        public BGFieldQuaternionNullable(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldQuaternionNullable(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        /// <inheritdoc />
        protected override int ValueSize => BGFieldQuaternion.SizeOfTheValue;

        //================================================================================================
        //                                              Serialization
        //================================================================================================
        /// <inheritdoc />
        protected override byte[] ValueToBytes(Quaternion value) => BGFieldQuaternion.ValueToBytes(value);

        /// <inheritdoc />
        protected override Quaternion ValueFromBytes(ArraySegment<byte> segment) => BGFieldQuaternion.ValueFromBytes(segment);

        protected override Quaternion ValueFromBytes(byte[] array, int offset) => new Quaternion
        (
            BitConverter.ToSingle(array, offset),
            BitConverter.ToSingle(array, offset + 4),
            BitConverter.ToSingle(array, offset + 8),
            BitConverter.ToSingle(array, offset + 12)
        );

        /// <inheritdoc />
        protected override string ValueToString(Quaternion value) => BGFieldQuaternion.ValueToString(value);

        /// <inheritdoc />
        protected override Quaternion? ValueFromString(string value)
        {
            try
            {
                return BGFieldQuaternion.ValueFromString(value);
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
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldQuaternionNullable(meta, id, name);
    }
}