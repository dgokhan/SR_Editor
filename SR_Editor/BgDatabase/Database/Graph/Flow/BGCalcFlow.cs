/*
<copyright file="BGCalcFlow.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Flow object is used during graph execution to hold graph values
    /// </summary>
    public class BGCalcFlow : BGCalcFlowI
    {
        public const int MaximumIterations = 10000;
        public const string UnitExceptionKey = "u";

        private readonly Dictionary<BGCalcPortI, object> localVars = new Dictionary<BGCalcPortI, object>();
        // private readonly Stack<BGCalcControlInputI> callbackStack = new Stack<BGCalcControlInputI>();

        private readonly BGCalcFlowContext context;
        private bool breakIsRequested;
        
        /// <inheritdoc />
        public object Result { get; set; }

        /// <inheritdoc />
        public bool BreakIsRequested
        {
            get
            {
                var result = breakIsRequested;
                breakIsRequested = false;
                return result;
            }
            set => breakIsRequested = value;
        }

        /// <inheritdoc />
        public int Level { get; set; }
        
        /// <inheritdoc />
        public BGCalcFlowI Parent { get; set; }
        
        /// <inheritdoc />
        public BGCalcFlowContext Context => context;

        public BGCalcFlow(BGCalcFlowContext context)
        {
            this.context = context;
            var graph = context.Graph;
            var varsOverrides = context.VarsOverrides;
            varsContainer = new BGCalcVarContainer(this);
            if (graph.GetVars()?.Variables.Count > 0)
                foreach (var variable in graph.GetVars().Variables)
                {
                    var clonedVar = variable.CloneTo(this, true, true);
                    if (varsOverrides != null && varsOverrides.TryGet(variable.Id, out var value)) clonedVar.Value = value;
                }
        }

        /// <summary>
        /// execute the graph
        /// </summary>
        public void Run()
        {
            localVars.Clear();
            // callbackStack.Clear();

            var startPort = context.Graph.StartUnit.StartPort;
            if (!startPort.IsConnected) return;

            new FlowBranch(this, startPort.ConnectedPort).Run();
        }

        /// <inheritdoc />
        public BGCalcControlOutputI Run(BGCalcControlInputI port)
        {
            // if (currentPort.Connection == null) return null;
            // var inControl = currentPort.Connection.To;

            return port.Action(this);
        }

        /// <inheritdoc />
        public void RunNested(BGCalcControlInputI connectedPort) => new FlowBranch(this, connectedPort).Run();

        /// <inheritdoc />
        public object GetLocalVar(BGCalcPortI port)
        {
            if (localVars.TryGetValue(port, out var result)) return result;
            return null;
        }

        /// <inheritdoc />
        public T GetValue<T>(BGCalcValueInputI input) => (T)GetValue(input);
        // public object GetValue(BGCalcValueInputI input, System.Type type) => GetValue(input);

        /// <inheritdoc />
        public object GetValue(BGCalcValueInputI input)
        {
            if (localVars.TryGetValue(input, out var result)) return result;

            var connectedPort = input.ConnectedPort;
            if (connectedPort != null)
            {
                var value = GetValue(connectedPort);
                //convert
                if (input.TypeCode != null)
                {
                    if (connectedPort.TypeCode != null)
                    {
                        if (input.TypeCode.TypeCode != connectedPort.TypeCode.TypeCode) value = input.TypeCode.ConvertFrom(connectedPort.TypeCode, value);
                    }
                    else value = input.TypeCode.ConvertFrom(null, value);
                }

                return value;
            }

            if (input.SupportDefaultValue && input.DefaultValue != null) return input.DefaultValue;
            throw new Exception($"Can not get value from port {input.Unit.Title}.{input.Name}: no connection and no default value!");
        }

        /// <inheritdoc />
        public object GetValue(BGCalcValueOutputI output)
        {
            if (localVars.TryGetValue(output, out var result)) return result;

            if (output.GetValue != null)
                try
                {
                    var value = output.GetValue(this);
                    return value;
                }
                catch (Exception e)
                {
                    if (!e.Data.Contains(UnitExceptionKey)) e.Data.Add(UnitExceptionKey, output.Unit.Title);
                    throw;
                }

            throw new Exception("Can not get a value for output port [" + output.Unit.Title + '.' + output.Name + "]: no local value and no function defined");
        }

        /// <inheritdoc />
        public void SetValue(BGCalcPortI port, object value) => localVars[port] = value;


        /// <summary>
        /// local variables have value for provided port 
        /// </summary>
        public bool IsLocal(BGCalcPort port) => localVars.ContainsKey(port);

        /// <summary>
        /// remove local variable for provided port 
        /// </summary>
        public bool RemoveLocal(BGCalcPort port) => localVars.Remove(port);

        //===========================================================================================================
        //                                        Control
        //===========================================================================================================
        //===========================================================================================================
        //                                        Vars
        //===========================================================================================================
        private readonly BGCalcVarContainer varsContainer;

        /// <inheritdoc />
        public BGCalcVarContainer GetVars(bool createIfMissing = false) => varsContainer;

        /// <inheritdoc />
        public void OnVarsChange()
        {
        }

        //===========================================================================================================
        //                                        Control
        //===========================================================================================================
        /// <summary>
        /// Flow branch is a graph part, a set of connected nodes
        /// </summary>
        private class FlowBranch
        {
            private readonly BGCalcFlowI flow;
            private readonly BGCalcControlInputI port;

            public FlowBranch(BGCalcFlowI flow, BGCalcControlInputI port)
            {
                this.flow = flow;
                this.port = port;
            }

            /// <summary>
            /// execute graph branch
            /// </summary>
            public void Run()
            {
                var currentPort = port;
                while (currentPort != null)
                {
                    BGCalcControlOutputI outputPort;
                    try
                    {
                        outputPort = flow.Run(currentPort);
                    }
                    catch (Exception e)
                    {
                        if (!e.Data.Contains(UnitExceptionKey)) e.Data.Add(UnitExceptionKey, currentPort.Unit.Title);
                        throw;
                    }

                    if (outputPort != null && outputPort.IsConnected) currentPort = outputPort.ConnectedPort;
                    else currentPort = null;
                }
            }
        }
    }
}