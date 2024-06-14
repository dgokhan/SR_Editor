/*
<copyright file="BGCalcUnitIntA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// abstract int based unit with result of type T
    /// </summary>
    public abstract class BGCalcUnitIntA<T> : BGCalcUnit
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
            a = ValueInput(BGCalcTypeCodeRegistry.Int, "A", "a");
            ValueOutput(OutputCode, OutputLabel, "r", flow => Operation(flow.GetValue<int>(a)));
        }

        /// <summary>
        /// function method to receive result value
        /// </summary>
        protected abstract T Operation(int a);
    }

    /// <summary>
    /// abstract int based unit with result of type int
    /// </summary>
    public abstract class BGCalcUnitIntAInt : BGCalcUnitIntA<int>
    {
        /// <inheritdoc />
        protected override BGCalcTypeCode<int> OutputCode => BGCalcTypeCodeRegistry.Int;
    }
}