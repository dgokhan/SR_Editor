/*
<copyright file="BGCalcUnitListRemoveAt.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// remove list item at specified index unit
    /// </summary>
    [BGCalcUnitDefinition("By type/list/List removeAt")]
    public class BGCalcUnitListRemoveAt : BGCalcUnit2ControlsA
    {
        private BGCalcValueInput listInput;
        private BGCalcValueInput index;
        private BGCalcValueOutput resultOutput;

        public const int Code = 64;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override void Definition()
        {
            base.Definition();
            listInput = ValueInput(BGCalcTypeCodeRegistry.List, "list", "a");
            index = ValueInput(BGCalcTypeCodeRegistry.Int, "index", "b");
            resultOutput = ValueOutput(BGCalcTypeCodeRegistry.List, "result", "d", null);
        }

        /// <inheritdoc />
        protected override void Run(BGCalcFlowI flow)
        {
            var list = flow.GetValue<IList>(listInput);
            var indexValue = flow.GetValue<int>(index);
            if (list is Array)
            {
                var arrayList = new ArrayList(list);
                arrayList.RemoveAt(indexValue);
                flow.SetValue(resultOutput, arrayList.ToArray(list.GetType().GetElementType()));
            }
            else
            {
                list.RemoveAt(indexValue);
                flow.SetValue(resultOutput, list);
            }
        }
    }
}