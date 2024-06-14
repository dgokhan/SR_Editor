/*
<copyright file="BGCalcControlOutput.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// output control port implementation
    /// </summary>
    public class BGCalcControlOutput : BGCalcPort, BGCalcControlOutputI
    {
        private BGCalcControlInputI connectedPort;
        
        /// <inheritdoc />
        public override bool IsConnected => connectedPort != null;

        /// <inheritdoc />
        public BGCalcControlInputI ConnectedPort => connectedPort;
        
        /// <inheritdoc />
        public override BGCalcTypeCode TypeCode => BGCalcTypeCodeRegistry.Control;

        /// <inheritdoc />
        public override List<BGCalcPortI> ConnectedPorts => connectedPort == null ? null : new List<BGCalcPortI>() { connectedPort };

        public BGCalcControlOutput(BGCalcUnit unit, string name, string id) : base(unit, name, id, BGCalcPortTypeEnum.ControlOut, typeof(BGCalcControl))
        {
        }

        /// <inheritdoc />
        public override void Connect(BGCalcPortI port, bool connectBoth = true)
        {
            if (port == null) throw new Exception("Can not connect: port is null");
            if (!(port is BGCalcControlInputI portTyped)) throw new Exception("Can not connect: wrong port type, should be BGCalcControlInputI! type=" + port.GetType().FullName);

            connectedPort?.Disconnect(this);
            connectedPort = portTyped;
            if (connectBoth)
            {
                portTyped.Connect(this, false);
                FireOnAnyChange();
            }
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
            if (toConnectPort.PortType != BGCalcPortTypeEnum.ControlIn) return false;
            if (Unit == toConnectPort.Unit) return false;
            if (HasRecursion((BGCalcControlInputI)toConnectPort)) return false;
            return true;
        }

        //check for recursion
        private bool HasRecursion(BGCalcControlInputI port)
        {
            var stack = new Stack<BGCalcUnitI>();
            stack.Push(port.Unit);

            while (stack.Count > 0)
            {
                var unit = stack.Pop();
                var outputPorts = unit.FindPorts(p => p.PortType == BGCalcPortTypeEnum.ControlOut && p.IsConnected);

                foreach (var outPort in outputPorts)
                {
                    var inputPort = ((BGCalcControlOutputI)outPort).ConnectedPort;
                    var unitToCheck = inputPort.Unit;
                    if (unitToCheck == Unit) return true;
                    if (stack.Contains(unitToCheck)) return true;
                    stack.Push(unitToCheck);
                }
            }

            return false;
        }

        public override bool IsEqual(BGCalcPortI other)
        {
            if (other == this) return true;
            if (!base.IsEqual(other)) return false;
            if (!(other is BGCalcControlOutput otherTyped)) return false;
            if (connectedPort != null)
            {
                if(!connectedPort.IsEqual(otherTyped.connectedPort))return false;
            }
            else if (otherTyped.connectedPort != null) return false;
                
            return true;
        }
    }
}