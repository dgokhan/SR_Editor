/*
<copyright file="BGCalcUnitCellGetValue2.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// get cell value2 unit
    /// </summary>
    [BGCalcUnitDefinition("Database/Generic/cell/Cell get value2")]
    public class BGCalcUnitCellGetValue2 : BGCalcUnit
    {
        private BGCalcValueInput a;
        private BGCalcValueInput b;
        public const int Code = 75;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override void Definition()
        {
            a = ValueInput(BGCalcTypeCodeRegistry.Entity, "entity", "a");
            b = ValueInput(BGCalcTypeCodeRegistry.Field, "field", "b");
            ValueOutput(BGCalcTypeCodeRegistry.Object, "result", "c", Operation);
            ValueOutput(BGCalcTypeCodeRegistry.Cell, "cell", "d", GetCell);
        }

        private object Operation(BGCalcFlowI flow)
        {
            var entity = GetEntity(flow);
            if (entity == null) throw new Exception("Can not retrieve cell value, cause the entity is not set!");
            var field = GetField(flow);
            switch (field)
            {
                case null:
                    throw new Exception("Can not retrieve cell value, cause the field is not set!");
                case BGFieldCalcI _:
                    throw new Exception("Can not get a value cause field is calculated field. To get calculated field value from graph, use 'Call calculated cell' unit!");
            }

            if (field.MetaId != entity.MetaId) throw new Exception("Can not retrieve cell value, cause entity and field belong to different tables!");
            BGCalcUnitCellGetValue.AddListeners(flow, field, entity);
            return field.GetValue(entity.Index);
        }

        private BGCalcCell GetCell(BGCalcFlowI flow) => new BGCalcCell(GetField(flow), GetEntity(flow));

        private BGField GetField(BGCalcFlowI flow) => flow.GetValue<BGField>(b);

        private BGEntity GetEntity(BGCalcFlowI flow) => flow.GetValue<BGEntity>(a);
    }
}