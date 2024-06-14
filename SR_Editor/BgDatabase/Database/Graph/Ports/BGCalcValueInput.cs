/*
<copyright file="BGCalcValueInput.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// input value port implementation
    /// </summary>
    public class BGCalcValueInput : BGCalcPort, BGCalcValueInputI
    {
        /// <inheritdoc />
        public event Action OnChange;

        private BGCalcValueOutputI connectedPort;
        private readonly BGCalcTypeCode typeCode;
        private object defaultValue;
        
        /// <inheritdoc />
        public override bool IsConnected => connectedPort != null;

        /// <inheritdoc />
        public BGCalcValueOutputI ConnectedPort => connectedPort;
        
        /// <inheritdoc />
        public override BGCalcTypeCode TypeCode => typeCode;

        /// <inheritdoc />
        public override List<BGCalcPortI> ConnectedPorts => connectedPort == null ? null : new List<BGCalcPortI>() { connectedPort };

        public BGCalcValueInput(BGCalcUnit graph, string name, string id, BGCalcTypeCode typeCode) : base(graph, name, id, BGCalcPortTypeEnum.ValueIn, typeCode.Type)
        {
            this.typeCode = typeCode;
            if (this.typeCode != null && this.typeCode.SupportDefaultValue) DefaultValue = this.typeCode.DefaultValue;
        }

        public BGCalcValueInput(BGCalcUnit graph, string name, string id, Type type) : base(graph, name, id, BGCalcPortTypeEnum.ValueIn, type)
        {
            typeCode = BGCalcTypeCodeRegistry.Get(type);
            if (typeCode != null && typeCode.SupportDefaultValue) DefaultValue = typeCode.DefaultValue;
        }

        /// <inheritdoc />
        public override void Connect(BGCalcPortI port, bool connectBoth = true)
        {
            if (port == null) throw new Exception("Can not connect: port is null");
            if (!(port is BGCalcValueOutput)) throw new Exception("Can not connect: wrong port type, should be BGCalcValueOutput! type=" + port.GetType().FullName);

            connectedPort?.Disconnect(this);
            var portTyped = (BGCalcValueOutputI)port;
            connectedPort = portTyped;
            if (connectBoth)
            {
                port.Connect(this, false);
                FireOnAnyChange();
            }

            OnChange?.Invoke();
        }

        /// <inheritdoc />
        public override void Disconnect(BGCalcPortI port, bool disconnectBoth = true)
        {
            if (connectedPort != port || port == null) return;
            connectedPort = null;
            if (disconnectBoth)
            {
                port.Disconnect(this, false);
                FireOnAnyChange();
            }
        }

        /// <inheritdoc />
        public override void DisconnectAll() => Disconnect(connectedPort);

        /// <inheritdoc />
        public override bool CanConnectTo(BGCalcPortI toConnectPort)
        {
            if (toConnectPort.PortType != BGCalcPortTypeEnum.ValueOut) return false;
            // var outputI = (BGCalcValueOutputI)toConnectPort;
            if (TypeCode != null)
            {
                if (!Equals(TypeCode, toConnectPort.TypeCode) && !TypeCode.CanBeConvertedFrom(toConnectPort.TypeCode)) return false;
            }
            else if (!Type.IsAssignableFrom(toConnectPort.Type)) return false;

            if (HasRecursion((BGCalcValueOutputI)toConnectPort)) return false;

            return true;
        }

        //check for recursion
        private bool HasRecursion(BGCalcValueOutputI port)
        {
            if (port.Unit == Unit) return true;

            var stack = new Stack<BGCalcUnitI>();
            stack.Push(port.Unit);
            var processed = new HashSet<BGCalcUnitI> { port.Unit };

            while (stack.Count > 0)
            {
                var unit = stack.Pop();
                var inputPorts = unit.FindPorts(p => p.PortType == BGCalcPortTypeEnum.ValueIn && p.IsConnected);

                foreach (var inPort in inputPorts)
                {
                    var inputPort = ((BGCalcValueInputI)inPort).ConnectedPort;
                    var unitToCheck = inputPort.Unit;
                    if (processed.Contains(unitToCheck)) continue;
                    if (unitToCheck == Unit) return true;
                    stack.Push(unitToCheck);
                    processed.Add(unitToCheck);
                }
            }

            return false;
        }


        //======================================================================
        //  Default value
        //======================================================================
        /// <inheritdoc />
        public object DefaultValue
        {
            get
            {
                switch (defaultValue)
                {
                    case null:
                        return defaultValue;
                    case BGObjectI dbObject when BGCalcVarA.RefreshDbValue(ref dbObject):
                        defaultValue = dbObject;
                        break;
                }

                return defaultValue;
            }
            set
            {
                if (Equals(defaultValue, value)) return;
                defaultValue = value;
                OnChange?.Invoke();
                FireOnAnyChange();
            }
        }

        /// <inheritdoc />
        public bool HasDefaultValue
        {
            get
            {
                if (IsConnected) return false;
                return TypeCode != null && TypeCode.SupportDefaultValue && !TypeCode.AreEqual(defaultValue, TypeCode.DefaultValue);
            }
        }

        /// <inheritdoc />
        public bool SupportDefaultValue => typeCode != null && typeCode.SupportDefaultValue;

        //===========================================================================================================
        //                                        Vars
        //===========================================================================================================
        /*
        private BGCalcVarLiteContainer varsContainer;

        public BGCalcVarLiteContainer GetVars(bool createIfMissing = false)
        {
            if (varsContainer == null && createIfMissing)
            {
                varsContainer = new BGCalcVarLiteContainer(this);
                // varsContainer.OnAnyChange += FireOnAnyChange;
            }
            return varsContainer;
        }
        */

        //===========================================================================================================
        //                                        Vars
        //===========================================================================================================
        public override bool IsEqual(BGCalcPortI other)
        {
            if (other == this) return true;
            if (!base.IsEqual(other)) return false;
            if (!(other is BGCalcValueInput otherTyped)) return false;
            if (typeCode != null && typeCode.SupportDefaultValue && !typeCode.AreEqual(defaultValue, otherTyped.defaultValue)) return false;
            if (connectedPort != null && otherTyped.connectedPort == null) return false;
            if (connectedPort == null && otherTyped.connectedPort != null) return false;
            // if (connectedPort != null && otherTyped.connectedPort != null && connectedPort.Id != otherTyped.connectedPort.Id) return false;
            if (connectedPort != null)
            {
                if(!connectedPort.IsEqual(otherTyped.connectedPort)) return false;
            }
            else if (otherTyped.connectedPort != null) return false;
            return true;
        }
    }
}