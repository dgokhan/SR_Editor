/*
<copyright file="BGCalcUnitFloatCos.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// cos unit
    /// </summary>
    [BGCalcUnitDefinition("By type/float/_Trigonometry/Cos")]
    public class BGCalcUnitFloatCos : BGCalcUnitFloatAFloat
    {
        public const int Code = 35;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        protected override string OutputLabel => "Cos(A)";

        /// <inheritdoc />
        protected override float Operation(float a) => Mathf.Cos(a);
    }
}