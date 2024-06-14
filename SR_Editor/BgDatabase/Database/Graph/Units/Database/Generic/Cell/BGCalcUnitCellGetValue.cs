/*
<copyright file="BGCalcUnitCellGetValue.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// get cell value unit
    /// </summary>
    [BGCalcUnitDefinition("Database/Generic/cell/Cell get value")]
    public class BGCalcUnitCellGetValue : BGCalcUnit
    {
        private BGCalcValueInput a;
        private BGCalcValueInput b;
        public const int Code = 73;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override void Definition()
        {
            a = ValueInput(BGCalcTypeCodeRegistry.Cell, "cell", "a");
            ValueOutput(BGCalcTypeCodeRegistry.Object, "result", "b", Operation);
        }

        private object Operation(BGCalcFlowI flow)
        {
            var cell = GetCell(flow);
            if (cell == null) throw new Exception("Can not retrieve cell value, cause the cell is not set!");
            AddListeners(flow, cell.Field, cell.Entity);
            return cell.Get();
        }

        public static void AddListeners(BGCalcFlowI flow, BGField field, BGEntity entity)
        {
            if (flow.Context.Events == null || field == null || entity == null) return;

            if (field is BGFieldNested nestedField)
            {
                // can we filter events by owner row?
                flow.Context.Events.AddOnCreate(nestedField.NestedMeta);
                flow.Context.Events.AddOnDelete(nestedField.NestedMeta);
            }
            else flow.Context.Events.AddOnEdit(field, entity);
        }

        private BGCalcCell GetCell(BGCalcFlowI flow)
        {
            return flow.GetValue<BGCalcCell>(a);
        }
    }
}