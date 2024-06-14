/*
<copyright file="BGCalcUnitListIndexOf.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System.Collections;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// get list item index unit
    /// </summary>
    [BGCalcUnitDefinition("By type/list/List indexOf")]
    public class BGCalcUnitListIndexOf : BGCalcUnit
    {
        private BGCalcValueInput a;
        private BGCalcValueInput b;

        public const int Code = 68;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override void Definition()
        {
            a = ValueInput(BGCalcTypeCodeRegistry.List, "list", "a");
            b = ValueInput(BGCalcTypeCodeRegistry.Object, "object", "b");
            ValueOutput(BGCalcTypeCodeRegistry.Int, "index", "r", IndexOf);
        }

        private int IndexOf(BGCalcFlowI flow)
        {
            var list = flow.GetValue<IList>(a);
            var obj = flow.GetValue(b);
            return list.IndexOf(obj);
        }
    }
}