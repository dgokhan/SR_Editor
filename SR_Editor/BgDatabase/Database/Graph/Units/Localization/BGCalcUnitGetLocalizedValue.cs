/*
<copyright file="BGCalcUnitGetLocalizedValue.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// get localized value
    /// </summary>
    public class BGCalcUnitGetLocalizedValue : BGCalcUnitDbRowBasedA
    {
        public const int Code = 114;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        protected override string Operation => "get localized";

        /// <inheritdoc />
        public override void Definition()
        {
            var meta = Meta;
            if (meta == null) throw new Exception("Meta is not found! id=" + MetaId);
            base.Definition();

            var valueType = BGCalcUnitLocalizationDelegateProvider.Delegate.GetValueType(meta);
            ValueOutput(valueType, "value", "r", GetValue);
        }

        private object GetValue(BGCalcFlowI flow) => BGCalcUnitLocalizationDelegateProvider.Delegate.GetValue(MetaCached, GetEntity(flow));
    }
}