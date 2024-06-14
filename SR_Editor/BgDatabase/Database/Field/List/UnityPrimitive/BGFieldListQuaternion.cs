/*
<copyright file="BGFieldListQuaternion.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Field with list of quaternions value
    /// </summary>
    [FieldDescriptor(Name = "listQuaternion", Folder = "List/Unity Primitive", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerListQuaternion")]
    public partial class BGFieldListQuaternion : BGFieldCachedStructListA<Quaternion>
    {
        public const ushort CodeType = 21;

        /// <inheritdoc/>
        public override ushort TypeCode => CodeType;

        /// <inheritdoc/>
        protected override int ValueSize => BGFieldQuaternion.SizeOfTheValue;

        //for new field
        public BGFieldListQuaternion(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldListQuaternion(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Serialization
        //================================================================================================
        /// <inheritdoc/>
        protected override byte[] ValueToBytes(Quaternion value) => BGFieldQuaternion.ValueToBytes(value);

        /// <inheritdoc/>
        protected override Quaternion ValueFromBytes(ArraySegment<byte> segment) => BGFieldQuaternion.ValueFromBytes(segment);
        protected override Quaternion ValueFromBytes(byte[] array, int offset)
        {
            return new Quaternion
            (
                BitConverter.ToSingle(array, offset),
                BitConverter.ToSingle(array, offset + 4),
                BitConverter.ToSingle(array, offset + 8),
                BitConverter.ToSingle(array, offset + 12)
            );
        }
        /// <inheritdoc/>
        protected override string ValueToString(Quaternion value) => BGFieldQuaternion.ValueToString(value);

        /// <inheritdoc/>
        protected override Quaternion ValueFromString(string value) => BGFieldQuaternion.ValueFromString(value);

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc/>
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldListQuaternion(meta, id, name);

        //================================================================================================
        //                                              Value
        //================================================================================================
        /// <inheritdoc/>
        protected override bool AreEqual(Quaternion myValue, Quaternion myValue2)
        {
            return Mathf.Approximately(myValue.x, myValue2.x)
                   && Mathf.Approximately(myValue.y, myValue2.y)
                   && Mathf.Approximately(myValue.z, myValue2.z)
                   && Mathf.Approximately(myValue.w, myValue2.w);
        }
    }
}