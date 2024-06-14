/*
<copyright file="BGCalcUnitFloatSin.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// sin unit
    /// </summary>
    [BGCalcUnitDefinition("By type/float/_Trigonometry/Sin")]
    public class BGCalcUnitFloatSin : BGCalcUnitFloatAFloat
    {
        public const int Code = 34;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        protected override string OutputLabel => "Sin(A)";

        /// <inheritdoc />
        protected override float Operation(float a) => Mathf.Sin(a);
    }
}