/*
<copyright file="BGCalcUnitIsNull.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// check if value is null unit
    /// </summary>
    [BGCalcUnitDefinition("By type/object/IsNull")]
    public class BGCalcUnitIsNull : BGCalcUnit
    {
        private BGCalcValueInput a;
        public const int Code = 50;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override void Definition()
        {
            a = ValueInput(BGCalcTypeCodeRegistry.Object, "A", "a");
            ValueOutput(BGCalcTypeCodeRegistry.Bool, "A is null", "r", IsNull);
        }

        private bool IsNull(BGCalcFlowI flow) => flow.GetValue(a) == null;
    }
}