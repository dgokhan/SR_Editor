/*
<copyright file="BGCalcUnitForEach.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System.Collections;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// foreach unit
    /// </summary>
    [BGCalcUnitDefinition("Flow/ForEach")]
    public class BGCalcUnitForEach : BGCalcUnit2ControlsA
    {
        public const int Code = 108;

        private BGCalcControlOutput bodyPort;

        private BGCalcValueInput listPort;
        private BGCalcValueOutput objPort;
        private BGCalcValueOutput indexPort;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override void Definition()
        {
            base.Definition();

            bodyPort = ControlOutput("body", "a");

            listPort = ValueInput(BGCalcTypeCodeRegistry.List, "list", "b");
            objPort = ValueOutput(BGCalcTypeCodeRegistry.Object, "object", "c", flow =>
            {
                var localVar = flow.GetLocalVar(objPort);
                return localVar;
            });
            indexPort = ValueOutput(BGCalcTypeCodeRegistry.Int, "index", "d", flow =>
            {
                var localVar = flow.GetLocalVar(indexPort);
                return localVar == null ? 0 : (int)localVar;
            });
        }

        /// <inheritdoc />
        protected override void Run(BGCalcFlowI flow)
        {
            if (!bodyPort.IsConnected) return;
            var list = flow.GetValue<IList>(listPort);
            if (list == null || list.Count == 0) return;
            for (var i = 0; i < list.Count; i++)
            {
                flow.SetValue(objPort, list[i]);
                flow.SetValue(indexPort, i);
                flow.RunNested(bodyPort.ConnectedPort);
                if (flow.BreakIsRequested) break;
            }
        }
    }
}