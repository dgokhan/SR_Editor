/*
<copyright file="BGCalcUnitIntLessOrEqual.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// int less or equal unit
    /// </summary>
    [BGCalcUnitDefinition("By type/int/_Comparisons/Int less or equal")]
    public class BGCalcUnitIntLessOrEqual : BGCalcUnitIntABBool
    {
        public const int Code = 9;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        protected override string OutputLabel => "A <= B";

        /// <inheritdoc />
        protected override bool Operation(int a, int b) => a <= b;
    }
}