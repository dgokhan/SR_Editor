/*
<copyright file="BGFieldListColor.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Field with list of Colors value
    /// </summary>
    [FieldDescriptor(Name = "listColor", Folder = "List/Unity Primitive", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerListColor")]
    public partial class BGFieldListColor : BGFieldCachedStructListA<Color>
    {
        public const ushort CodeType = 20;

        /// <inheritdoc/>
        public override ushort TypeCode => CodeType;

        /// <inheritdoc/>
        protected override int ValueSize => BGFieldColor.SizeOfTheValue;

        //for new field
        public BGFieldListColor(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldListColor(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Serialization
        //================================================================================================
        /// <inheritdoc/>
        protected override byte[] ValueToBytes(Color value) => BGFieldColor.ValueToBytes(value);

        /// <inheritdoc/>
        protected override Color ValueFromBytes(ArraySegment<byte> segment) => BGFieldColor.ValueFromBytes(segment);

        protected override Color ValueFromBytes(byte[] array, int offset)
        {
            return new Color
            (
                BitConverter.ToSingle(array, offset),
                BitConverter.ToSingle(array, offset + 4),
                BitConverter.ToSingle(array, offset + 8),
                BitConverter.ToSingle(array, offset + 12)
            );
        }

        /// <inheritdoc/>
        protected override string ValueToString(Color value) => BGFieldColor.ValueToString(value);

        /// <inheritdoc/>
        protected override Color ValueFromString(string value) => BGFieldColor.ValueFromString(value);

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc/>
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldListColor(meta, id, name);

        //================================================================================================
        //                                              Value
        //================================================================================================
        /// <inheritdoc/>
        protected override bool AreEqual(Color myValue, Color myValue2)
        {
            return Mathf.Approximately(myValue.r, myValue2.r)
                   && Mathf.Approximately(myValue.g, myValue2.g)
                   && Mathf.Approximately(myValue.b, myValue2.b)
                   && Mathf.Approximately(myValue.a, myValue2.a);
        }
    }
}