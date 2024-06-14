/*
<copyright file="BGCalcUnitListContains.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System.Collections;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// list contains function unit
    /// </summary>
    [BGCalcUnitDefinition("By type/list/List contains")]
    public class BGCalcUnitListContains : BGCalcUnit
    {
        private BGCalcValueInput a;
        private BGCalcValueInput b;

        public const int Code = 83;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override void Definition()
        {
            a = ValueInput(BGCalcTypeCodeRegistry.List, "list", "a");
            b = ValueInput(BGCalcTypeCodeRegistry.Object, "object", "b");
            ValueOutput(BGCalcTypeCodeRegistry.Bool, "result", "r", Contains);
        }

        private bool Contains(BGCalcFlowI flow)
        {
            var list = flow.GetValue<IList>(a);
            var obj = flow.GetValue(b);
            return list.Contains(obj);
        }
    }
}