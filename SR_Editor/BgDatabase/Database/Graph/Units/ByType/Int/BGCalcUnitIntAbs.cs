/*
<copyright file="BGCalcUnitIntAbs.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// int ABS function unit
    /// </summary>
    [BGCalcUnitDefinition("By type/int/Int Abs")]
    public class BGCalcUnitIntAbs : BGCalcUnitIntAInt
    {
        public const int Code = 15;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        protected override string OutputLabel => "Abs(A)";

        /// <inheritdoc />
        protected override int Operation(int a) => Math.Abs(a);
    }
}