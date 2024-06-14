/*
<copyright file="BGFieldListVector2.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Field with list of Vector2 value
    /// </summary>
    [FieldDescriptor(Name = "listVector2", Folder = "List/Unity Primitive", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerListVector2")]
    public partial class BGFieldListVector2 : BGFieldCachedStructListA<Vector2>
    {
        public const ushort CodeType = 22;

        /// <inheritdoc/>
        public override ushort TypeCode => CodeType;

        /// <inheritdoc/>
        protected override int ValueSize => BGFieldVector2.SizeOfTheValue;

        //for new field
        public BGFieldListVector2(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldListVector2(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Serialization
        //================================================================================================
        /// <inheritdoc/>
        protected override byte[] ValueToBytes(Vector2 value) => BGFieldVector2.ValueToBytes(value);

        /// <inheritdoc/>
        protected override Vector2 ValueFromBytes(ArraySegment<byte> segment) => BGFieldVector2.ValueFromBytes(segment);
        protected override Vector2 ValueFromBytes(byte[] array, int offset)
        {
            return new Vector2
            (
                BitConverter.ToSingle(array, offset),
                BitConverter.ToSingle(array, offset + 4)
            );
        }
        /// <inheritdoc/>
        protected override string ValueToString(Vector2 value) => BGFieldVector2.ValueToString(value);

        /// <inheritdoc/>
        protected override Vector2 ValueFromString(string value) => BGFieldVector2.ValueFromString(value);

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc/>
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldListVector2(meta, id, name);

        //================================================================================================
        //                                              Value
        //================================================================================================
        /// <inheritdoc/>
        protected override bool AreEqual(Vector2 myValue, Vector2 myValue2) => Mathf.Approximately(myValue.x, myValue2.x)
                                                                               && Mathf.Approximately(myValue.y, myValue2.y);
    }
}