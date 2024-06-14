/*
<copyright file="BGCalcUnitConstructCell.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// construct cell unit
    /// </summary>
    [BGCalcUnitDefinition("Database/Generic/cell/Cell construct")]
    public class BGCalcUnitConstructCell : BGCalcUnit
    {
        private BGCalcValueInput a;
        private BGCalcValueInput b;
        public const int Code = 77;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override void Definition()
        {
            a = ValueInput(BGCalcTypeCodeRegistry.Entity, "entity", "a");
            b = ValueInput(BGCalcTypeCodeRegistry.Field, "field", "b");
            ValueOutput(BGCalcTypeCodeRegistry.Cell, "cell", "c", GetCell);
        }

        private BGCalcCell GetCell(BGCalcFlowI flow) => new BGCalcCell(GetField(flow), GetEntity(flow));

        private BGField GetField(BGCalcFlowI flow) => flow.GetValue<BGField>(b);

        private BGEntity GetEntity(BGCalcFlowI flow) => flow.GetValue<BGEntity>(a);
    }
}