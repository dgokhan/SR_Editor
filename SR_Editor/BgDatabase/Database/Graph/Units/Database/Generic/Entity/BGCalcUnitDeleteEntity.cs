/*
<copyright file="BGCalcUnitDeleteEntity.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// delete entity unit
    /// </summary>
    [BGCalcUnitDefinition("Database/Generic/entity/Delete entity", true)]
    public class BGCalcUnitDeleteEntity : BGCalcUnit2ControlsA
    {
        private BGCalcValueInput entityInput;

        public const int Code = 80;
        
        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override void Definition()
        {
            base.Definition();
            entityInput = ValueInput(BGCalcTypeCodeRegistry.Entity, "entity", "a");
        }

        protected override void Run(BGCalcFlowI flow)
        {
            var entity = flow.GetValue<BGEntity>(entityInput);
            if (entity == null) throw new Exception("Can not delete an entity, cause entity is not set!");
            entity.Delete();
        }
    }
}