/*
<copyright file="BGCalcUnitStringJoin.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// join list of strings values unit
    /// </summary>
    [BGCalcUnitDefinition("By type/string/String join")]
    public class BGCalcUnitStringJoin : BGCalcUnit
    {
        private BGCalcValueInput a;
        private BGCalcValueInput b;

        public const int Code = 69;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override void Definition()
        {
            a = ValueInput(BGCalcTypeCodeRegistry.List, "list", "a");
            b = ValueInput(BGCalcTypeCodeRegistry.String, "separator", "b");

            ValueOutput(BGCalcTypeCodeRegistry.String, "result", "r", GetValue);
        }

        private string GetValue(BGCalcFlowI flow)
        {
            var list = flow.GetValue<IList>(a);
            var separator = flow.GetValue<string>(b);
            var builder = new StringBuilder();
            var hasSeparator = !string.IsNullOrEmpty(separator);
            for (var i = 0; i < list.Count; i++)
            {
                if (hasSeparator && i != 0) builder.Append(separator);
                var item = list[i];
                builder.Append(item);
            }

            return builder.ToString();
        }
    }
}