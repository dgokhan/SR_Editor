/*
<copyright file="BGCalcUnitFloatAtan2.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// atan2 unit
    /// </summary>
    [BGCalcUnitDefinition("By type/float/_Trigonometry/Atan2")]
    public class BGCalcUnitFloatAtan2 : BGCalcUnitFloatABFloat
    {
        public const int Code = 40;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        protected override string OutputLabel => "Atan2(A)";

        /// <inheritdoc />
        protected override float Operation(float a, float b) => Mathf.Atan2(a, b);
    }
}