/*
<copyright file="BGCalcTypeCodeUShort.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System.Globalization;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// type code for ushort type
    /// </summary>
    public class BGCalcTypeCodeUShort : BGCalcTypeCode<ushort>
    {
        public const byte Code = 20;
        
        /// <inheritdoc />
        public override bool SupportDefaultValue => true;
        
        /// <inheritdoc />
        public override byte TypeCode => Code;
        
        /// <inheritdoc />
        public override object DefaultValue => (ushort)0;

        /// <inheritdoc />
        public override string Name => "ushort";

        /// <inheritdoc />
        public override void ValueToBytes(BGBinaryWriter writer, object value) => writer.AddUShort((ushort)value);

        /// <inheritdoc />
        public override object ValueFromBytes(BGBinaryReader reader) => reader.ReadUShort();

        /// <inheritdoc />
        public override string ValueToString(object value) => value.ToString();

        /// <inheritdoc />
        public override object ValueFromString(string value)
        {
            if (string.IsNullOrEmpty(value)) return (ushort)0;
            return ushort.Parse(value, CultureInfo.InvariantCulture);
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
                case BGCalcTypeCodeShort.Code:
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
                    return (ushort)(float)value;
                }
                case BGCalcTypeCodeByte.Code:
                {
                    return (ushort)(byte)value;
                }
                case BGCalcTypeCodeInt.Code:
                {
                    return (ushort)(int)value;
                }
                case BGCalcTypeCodeSByte.Code:
                {
                    return (ushort)(sbyte)value;
                }
                case BGCalcTypeCodeShort.Code:
                {
                    return (ushort)(short)value;
                }
            }

            return value;
        }
    }
}