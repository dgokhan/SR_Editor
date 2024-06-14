/*
<copyright file="BGCalcUnitFloatAtan.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// atan unit
    /// </summary>
    [BGCalcUnitDefinition("By type/float/_Trigonometry/Atan")]
    public class BGCalcUnitFloatAtan : BGCalcUnitFloatAFloat
    {
        public const int Code = 39;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        protected override string OutputLabel => "Atan(A)";

        /// <inheritdoc />
        protected override float Operation(float a) => Mathf.Atan(a);
    }
}