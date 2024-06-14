/*
<copyright file="BGCalcUnitNull.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// null literal unit
    /// </summary>
    [BGCalcUnitDefinition("By type/object/null literal")]
    public class BGCalcUnitNull : BGCalcUnit
    {
        public const int Code = 51;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override void Definition() => ValueOutput(BGCalcTypeCodeRegistry.Object, "null", "r", flow => null);
    }
}