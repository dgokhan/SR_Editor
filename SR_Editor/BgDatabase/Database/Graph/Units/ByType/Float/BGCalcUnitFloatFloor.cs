/*
<copyright file="BGCalcUnitFloatFloor.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// float floor unit
    /// </summary>
    [BGCalcUnitDefinition("By type/float/Floor")]
    public class BGCalcUnitFloatFloor : BGCalcUnitFloatAFloat
    {
        public const int Code = 31;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        protected override string OutputLabel => "Floor(A)";

        /// <inheritdoc />
        protected override float Operation(float a) => Mathf.Floor(a);
    }
}