/*
<copyright file="BGCalcUnitFloatConstantInfinityNegative.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// NegativeInfinity constant
    /// </summary>
    [BGCalcUnitDefinition("By type/float/_Constants/NegativeInfinity constant")]
    public class BGCalcUnitFloatConstantInfinityNegative : BGCalcUnitFloatConstant
    {
        public const int Code = 45;
        
        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        protected override string OutputLabel => "NegativeInfinity";

        /// <inheritdoc />
        protected override float Operation() => Mathf.NegativeInfinity;
    }
}