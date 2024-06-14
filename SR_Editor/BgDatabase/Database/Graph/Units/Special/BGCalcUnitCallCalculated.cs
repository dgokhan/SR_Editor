/*
<copyright file="BGCalcUnitCallCalculated.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Unit for calling another calculated cell
    /// </summary>
    [BGCalcUnitDefinition("Special/Call calculated cell")]
    public class BGCalcUnitCallCalculated : BGCalcUnit2ControlsA
    {
        private BGCalcValueOutput r;
        public const byte CellVarId = 1;
        public event Action OnCellChange;
        public const int Code = 115;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override string Title => "Call calculated";

        /// <inheritdoc />
        public override string GetPublicVarLabel(byte varId) => CellVarId == varId ? "cell" : null;

        /// <inheritdoc />
        public override void Definition()
        {
            base.Definition();
            var cellVar = GetOrAddVar(CellVarId, BGCalcTypeCodeRegistry.Cell);
            OnCellChanged();
            cellVar.OnValueChange += OnCellChanged;
        }

        private void OnCellChanged()
        {
            for (var i = InControls.Count - 1; i >= 0; i--)
            {
                var inControl = InControls[i];
                if (inControl.Id == EnterPortName) continue;
                RemovePort(inControl);
            }

            for (var i = OutControls.Count - 1; i >= 0; i--)
            {
                var outControl = OutControls[i];
                if (outControl.Id == ExitPortName) continue;
                RemovePort(outControl);
            }

            for (var i = InValues.Count - 1; i >= 0; i--)
            {
                var inValue = InValues[i];
                RemovePort(inValue);
            }

            for (var i = OutValues.Count - 1; i >= 0; i--)
            {
                var outValue = OutValues[i];
                RemovePort(outValue);
            }

            var cellVar = GetVar(CellVarId);
            if (cellVar?.Value != null)
            {
                var refCell = (BGCalcCell)cellVar.Value;
                var field = refCell.Field;
                var entity = refCell.Entity;
                if (field != null && entity != null)
                    if (field is BGFieldCalcI calcI && field is BGStorable<BGFieldCalcValue> storable)
                    {
                        var graph = CellGraph(storable, calcI, entity, out var varsOverrides);

                        if (graph != null)
                        {
                            AddResultPort(calcI);
                            var vars = graph.GetVars(false);
                            foreach (var v in vars.Variables)
                            {
                                if (!v.IsPublic) continue;
                                AddPorts(v);
                            }
                        }
                    }
            }

            OnCellChange?.Invoke();
        }

        private void AddResultPort(BGFieldCalcI field) => r = ValueOutput(field.ResultCode, "result", "r", null);

        private static BGCalcGraph CellGraph(BGStorable<BGFieldCalcValue> storable, BGFieldCalcI calcI, BGEntity entity, out BGCalcVarsProvider varsOverrides)
        {
            varsOverrides = null;
            BGCalcGraph graph;
            var innerValue = storable.GetStoredValue(entity.Index);
            if (innerValue?.Graph != null)
            {
                graph = innerValue.Graph;
                varsOverrides = innerValue;
            }
            else graph = calcI.Graph;

            return graph;
        }

        private void AddPorts(BGCalcVar calcVar)
        {
            ValueInput(calcVar.TypeCode, calcVar.Name, GetInputId(calcVar.Id));
            ValueOutput(calcVar.TypeCode, calcVar.Name, GetOutputId(calcVar.Id), null);
        }

        private string GetInputId(BGId calcVarId) => "i_" + calcVarId;

        private string GetOutputId(BGId calcVarId) => "o_" + calcVarId;

        /// <inheritdoc />
        protected override void Run(BGCalcFlowI flow)
        {
            var cellVar = GetVar(CellVarId);
            var cell = (BGCalcCell)cellVar.Value;
            if (cell == null) throw new Exception("Can not execute calculated cell, cause the cell value was not set!");
            var field = cell.Field;
            if (field == null) throw new Exception("Can not execute calculated cell, cause the field is null!");
            if (!(field is BGFieldCalcI calcI)) throw new Exception("Can not execute calculated cell, cause the field is not a calculated field!");

            if (flow.Context.GraphType != BGCalcGraphTypeEnum.Action && field is BGFieldCalcAction)
                throw new Exception("Action calculated field can only be called from another Action calculated field!");

            if (!(field is BGStorable<BGFieldCalcValue> storable)) throw new Exception("Can not execute calculated cell, cause the field is not a BGStorable<BGFieldCalcValue> field!");
            var entity = cell.Entity;
            if (entity == null) throw new Exception("Can not execute calculated cell, cause the entity is null!");
            var graph = CellGraph(storable, calcI, entity, out var varsOverrides);
            if (graph == null) throw new Exception("Can not execute calculated cell, cause the graph is null!");

            if (flow.Level >= 16) throw new Exception("Can not execute calculated cell, cause the maximum nested level=16 is exceeded (seems like recursive call)!");

            var subContext = BGCalcFlowContext.Get();
            try
            {
                subContext.Graph = graph;
                subContext.CurrentEntity = entity;
                subContext.CurrentGameObject = flow.Context.CurrentGameObject;
                subContext.VarsOverrides = varsOverrides;
                subContext.GraphType = flow.Context.GraphType;

                var subFlow = new BGCalcFlow(subContext)
                {
                    Level = flow.Level + 1
                };

                //copy vars 
                var flowVars = subFlow.GetVars(false).Variables;
                foreach (var variable in flowVars)
                {
                    if (!variable.IsPublic) continue;
                    var inPortId = GetInputId(variable.Id);
                    var port = FindPort(inPortId);
                    if (port == null) continue;
                    variable.Value = flow.GetValue((BGCalcValueInputI)port);
                }


                var startPort = graph.StartUnit.StartPort;
                subContext.Graph = graph;
                if (startPort.IsConnected) subFlow.Run();

                //copy vars back
                flow.SetValue(r, subFlow.Result);
                flowVars = subFlow.GetVars(false).Variables;
                foreach (var variable in flowVars)
                {
                    if (!variable.IsPublic) continue;
                    var outPortId = GetOutputId(variable.Id);
                    var port = FindPort(outPortId);
                    if (port == null) continue;
                    flow.SetValue((BGCalcValueOutputI)port, variable.Value);
                }
            }
            finally
            {
                BGCalcFlowContext.Return(subContext);
            }

        }
    }
}