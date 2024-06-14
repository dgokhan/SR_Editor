/*
<copyright file="BGCalcUnitEqual.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// 2 objects equal function unit
    /// </summary>
    [BGCalcUnitDefinition("By type/object/Equal")]
    public class BGCalcUnitEqual : BGCalcUnit
    {
        private BGCalcValueInput a;
        private BGCalcValueInput b;
        public const int Code = 6;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override void Definition()
        {
            a = ValueInput(BGCalcTypeCodeRegistry.Object, "A", "a");
            b = ValueInput(BGCalcTypeCodeRegistry.Object, "B", "b");
            ValueOutput(BGCalcTypeCodeRegistry.Bool, "A==B", "r", IsEqual);
        }

        private bool IsEqual(BGCalcFlowI flow) => Equals(flow.GetValue(a), flow.GetValue(b));
    }
}