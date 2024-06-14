/*
<copyright file="BGCalcUnitEnumToInt.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// convert enum to int unit
    /// </summary>
    [BGCalcUnitDefinition("By type/enum/Enum convert")]
    public class BGCalcUnitEnumToInt : BGCalcUnit
    {
        private BGCalcValueInput input;

        public const int Code = 94;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override void Definition()
        {
            input = ValueInput(typeof(Enum), "value", "a");
            ValueOutput(BGCalcTypeCodeRegistry.Int, "ToInt()", "b", GetValueInt);
        }

        private int GetValueInt(BGCalcFlowI flow)
        {
            var enumValue = flow.GetValue<Enum>(input);
            var result = (int)Convert.ChangeType(enumValue, System.TypeCode.Int32);
            return result;
        }
    }
}