/*
<copyright file="BGCalcUnitBoolParse.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System.Globalization;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// parse bool unit
    /// </summary>
    [BGCalcUnitDefinition("By type/bool/Bool parse")]
    public class BGCalcUnitBoolParse : BGCalcUnit
    {
        public const int Code = 88;

        private BGCalcValueInput a;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override void Definition()
        {
            a = ValueInput(BGCalcTypeCodeRegistry.String, "A", "a");
            ValueOutput(BGCalcTypeCodeRegistry.Bool, "Parse(A)", "r", flow => bool.Parse(flow.GetValue<string>(a)));
        }
    }
}