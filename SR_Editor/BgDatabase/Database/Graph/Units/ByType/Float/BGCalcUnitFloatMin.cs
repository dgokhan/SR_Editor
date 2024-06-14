/*
<copyright file="BGCalcUnitFloatMin.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// float modulo unit
    /// </summary>
    [BGCalcUnitDefinition("By type/float/Float min")]
    public class BGCalcUnitFloatMin : BGCalcUnitFloatABFloat
    {
        public const int Code = 137;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        protected override string OutputLabel => "MIN(A,B)";

        /// <inheritdoc />
        protected override float Operation(float a, float b) => Mathf.Min(a, b);
    }
}