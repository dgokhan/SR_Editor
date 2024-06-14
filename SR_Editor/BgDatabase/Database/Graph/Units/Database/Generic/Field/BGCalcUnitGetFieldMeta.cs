/*
<copyright file="BGCalcUnitGetFieldMeta.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// get meta by field unit
    /// </summary>
    [BGCalcUnitDefinition("Database/Generic/field/Get field meta")]
    public class BGCalcUnitGetFieldMeta : BGCalcUnit
    {
        private BGCalcValueInput fieldInput;
        public const int Code = 119;
        
        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override void Definition()
        {
            fieldInput = ValueInput(BGCalcTypeCodeRegistry.Field, "field", "a");
            ValueOutput(BGCalcTypeCodeRegistry.Meta, "meta", "b", GetMeta);
        }

        private BGMetaEntity GetMeta(BGCalcFlowI flow)
        {
            var field = flow.GetValue<BGField>(fieldInput);
            if (field == null) throw new Exception("Can not get a meta, cause the field is not set!");
            return field.Meta;
        }
    }
}