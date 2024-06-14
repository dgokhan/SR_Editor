/*
<copyright file="BGCalcUnitIf.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// If node
    /// </summary>
    [BGCalcUnitDefinition("Flow/If")]
    public class BGCalcUnitIf : BGCalcUnit
    {
        public const int Code = 106;
        
        /// <inheritdoc />
        public override ushort TypeCode => Code;

        // private BGCalcControlInput enterPort;
        private BGCalcControlOutput truePort;
        private BGCalcControlOutput falsePort;

        private BGCalcValueInput conditionPort;

        /// <inheritdoc />
        public override void Definition()
        {
            ControlInput("enter", "a", Eval);
            truePort = ControlOutput("true", "b");
            falsePort = ControlOutput("false", "c");

            conditionPort = ValueInput(BGCalcTypeCodeRegistry.Bool, "condition", "d");
        }

        private BGCalcControlOutput Eval(BGCalcFlowI flow) => flow.GetValue<bool>(conditionPort) ? truePort : falsePort;
    }
}