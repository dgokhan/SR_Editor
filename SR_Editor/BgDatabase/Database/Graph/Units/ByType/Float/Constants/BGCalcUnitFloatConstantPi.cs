/*
<copyright file="BGCalcUnitFloatConstantPi.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// PI constant
    /// </summary>
    [BGCalcUnitDefinition("By type/float/_Constants/PI")]
    public class BGCalcUnitFloatConstantPi : BGCalcUnitFloatConstant
    {
        public const int Code = 43;
        
        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        protected override string OutputLabel => "PI";

        /// <inheritdoc />
        protected override float Operation() => Mathf.PI;
    }
}