/*
<copyright file="BGFieldListFloat.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Field with list of floats value
    /// </summary>
    [FieldDescriptor(Name = "listFloat", Folder = "List/Primitive", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerListFloat")]
    public partial class BGFieldListFloat : BGFieldCachedStructListA<float>
    {
        public const ushort CodeType = 15;

        /// <inheritdoc/>
        public override ushort TypeCode => CodeType;

        /// <inheritdoc/>
        protected override int ValueSize => BGFieldFloat.SizeOfTheValue;

        //for new field
        public BGFieldListFloat(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldListFloat(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Serialization
        //================================================================================================
        /// <inheritdoc/>
        protected override byte[] ValueToBytes(float value) => BGFieldFloat.ValueToBytes(value);

        /// <inheritdoc/>
        protected override float ValueFromBytes(ArraySegment<byte> segment) => BGFieldFloat.ValueFromBytes(segment);

        protected override float ValueFromBytes(byte[] array, int offset) => BitConverter.ToSingle(array, offset);

        /// <inheritdoc/>
        protected override string ValueToString(float value) => BGFieldFloat.ValueToString(value);

        /// <inheritdoc/>
        protected override float ValueFromString(string value) => BGFieldFloat.ValueFromString(value);

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc/>
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldListFloat(meta, id, name);

        //================================================================================================
        //                                              Value
        //================================================================================================
        /// <inheritdoc/>
        protected override bool AreEqual(float myValue, float myValue2) => Mathf.Approximately(myValue, myValue2);
    }
}