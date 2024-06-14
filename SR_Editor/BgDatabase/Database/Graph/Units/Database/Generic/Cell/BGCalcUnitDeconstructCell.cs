/*
<copyright file="BGCalcUnitDeconstructCell.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// deconstruct cell unit
    /// </summary>
    [BGCalcUnitDefinition("Database/Generic/cell/Cell deconstruct")]
    public class BGCalcUnitDeconstructCell : BGCalcUnit
    {
        private BGCalcValueInput a;
        public const int Code = 78;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override void Definition()
        {
            a = ValueInput(BGCalcTypeCodeRegistry.Cell, "cell", "a");
            ValueOutput(BGCalcTypeCodeRegistry.Entity, "entity", "b", GetEntity);
            ValueOutput(BGCalcTypeCodeRegistry.Field, "field", "c", GetField);
        }

        private BGCalcCell GetCell(BGCalcFlowI flow)
        {
            var calcCell = flow.GetValue<BGCalcCell>(a);
            if (calcCell == null) throw new Exception("Can not deconstruct a cell, cause the cell is not set!");
            return calcCell;
        }

        private BGField GetField(BGCalcFlowI flow) => GetCell(flow).Field;

        private BGEntity GetEntity(BGCalcFlowI flow) => GetCell(flow).Entity;
    }
}