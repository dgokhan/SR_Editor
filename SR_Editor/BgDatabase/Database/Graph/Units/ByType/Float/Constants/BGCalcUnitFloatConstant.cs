/*
<copyright file="BGCalcUnitFloatConstant.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// float constant abstract base unit
    /// </summary>
    public abstract class BGCalcUnitFloatConstant : BGCalcUnit
    {
        /// <summary>
        /// label for result port
        /// </summary>
        protected abstract string OutputLabel { get; }

        /// <summary>
        /// value provider
        /// </summary>
        protected abstract float Operation();

        public override void Definition() => ValueOutput(BGCalcTypeCodeRegistry.Float, OutputLabel, "r", flow => Operation());
    }
}