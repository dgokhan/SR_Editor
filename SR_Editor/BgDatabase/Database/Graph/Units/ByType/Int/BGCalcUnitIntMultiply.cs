/*
<copyright file="BGCalcUnitIntMultiply.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// int MULTIPLY function unit
    /// </summary>
    [BGCalcUnitDefinition("By type/int/Int multiply")]
    public class BGCalcUnitIntMultiply : BGCalcUnitIntABInt
    {
        public const int Code = 16;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        protected override string OutputLabel => "A * B";

        /// <inheritdoc />
        protected override int Operation(int a, int b) => a * b;
    }
}