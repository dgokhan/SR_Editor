/*
<copyright file="BGCalcUnitGetCurrentLocale.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Get current locale
    /// </summary>
    public class BGCalcUnitGetCurrentLocale : BGCalcUnit
    {
        public const int Code = 112;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override string Title => "Get locale";

        /// <inheritdoc />
        public override void Definition() => ValueOutput(BGCalcTypeCodeRegistry.String, "locale", "l", GetLocale);

        private string GetLocale(BGCalcFlowI flow) => BGCalcUnitLocalizationDelegateProvider.Delegate.CurrentLocale;
    }
}