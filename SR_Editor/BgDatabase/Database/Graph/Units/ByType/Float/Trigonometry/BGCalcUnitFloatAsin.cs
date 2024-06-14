/*
<copyright file="BGCalcUnitFloatAsin.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// asin unit
    /// </summary>
    [BGCalcUnitDefinition("By type/float/_Trigonometry/Asin")]
    public class BGCalcUnitFloatAsin : BGCalcUnitFloatAFloat
    {
        public const int Code = 37;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        protected override string OutputLabel => "Asin(A)";

        /// <inheritdoc />
        protected override float Operation(float a) => Mathf.Asin(a);
    }
}