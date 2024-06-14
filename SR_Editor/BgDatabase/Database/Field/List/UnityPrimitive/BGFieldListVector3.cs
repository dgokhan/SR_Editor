/*
<copyright file="BGFieldListVector3.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Field with list of Vector3 value
    /// </summary>
    [FieldDescriptor(Name = "listVector3", Folder = "List/Unity Primitive", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerListVector3")]
    public partial class BGFieldListVector3 : BGFieldCachedStructListA<Vector3>
    {
        public const ushort CodeType = 23;

        /// <inheritdoc/>
        public override ushort TypeCode => CodeType;

        /// <inheritdoc/>
        protected override int ValueSize => BGFieldVector3.SizeOfTheValue;

        //for new field
        public BGFieldListVector3(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldListVector3(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Serialization
        //================================================================================================
        /// <inheritdoc/>
        protected override byte[] ValueToBytes(Vector3 value) => BGFieldVector3.ValueToBytes(value);

        /// <inheritdoc/>
        protected override Vector3 ValueFromBytes(ArraySegment<byte> segment) => BGFieldVector3.ValueFromBytes(segment);
        protected override Vector3 ValueFromBytes(byte[] array, int offset)
        {
            return new Vector3
            (
                BitConverter.ToSingle(array, offset),
                BitConverter.ToSingle(array, offset + 4),
                BitConverter.ToSingle(array, offset + 8)
            );
        }
        /// <inheritdoc/>
        protected override string ValueToString(Vector3 value) => BGFieldVector3.ValueToString(value);

        /// <inheritdoc/>
        protected override Vector3 ValueFromString(string value) => BGFieldVector3.ValueFromString(value);

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc/>
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldListVector3(meta, id, name);

        //================================================================================================
        //                                              Value
        //================================================================================================
        /// <inheritdoc/>
        protected override bool AreEqual(Vector3 myValue, Vector3 myValue2)
        {
            return Mathf.Approximately(myValue.x, myValue2.x)
                   && Mathf.Approximately(myValue.y, myValue2.y)
                   && Mathf.Approximately(myValue.z, myValue2.z);
        }
    }
}