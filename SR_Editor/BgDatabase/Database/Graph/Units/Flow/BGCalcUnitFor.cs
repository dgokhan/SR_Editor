/*
<copyright file="BGCalcUnitFor.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// For loop unit
    /// </summary>
    [BGCalcUnitDefinition("Flow/For")]
    public class BGCalcUnitFor : BGCalcUnit2ControlsA
    {
        public const int Code = 102;

        private BGCalcControlOutput bodyPort;

        private BGCalcValueInput firstPort;
        private BGCalcValueInput lastPort;
        private BGCalcValueInput stepPort;

        private BGCalcValueOutput indexPort;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override void Definition()
        {
            base.Definition();

            bodyPort = ControlOutput("body", "a");

            firstPort = ValueInput(BGCalcTypeCodeRegistry.Int, "First", "b");
            lastPort = ValueInput(BGCalcTypeCodeRegistry.Int, "Last", "c");
            stepPort = ValueInput(BGCalcTypeCodeRegistry.Int, "Step", "d");

            indexPort = ValueOutput(BGCalcTypeCodeRegistry.Int, "Index", "e", flow =>
            {
                var localVar = flow.GetLocalVar(indexPort);
                return localVar == null ? 0 : (int)localVar;
            });
        }

        /// <inheritdoc />
        protected override void Run(BGCalcFlowI flow)
        {
            var first = flow.GetValue<int>(firstPort);
            var last = flow.GetValue<int>(lastPort);
            var step = flow.GetValue<int>(stepPort);

            if (last - first >= last - first + step) throw new Exception($"Loop can not be executed, cause with such parameters (First={first}, last={last}, step={step}), the loop will never end");
            if (first + step * BGCalcFlow.MaximumIterations < last) throw new Exception($"Maximum number of iterations={BGCalcFlow.MaximumIterations} is exceeded!");
            if (bodyPort.IsConnected)
            {
                var index = 0;
                for (var i = first; i < last; i += step)
                {
                    flow.SetValue(indexPort, i);
                    flow.RunNested(bodyPort.ConnectedPort);
                    if (index++ > BGCalcFlow.MaximumIterations) throw new Exception($"Maximum number of iterations={BGCalcFlow.MaximumIterations} is exceeded!");
                    if (flow.BreakIsRequested) break;
                }
            }
            else flow.SetValue(indexPort, last - step);
        }
    }
}