/*
<copyright file="BGCalcTypeCodeSource.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Globalization;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// type code for possible row source type
    /// </summary>
    public class BGCalcTypeCodeSource : BGCalcTypeCode<BGCalcUnitSourceEnum>
    {
        public const byte Code = 9;
        
        /// <inheritdoc />
        public override bool SupportDefaultValue => true;
        
        /// <inheritdoc />
        public override byte TypeCode => Code;
        
        /// <inheritdoc />
        public override object DefaultValue => BGCalcUnitSourceEnum.DB_Object;
        
        /// <inheritdoc />
        public override string Name => "sourceMode";

        /// <inheritdoc />
        public override void ValueToBytes(BGBinaryWriter writer, object value) => writer.AddByte((byte)(BGCalcUnitSourceEnum)value);

        /// <inheritdoc />
        public override object ValueFromBytes(BGBinaryReader reader) => (BGCalcUnitSourceEnum)reader.ReadByte();

        /// <inheritdoc />
        public override string ValueToString(object value) => ((byte)(BGCalcUnitSourceEnum)value).ToString(CultureInfo.InvariantCulture);

        /// <inheritdoc />
        public override object ValueFromString(string value)
        {
            if (string.IsNullOrEmpty(value)) return BGCalcUnitSourceEnum.DB_Object;
            return (BGCalcUnitSourceEnum)byte.Parse(value);
        }
    }
}