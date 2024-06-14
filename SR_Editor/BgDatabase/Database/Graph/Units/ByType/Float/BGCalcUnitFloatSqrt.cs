/*
<copyright file="BGCalcUnitFloatSqrt.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// float SQRT unit
    /// </summary>
    [BGCalcUnitDefinition("By type/float/Sqrt")]
    public class BGCalcUnitFloatSqrt : BGCalcUnitFloatAFloat
    {
        public const int Code = 47;
        
        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        protected override string OutputLabel => "Sqrt(A)";

        /// <inheritdoc />
        protected override float Operation(float a) => Mathf.Sqrt(a);
    }
}