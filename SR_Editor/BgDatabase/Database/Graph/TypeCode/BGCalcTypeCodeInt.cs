/*
<copyright file="BGCalcTypeCodeInt.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Globalization;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// type code for int type
    /// </summary>
    public class BGCalcTypeCodeInt : BGCalcTypeCode<int>
    {
        public const byte Code = 4;

        /// <inheritdoc />
        public override bool SupportDefaultValue => true;
        
        /// <inheritdoc />
        public override byte TypeCode => Code;
        
        /// <inheritdoc />
        public override object DefaultValue => 0;
        
        /// <inheritdoc />
        public override string Name => "int";

        /// <inheritdoc />
        public override void ValueToBytes(BGBinaryWriter writer, object value) => writer.AddInt((int)value);

        /// <inheritdoc />
        public override object ValueFromBytes(BGBinaryReader reader) => reader.ReadInt();

        /// <inheritdoc />
        public override string ValueToString(object value) => ((int)value).ToString(CultureInfo.InvariantCulture);

        /// <inheritdoc />
        public override object ValueFromString(string value)
        {
            if (string.IsNullOrEmpty(value)) return 0;
            return int.Parse(value);
        }

        /// <inheritdoc />
        public override bool CanBeConvertedFrom(BGCalcTypeCode otherCode)
        {
            if (otherCode == null) return false;
            switch (otherCode.TypeCode)
            {
                case BGCalcTypeCodeFloat.Code:
                case BGCalcTypeCodeShort.Code:
                case BGCalcTypeCodeByte.Code:
                case BGCalcTypeCodeSByte.Code:
                case BGCalcTypeCodeUShort.Code:
                {
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc />
        public override object ConvertFrom(BGCalcTypeCode otherCode, object value)
        {
            if (otherCode == null) return value;
            switch (otherCode.TypeCode)
            {
                case BGCalcTypeCodeFloat.Code:
                {
                    return (int)(float)value;
                }
                case BGCalcTypeCodeByte.Code:
                {
                    return (int)(byte)value;
                }
                case BGCalcTypeCodeShort.Code:
                {
                    return (int)(short)value;
                }
                case BGCalcTypeCodeSByte.Code:
                {
                    return (int)(sbyte)value;
                }
                case BGCalcTypeCodeUShort.Code:
                {
                    return (int)(ushort)value;
                }
            }

            return value;
        }
    }
}