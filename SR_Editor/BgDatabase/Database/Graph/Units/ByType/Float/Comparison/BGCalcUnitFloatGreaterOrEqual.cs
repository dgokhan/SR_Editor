/*
<copyright file="BGCalcUnitFloatGreaterOrEqual.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// greater or equal float unit
    /// </summary>
    [BGCalcUnitDefinition("By type/float/_Comparisons/Float greater or equal")]
    public class BGCalcUnitFloatGreaterOrEqual : BGCalcUnitFloatABBool
    {
        public const int Code = 22;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        protected override string OutputLabel => "A >= B";

        /// <inheritdoc />
        protected override bool Operation(float a, float b) => a >= b;
    }
}