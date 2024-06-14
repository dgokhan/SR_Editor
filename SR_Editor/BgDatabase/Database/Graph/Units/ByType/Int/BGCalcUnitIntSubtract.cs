/*
<copyright file="BGCalcUnitIntSubtract.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// int SUBTRACT function unit
    /// </summary>
    [BGCalcUnitDefinition("By type/int/Int subtract")]
    public class BGCalcUnitIntSubtract : BGCalcUnitIntABInt
    {
        public const int Code = 14;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        protected override string OutputLabel => "A - B";

        /// <inheritdoc />
        protected override int Operation(int a, int b) => a - b;
    }
}