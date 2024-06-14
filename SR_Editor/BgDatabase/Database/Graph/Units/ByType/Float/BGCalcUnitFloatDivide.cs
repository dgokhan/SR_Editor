/*
<copyright file="BGCalcUnitFloatDivide.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// divide float unit
    /// </summary>
    [BGCalcUnitDefinition("By type/float/Float divide")]
    public class BGCalcUnitFloatDivide : BGCalcUnitFloatABFloat
    {
        public const int Code = 27;

        /// <inheritdoc /> 
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        protected override string OutputLabel => "A / B";

        /// <inheritdoc />
        protected override float Operation(float a, float b) => a / b;
    }
}