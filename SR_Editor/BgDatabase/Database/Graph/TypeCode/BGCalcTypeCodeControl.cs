/*
<copyright file="BGCalcTypeCodeControl.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// type code for control type
    /// </summary>
    public class BGCalcTypeCodeControl : BGCalcTypeCode<BGCalcControl>
    {
        public const byte Code = 1;

        /// <inheritdoc />
        public override bool SupportDefaultValue => false;
        
        /// <inheritdoc />
        public override object DefaultValue => throw new NotImplementedException();
        
        /// <inheritdoc />
        public override byte TypeCode => Code;
        
        /// <inheritdoc />
        public override string Name => "control";

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