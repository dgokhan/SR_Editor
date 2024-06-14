/*
<copyright file="BGCalcUnitCountEntities.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// count rows unit
    /// </summary>
    [BGCalcUnitDefinition("Database/Generic/meta/Count entities")]
    public class BGCalcUnitCountEntities : BGCalcUnit
    {
        private BGCalcValueInput metaInput;
        public const int Code = 124;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override void Definition()
        {
            metaInput = ValueInput(BGCalcTypeCodeRegistry.Meta, "meta", "a");
            ValueOutput(BGCalcTypeCodeRegistry.Int, "count", "b", GetValue);
        }

        private object GetValue(BGCalcFlowI flow)
        {
            var meta = flow.GetValue<BGMetaEntity>(metaInput);
            if (meta == null) throw new Exception("Meta is not set!");
            flow.Context.Events?.AddOnCreate(meta);
            flow.Context.Events?.AddOnDelete(meta);
            return meta.CountEntities;
        }
    }
}