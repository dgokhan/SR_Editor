/*
<copyright file="BGCalcUnitGetField.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Get field unit
    /// </summary>
    [BGCalcUnitDefinition("Database/Generic/field/Get field")]
    public class BGCalcUnitGetField : BGCalcUnitWithSource
    {
        private BGCalcValueInput metaInput;

        public const int Code = 71;
        
        /// <inheritdoc />
        public override ushort TypeCode => Code;
        
        /// <inheritdoc />
        protected override BGCalcTypeCode ObjectTypeCode => BGCalcTypeCodeRegistry.Field;

        /// <inheritdoc />        
        public override void Definition()
        {
            metaInput = ValueInput(BGCalcTypeCodeRegistry.Meta, "meta", "q");
            base.Definition();
        }

        /// <inheritdoc />
        protected override BGObject FetchObjectByName(BGCalcFlowI flow, string name)
        {
            var meta = GetMeta(flow);
            return meta.GetField(name, false);
        }

        /// <inheritdoc />
        protected override BGObject FetchObjectById(BGCalcFlowI flow, BGId id)
        {
            var meta = GetMeta(flow);
            return meta.GetField(id, false);
        }

        /// <inheritdoc />
        protected override BGObject FetchObjectByIndex(BGCalcFlowI flow, int index)
        {
            var meta = GetMeta(flow);
            return meta.GetField(index);
        }

        private BGMetaEntity GetMeta(BGCalcFlowI flow)
        {
            var meta = flow.GetValue<BGMetaEntity>(metaInput);
            if (meta == null) throw new Exception("Can not get a field cause meta is not set!");
            return meta;
        }
    }
}