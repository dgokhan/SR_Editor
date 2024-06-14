/*
<copyright file="BGCalcUnitFloatParse.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System.Globalization;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// float parse unit
    /// </summary>
    [BGCalcUnitDefinition("By type/float/Float parse")]
    public class BGCalcUnitFloatParse : BGCalcUnit
    {
        public const int Code = 86;

        private BGCalcValueInput a;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override void Definition()
        {
            a = ValueInput(BGCalcTypeCodeRegistry.String, "A", "a");
            ValueOutput(BGCalcTypeCodeRegistry.Float, "Parse(A)", "r", flow => float.Parse(flow.GetValue<string>(a), CultureInfo.InvariantCulture));
        }
    }
}