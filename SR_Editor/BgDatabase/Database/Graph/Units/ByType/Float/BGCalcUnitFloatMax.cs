/*
<copyright file="BGCalcUnitFloatMax.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// float modulo unit
    /// </summary>
    [BGCalcUnitDefinition("By type/float/Float max")]
    public class BGCalcUnitFloatMax: BGCalcUnitFloatABFloat
    {
        public const int Code = 138;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        protected override string OutputLabel => "MAX(A,B)";

        /// <inheritdoc />
        protected override float Operation(float a, float b) => Mathf.Max(a, b);
    }
}