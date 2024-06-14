/*
<copyright file="BGCalcUnitGetEntity.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// get entity unit
    /// </summary>
    [BGCalcUnitDefinition("Database/Generic/entity/Get entity")]
    public class BGCalcUnitGetEntity : BGCalcUnitWithSource
    {
        private BGCalcValueInput metaInput;

        public const int Code = 72;
        
        /// <inheritdoc />
        public override ushort TypeCode => Code;
        
        /// <inheritdoc />
        protected override BGCalcTypeCode ObjectTypeCode => BGCalcTypeCodeRegistry.Entity;

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
            return meta.GetEntity(name);
        }

        /// <inheritdoc />
        protected override BGObject FetchObjectById(BGCalcFlowI flow, BGId id)
        {
            var meta = GetMeta(flow);
            return meta.GetEntity(id);
        }

        /// <inheritdoc />
        protected override BGObject FetchObjectByIndex(BGCalcFlowI flow, int index)
        {
            var meta = GetMeta(flow);
            return meta.GetEntity(index);
        }

        private BGMetaEntity GetMeta(BGCalcFlowI flow)
        {
            var meta = flow.GetValue<BGMetaEntity>(metaInput);
            if (meta == null) throw new Exception("Can not get an entity cause meta is not set!");
            return meta;
        }
    }
}