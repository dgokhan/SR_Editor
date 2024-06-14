/*
<copyright file="BGCalcUnitStringAdd.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// 2 string values addition unit
    /// </summary>
    [BGCalcUnitDefinition("By type/string/String add")]
    public class BGCalcUnitStringAdd : BGCalcUnit
    {
        private BGCalcValueInput a;
        private BGCalcValueInput b;

        public const int Code = 57;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override void Definition()
        {
            a = ValueInput(BGCalcTypeCodeRegistry.String, "A", "a");
            b = ValueInput(BGCalcTypeCodeRegistry.String, "B", "b");

            ValueOutput(BGCalcTypeCodeRegistry.String, "A + B", "r", GetValue);
        }

        private string GetValue(BGCalcFlowI flow) => flow.GetValue<string>(a) + flow.GetValue<string>(b);
    }
}