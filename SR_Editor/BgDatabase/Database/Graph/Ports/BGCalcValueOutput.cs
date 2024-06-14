/*
<copyright file="BGCalcValueOutput.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// output value port implementation
    /// </summary>
    public class BGCalcValueOutput : BGCalcPort, BGCalcValueOutputI
    {
        private readonly Func<BGCalcFlowI, object> getValue;
        private readonly BGCalcTypeCode typeCode;

        private readonly List<BGCalcValueInputI> connectedPorts = new List<BGCalcValueInputI>();

        /// <inheritdoc />
        public Func<BGCalcFlowI, object> GetValue => getValue;

        /// <inheritdoc />
        public override bool IsConnected => connectedPorts.Count > 0;

        /// <inheritdoc />
        public override BGCalcTypeCode TypeCode => typeCode;

        /// <inheritdoc />
        public override List<BGCalcPortI> ConnectedPorts => connectedPorts.Count == 0 ? null : new List<BGCalcPortI>(connectedPorts);

        public BGCalcValueOutput(BGCalcUnit unit, string name, string id, Type type, Func<BGCalcFlowI, object> getValue) : base(unit, name, id, BGCalcPortTypeEnum.ValueOut, type)
        {
            // if (getValue == null) throw new Exception("getValue function can not be null");
            typeCode = BGCalcTypeCodeRegistry.Get(type);
            this.getValue = getValue;
        }

        public BGCalcValueOutput(BGCalcUnit unit, string name, string id, BGCalcTypeCode typeCode, Func<BGCalcFlowI, object> getValue) : base(unit, name, id, BGCalcPortTypeEnum.ValueOut,
            typeCode.Type)
        {
            // if (getValue == null) throw new Exception("getValue function can not be null");
            this.typeCode = typeCode;
            this.getValue = getValue;
        }

        /// <inheritdoc />
        public override void Connect(BGCalcPortI port, bool connectBoth = true)
        {
            if (port == null) throw new Exception("Can not connect: port is null");
            if (!(port is BGCalcValueInputI portTyped)) throw new Exception("Can not connect: wrong port type, should be BGCalcValueInputI! type=" + port.GetType().FullName);
            if (connectedPorts.Contains(portTyped)) return;
            connectedPorts.Add(portTyped);
            if (connectBoth)
            {
                portTyped.Connect(this, false);
                FireOnAnyChange();
            }
        }


        /// <inheritdoc />
        public override void Disconnect(BGCalcPortI port, bool disconnectBoth = true)
        {
            if (!(port is BGCalcValueInputI portI)) return;
            connectedPorts.Remove(portI);
            if (disconnectBoth)
            {
                portI.Disconnect(this, false);
                FireOnAnyChange();
            }
        }

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
            if (toConnectPort.PortType != BGCalcPortTypeEnum.ValueIn) return false;
            return ((BGCalcValueInputI)toConnectPort).CanConnectTo(this);
        }

        /// <inheritdoc />
        public override bool IsEqual(BGCalcPortI other)
        {
            if (other == this) return true;
            if (!base.IsEqual(other)) return false;
            if (!(other is BGCalcValueOutput valueOutput)) return false;
            var otherConnectedPorts = valueOutput.connectedPorts;
            if (!ListEqual(connectedPorts, otherConnectedPorts)) return false;
            return true;
        }

        //===========================================================================================================
        //                                        Vars
        //===========================================================================================================

        /*
        private BGCalcVarContainerI varsContainer;

        public BGCalcVarContainerI GetVars(bool createIfMissing = false)
        {
            if (varsContainer == null && createIfMissing) varsContainer = new BGCalcVarContainer();
            return varsContainer;
        }
    */
    }
}