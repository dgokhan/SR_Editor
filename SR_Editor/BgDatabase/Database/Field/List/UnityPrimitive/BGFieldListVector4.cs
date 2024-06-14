/*
<copyright file="BGFieldListVector4.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Field with list of Vector4 value
    /// </summary>
    [FieldDescriptor(Name = "listVector4", Folder = "List/Unity Primitive", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerListVector4")]
    public partial class BGFieldListVector4 : BGFieldCachedStructListA<Vector4>
    {
        public const ushort CodeType = 24;

        /// <inheritdoc/>
        public override ushort TypeCode => CodeType;

        /// <inheritdoc/>
        protected override int ValueSize => BGFieldVector4.SizeOfTheValue;

        //for new field
        public BGFieldListVector4(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldListVector4(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Serialization
        //================================================================================================
        /// <inheritdoc/>
        protected override byte[] ValueToBytes(Vector4 value) => BGFieldVector4.ValueToBytes(value);

        /// <inheritdoc/>
        protected override Vector4 ValueFromBytes(ArraySegment<byte> segment) => BGFieldVector4.ValueFromBytes(segment);

        protected override Vector4 ValueFromBytes(byte[] array, int offset)
        {
            return new Vector4
            (
                BitConverter.ToSingle(array, offset),
                BitConverter.ToSingle(array, offset + 4),
                BitConverter.ToSingle(array, offset + 8),
                BitConverter.ToSingle(array, offset + 12)
            );
        }
        /// <inheritdoc/>
        protected override string ValueToString(Vector4 value) => BGFieldVector4.ValueToString(value);

        /// <inheritdoc/>
        protected override Vector4 ValueFromString(string value) => BGFieldVector4.ValueFromString(value);

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc/>
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldListVector4(meta, id, name);

        //================================================================================================
        //                                              Value
        //================================================================================================
        /// <inheritdoc/>
        protected override bool AreEqual(Vector4 myValue, Vector4 myValue2)
        {
            return Mathf.Approximately(myValue.x, myValue2.x)
                   && Mathf.Approximately(myValue.y, myValue2.y)
                   && Mathf.Approximately(myValue.z, myValue2.z)
                   && Mathf.Approximately(myValue.w, myValue2.w);
        }
    }
}