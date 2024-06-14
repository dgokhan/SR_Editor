/*
<copyright file="BGCalcUnitObjectToString.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// convert object to string unit 
    /// </summary>
    [BGCalcUnitDefinition("By type/object/Object to string")]
    public class BGCalcUnitObjectToString : BGCalcUnit
    {
        private BGCalcValueInput a;

        public const int Code = 81;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override void Definition()
        {
            a = ValueInput(BGCalcTypeCodeRegistry.Object, "A", "a");

            ValueOutput(BGCalcTypeCodeRegistry.String, "A.ToString()", "r", GetValue);
        }

        private string GetValue(BGCalcFlowI flow) => flow.GetValue<object>(a)?.ToString();
    }
}