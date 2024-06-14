/*
<copyright file="BGCalcPortI.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// interface for node port
    /// </summary>
    public interface BGCalcPortI
    {
        /// <summary>
        /// port ID
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Port name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// port type
        /// </summary>
        BGCalcPortTypeEnum PortType { get; }

        /// <summary>
        /// can have multiple connections
        /// </summary>
        bool IsSingle { get; }

        /// <summary>
        /// port value type
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// is port input
        /// </summary>
        bool IsInput { get; }

        /// <summary>
        /// port value type code
        /// </summary>
        BGCalcTypeCode TypeCode { get; }

        /// <summary>
        /// owner node
        /// </summary>
        BGCalcUnitI Unit { get; }

        /// <summary>
        /// connect to provided port
        /// </summary>
        void Connect(BGCalcPortI port, bool connectBoth = true);

        /// <summary>
        /// disconnect from provided port
        /// </summary>
        void Disconnect(BGCalcPortI port, bool disconnectBoth = true);

        /// <summary>
        /// is port connected
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// all connected ports
        /// </summary>
        List<BGCalcPortI> ConnectedPorts { get; }

        /// <summary>
        /// disconnect all ports
        /// </summary>
        void DisconnectAll();

        /// <summary>
        /// can this port be connected to provided port 
        /// </summary>
        bool CanConnectTo(BGCalcPortI toConnectPort);

        bool IsEqual(BGCalcPortI other);
    }
}