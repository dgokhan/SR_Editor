/*
<copyright file="BGCalcTypeCodeByte.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System.Globalization;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// type code for byte type
    /// </summary>
    public class BGCalcTypeCodeByte : BGCalcTypeCode<byte>
    {
        public const byte Code = 17;
        
        /// <inheritdoc />
        public override bool SupportDefaultValue => true;
        
        /// <inheritdoc />
        public override byte TypeCode => Code;
        
        /// <inheritdoc />
        public override object DefaultValue => (byte)0;

        /// <inheritdoc />
        public override string Name => "byte";

        /// <inheritdoc />
        public override void ValueToBytes(BGBinaryWriter writer, object value) => writer.AddByte((byte)value);

        /// <inheritdoc />
        public override object ValueFromBytes(BGBinaryReader reader) => reader.ReadByte();

        /// <inheritdoc />
        public override string ValueToString(object value) => value.ToString();

        /// <inheritdoc />
        public override object ValueFromString(string value)
        {
            if (string.IsNullOrEmpty(value)) return (byte)0;
            return byte.Parse(value, CultureInfo.InvariantCulture);
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
                    return (byte)(float)value;
                }
                case BGCalcTypeCodeInt.Code:
                {
                    return (byte)(int)value;
                }
                case BGCalcTypeCodeShort.Code:
                {
                    return (byte)(short)value;
                }
                case BGCalcTypeCodeSByte.Code:
                {
                    return (byte)(sbyte)value;
                }
                case BGCalcTypeCodeUShort.Code:
                {
                    return (byte)(ushort)value;
                }
            }

            return value;
        }
    }
}