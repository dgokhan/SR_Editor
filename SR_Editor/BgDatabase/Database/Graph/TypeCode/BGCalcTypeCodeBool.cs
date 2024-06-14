/*
<copyright file="BGCalcTypeCodeBool.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// type code for bool type
    /// </summary>
    public class BGCalcTypeCodeBool : BGCalcTypeCode<bool>
    {
        public const byte Code = 2;
        
        /// <inheritdoc />
        public override bool SupportDefaultValue => true;
        
        /// <inheritdoc />
        public override byte TypeCode => Code;
        
        /// <inheritdoc />
        public override object DefaultValue => false;

        /// <inheritdoc />
        public override string Name => "bool";

        /// <inheritdoc />
        public override void ValueToBytes(BGBinaryWriter writer, object value) => writer.AddBool((bool)value);

        /// <inheritdoc />
        public override object ValueFromBytes(BGBinaryReader reader) => reader.ReadBool();

        /// <inheritdoc />
        public override string ValueToString(object value) => (bool)value ? "1" : "0";

        /// <inheritdoc />
        public override object ValueFromString(string value) => string.Equals(value, "1");
    }
}