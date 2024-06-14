/*
<copyright file="BGCalcUnitIntParse.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System.Globalization;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// int PARSE function unit
    /// </summary>
    [BGCalcUnitDefinition("By type/int/Int parse")]
    public class BGCalcUnitIntParse : BGCalcUnit
    {
        public const int Code = 85;

        private BGCalcValueInput a;

        /// <inheritdoc />        
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override void Definition()
        {
            a = ValueInput(BGCalcTypeCodeRegistry.String, "A", "a");
            ValueOutput(BGCalcTypeCodeRegistry.Int, "Parse(A)", "r", flow => int.Parse(flow.GetValue<string>(a), CultureInfo.InvariantCulture));
        }
    }
}