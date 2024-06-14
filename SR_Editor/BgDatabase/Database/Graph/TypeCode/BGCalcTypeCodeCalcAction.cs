/*
<copyright file="BGCalcTypeCodeCalcAction.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// type code for action type
    /// </summary>
    public class BGCalcTypeCodeCalcAction : BGCalcTypeCode<BGFieldCalcActionValue>
    {
        public const byte Code = 26;
        
        /// <inheritdoc />
        public override bool SupportDefaultValue => false;
        
        /// <inheritdoc />
        public override byte TypeCode => Code;
        
        /// <inheritdoc />
        public override object DefaultValue => throw new NotImplementedException();
        
        /// <inheritdoc />
        public override string Name => "calcAction";

        /// <inheritdoc />
        public override bool CanBeConvertedFrom(BGCalcTypeCode otherCode) => false;

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