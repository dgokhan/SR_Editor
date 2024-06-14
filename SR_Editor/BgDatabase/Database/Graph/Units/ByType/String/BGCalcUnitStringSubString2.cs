/*
<copyright file="BGCalcUnitStringSubString2.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// string substring2 function unit
    /// </summary>
    [BGCalcUnitDefinition("By type/string/Substring2")]
    public class BGCalcUnitStringSubString2 : BGCalcUnit
    {
        private BGCalcValueInput startIndex;
        private BGCalcValueInput length;
        private BGCalcValueInput a;

        public const int Code = 55;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override void Definition()
        {
            a = ValueInput(BGCalcTypeCodeRegistry.String, "A", "a");
            startIndex = ValueInput(BGCalcTypeCodeRegistry.Int, "start", "s");
            length = ValueInput(BGCalcTypeCodeRegistry.Int, "length", "l");

            ValueOutput(BGCalcTypeCodeRegistry.String, "Substring(A)", "r", GetValue);
        }

        private string GetValue(BGCalcFlowI flow) => flow.GetValue<string>(a).Substring(flow.GetValue<int>(startIndex), flow.GetValue<int>(length));
    }
}