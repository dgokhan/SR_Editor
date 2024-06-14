/*
<copyright file="BGCalcUnitOrNegate.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// NEGATE bool unit
    /// </summary>
    [BGCalcUnitDefinition("By type/bool/Negate")]
    public class BGCalcUnitOrNegate : BGCalcUnit
    {
        private BGCalcValueInput a;
        public const int Code = 5;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override void Definition()
        {
            a = ValueInput(BGCalcTypeCodeRegistry.Bool, "A", "a");
            ValueOutput(BGCalcTypeCodeRegistry.Bool, "!A", "r", Operation);
        }

        private bool Operation(BGCalcFlowI flow) => !flow.GetValue<bool>(a);
    }
}