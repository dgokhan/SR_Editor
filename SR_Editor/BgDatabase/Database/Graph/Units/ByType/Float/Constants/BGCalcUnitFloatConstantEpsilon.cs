/*
<copyright file="BGCalcUnitFloatConstantEpsilon.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Epsilon constant
    /// </summary>
    [BGCalcUnitDefinition("By type/float/_Constants/Epsilon constant")]
    public class BGCalcUnitFloatConstantEpsilon : BGCalcUnitFloatConstant
    {
        public const int Code = 46;
        
        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        protected override string OutputLabel => "Epsilon";

        /// <inheritdoc />
        protected override float Operation() => Mathf.Epsilon;
    }
}