/*
<copyright file="BGCalcUnitFloatAdd.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// add float values unit
    /// </summary>
    [BGCalcUnitDefinition("By type/float/Float add")]
    public class BGCalcUnitFloatAdd : BGCalcUnitFloatABFloat
    {
        public const int Code = 23;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        protected override string OutputLabel => "A + B";

        /// <inheritdoc />
        protected override float Operation(float a, float b) => a + b;
    }
}