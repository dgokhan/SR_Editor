/*
<copyright file="BGCalcUnitIntMax.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// int MAX function unit
    /// </summary>
    [BGCalcUnitDefinition("By type/int/Int max")]
    public class BGCalcUnitIntMax : BGCalcUnitIntABInt
    {
        public const int Code = 136;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        protected override string OutputLabel => "MAX(A,B)";

        /// <inheritdoc />
        protected override int Operation(int a, int b) => Math.Max(a, b);
    }
}