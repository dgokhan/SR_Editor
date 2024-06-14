/*
<copyright file="BGCalcUnitFloatEqual.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// equal float unit
    /// </summary>
    [BGCalcUnitDefinition("By type/float/_Comparisons/Float equal")]
    public class BGCalcUnitFloatEqual : BGCalcUnitFloatABBool
    {
        public const int Code = 90;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        protected override string OutputLabel => "A = B";

        /// <inheritdoc />
        protected override bool Operation(float a, float b) => a.Equals(b);
    }
}