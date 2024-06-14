/*
<copyright file="BGCalcUnitFloatModulo.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// float modulo unit
    /// </summary>
    [BGCalcUnitDefinition("By type/float/Float modulo")]
    public class BGCalcUnitFloatModulo : BGCalcUnitFloatABFloat
    {
        public const int Code = 28;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        protected override string OutputLabel => "A % B";

        /// <inheritdoc />
        protected override float Operation(float a, float b) => a % b;
    }
}