/*
<copyright file="BGCalcUnitCreateEntity.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// create entity unit
    /// </summary>
    [BGCalcUnitDefinition("Database/Generic/entity/New entity", true)]
    public class BGCalcUnitCreateEntity : BGCalcUnit2ControlsA
    {
        private BGCalcValueInput metaInput;
        private BGCalcValueOutput newEntityOutput;

        public const int Code = 79;
        
        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override void Definition()
        {
            base.Definition();
            metaInput = ValueInput(BGCalcTypeCodeRegistry.Meta, "meta", "q");
            newEntityOutput = ValueOutput(BGCalcTypeCodeRegistry.Entity, "entity", "e", null);
        }

        /// <inheritdoc />
        protected override void Run(BGCalcFlowI flow)
        {
            var newEntity = GetMeta(flow).NewEntity();
            flow.SetValue(newEntityOutput, newEntity);
        }

        private BGMetaEntity GetMeta(BGCalcFlowI flow)
        {
            var meta = flow.GetValue<BGMetaEntity>(metaInput);
            if (meta == null) throw new Exception("Can not create an entity cause meta is not set!");
            return meta;
        }
    }
}