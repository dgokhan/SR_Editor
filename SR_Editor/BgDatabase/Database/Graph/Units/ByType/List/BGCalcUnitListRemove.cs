/*
<copyright file="BGCalcUnitListRemove.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// remove list item unit
    /// </summary>
    [BGCalcUnitDefinition("By type/list/List remove")]
    public class BGCalcUnitListRemove : BGCalcUnit2ControlsA
    {
        private BGCalcValueInput listInput;
        private BGCalcValueInput allInput;
        private BGCalcValueInput objInput;
        private BGCalcValueOutput resultOutput;

        public const int Code = 92;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override void Definition()
        {
            base.Definition();
            listInput = ValueInput(BGCalcTypeCodeRegistry.List, "list", "a");
            objInput = ValueInput(BGCalcTypeCodeRegistry.Object, "object", "b");
            allInput = ValueInput(BGCalcTypeCodeRegistry.Bool, "all?", "c");
            resultOutput = ValueOutput(BGCalcTypeCodeRegistry.List, "result", "d", null);
        }

        /// <inheritdoc />
        protected override void Run(BGCalcFlowI flow)
        {
            var list = flow.GetValue<IList>(listInput);
            var obj = flow.GetValue<object>(objInput);
            var all = flow.GetValue<bool>(allInput);
            if (list is Array)
            {
                var arrayList = new ArrayList(list);
                if (all)
                {
                    var ind = arrayList.IndexOf(obj);
                    while (ind != -1)
                    {
                        arrayList.RemoveAt(ind);
                        ind = arrayList.IndexOf(obj);
                    }
                }
                else arrayList.Remove(obj);

                flow.SetValue(resultOutput, arrayList.ToArray(list.GetType().GetElementType()));
            }
            else
            {
                if (all)
                {
                    var ind = list.IndexOf(obj);
                    while (ind != -1)
                    {
                        list.RemoveAt(ind);
                        ind = list.IndexOf(obj);
                    }
                }
                else list.Remove(obj);

                flow.SetValue(resultOutput, list);
            }
        }
    }
}