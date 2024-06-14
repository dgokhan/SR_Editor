/*
<copyright file="BGCalcUnitFloatCeilToInt.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// ceil to int float  unit
    /// </summary>
    [BGCalcUnitDefinition("By type/float/Ceil to int")]
    public class BGCalcUnitFloatCeilToInt : BGCalcUnitFloatAInt
    {
        public const int Code = 30;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        protected override string OutputLabel => "CeilToInt(A)";

        /// <inheritdoc />
        protected override int Operation(float a) => Mathf.CeilToInt(a);
    }
}