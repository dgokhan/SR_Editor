/*
<copyright file="BGCalcUnitWhile.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// while loop node
    /// </summary>
    [BGCalcUnitDefinition("Flow/While")]
    public class BGCalcUnitWhile : BGCalcUnit2ControlsA
    {
        public const int Code = 107;

        private BGCalcControlOutput bodyPort;

        private BGCalcValueInput condition;
        private BGCalcValueOutput indexOutput;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override void Definition()
        {
            base.Definition();

            bodyPort = ControlOutput("body", "a");

            condition = ValueInput(BGCalcTypeCodeRegistry.Bool, "condition", "b");
            indexOutput = ValueOutput(BGCalcTypeCodeRegistry.Int, "index", "c", flow =>
            {
                var localVar = flow.GetLocalVar(indexOutput);
                return localVar == null ? 0 : (int)localVar;
            });
        }

        private bool GetCondition(BGCalcFlowI flow) => flow.GetValue<bool>(condition);

        /// <inheritdoc />
        protected override void Run(BGCalcFlowI flow)
        {
            if (!bodyPort.IsConnected) return;
            var index = 0;
            while (GetCondition(flow))
            {
                flow.SetValue(indexOutput, index);
                flow.RunNested(bodyPort.ConnectedPort);
                if (index++ > BGCalcFlow.MaximumIterations) throw new Exception($"Maximum number of iterations={BGCalcFlow.MaximumIterations} is exceeded!");
                if (flow.BreakIsRequested) break;
            }
        }
    }
}