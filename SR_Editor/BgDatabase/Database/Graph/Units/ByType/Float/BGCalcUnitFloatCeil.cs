/*
<copyright file="BGCalcUnitFloatCeil.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// ceiling float unit
    /// </summary>
    [BGCalcUnitDefinition("By type/float/Ceil")]
    public class BGCalcUnitFloatCeil : BGCalcUnitFloatAFloat
    {
        public const int Code = 29;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        protected override string OutputLabel => "Ceil(A)";

        /// <inheritdoc />
        protected override float Operation(float a) => Mathf.Ceil(a);
    }
}