/*
<copyright file="BGCalcUnitFloatAcos.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// acos unit
    /// </summary>
    [BGCalcUnitDefinition("By type/float/_Trigonometry/Acos")]
    public class BGCalcUnitFloatAcos : BGCalcUnitFloatAFloat
    {
        public const int Code = 38;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        protected override string OutputLabel => "Acos(A)";

        /// <inheritdoc />
        protected override float Operation(float a) => Mathf.Acos(a);
    }
}