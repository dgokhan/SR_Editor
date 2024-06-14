/*
<copyright file="BGCalcUnitGetMetaFields.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Get fields unit
    /// </summary>
    [BGCalcUnitDefinition("Database/Generic/meta/Get meta fields")]
    public class BGCalcUnitGetMetaFields : BGCalcUnit
    {
        private BGCalcValueInput metaInput;
        public const int Code = 121;
        
        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override void Definition()
        {
            metaInput = ValueInput(BGCalcTypeCodeRegistry.Meta, "meta", "a");
            ValueOutput(BGCalcTypeCodeRegistry.List, "fields", "b", GetFields);
        }

        private IList GetFields(BGCalcFlowI flow)
        {
            var meta = flow.GetValue<BGMetaEntity>(metaInput);
            if (meta == null) throw new Exception("Can not get meta fields, cause the meta is not set!");
            return meta.FindFields();
        }
    }
}