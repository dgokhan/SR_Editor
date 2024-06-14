/*
<copyright file="BGCalcUnitListClear.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// clear list unit
    /// </summary>
    [BGCalcUnitDefinition("By type/list/List clear")]
    public class BGCalcUnitListClear : BGCalcUnit2ControlsA
    {
        private BGCalcValueInput listInput;
        private BGCalcValueOutput resultOutput;

        public const int Code = 66;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override void Definition()
        {
            base.Definition();
            listInput = ValueInput(BGCalcTypeCodeRegistry.List, "list", "a");
            resultOutput = ValueOutput(BGCalcTypeCodeRegistry.List, "result", "b", null);
        }

        /// <inheritdoc />
        protected override void Run(BGCalcFlowI flow)
        {
            var list = flow.GetValue<IList>(listInput);
            if (list is Array) flow.SetValue(resultOutput, Array.CreateInstance(list.GetType().GetElementType(), 0));
            else
            {
                list.Clear();
                flow.SetValue(resultOutput, list);
            }
        }
    }
}