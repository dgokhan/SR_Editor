/*
<copyright file="BGCalcGraph.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Data container for calculation graph
    /// </summary>
    public class BGCalcGraph : BGCalcVarsOwnerI, ICloneable
    {
        public const int MaxUnitsNumber = 255;
        public event Action OnAnyChange;

        private readonly List<BGCalcUnitI> units = new List<BGCalcUnitI>();

        private BGCalcUnitGraphStart startUnit;
        private byte[] byteContent;
        private string stringContent;

        private bool disableEvents;

        /// <summary>
        /// Start calculation unit
        /// </summary>
        public BGCalcUnitGraphStart StartUnit
        {
            get
            {
                EnsureGraphIsLoaded();
                return startUnit;
            }
        }

        private BGCalcGraph()
        {
            varsContainer = new BGCalcVarContainer(this);
            varsContainer.OnDelete += OnVarDelete;
            // varsContainer.OnAnyChange += FireOnAnyChange;
        }

        /*
        public void Reset()
        {
            // CheckIfLoaded();
            // varsContainer?.Reset();
            // foreach (var unit in units) unit.Reset();
        }
        */

        //=======================================================================
        //                          Units
        //=======================================================================
        /*
        public List<BGCalcUnitI> Units
        {
            get
            {
                EnsureGraphIsLoaded();
                return units;
            }
        }
        */
        /// <summary>
        /// Get unit with provided index
        /// </summary>
        public BGCalcUnitI GetUnit(int index)
        {
            EnsureGraphIsLoaded();
            return units[index];
        }

        /// <summary>
        /// Find first unit using provided filter
        /// </summary>
        public BGCalcUnitI FindUnit(Predicate<BGCalcUnitI> filter = null)
        {
            EnsureGraphIsLoaded();
            for (var i = 0; i < units.Count; i++)
            {
                var unit = units[i];
                if (filter == null || filter(unit)) return unit;
            }

            return null;
        }

        /// <summary>
        /// Find all units using provided filter
        /// </summary>
        public List<BGCalcUnitI> FindUnits(Predicate<BGCalcUnitI> filter = null, List<BGCalcUnitI> result = null)
        {
            EnsureGraphIsLoaded();
            var list = result ?? new List<BGCalcUnitI>();
            for (var i = 0; i < units.Count; i++)
            {
                var unit = units[i];
                if (filter != null && !filter(unit)) continue;
                list.Add(unit);
            }

            return list;
        }

        /// <summary>
        /// how many units the graph has
        /// </summary>
        public int UnitsCount
        {
            get
            {
                EnsureGraphIsLoaded();
                return units.Count;
            }
        }

        /// <summary>
        /// Add unit without initializing it
        /// </summary>
        public void AddUnitNoInit(BGCalcUnitI unit)
        {
            EnsureGraphIsLoaded();
            if (units.Count >= byte.MaxValue) throw new Exception($"Maximum number of units [{MaxUnitsNumber}] was already added!");
            if (unit is BGCalcUnitGraphStart start)
            {
                if (startUnit != null) throw new Exception("Can not add a start node cause start node already added!");
                startUnit = start;
            }

            units.Add(unit);
            unit.Graph = this;
            FireOnAnyChange();
        }

        /// <summary>
        /// iterate over all units
        /// </summary>
        public void ForEachUnit(Action<BGCalcUnitI> action, Func<BGCalcUnitI, bool> filter = null)
        {
            EnsureGraphIsLoaded();
            for (var i = 0; i < units.Count; i++)
            {
                var calcUnitI = units[i];
                if (filter != null && !filter(calcUnitI)) continue;
                action(calcUnitI);
            }
        }

        /// <summary>
        /// remove provided unit 
        /// </summary>
        public void RemoveUnit(BGCalcUnitI unit)
        {
            EnsureGraphIsLoaded();
            Batch(() =>
            {
                RemoveUnitInternal(unit);
            });
        }

        /// <summary>
        /// remove all provided units 
        /// </summary>
        public void RemoveUnits(List<BGCalcUnitI> unitsToRemove)
        {
            if (unitsToRemove == null || unitsToRemove.Count == 0) return;
            EnsureGraphIsLoaded();
            Batch(() =>
            {
                foreach (var unit in unitsToRemove) RemoveUnitInternal(unit);
            });
        }

        private void RemoveUnitInternal(BGCalcUnitI unit)
        {
            if (unit is BGCalcUnitGraphStart) throw new Exception("Can not delete a start unit");
            foreach (var port in unit.Ports) port.DisconnectAll();
            units.Remove(unit);
        }

        //=======================================================================
        //                          Variables
        //=======================================================================
        private readonly BGCalcVarContainer varsContainer;

        /// <inheritdoc />
        public BGCalcVarContainer GetVars(bool createIfMissing = false)
        {
            EnsureGraphIsLoaded();
            return varsContainer;
        }


        //on variable removed callback- we need to remove all GetVar and SetVar units for removed variable 
        private void OnVarDelete(List<BGCalcVar> list)
        {
            //remove related units
            List<BGCalcUnitI> unitsToRemove = null;
            foreach (var unit in units)
            {
                if (!(unit is BGCalcUnitVarA varUnit)) continue;
                foreach (var calcVar in list)
                {
                    var unitVar = varUnit.GraphVar;
                    if (!Equals(unitVar, calcVar)) continue;
                    unitsToRemove = unitsToRemove ?? new List<BGCalcUnitI>();
                    unitsToRemove.Add(varUnit);
                }
            }

            RemoveUnits(unitsToRemove);
        }

        //this method is not used !
        private void OnBeforeAdd(BGCalcVar variable)
        {
            if (variable == null) throw new Exception("Variable is null");
            var checkName = BGMetaObject.CheckName(variable.Name);
            if (checkName != null) throw new Exception(checkName);
            if (varsContainer.HasVar(variable.Id)) throw new Exception("var with such id already added!");
        }

        /// <inheritdoc />
        public void OnVarsChange() => FireOnAnyChange();

        //=======================================================================
        //                          execute
        //=======================================================================
        /// <summary>
        /// Execute the graph using provided context and return the flow
        /// </summary>
        public BGCalcFlowI Launch(BGCalcFlowContext context)
        {
            EnsureGraphIsLoaded();
            var startPort = StartUnit.StartPort;
            context.Graph = this;
            var flow = new BGCalcFlow(context);
            if (!startPort.IsConnected) return flow;
            flow.Run();
            return flow;
        }

        /// <summary>
        /// Execute the graph using provided context and return calculation result
        /// </summary>
        public object Execute(BGCalcFlowContext context)
        {
            var flow = Launch(context);
            return flow.Result;
        }

        /// <summary>
        /// Execute the graph using provided context and return calculation result as T
        /// </summary>
        public T Execute<T>(BGCalcFlowContext context)
        {
            var result = Execute(context);
            if (result == null) return default;
            return (T)result;
        }


        //=======================================================================
        //                          serialization
        //=======================================================================
        /// <summary>
        /// serialize graph content into byte array
        /// </summary>
        public byte[] ToBytes()
        {
            EnsureGraphIsLoaded();
            var saver = new BGCalcSaver(this);
            return saver.Save();
        }

        /// <summary>
        /// restore graph content from byte array
        /// The graph is not reconstructed immediately, it's constructed on demand 
        /// </summary>
        public void FromBytes(ArraySegment<byte> arraySegment)
        {
            byteContent = BGUtil.ToArray(arraySegment);
            stringContent = null;
        }

        /// <summary>
        /// serialize graph content into JSON string
        /// </summary>
        public string ToJsonString()
        {
            EnsureGraphIsLoaded();
            var saver = new BGCalcSaverString(this);
            return saver.Save();
        }

        /// <summary>
        /// restore graph content from string
        /// The graph is not reconstructed immediately, it's constructed on demand 
        /// </summary>
        public void FromJsonString(string json)
        {
            stringContent = json;
            byteContent = null;
        }

        /// <summary>
        /// Ensures the graph is constructed from binary stream or JSON string
        /// </summary>
        public void EnsureGraphIsLoaded()
        {
            if (byteContent != null)
                Batch(() =>
                {
                    var toLoad = byteContent;
                    Clear();
                    var loader = new BGCalcLoader(this, new ArraySegment<byte>(toLoad));
                    loader.Load();
                }, false);
            else if (stringContent != null)
                Batch(() =>
                {
                    var toLoad = stringContent;
                    Clear();
                    var loader = new BGCalcLoaderString();
                    loader.Load(this, toLoad);
                }, false);
        }

        /// <inheritdoc />
        public object Clone()
        {
            var content = new BGCalcSaver(this).Save();
            var clone = ExistingGraph();
            var loader = new BGCalcLoader(clone, new ArraySegment<byte>(content));
            loader.Load();
            return clone;
        }

        /// <summary>
        /// Fire on any change event
        /// </summary>
        public void FireOnAnyChange()
        {
            if (disableEvents) return;
            OnAnyChange?.Invoke();
        }

        /// <summary>
        /// Clear graph 
        /// </summary>
        public void Clear()
        {
            units.Clear();
            varsContainer.ClearVarsNoEvent();
            startUnit = null;
            byteContent = null;
            stringContent = null;
        }

        /// <summary>
        /// Batch mode to suppress individual events firing
        /// </summary>
        public void Batch(Action action, bool fireEventInTheEnd = true)
        {
            /*
            using (new BGDisableEvents(this))
            {
                action();
            }
            */
            disableEvents = true;
            try
            {
                action();
            }
            finally
            {
                disableEvents = false;
            }

            if (fireEventInTheEnd) FireOnAnyChange();
        }

        /// <summary>
        /// Clear anyChange event
        /// </summary>
        public void ClearOnAnyChange() => OnAnyChange = null;

        //=======================================================================
        //                          Equals
        //=======================================================================
        public bool IsEqual(BGCalcGraph other)
        {
            if (!BGCalcVarContainerBaseA<BGCalcVar>.IsEqual(GetVars(), other.GetVars())) return false;
            if (units.Count != other.units.Count) return false;
            for (var i = 0; i < units.Count; i++)
            {
                var unit = units[i];
                var unit2 = other.units[i];
                if (!unit.IsEqual(unit2)) return false;
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            if (!(obj is BGCalcGraph typed)) return false;
            return IsEqual(typed);
        }

        public override int GetHashCode() => units != null ? units.GetHashCode() : 0;

        //=======================================================================
        //                          Static
        //=======================================================================

        /// <summary>
        /// Construct new graph with provided graph result
        /// </summary>
        public static BGCalcGraph NewGraph(BGCalcTypeCode resultCode)
        {
            var graph = new BGCalcGraph();

            //start unit
            graph.AddUnitNoInit(new BGCalcUnitGraphStart());
            graph.Init(graph.StartUnit);

            //if resultCode is set
            if (resultCode != null && !Equals(resultCode, BGCalcTypeCodeRegistry.CalcAction))
            {
                var setResultNode = new BGCalcUnitSetResult();
                setResultNode.Init(resultCode);
                setResultNode.Position = new Vector2(250, 0);
                graph.AddUnit(setResultNode);
                var startOutControl = (BGCalcControlOutputI)graph.StartUnit.FindPort();
                var setResultInControl = setResultNode.FindPort(p => p is BGCalcControlInput);
                startOutControl.Connect(setResultInControl);
            }

            return graph;
        }

        /// <summary>
        /// Construct previously created graph 
        /// </summary>
        public static BGCalcGraph ExistingGraph() => new BGCalcGraph();

        /// <summary>
        /// Initialized graph unit
        /// </summary>
        public void Init(BGCalcUnitI unitI)
        {
            unitI.Graph = this;
            unitI.Definition();
        }

        /// <summary>
        /// add unit to the graph
        /// </summary>
        public void AddUnit(BGCalcUnitI unitI)
        {
            Init(unitI);
            AddUnitNoInit(unitI);
        }
        //=======================================================================
        //                          Inner classes
        //=======================================================================

        /*
        private struct BGDisableEvents : IDisposable
        {
            private readonly BGCalcGraph graph;
            internal BGDisableEvents(BGCalcGraph graph)
            {
                this.graph = graph;
                this.graph.disableEvents = true;
            }

            public void Dispose()
            {
                this.graph.disableEvents = false;
            }
        }
    */
    }
}