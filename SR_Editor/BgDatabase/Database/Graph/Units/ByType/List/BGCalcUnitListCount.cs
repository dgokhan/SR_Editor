/*
<copyright file="BGCalcUnitListCount.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System.Collections;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// count list items unit
    /// </summary>
    [BGCalcUnitDefinition("By type/list/List count")]
    public class BGCalcUnitListCount : BGCalcUnit
    {
        private BGCalcValueInput a;

        public const int Code = 61;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override void Definition()
        {
            a = ValueInput(BGCalcTypeCodeRegistry.List, "list", "a");
            ValueOutput(BGCalcTypeCodeRegistry.Int, "count", "r", Count);
        }

        private int Count(BGCalcFlowI flow) => flow.GetValue<IList>(a).Count;
    }
}