/*
<copyright file="BGCalcTypeCodeList.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// type code for list type
    /// </summary>
    public class BGCalcTypeCodeList : BGCalcTypeCode<IList>
    {
        public const byte Code = 11;
        
        /// <inheritdoc />
        public override bool SupportDefaultValue => false;
        
        /// <inheritdoc />
        public override byte TypeCode => Code;
        
        /// <inheritdoc />
        public override object DefaultValue => throw new NotImplementedException();
        
        /// <inheritdoc />
        public override string Name => "list";

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