/*
<copyright file="BGCalcUnitCellSetValue.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// set cell value unit
    /// </summary>
    [BGCalcUnitDefinition("Database/Generic/cell/Cell set value", true)]
    public class BGCalcUnitCellSetValue : BGCalcUnit2ControlsA
    {
        private BGCalcValueInput a;
        private BGCalcValueInput b;
        private BGCalcValueOutput c;
        public const int Code = 74;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override void Definition()
        {
            base.Definition();
            a = ValueInput(BGCalcTypeCodeRegistry.Cell, "cell", "a");
            b = ValueInput(BGCalcTypeCodeRegistry.Object, "value", "b");
            c = ValueOutput(BGCalcTypeCodeRegistry.Cell, "result", "c", null);
        }

        /// <inheritdoc />
        protected override void Run(BGCalcFlowI flow)
        {
            var cell = GetCell(flow);
            if (cell == null) throw new Exception("Can not retrieve field value, cause the cell is not set!");
            var value = flow.GetValue(b);
            cell.Set(value);
            flow.SetValue(c, cell);
        }

        private BGCalcCell GetCell(BGCalcFlowI flow) => flow.GetValue<BGCalcCell>(a);
    }
}