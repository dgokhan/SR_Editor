/*
<copyright file="BGCalcUnitStringIndexOf.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// index of substring unit
    /// </summary>
    [BGCalcUnitDefinition("By type/string/IndexOf")]
    public class BGCalcUnitStringIndexOf : BGCalcUnit
    {
        private BGCalcValueInput a;
        private BGCalcValueInput b;

        public const int Code = 56;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override void Definition()
        {
            a = ValueInput(BGCalcTypeCodeRegistry.String, "A", "a");
            b = ValueInput(BGCalcTypeCodeRegistry.String, "value", "v");

            ValueOutput(BGCalcTypeCodeRegistry.Int, "IndexOf(value)", "r", GetValue);
        }

        private int GetValue(BGCalcFlowI flow) => flow.GetValue<string>(a).IndexOf(flow.GetValue<string>(b), StringComparison.Ordinal);
    }
}