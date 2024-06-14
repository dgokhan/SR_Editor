/*
<copyright file="BGCalcUnitFloatSubtract.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// float subtract unit
    /// </summary>
    [BGCalcUnitDefinition("By type/float/Float subtract")]
    public class BGCalcUnitFloatSubtract : BGCalcUnitFloatABFloat
    {
        public const int Code = 24;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        protected override string OutputLabel => "A - B";

        /// <inheritdoc />
        protected override float Operation(float a, float b) => a - b;
    }
}