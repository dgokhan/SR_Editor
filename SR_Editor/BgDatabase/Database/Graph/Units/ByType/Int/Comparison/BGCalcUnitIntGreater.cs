/*
<copyright file="BGCalcUnitIntGreater.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// int greater unit
    /// </summary>
    [BGCalcUnitDefinition("By type/int/_Comparisons/Int greater")]
    public class BGCalcUnitIntGreater : BGCalcUnitIntABBool
    {
        public const int Code = 10;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        protected override string OutputLabel => "A > B";

        /// <inheritdoc />
        protected override bool Operation(int a, int b) => a > b;
    }
}