/*
<copyright file="BGCalcUnitListGet.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System.Collections;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// get list item unit
    /// </summary>
    [BGCalcUnitDefinition("By type/list/List get")]
    public class BGCalcUnitListGet : BGCalcUnit
    {
        private BGCalcValueInput a;
        private BGCalcValueInput index;

        public const int Code = 62;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override void Definition()
        {
            a = ValueInput(BGCalcTypeCodeRegistry.List, "list", "a");
            index = ValueInput(BGCalcTypeCodeRegistry.Int, "index", "b");
            ValueOutput(BGCalcTypeCodeRegistry.Object, "value", "r", Get);
        }

        private object Get(BGCalcFlowI flow)
        {
            var list = flow.GetValue<IList>(a);
            var ind = flow.GetValue<int>(index);
            return list[ind];
        }
    }
}