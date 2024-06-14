/*
<copyright file="BGCalcUnitFloatDeg2Rad.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Deg2Rad unit
    /// </summary>
    [BGCalcUnitDefinition("By type/float/_Trigonometry/Deg2Rad")]
    public class BGCalcUnitFloatDeg2Rad : BGCalcUnitFloatAFloat
    {
        public const int Code = 41;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        protected override string OutputLabel => "Deg2Rad(A)";

        /// <inheritdoc />
        protected override float Operation(float a) => Mathf.Deg2Rad * a;
    }
}