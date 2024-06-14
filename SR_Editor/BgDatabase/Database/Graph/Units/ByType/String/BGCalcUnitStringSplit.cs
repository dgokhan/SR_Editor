/*
<copyright file="BGCalcUnitStringSplit.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// split string value unit
    /// </summary>
    [BGCalcUnitDefinition("By type/string/Split")]
    public class BGCalcUnitStringSplit : BGCalcUnit
    {
        private BGCalcValueInput a;
        private BGCalcValueInput b;
        private BGCalcValueInput removeEmpty;

        public const int Code = 59;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override void Definition()
        {
            a = ValueInput(BGCalcTypeCodeRegistry.String, "A", "a");
            b = ValueInput(BGCalcTypeCodeRegistry.String, "separator", "b");
            removeEmpty = ValueInput(BGCalcTypeCodeRegistry.Bool, "remove empty", "c");

            ValueOutput(BGCalcTypeCodeRegistry.List, "Split(A,B)", "r", GetValue);
        }

        private IList GetValue(BGCalcFlowI flow)
        {
            var value = flow.GetValue<string>(a);
            var separator = new[] { flow.GetValue<string>(b) };
            var remove = flow.GetValue<bool>(removeEmpty);
            var strings = value.Split(separator, remove ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None);
            return strings;
        }
    }
}