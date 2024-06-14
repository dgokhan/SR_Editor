/*
<copyright file="BGCalcUnitFloatTan.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Tan unit
    /// </summary>
    [BGCalcUnitDefinition("By type/float/_Trigonometry/Tan")]
    public class BGCalcUnitFloatTan : BGCalcUnitFloatAFloat
    {
        public const int Code = 36;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        protected override string OutputLabel => "Tan(A)";

        /// <inheritdoc />
        protected override float Operation(float a) => Mathf.Tan(a);
    }
}