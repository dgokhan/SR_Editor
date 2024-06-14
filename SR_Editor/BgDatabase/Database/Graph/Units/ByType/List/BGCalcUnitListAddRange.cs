/*
<copyright file="BGCalcUnitListAddRange.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// add list items  unit
    /// </summary>
    [BGCalcUnitDefinition("By type/list/List addRange")]
    public class BGCalcUnitListAddRange : BGCalcUnit2ControlsA
    {
        private BGCalcValueInput listInput;
        private BGCalcValueInput listToAdd;
        private BGCalcValueOutput resultOutput;

        public const int Code = 82;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override void Definition()
        {
            base.Definition();
            listInput = ValueInput(BGCalcTypeCodeRegistry.List, "list", "a");
            listToAdd = ValueInput(BGCalcTypeCodeRegistry.Object, "list2", "b");
            resultOutput = ValueOutput(BGCalcTypeCodeRegistry.List, "result", "c", null);
        }

        /// <inheritdoc />
        protected override void Run(BGCalcFlowI flow)
        {
            var list = flow.GetValue<IList>(listInput);
            var list2 = flow.GetValue<IList>(listToAdd);
            if (list is Array array)
            {
                var arrayList = new ArrayList(list);
                arrayList.AddRange(list2);
                flow.SetValue(resultOutput, arrayList.ToArray(array.GetType().GetElementType()));
            }
            else
            {
                for (var i = 0; i < list2.Count; i++) list.Add(list2[i]);
                flow.SetValue(resultOutput, list);
            }
        }
    }
}