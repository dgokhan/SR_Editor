/*
<copyright file="BGCalcUnit.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// abstract class for graph node
    /// </summary>
    public abstract class BGCalcUnit : BGCalcUnitI
    {
        //===========================================================================================================
        //                                        Static
        //===========================================================================================================


        //===========================================================================================================
        //                                        Fields
        //===========================================================================================================

        private readonly List<BGCalcControlInputI> inControls = new List<BGCalcControlInputI>();
        private readonly List<BGCalcControlOutputI> outControls = new List<BGCalcControlOutputI>();
        private readonly List<BGCalcValueInputI> inValues = new List<BGCalcValueInputI>();
        private readonly List<BGCalcValueOutputI> outValues = new List<BGCalcValueOutputI>();

        /// <inheritdoc />
        public BGCalcGraph Graph { get; set; }

        /// <inheritdoc />
        public Vector2 Position
        {
            get => position;
            set
            {
                if (position.Equals(value)) return;
                position = value;
                FireOnAnyChange();
            }
        }

        /// <inheritdoc />
        public virtual ushort TypeCode => 0;

        /// <inheritdoc />
        public List<BGCalcControlInputI> InControls => inControls;

        /// <inheritdoc />
        public List<BGCalcControlOutputI> OutControls => outControls;

        /// <inheritdoc />
        public List<BGCalcValueInputI> InValues => inValues;

        /// <inheritdoc />
        public List<BGCalcValueOutputI> OutValues => outValues;

        /// <inheritdoc />
        public virtual string Title
        {
            get
            {
                var type = GetType();
                var attribute = BGUtil.GetAttribute<BGCalcUnitDefinitionAttribute>(type);
                if (string.IsNullOrEmpty(attribute.name)) return "[Unknown]";
                var lastIndex = attribute.name.LastIndexOf('/');
                if (lastIndex == -1) return attribute.name;
                return attribute.name.Substring(lastIndex + 1);
            }
        }

        /// <inheritdoc />
        public List<BGCalcPortI> Ports
        {
            get
            {
                var result = new List<BGCalcPortI>(PortsCount);
                result.AddRange(inControls);
                result.AddRange(inValues);
                result.AddRange(outControls);
                result.AddRange(outValues);
                return result;
            }
        }

        /// <inheritdoc />
        public int PortsCount => inControls.Count + inValues.Count + outControls.Count + outValues.Count;

        //===========================================================================================================
        //                                        Abstract
        //===========================================================================================================
        /// <inheritdoc />
        public abstract void Definition();

        //===========================================================================================================
        //                                        Vars
        //===========================================================================================================
        private BGCalcVarLiteContainer varsContainer;
        private Vector2 position;

        /// <inheritdoc />
        public BGCalcVarLiteContainer GetVars(bool createIfMissing = false)
        {
            if (varsContainer == null && createIfMissing) varsContainer = new BGCalcVarLiteContainer(this);
            return varsContainer;
        }

        /// <summary>
        /// get variable by its ID
        /// </summary>
        public BGCalcVarLite GetVar(byte id) => GetVars(true).GetVar(id);

        //get or add variable with provided ID
        protected BGCalcVarLite GetOrAddVar(byte id, BGCalcTypeCode codeType)
        {
            var containerI = GetVars(true);
            var variable = containerI.GetVar(id);
            if (variable == null) variable = BGCalcVarLite.Create(this, id, codeType);
            return variable;
        }

        /// <inheritdoc />
        public virtual string GetPublicVarLabel(byte varId) => null;

        //===========================================================================================================
        //                                        Ports
        //===========================================================================================================
        /// <summary>
        /// Construct input control port.  
        /// </summary>
        protected BGCalcControlInput ControlInput(string name, string id, Func<BGCalcFlowI, BGCalcControlOutputI> action)
        {
            var controlInput = new BGCalcControlInput(this, name, id, action);
            inControls.Add(controlInput);
            // id2port?.Add(id, controlInput);
            return controlInput;
        }

        /// <summary>
        /// Construct output control port.  
        /// </summary>
        protected BGCalcControlOutput ControlOutput(string name, string id)
        {
            var controlOutput = new BGCalcControlOutput(this, name, id);
            outControls.Add(controlOutput);
            // id2port?.Add(id, controlOutput);
            return controlOutput;
        }

        /// <summary>
        /// Construct input value port.  
        /// </summary>
        protected BGCalcValueInput ValueInput<T>(string name, string id)
        {
            return ValueInput(typeof(T), name, id);
        }

        /// <summary>
        /// Construct input value port.  
        /// </summary>
        protected BGCalcValueInput ValueInput(Type type, string name, string id)
        {
            var valueInput = new BGCalcValueInput(this, name, id, type);
            inValues.Add(valueInput);
            // id2port?.Add(id, valueInput);
            return valueInput;
        }

        /// <summary>
        /// Construct input value port. Use this method for max performance 
        /// </summary>
        protected BGCalcValueInput ValueInput(BGCalcTypeCode typeCode, string name, string id)
        {
            var valueInput = new BGCalcValueInput(this, name, id, typeCode);
            inValues.Add(valueInput);
            // id2port?.Add(id, valueInput);
            return valueInput;
        }

        /// <summary>
        /// Construct output value port 
        /// </summary>
        protected BGCalcValueOutput ValueOutput<T>(string name, string id, Func<BGCalcFlowI, T> getValue)
        {
            return ValueOutput(typeof(T), name, id, flow => (object)getValue(flow));
        }

        /// <summary>
        /// Construct output value port 
        /// </summary>
        protected BGCalcValueOutput ValueOutput(Type type, string name, string id, Func<BGCalcFlowI, object> getValue)
        {
            var valueOutput = new BGCalcValueOutput(this, name, id, type, getValue);
            outValues.Add(valueOutput);
            // id2port?.Add(id, valueOutput);
            return valueOutput;
        }

        /// <summary>
        /// Construct output value port 
        /// </summary>
        protected BGCalcValueOutput ValueOutput(BGCalcTypeCode typeCode, string name, string id, Func<BGCalcFlowI, object> getValue)
        {
            var valueOutput = new BGCalcValueOutput(this, name, id, typeCode, getValue);
            outValues.Add(valueOutput);
            // id2port?.Add(id, valueOutput);
            return valueOutput;
        }

        /// <summary>
        /// Construct output value port 
        /// </summary>
        protected BGCalcValueOutput ValueOutput<T>(BGCalcTypeCode<T> typeCode, string name, string id, Func<BGCalcFlowI, T> getValue)
        {
            var valueOutput = new BGCalcValueOutput(this, name, id, typeCode,
                getValue == null
                    ? (Func<BGCalcFlowI, object>)null
                    : flow => getValue(flow)
            );
            outValues.Add(valueOutput);
            // id2port?.Add(id, valueOutput);
            return valueOutput;
        }

        //too slow
        internal void CheckPortName(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new Exception("Port name can not be null or empty string");
            if (!char.IsLetter(name[0])) throw new Exception($"Invalid port name {name[0]}. Port name should start with a letter");
            foreach (var @char in name)
                if (!char.IsLetter(@char) && @char != '_')
                    throw new Exception($"Invalid character {@char} in port name. Port name should contain letters and underscore only");
            CheckUnique(inControls, name);
            CheckUnique(outControls, name);
            CheckUnique(inValues, name);
            CheckUnique(outValues, name);
        }

        private void CheckUnique<T>(List<T> collection, string name) where T : BGCalcPortI
        {
            for (var i = 0; i < collection.Count; i++)
                if (collection[i].Name == name)
                    throw new Exception($"Unit {GetType().Name} already has a port with name {name}");
        }

        //slow
        /// <inheritdoc />
        public BGCalcPortI FindPort(Predicate<BGCalcPortI> filter = null)
        {
            BGCalcPortI result = null;
            if (FindPort(inControls, filter, ref result)) return result;
            if (FindPort(outControls, filter, ref result)) return result;
            if (FindPort(inValues, filter, ref result)) return result;
            if (FindPort(outValues, filter, ref result)) return result;
            return null;
        }

        /// <inheritdoc />
        public List<BGCalcPortI> FindPorts(Predicate<BGCalcPortI> filter)
        {
            var result = new List<BGCalcPortI>();
            FindPorts(inControls, filter, result);
            FindPorts(outControls, filter, result);
            FindPorts(inValues, filter, result);
            FindPorts(outValues, filter, result);
            return result;
        }

        private void FindPorts<T>(List<T> ports, Predicate<BGCalcPortI> predicate, List<BGCalcPortI> result) where T : BGCalcPortI
        {
            for (var i = 0; i < ports.Count; i++)
            {
                var port = ports[i];
                if (predicate != null && !predicate(port)) continue;
                result.Add(port);
            }
        }

        /// <inheritdoc />
        public BGCalcPortI FindPort(string id)
        {
            // return Id2port.TryGetValue(id, out var result) ? result : null;
            if (FindPort(id, inControls, out var inPort)) return inPort;
            if (FindPort(id, outControls, out var outPort)) return outPort;
            if (FindPort(id, inValues, out var inValue)) return inValue;
            if (FindPort(id, outValues, out var outValue)) return outValue;
            return null;
        }

        private static bool FindPort<T>(string id, List<T> ports, out BGCalcPortI findPort) where T : BGCalcPortI
        {
            findPort = null;
            for (var i = 0; i < ports.Count; i++)
            {
                var inControl = ports[i];
                if (!string.Equals(inControl.Id, id, StringComparison.Ordinal)) continue;
                findPort = inControl;
                return true;
            }

            return false;
        }


        private bool FindPort<T>(List<T> list, Predicate<T> filter, ref BGCalcPortI result) where T : BGCalcPortI
        {
            if (filter == null)
            {
                if (list.Count == 0)
                {
                    result = null;
                    return false;
                }

                result = list[0];
                return true;
            }

            result = list.Find(filter);
            return result != null;
        }

        /// <inheritdoc />
        public void RemovePort(BGCalcPortI port)
        {
            bool Filter(BGCalcPortI p)
            {
                return p.Id == port.Id;
            }

            BGCalcPortI result = null;
            switch (port.PortType)
            {
                case BGCalcPortTypeEnum.ControlIn:
                    if (FindPort(inControls, (Predicate<BGCalcPortI>)Filter, ref result)) inControls.Remove((BGCalcControlInputI)port);
                    break;
                case BGCalcPortTypeEnum.ControlOut:
                    if (FindPort(outControls, (Predicate<BGCalcPortI>)Filter, ref result)) outControls.Remove((BGCalcControlOutputI)port);
                    break;
                case BGCalcPortTypeEnum.ValueIn:
                    if (FindPort(inValues, (Predicate<BGCalcPortI>)Filter, ref result)) inValues.Remove((BGCalcValueInputI)port);
                    break;
                case BGCalcPortTypeEnum.ValueOut:
                    if (FindPort(outValues, (Predicate<BGCalcPortI>)Filter, ref result)) outValues.Remove((BGCalcValueOutputI)port);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(port.PortType));
            }
            // id2port?.Remove(port.Id);
        }

        //===========================================================================================================
        //                                        Methods
        //===========================================================================================================

        /*
        public virtual void Reset()
        {
            inControls.Clear();
            outControls.Clear();
            inValues.Clear();
            outValues.Clear();
            varsContainer?.ClearVars();
        }
        */
        public override string ToString() => Title;

        /*
        public BGCalcUnitI Clone()
        {
            //this is a slow version
            var unitI = (BGCalcUnitI)Activator.CreateInstance(GetType());
            if (varsContainer != null && varsContainer)
            {
                foreach (var variable in Variables) variable.CloneTo(unitI, true, true);
            }

            return unitI;
        }
        */

        /// <summary>
        /// fire any changed event
        /// </summary>
        public void FireOnAnyChange() => Graph?.FireOnAnyChange();

        /// <inheritdoc />
        public void OnVarsChange() => FireOnAnyChange();

        //=======================================================================
        //                          Equals
        //=======================================================================
        /*
        protected bool Equals(BGCalcUnit other)
        {
            if (!BGCalcVarContainerBaseA<BGCalcVarLite>.IsEqual(GetVars(), other.GetVars())) return false;
            if (!Position.Equals(other.Position)) return false;
            if (!ListAreEqual(inControls, other.inControls)) return false;
            if (!ListAreEqual(outControls, other.outControls)) return false;
            if (!ListAreEqual(inValues, other.inValues)) return false;
            if (!ListAreEqual(outValues, other.outValues)) return false;
            return true;
        }
        */

        public bool IsEqual(BGCalcUnitI other)
        {
            if (!BGCalcVarContainerBaseA<BGCalcVarLite>.IsEqual(GetVars(), other.GetVars())) return false;
            if (!Position.Equals(other.Position)) return false;
            if (!ListAreEqual(inControls, other.InControls)) return false;
            if (!ListAreEqual(outControls, other.OutControls)) return false;
            if (!ListAreEqual(inValues, other.InValues)) return false;
            if (!ListAreEqual(outValues, other.OutValues)) return false;
            return true;
        }
        private bool ListAreEqual<T>(List<T> list, List<T> list2) where T : BGCalcPortI
        {
            if (list.Count != list2.Count) return false;
            for (var i = 0; i < list.Count; i++)
            {
                var port = list[i];
                var port2 = list2[i];
                // if (!Equals(port, port2)) return false;
                if (!port.IsEqual(port2)) return false;
            }
            return true;
        }

        /*
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((BGCalcUnit)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = GetType().Name.GetHashCode();
                hashCode = (hashCode * 397) ^ (inControls != null ? inControls.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (outControls != null ? outControls.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (inValues != null ? inValues.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (outValues != null ? outValues.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (varsContainer != null ? varsContainer.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Position.GetHashCode();
                return hashCode;
            }
        }
    */
    }
}