/*
<copyright file="BGCalcUnitFloatRad2Deg.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Rad2Deg unit
    /// </summary>
    [BGCalcUnitDefinition("By type/float/_Trigonometry/Rad2Deg")]
    public class BGCalcUnitFloatRad2Deg : BGCalcUnitFloatAFloat
    {
        public const int Code = 42;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        protected override string OutputLabel => "Rad2Deg(A)";

        /// <inheritdoc />
        protected override float Operation(float a) => Mathf.Rad2Deg * a;
    }
}