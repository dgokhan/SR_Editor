/*
<copyright file="BGCalcUnitFloatConstantInfinity.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Infinity constant
    /// </summary>
    [BGCalcUnitDefinition("By type/float/_Constants/Infinity constant")]
    public class BGCalcUnitFloatConstantInfinity : BGCalcUnitFloatConstant
    {
        public const int Code = 44;
        
        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        protected override string OutputLabel => "Infinity";

        /// <inheritdoc />
        protected override float Operation() => Mathf.Infinity;
    }
}