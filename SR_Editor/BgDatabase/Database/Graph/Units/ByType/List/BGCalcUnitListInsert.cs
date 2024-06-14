/*
<copyright file="BGCalcUnitListInsert.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// insert an item to the list unit
    /// </summary>
    [BGCalcUnitDefinition("By type/list/List insert")]
    public class BGCalcUnitListInsert : BGCalcUnit2ControlsA
    {
        private BGCalcValueInput listInput;
        private BGCalcValueInput obj;
        private BGCalcValueInput index;
        private BGCalcValueOutput resultOutput;

        public const int Code = 67;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override void Definition()
        {
            base.Definition();
            listInput = ValueInput(BGCalcTypeCodeRegistry.List, "list", "a");
            obj = ValueInput(BGCalcTypeCodeRegistry.Object, "object", "b");
            index = ValueInput(BGCalcTypeCodeRegistry.Int, "index", "c");
            resultOutput = ValueOutput(BGCalcTypeCodeRegistry.List, "result", "d", null);
        }

        /// <inheritdoc />
        protected override void Run(BGCalcFlowI flow)
        {
            var list = flow.GetValue<IList>(listInput);
            var toAdd = flow.GetValue(obj);
            var indexValue = flow.GetValue<int>(index);
            if (list is Array)
            {
                var arrayList = new ArrayList(list);
                arrayList.Insert(indexValue, obj);
                flow.SetValue(resultOutput, arrayList.ToArray(list.GetType().GetElementType()));
            }
            else
            {
                list.Insert(indexValue, toAdd);
                flow.SetValue(resultOutput, list);
            }
        }
    }
}