/*
<copyright file="BGCalcUnitStringA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// abstract string based unit with result of type T 
    /// </summary>
    public abstract class BGCalcUnitStringA<T> : BGCalcUnit
    {
        private BGCalcValueInput a;

        /// <summary>
        /// output type code
        /// </summary>
        protected abstract BGCalcTypeCode<T> OutputCode { get; }
        
        /// <summary>
        /// output label
        /// </summary>
        protected abstract string OutputLabel { get; }

        /// <inheritdoc />
        public override void Definition()
        {
            a = ValueInput(BGCalcTypeCodeRegistry.String, "A", "a");
            ValueOutput(OutputCode, OutputLabel, "r", flow => Operation(flow.GetValue<string>(a)));
        }

        /// <summary>
        /// function method to receive result value
        /// </summary>
        protected abstract T Operation(string a);
    }

    /// <summary>
    /// abstract string based unit with result of type string 
    /// </summary>
    public abstract class BGCalcUnitStringAString : BGCalcUnitStringA<string>
    {
        /// <inheritdoc />
        protected override BGCalcTypeCode<string> OutputCode => BGCalcTypeCodeRegistry.String;
    }
}