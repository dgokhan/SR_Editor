/*
<copyright file="BGCalcUnitIntAB.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// abstract unit with two int input parameters and result of type T
    /// </summary>
    public abstract class BGCalcUnitIntAB<T> : BGCalcUnit
    {
        private BGCalcValueInput b;
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
            b = ValueInput(BGCalcTypeCodeRegistry.Int, "B", "b");
            ValueOutput(OutputCode, OutputLabel, "r", flow => Operation(flow.GetValue<int>(a), flow.GetValue<int>(b)));
        }

        /// <summary>
        /// function method to receive result value
        /// </summary>
        protected abstract T Operation(int a, int b);
    }

    /// <summary>
    /// abstract unit with two int input parameters and result of type int
    /// </summary>
    public abstract class BGCalcUnitIntABInt : BGCalcUnitIntAB<int>
    {
        /// <inheritdoc />
        protected override BGCalcTypeCode<int> OutputCode => BGCalcTypeCodeRegistry.Int;
    }

    /// <summary>
    /// abstract unit with two int input parameters and result of type bool
    /// </summary>
    public abstract class BGCalcUnitIntABBool : BGCalcUnitIntAB<bool>
    {
        /// <inheritdoc />
        protected override BGCalcTypeCode<bool> OutputCode => BGCalcTypeCodeRegistry.Bool;
    }
}