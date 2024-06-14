/*
<copyright file="BGCalcUnitFloatPow.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// float POW unit
    /// </summary>
    [BGCalcUnitDefinition("By type/float/Pow")]
    public class BGCalcUnitFloatPow : BGCalcUnitFloatABFloat
    {
        public const int Code = 48;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        protected override string OutputLabel => "Pow(A, B)";

        /// <inheritdoc />
        protected override float Operation(float a, float b) => Mathf.Pow(a, b);
    }
}