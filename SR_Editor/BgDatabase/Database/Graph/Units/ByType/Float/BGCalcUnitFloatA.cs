/*
<copyright file="BGCalcUnitFloatA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// abstract float based unit with one float parameter and result of type T 
    /// </summary>
    public abstract class BGCalcUnitFloatA<T> : BGCalcUnit
    {
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
            ValueOutput(OutputCode, OutputLabel, "r", flow => Operation(flow.GetValue<float>(a)));
        }

        /// <summary>
        /// convert float value to result value 
        /// </summary>
        protected abstract T Operation(float a);
    }

    /// <summary>
    /// abstract float based unit with one float parameter and result of type float 
    /// </summary>
    public abstract class BGCalcUnitFloatAFloat : BGCalcUnitFloatA<float>
    {
        /// <inheritdoc />
        protected override BGCalcTypeCode<float> OutputCode => BGCalcTypeCodeRegistry.Float;
    }

    /// <summary>
    /// abstract float based unit with one float parameter and result of type int 
    /// </summary>
    public abstract class BGCalcUnitFloatAInt : BGCalcUnitFloatA<int>
    {
        /// <inheritdoc />
        protected override BGCalcTypeCode<int> OutputCode => BGCalcTypeCodeRegistry.Int;
    }
}