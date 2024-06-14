/*
<copyright file="BGCalcTypeCodeBGId.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// type code for BGId type
    /// </summary>
    public class BGCalcTypeCodeBGId : BGCalcTypeCode<BGId>
    {
        public const byte Code = 6;
        
        /// <inheritdoc />
        public override bool SupportDefaultValue => true;
        
        /// <inheritdoc />
        public override byte TypeCode => Code;
        
        /// <inheritdoc />
        public override object DefaultValue => BGId.Empty;

        /// <inheritdoc />
        public override string Name => "BGId";

        /// <inheritdoc />
        public override void ValueToBytes(BGBinaryWriter writer, object value) => writer.AddId((BGId)value);

        /// <inheritdoc />
        public override object ValueFromBytes(BGBinaryReader reader) => reader.ReadId();

        /// <inheritdoc />
        public override string ValueToString(object value) => ((BGId)value).ToString();

        /// <inheritdoc />
        public override object ValueFromString(string value) => !BGId.TryParse(value, out var id) ? BGId.Empty : id;
    }
}