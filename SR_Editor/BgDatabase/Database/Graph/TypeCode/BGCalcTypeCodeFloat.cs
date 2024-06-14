/*
<copyright file="BGCalcTypeCodeFloat.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Globalization;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// type code for float type
    /// </summary>
    public class BGCalcTypeCodeFloat : BGCalcTypeCode<float>
    {
        public const byte Code = 5;

        /// <inheritdoc />
        public override bool SupportDefaultValue => true;
        
        /// <inheritdoc />
        public override byte TypeCode => Code;
        
        /// <inheritdoc />
        public override object DefaultValue => 0f;
        
        /// <inheritdoc />
        public override string Name => "float";

        /// <inheritdoc />
        public override void ValueToBytes(BGBinaryWriter writer, object value) => writer.AddFloat((float)value);

        /// <inheritdoc />
        public override object ValueFromBytes(BGBinaryReader reader) => reader.ReadFloat();

        /// <inheritdoc />
        public override string ValueToString(object value) => ((float)value).ToString(CultureInfo.InvariantCulture);

        /// <inheritdoc />
        public override object ValueFromString(string value)
        {
            if (string.IsNullOrEmpty(value)) return 0f;
            return float.Parse(value);
        }

        /// <inheritdoc />
        public override bool CanBeConvertedFrom(BGCalcTypeCode otherCode)
        {
            if (otherCode == null) return false;
            switch (otherCode.TypeCode)
            {
                case BGCalcTypeCodeInt.Code:
                case BGCalcTypeCodeByte.Code:
                case BGCalcTypeCodeShort.Code:
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
                case BGCalcTypeCodeInt.Code:
                {
                    return (float)(int)value;
                }
                case BGCalcTypeCodeByte.Code:
                {
                    return (float)(byte)value;
                }
                case BGCalcTypeCodeShort.Code:
                {
                    return (float)(short)value;
                }
                case BGCalcTypeCodeSByte.Code:
                {
                    return (float)(sbyte)value;
                }
                case BGCalcTypeCodeUShort.Code:
                {
                    return (float)(ushort)value;
                }
            }

            return value;
        }
    }
}