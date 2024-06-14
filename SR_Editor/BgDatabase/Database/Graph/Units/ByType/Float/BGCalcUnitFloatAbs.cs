/*
<copyright file="BGCalcUnitFloatAbs.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// float absolute unit
    /// </summary>
    [BGCalcUnitDefinition("By type/float/Float Abs")]
    public class BGCalcUnitFloatAbs : BGCalcUnitFloatAFloat
    {
        public const int Code = 25;
        
        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        protected override string OutputLabel => "Abs(A)";

        /// <inheritdoc />
        protected override float Operation(float a) => Mathf.Abs(a);
    }
}