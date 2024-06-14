/*
<copyright file="BGCalcUnitOr.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// OR bool unit
    /// </summary>
    [BGCalcUnitDefinition("By type/bool/Or")]
    public class BGCalcUnitOr : BGCalcUnit
    {
        private BGCalcValueInput a;
        private BGCalcValueInput b;
        public const int Code = 3;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override void Definition()
        {
            a = ValueInput(BGCalcTypeCodeRegistry.Bool, "A", "a");
            b = ValueInput(BGCalcTypeCodeRegistry.Bool, "B", "b");
            ValueOutput(BGCalcTypeCodeRegistry.Bool, "A | B", "r", Operation);
        }

        private bool Operation(BGCalcFlowI flow) => flow.GetValue<bool>(a) || flow.GetValue<bool>(b);
    }
}