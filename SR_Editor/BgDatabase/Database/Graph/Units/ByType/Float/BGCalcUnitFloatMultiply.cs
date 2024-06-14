/*
<copyright file="BGCalcUnitFloatMultiply.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// float multiply unit
    /// </summary>
    [BGCalcUnitDefinition("By type/float/Float multiply")]
    public class BGCalcUnitFloatMultiply : BGCalcUnitFloatABFloat
    {
        public const int Code = 26;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        protected override string OutputLabel => "A * B";

        /// <inheritdoc />
        protected override float Operation(float a, float b) => a * b;
    }
}