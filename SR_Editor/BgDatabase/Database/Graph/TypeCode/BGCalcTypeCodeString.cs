/*
<copyright file="BGCalcTypeCodeString.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// type code for string type
    /// </summary>
    public class BGCalcTypeCodeString : BGCalcTypeCode<string>
    {
        public const byte Code = 3;

        /// <inheritdoc />
        public override bool SupportDefaultValue => true;
        
        /// <inheritdoc />
        public override byte TypeCode => Code;
        
        /// <inheritdoc />
        public override object DefaultValue => "";

        /// <inheritdoc />
        public override string Name => "string";

        /// <inheritdoc />
        public override bool AreEqual(object o1, object o2)
        {
            var s1 = (string)o1;
            var s2 = (string)o2;
            var e1 = string.IsNullOrEmpty(s1);
            var e2 = string.IsNullOrEmpty(s2);
            if (e1 && e2) return true;
            if (e1 || e2) return false;
            return s1.Equals(s2);
        }

        /// <inheritdoc />
        public override void ValueToBytes(BGBinaryWriter writer, object value) => writer.AddString((string)value);

        /// <inheritdoc />
        public override object ValueFromBytes(BGBinaryReader reader) => reader.ReadString() ?? "";

        /// <inheritdoc />
        public override string ValueToString(object value) => (string)value ?? "";

        /// <inheritdoc />
        public override object ValueFromString(string value) => value ?? "";

        /// <inheritdoc />
        public override bool CanBeConvertedFrom(BGCalcTypeCode otherCode)
        {
            if (otherCode == null) return false;
            switch (otherCode.TypeCode)
            {
                case BGCalcTypeCodeInt.Code:
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
                case BGCalcTypeCodeInt.Code:
                case BGCalcTypeCodeFloat.Code:
                case BGCalcTypeCodeByte.Code:
                case BGCalcTypeCodeShort.Code:
                case BGCalcTypeCodeSByte.Code:
                case BGCalcTypeCodeUShort.Code:
                {
                    return value.ToString();
                }
            }

            return value;
        }
    }
}