/*
<copyright file="BGCalcTypeCodeShort.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System.Globalization;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// type code for short type
    /// </summary>
    public class BGCalcTypeCodeShort : BGCalcTypeCode<short>
    {
        public const byte Code = 18;
        
        /// <inheritdoc />
        public override bool SupportDefaultValue => true;
        
        /// <inheritdoc />
        public override byte TypeCode => Code;
        
        /// <inheritdoc />
        public override object DefaultValue => (short)0;

        /// <inheritdoc />
        public override string Name => "short";

        /// <inheritdoc />
        public override void ValueToBytes(BGBinaryWriter writer, object value) => writer.AddShort((short)value);

        /// <inheritdoc />
        public override object ValueFromBytes(BGBinaryReader reader) => reader.ReadShort();

        /// <inheritdoc />
        public override string ValueToString(object value) => value.ToString();

        /// <inheritdoc />
        public override object ValueFromString(string value)
        {
            if (string.IsNullOrEmpty(value)) return (short)0;
            return short.Parse(value, CultureInfo.InvariantCulture);
        }

        /// <inheritdoc />
        public override bool CanBeConvertedFrom(BGCalcTypeCode otherCode)
        {
            if (otherCode == null) return false;
            switch (otherCode.TypeCode)
            {
                case BGCalcTypeCodeFloat.Code:
                case BGCalcTypeCodeInt.Code:
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
                    return (short)(float)value;
                }
                case BGCalcTypeCodeByte.Code:
                {
                    return (short)(byte)value;
                }
                case BGCalcTypeCodeInt.Code:
                {
                    return (short)(int)value;
                }
                case BGCalcTypeCodeSByte.Code:
                {
                    return (short)(sbyte)value;
                }
                case BGCalcTypeCodeUShort.Code:
                {
                    return (short)(ushort)value;
                }
            }

            return value;
        }
    }
}