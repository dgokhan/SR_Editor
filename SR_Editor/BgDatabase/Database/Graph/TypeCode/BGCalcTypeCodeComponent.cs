/*
<copyright file="BGCalcTypeCodeComponent.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// type code for Unity Component type
    /// </summary>
    public class BGCalcTypeCodeComponent : BGCalcTypeCode<Component>
    {
        public const byte Code = 25;
        
        /// <inheritdoc />
        public override bool SupportDefaultValue => false;
        
        /// <inheritdoc />
        public override byte TypeCode => Code;
        
        /// <inheritdoc />
        public override object DefaultValue => throw new NotImplementedException();

        /// <inheritdoc />
        public override string Name => "Component";

        /// <inheritdoc />
        public override void ValueToBytes(BGBinaryWriter writer, object value) => throw new NotImplementedException();

        /// <inheritdoc />
        public override object ValueFromBytes(BGBinaryReader reader) => throw new NotImplementedException();

        /// <inheritdoc />
        public override string ValueToString(object value) => throw new NotImplementedException();

        /// <inheritdoc />
        public override object ValueFromString(string value) => throw new NotImplementedException();
    }
}