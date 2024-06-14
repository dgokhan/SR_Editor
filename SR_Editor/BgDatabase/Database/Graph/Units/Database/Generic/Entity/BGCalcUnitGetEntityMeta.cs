/*
<copyright file="BGCalcUnitGetEntityMeta.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// get table by entity unit
    /// </summary>
    [BGCalcUnitDefinition("Database/Generic/entity/Get entity meta")]
    public class BGCalcUnitGetEntityMeta : BGCalcUnit
    {
        private BGCalcValueInput entityInput;
        public const int Code = 120;
        
        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override void Definition()
        {
            entityInput = ValueInput(BGCalcTypeCodeRegistry.Entity, "entity", "a");
            ValueOutput(BGCalcTypeCodeRegistry.Meta, "meta", "b", GetMeta);
        }

        private BGMetaEntity GetMeta(BGCalcFlowI flow)
        {
            var entity = flow.GetValue<BGEntity>(entityInput);
            if (entity == null) throw new Exception("Can not get a meta, cause the entity is not set!");
            return entity.Meta;
        }
    }
}