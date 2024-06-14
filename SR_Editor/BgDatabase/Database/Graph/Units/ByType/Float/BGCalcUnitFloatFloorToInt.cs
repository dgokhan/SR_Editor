/*
<copyright file="BGCalcUnitFloatFloorToInt.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// float floor to int unit
    /// </summary>
    [BGCalcUnitDefinition("By type/float/Floor to int")]
    public class BGCalcUnitFloatFloorToInt : BGCalcUnitFloatAInt
    {
        public const int Code = 32;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        protected override string OutputLabel => "FloorToInt(A)";

        /// <inheritdoc />
        protected override int Operation(float a) => Mathf.FloorToInt(a);
    }
}