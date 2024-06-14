/*
<copyright file="BGCalcUnitFloatLess.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// less float unit
    /// </summary>
    [BGCalcUnitDefinition("By type/float/_Comparisons/Float less")]
    public class BGCalcUnitFloatLess : BGCalcUnitFloatABBool
    {
        private BGCalcValueInput a;
        private BGCalcValueInput b;
        public const int Code = 19;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        protected override string OutputLabel => "A < B";

        /// <inheritdoc />
        protected override bool Operation(float a, float b) => a < b;
    }
}