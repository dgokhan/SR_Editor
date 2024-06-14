/*
<copyright file="BGCalcControlInput.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// input control port implementation
    /// </summary>
    public class BGCalcControlInput : BGCalcPort, BGCalcControlInputI
    {
        private readonly List<BGCalcControlOutputI> connectedPorts = new List<BGCalcControlOutputI>();

        /// <inheritdoc />
        public override bool IsConnected => connectedPorts.Count > 0;

        /// <inheritdoc />
        public Func<BGCalcFlowI, BGCalcControlOutputI> Action { get; }

        /// <inheritdoc />
        public override BGCalcTypeCode TypeCode => BGCalcTypeCodeRegistry.Control;

        /// <inheritdoc />
        public override List<BGCalcPortI> ConnectedPorts => connectedPorts.Count == 0 ? null : new List<BGCalcPortI>(connectedPorts);

        public BGCalcControlInput(BGCalcUnit unit, string name, string id, Func<BGCalcFlowI, BGCalcControlOutputI> action) : base(unit, name, id, BGCalcPortTypeEnum.ControlIn, typeof(BGCalcControl))
        {
            Action = action;
        }

        /// <inheritdoc />
        public override void Connect(BGCalcPortI port, bool connectBoth = true)
        {
            if (port == null) throw new Exception("Can not connect: port is null");
            if (!(port is BGCalcControlOutputI portI)) throw new Exception("Can not connect: wrong port type, should be BGCalcControlOutputI! type=" + port.GetType().FullName);
            if (connectedPorts.Contains(portI)) return;
            connectedPorts.Add(portI);
            if (connectBoth)
            {
                portI.Connect(this, false);
                FireOnAnyChange();
            }
        }

        /// <inheritdoc />
        public override void Disconnect(BGCalcPortI port, bool disconnectBoth = true)
        {
            if (!(port is BGCalcControlOutputI outControl)) return;
            connectedPorts.Remove(outControl);
            if (disconnectBoth)
            {
                outControl.Disconnect(this, false);
                FireOnAnyChange();
            }
        }

        /// <inheritdoc />
        public override void DisconnectAll()
        {
            if (connectedPorts.Count == 0) return;
            Unit.Graph.Batch(() =>
            {
                for (var i = connectedPorts.Count - 1; i >= 0; i--) Disconnect(connectedPorts[i]);
            });
        }

        /// <inheritdoc />
        public override bool CanConnectTo(BGCalcPortI toConnectPort)
        {
            if (toConnectPort.PortType != BGCalcPortTypeEnum.ControlOut) return false;
            return ((BGCalcControlOutputI)toConnectPort).CanConnectTo(this);
        }

        /// <inheritdoc />
        public override bool IsEqual(BGCalcPortI other)
        {
            if (other == this) return true;
            if (!base.IsEqual(other)) return false;
            if (!(other is BGCalcControlInput valueInput)) return false;
            var otherConnectedPorts = valueInput.connectedPorts;
            if (!ListEqual(connectedPorts, otherConnectedPorts)) return false;
            return true;
        }
    }
}