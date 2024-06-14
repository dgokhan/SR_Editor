/*
<copyright file="BGCalcUnitFloatConstantNan.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Nan constant
    /// </summary>
    [BGCalcUnitDefinition("By type/float/_Constants/NaN constant")]
    public class BGCalcUnitFloatConstantNan : BGCalcUnitFloatConstant
    {
        public const int Code = 87;
        
        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        protected override string OutputLabel => "Nan";

        /// <inheritdoc />
        protected override float Operation() => float.NaN;
    }
}