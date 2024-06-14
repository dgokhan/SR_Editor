/*
<copyright file="BGCalcUnitListSet.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System.Collections;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// set list item at specified index unit
    /// </summary>
    [BGCalcUnitDefinition("By type/list/List set")]
    public class BGCalcUnitListSet : BGCalcUnit2ControlsA
    {
        private BGCalcValueInput listInput;
        private BGCalcValueInput indexInput;
        private BGCalcValueInput objInput;
        private BGCalcValueOutput resultOutput;

        public const int Code = 91;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override void Definition()
        {
            base.Definition();
            listInput = ValueInput(BGCalcTypeCodeRegistry.List, "list", "a");
            indexInput = ValueInput(BGCalcTypeCodeRegistry.Int, "index", "b");
            objInput = ValueInput(BGCalcTypeCodeRegistry.Object, "object", "c");
            resultOutput = ValueOutput(BGCalcTypeCodeRegistry.List, "result", "d", null);
        }

        /// <inheritdoc />
        protected override void Run(BGCalcFlowI flow)
        {
            var list = flow.GetValue<IList>(listInput);
            var index = flow.GetValue<int>(indexInput);
            var obj = flow.GetValue<object>(objInput);
            list[index] = obj;
            flow.SetValue(resultOutput, list);
        }
    }
}