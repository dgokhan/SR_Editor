/*
<copyright file="BGCalcLoadContext.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// loading context, used while graph loading
    /// </summary>
    public class BGCalcLoadContext
    {
        private readonly List<UnitWrapper> units = new List<UnitWrapper>();

        /// <summary>
        /// add unit to the context
        /// </summary>
        public void Add(UnitWrapper unitWrapper) => units.Add(unitWrapper);

        /// <summary>
        /// map all graph ports
        /// </summary>
        public void MapPorts()
        {
            //find all ports
            for (var i = 0; i < units.Count; i++)
            {
                var unit = units[i];
                var ports = unit.ports;
                for (var j = 0; j < ports.Count; j++)
                {
                    var portWrapper = ports[j];

                    var error = portWrapper.MapPort(unit.unit);
                    if (error != null) Debug.Log("BGDatabase graph deserialization warning: " + error);
                }
            }

            //connect ports and restore vars
            for (var i = 0; i < units.Count; i++)
            {
                var unit = units[i];
                var ports = unit.ports;
                for (var j = 0; j < ports.Count; j++)
                {
                    var portWrapper = ports[j];
                    if (portWrapper.port == null) continue;
                    switch (portWrapper.port.PortType)
                    {
                        case BGCalcPortTypeEnum.ControlIn:
                            break;
                        case BGCalcPortTypeEnum.ControlOut:
                        {
                            if (portWrapper.UnitRef > 0) portWrapper.Connect(units);
                            break;
                        }
                        case BGCalcPortTypeEnum.ValueIn:
                        {
                            // var inputPort = ((BGCalcValueInputI)portWrapper.port);
                            if (portWrapper.UnitRef > 0) portWrapper.Connect(units);
                            else if (portWrapper.HasValue) ((BGCalcValueInputI)portWrapper.port).DefaultValue = portWrapper.Value;

                            break;
                        }
                        case BGCalcPortTypeEnum.ValueOut:
                            // portWrapper.MapVars();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }

        //======================================= Internal classes
        /// <summary>
        /// helper class for mapping ports
        /// </summary>
        public class UnitWrapper
        {
            public readonly BGCalcUnitI unit;
            public readonly List<PortWrapper> ports = new List<PortWrapper>();

            public UnitWrapper(BGCalcUnitI unit) => this.unit = unit;
        }

        /// <summary>
        /// helper class for single port
        /// </summary>
        public class PortWrapper //: BGCalcVarsLiteOwnerI
        {
            public string portId;
            public BGCalcPortTypeEnum portType;
            public int UnitRef;
            public int PortRef;
            public byte typeCode;
            public string Type;
            public BGCalcTypeCode typeCodeObj;

            public BGCalcPortI port;

            private object value;
            public bool HasValue;


            public object Value
            {
                get => value;
                set
                {
                    this.value = value;
                    HasValue = true;
                }
            }

            /// <summary>
            /// resolve the port using provided node
            /// </summary>
            public string MapPort(BGCalcUnitI unit)
            {
                var p = unit.FindPort(portId);
                if (p == null) return $"Can not find a port with id={portId} at {unit.Title} unit- all connections to this port can not be resolved";
                if (p.PortType != portType) return $"Port with id={portId} at {unit.Title} unit changed port type- all connections to this port can not be resolved";
                if (!Equals(p.TypeCode, typeCodeObj)) return $"Port with id={portId} at {unit.Title} unit changed type code- all connections to this port can not be resolved";
                if (p.TypeCode == null)
                {
                    var pType = BGUtil.GetType(Type);
                    if (p.Type != pType) return $"Port with id={portId} at {unit.Title} unit changed type- all connections to this port can not be resolved";
                }

                port = p;
                return null;
            }

            /// <summary>
            /// connect the port 
            /// </summary>
            public void Connect(List<UnitWrapper> units)
            {
                var connectedPort = units[UnitRef].ports[PortRef];
                if (connectedPort.port != null) port.Connect(connectedPort.port);
            }
        }
    }
}