/*
<copyright file="BGCalcUnitFloatGreater.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// greater float unit
    /// </summary>
    [BGCalcUnitDefinition("By type/float/_Comparisons/Float greater")]
    public class BGCalcUnitFloatGreater : BGCalcUnitFloatABBool
    {
        public const int Code = 21;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        protected override string OutputLabel => "A > B";

        /// <inheritdoc />
        protected override bool Operation(float a, float b) => a > b;
    }
}