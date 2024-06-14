/*
<copyright file="BGCalcUnitFloatApproximately.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// approximate float value unit
    /// </summary>
    [BGCalcUnitDefinition("By type/float/Approximately")]
    public class BGCalcUnitFloatApproximate : BGCalcUnitFloatABBool
    {
        public const int Code = 33;
        
        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        protected override string OutputLabel => "Approximately(A, B)";

        /// <inheritdoc />
        protected override bool Operation(float a, float b) => Mathf.Approximately(a, b);
    }
}