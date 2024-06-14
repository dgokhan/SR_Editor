/*
<copyright file="BGCalcUnitGetMetaEntities.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Get all rows unit
    /// </summary>
    [BGCalcUnitDefinition("Database/Generic/meta/Get meta entities")]
    public class BGCalcUnitGetMetaEntities : BGCalcUnit
    {
        private BGCalcValueInput metaInput;
        public const int Code = 122;
        
        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override void Definition()
        {
            metaInput = ValueInput(BGCalcTypeCodeRegistry.Meta, "meta", "a");
            ValueOutput(BGCalcTypeCodeRegistry.List, "entities", "b", GetEntities);
        }

        private IList GetEntities(BGCalcFlowI flow)
        {
            var meta = flow.GetValue<BGMetaEntity>(metaInput);
            if (meta == null) throw new Exception("Can not get meta fields, cause the meta is not set!");
            return meta.EntitiesToList();
        }
    }
}