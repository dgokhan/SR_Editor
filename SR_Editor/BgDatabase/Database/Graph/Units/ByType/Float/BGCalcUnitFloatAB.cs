/*
<copyright file="BGCalcUnitFloatAB.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// abstract float based unit with two float parameter and result of type T 
    /// </summary>
    public abstract class BGCalcUnitFloatAB<T> : BGCalcUnit
    {
        private BGCalcValueInput b;
        private BGCalcValueInput a;

        /// <summary>
        /// result output code
        /// </summary>
        protected abstract BGCalcTypeCode<T> OutputCode { get; }
        
        /// <summary>
        /// result output label
        /// </summary>
        protected abstract string OutputLabel { get; }

        /// <inheritdoc />
        public override void Definition()
        {
            a = ValueInput(BGCalcTypeCodeRegistry.Float, "A", "a");
            b = ValueInput(BGCalcTypeCodeRegistry.Float, "B", "b");
            ValueOutput(OutputCode, OutputLabel, "r", flow => Operation(flow.GetValue<float>(a), flow.GetValue<float>(b)));
        }

        /// <summary>
        /// convert parameters float values to result value 
        /// </summary>
        protected abstract T Operation(float a, float b);
    }

    /// <summary>
    /// abstract float based unit with two float parameter and result of type float 
    /// </summary>
    public abstract class BGCalcUnitFloatABFloat : BGCalcUnitFloatAB<float>
    {
        /// <inheritdoc />
        protected override BGCalcTypeCode<float> OutputCode => BGCalcTypeCodeRegistry.Float;
    }

    /// <summary>
    /// abstract float based unit with two float parameter and result of type bool 
    /// </summary>
    public abstract class BGCalcUnitFloatABBool : BGCalcUnitFloatAB<bool>
    {
        /// <inheritdoc />
        protected override BGCalcTypeCode<bool> OutputCode => BGCalcTypeCodeRegistry.Bool;
    }

    /// <summary>
    /// abstract float based unit with two float parameter and result of type int 
    /// </summary>
    public abstract class BGCalcUnitFloatABInt : BGCalcUnitFloatAB<int>
    {
        /// <inheritdoc />
        protected override BGCalcTypeCode<int> OutputCode => BGCalcTypeCodeRegistry.Int;
    }
}