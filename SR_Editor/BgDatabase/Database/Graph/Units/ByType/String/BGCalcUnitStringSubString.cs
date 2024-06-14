/*
<copyright file="BGCalcUnitStringSubString.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Globalization;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// string substring function unit
    /// </summary>
    [BGCalcUnitDefinition("By type/string/Substring")]
    public class BGCalcUnitStringSubString : BGCalcUnit
    {
        private BGCalcValueInput startIndex;
        private BGCalcValueInput a;

        public const int Code = 54;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override void Definition()
        {
            a = ValueInput(BGCalcTypeCodeRegistry.String, "A", "a");
            startIndex = ValueInput(BGCalcTypeCodeRegistry.Int, "start", "s");

            ValueOutput(BGCalcTypeCodeRegistry.String, "Substring(A)", "r", GetValue);
        }

        private string GetValue(BGCalcFlowI flow) => flow.GetValue<string>(a).Substring(flow.GetValue<int>(startIndex));
    }
}