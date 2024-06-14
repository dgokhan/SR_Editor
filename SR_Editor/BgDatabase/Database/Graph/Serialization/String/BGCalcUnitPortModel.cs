/*
<copyright file="BGCalcUnitPortModel.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// serializable port model
    /// </summary>
    [Serializable]
    public class BGCalcUnitPortModel
    {
        // public List<BGCalcVarLiteModel> variables;

        public string Id;

        public byte PortType;

        public string State;
        public byte TypeCode;
        public string Type;

        public byte UnitRef;
        public byte PortRef;
        public bool HasValue;
        public string Value;

        private readonly BGCalcPortI port;

        public BGCalcPortI Port => port;

        public BGCalcUnitPortModel(BGCalcPortI port, BGCalcSaveContext context)
        {
            this.port = port;
            Id = port.Id;
            PortType = (byte)port.PortType;
            switch (port.PortType)
            {
                case BGCalcPortTypeEnum.ControlIn:
                {
                    break;
                }
                case BGCalcPortTypeEnum.ControlOut:
                {
                    //port is 100% connected, cause it can not have variables
                    AddReference(((BGCalcControlOutputI)port).ConnectedPort, context);
                    break;
                }
                case BGCalcPortTypeEnum.ValueIn:
                {
                    AddType(port);
                    var valueInputI = (BGCalcValueInputI)port;
                    AddReference(valueInputI.ConnectedPort, context);
                    if (valueInputI.HasDefaultValue)
                    {
                        HasValue = true;
                        Value = valueInputI.TypeCode.ValueToString(valueInputI.DefaultValue);
                    }

                    break;
                }
                case BGCalcPortTypeEnum.ValueOut:
                {
                    AddType(port);
                    // ProcessVars((BGCalcValueOutputI)port);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(port.PortType));
            }
        }

        /*private void ProcessVars(BGCalcVarsLiteOwnerI port)
        {
            if (port.GetVars() == null || port.GetVars().Count == 0) return;
            variables = new List<BGCalcVarLiteModel>(port.GetVars().Count);
            foreach (var variable in port.GetVars().Variables) variables.Add(new BGCalcVarLiteModel(variable));
        }*/

        private void AddType(BGCalcPortI port)
        {
            if (port.TypeCode != null)
            {
                TypeCode = (byte)port.TypeCode.TypeCode;
                if (port.TypeCode is BGCalcTypeCodeStateful stateful) State = stateful.WriteState();
            }
            else Type = port.Type.AssemblyQualifiedName;
        }

        private void AddReference(BGCalcPortI connectedPort, BGCalcSaveContext context)
        {
            if (connectedPort == null) return;
            var connectedUnit = context.GetUnitWrapper(connectedPort.Unit);
            if (connectedUnit == null) return;
            var portIndex = connectedUnit.Value.GetPortIndex(connectedPort);
            UnitRef = (byte)connectedUnit.Value.index;
            PortRef = (byte)portIndex;
        }

        public void ToPort(BGCalcLoadContext.UnitWrapper unitWrapper)
        {
            var wrapper = new BGCalcLoadContext.PortWrapper();
            unitWrapper.ports.Add(wrapper);

            wrapper.portId = Id;
            wrapper.portType = (BGCalcPortTypeEnum)PortType;
            switch (wrapper.portType)
            {
                case BGCalcPortTypeEnum.ControlIn:
                    wrapper.typeCode = BGCalcTypeCodeControl.Code;
                    wrapper.typeCodeObj = BGCalcTypeCodeRegistry.Control;
                    break;
                case BGCalcPortTypeEnum.ControlOut:
                    wrapper.typeCode = BGCalcTypeCodeControl.Code;
                    wrapper.typeCodeObj = BGCalcTypeCodeRegistry.Control;
                    //if port is present->means it's connected
                    wrapper.UnitRef = UnitRef;
                    wrapper.PortRef = PortRef;
                    break;
                case BGCalcPortTypeEnum.ValueIn:
                    ReadType(wrapper);
                    if (UnitRef > 0)
                    {
                        wrapper.UnitRef = UnitRef;
                        wrapper.PortRef = PortRef;
                    }
                    else if (HasValue)
                    {
                        wrapper.HasValue = true;
                        wrapper.Value = wrapper.typeCodeObj.ValueFromString(Value);
                    }

                    break;
                case BGCalcPortTypeEnum.ValueOut:
                    ReadType(wrapper);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(wrapper.portType));
            }
        }

        private void ReadType(BGCalcLoadContext.PortWrapper wrapper)
        {
            wrapper.typeCode = TypeCode;
            if (wrapper.typeCode != 0)
            {
                wrapper.typeCodeObj = BGCalcTypeCodeRegistry.Get(wrapper.typeCode);
                if (wrapper.typeCodeObj is BGCalcTypeCodeStateful stateful) stateful.ReadState(State);
            }
            else wrapper.Type = Type;
        }

        /*
        private void ReadVars(BGCalcLoadContext.PortWrapper wrapper)
        {
            if (variables?.Count > 0)
            {
                foreach (var variable in variables) variable.ToVar(wrapper);
            }
        }
    */
    }
}