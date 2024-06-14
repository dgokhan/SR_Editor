/*
<copyright file="BGCalcUnitCellSetValue2.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// set cell value2 unit
    /// </summary>
    [BGCalcUnitDefinition("Database/Generic/cell/Cell set value2", true)]
    public class BGCalcUnitCellSetValue2 : BGCalcUnit2ControlsA
    {
        private BGCalcValueInput a;
        private BGCalcValueInput b;
        private BGCalcValueInput c;
        private BGCalcValueOutput d;
        public const int Code = 76;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override void Definition()
        {
            base.Definition();
            a = ValueInput(BGCalcTypeCodeRegistry.Entity, "entity", "a");
            b = ValueInput(BGCalcTypeCodeRegistry.Field, "field", "b");
            c = ValueInput(BGCalcTypeCodeRegistry.Object, "value", "c");
            d = ValueOutput(BGCalcTypeCodeRegistry.Cell, "cell", "d", null);
        }

        /// <inheritdoc />
        protected override void Run(BGCalcFlowI flow)
        {
            var entity = GetEntity(flow);
            if (entity == null) throw new Exception("Can not retrieve field value, cause the entity is not set!");
            var field = GetField(flow);
            if (field == null) throw new Exception("Can not retrieve field value, cause the field is not set!");
            if (field.ReadOnly) throw new Exception($"Can not set cell value, cause field {field.FullName} is readonly!");
            if (field.MetaId != entity.MetaId) throw new Exception("Can not retrieve field value, cause entity and field belong to different tables!");
            field.SetValue(entity.Index, flow.GetValue(c));
            flow.SetValue(d, new BGCalcCell(field, entity));
        }

        // private BGCalcCell GetCell(BGCalcFlowI flow)
        // {
        //     return new BGCalcCell(GetField(flow), GetEntity(flow));
        // }
        private BGField GetField(BGCalcFlowI flow) => flow.GetValue<BGField>(b);

        private BGEntity GetEntity(BGCalcFlowI flow) => flow.GetValue<BGEntity>(a);
    }
}