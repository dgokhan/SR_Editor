/*
<copyright file="BGCalcUnitFloatRoundToInt.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// float round unit
    /// </summary>
    [BGCalcUnitDefinition("By type/float/RoundToInt")]
    public class BGCalcUnitFloatRoundToInt : BGCalcUnitFloatAInt
    {
        public const int Code = 49;
        
        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        protected override string OutputLabel => "RoundToInt(A)";

        /// <inheritdoc />
        protected override int Operation(float a) => Mathf.RoundToInt(a);
    }
}