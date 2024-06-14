/*
<copyright file="BGCalcUnitGetCurrentEntity.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Get current entity unit
    /// </summary>
    public class BGCalcUnitGetCurrentEntity : BGCalcUnit
    {
        public const int Code = 111;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override string Title => "Get current entity";

        /// <inheritdoc />
        public override void Definition() => ValueOutput(BGCalcTypeCodeRegistry.Entity, "entity", "e", GetEntity);

        private BGEntity GetEntity(BGCalcFlowI flow) => flow.Context.CurrentEntity;
    }
}