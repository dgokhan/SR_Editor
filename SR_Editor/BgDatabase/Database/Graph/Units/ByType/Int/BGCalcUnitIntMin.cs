/*
<copyright file="BGCalcUnitIntMin.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// int MIN function unit
    /// </summary>
    [BGCalcUnitDefinition("By type/int/Int min")]
    public class BGCalcUnitIntMin : BGCalcUnitIntABInt
    {
        public const int Code = 135;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        protected override string OutputLabel => "MIN(A,B)";

        /// <inheritdoc />
        protected override int Operation(int a, int b) => Math.Min(a, b);
    }
}