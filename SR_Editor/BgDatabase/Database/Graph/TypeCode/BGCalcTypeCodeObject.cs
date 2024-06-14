/*
<copyright file="BGCalcTypeCodeObject.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// type code for object type
    /// </summary>
    public class BGCalcTypeCodeObject : BGCalcTypeCode<object>
    {
        public const byte Code = 10;
        
        /// <inheritdoc />
        public override bool SupportDefaultValue => false;
        
        /// <inheritdoc />
        public override byte TypeCode => Code;
        
        /// <inheritdoc />
        public override object DefaultValue => throw new NotImplementedException();
        
        /// <inheritdoc />
        public override string Name => "object";

        /// <inheritdoc />
        public override bool CanBeConvertedFrom(BGCalcTypeCode otherCode) => true;

        /// <inheritdoc />
        public override object ConvertFrom(BGCalcTypeCode otherCode, object value) => value;

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