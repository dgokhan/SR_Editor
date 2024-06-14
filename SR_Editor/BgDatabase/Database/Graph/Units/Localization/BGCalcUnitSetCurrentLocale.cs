/*
<copyright file="BGCalcUnitSetCurrentLocale.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// set current locale
    /// </summary>
    public class BGCalcUnitSetCurrentLocale : BGCalcUnit2ControlsA
    {
        private BGCalcValueInput locale;
        public const int Code = 113;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override string Title => "Set locale";

        /// <inheritdoc />
        public override void Definition()
        {
            base.Definition();
            locale = ValueInput(BGCalcTypeCodeRegistry.String, "locale", "l");
        }

        /// <inheritdoc />
        protected override void Run(BGCalcFlowI flow) => BGCalcUnitLocalizationDelegateProvider.Delegate.CurrentLocale = flow.GetValue<string>(locale);
    }
}