/*
<copyright file="BGCalcTypeCodeSByte.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System.Globalization;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// type code for sbyte type
    /// </summary>
    public class BGCalcTypeCodeSByte : BGCalcTypeCode<sbyte>
    {
        public const byte Code = 19;
        
        /// <inheritdoc />
        public override bool SupportDefaultValue => true;
        
        /// <inheritdoc />
        public override byte TypeCode => Code;
        
        /// <inheritdoc />
        public override object DefaultValue => (sbyte)0;

        /// <inheritdoc />
        public override string Name => "sbyte";

        /// <inheritdoc />
        public override void ValueToBytes(BGBinaryWriter writer, object value) => writer.AddSByte((sbyte)value);

        /// <inheritdoc />
        public override object ValueFromBytes(BGBinaryReader reader) => reader.ReadSByte();

        /// <inheritdoc />
        public override string ValueToString(object value) => value.ToString();

        /// <inheritdoc />
        public override object ValueFromString(string value)
        {
            if (string.IsNullOrEmpty(value)) return (sbyte)0;
            return sbyte.Parse(value, CultureInfo.InvariantCulture);
        }

        /// <inheritdoc />
        public override bool CanBeConvertedFrom(BGCalcTypeCode otherCode)
        {
            if (otherCode == null) return false;
            switch (otherCode.TypeCode)
            {
                case BGCalcTypeCodeFloat.Code:
                case BGCalcTypeCodeShort.Code:
                case BGCalcTypeCodeInt.Code:
                case BGCalcTypeCodeByte.Code:
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
                    return (sbyte)(float)value;
                }
                case BGCalcTypeCodeInt.Code:
                {
                    return (sbyte)(int)value;
                }
                case BGCalcTypeCodeShort.Code:
                {
                    return (sbyte)(short)value;
                }
                case BGCalcTypeCodeByte.Code:
                {
                    return (sbyte)(byte)value;
                }
                case BGCalcTypeCodeUShort.Code:
                {
                    return (sbyte)(ushort)value;
                }
            }

            return value;
        }
    }
}