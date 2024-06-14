/*
<copyright file="BGCalcSaver.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Text;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// graph binary writer
    /// </summary>
    public class BGCalcSaver
    {
        public const ushort LastVersion = 1;

        private readonly BGCalcGraph graph;
        private readonly BGBinaryWriter writer;

        public BGBinaryWriter Writer => writer;

        public BGCalcSaver(BGCalcGraph graph)
        {
            this.graph = graph;
            writer = new BGBinaryWriter();
        }

        /// <summary>
        /// convert graph to byte array
        /// </summary>
        public byte[] Save()
        {
            if (graph.UnitsCount > byte.MaxValue) throw new Exception($"Can not serialize graph, cause the number of units={graph.UnitsCount} exceeds maximum {byte.MaxValue}");

            writer.Clear();

            //version
            writer.AddUShort(LastVersion);

            //vars
            BGCalcVarContainer.ToBytes(writer, (BGCalcVarContainer)graph.GetVars());

            //units
            var context = new BGCalcSaveContext(graph);

            AddArrayByteLength(() =>
            {
                for (var i = 0; i < context.UnitWrappers.Count; i++) UnitToBytes(context, context.UnitWrappers[i]);
            }, (byte)context.UnitWrappers.Count);

            return writer.ToArray();
        }

        //unit to binary array
        private void UnitToBytes(BGCalcSaveContext context, BGCalcSaveContext.CalcUnityWrapper unitWrapper)
        {
            if (unitWrapper.unit.PortsCount > byte.MaxValue)
                throw new Exception($"Can not serialize graph, cause the number of ports={unitWrapper.unit.PortsCount} " +
                                    $"for unit {unitWrapper.unit.Title} exceeds maximum {byte.MaxValue}");

            var unit = unitWrapper.unit;
            var typeCode = unit.TypeCode;
            writer.AddUShort(typeCode);
            if (typeCode == 0) writer.AddString(unit.GetType().AssemblyQualifiedName);

            //pos
            var position = unit.Position;
            writer.AddFloat(position.x);
            writer.AddFloat(position.y);

            //vars
            BGCalcVarLiteContainer.ToBytes(writer, unit.GetVars());

            //ports
            ProcessPorts(unitWrapper, context);
        }

        //ports to binary array
        private void ProcessPorts(BGCalcSaveContext.CalcUnityWrapper unitWrapper, BGCalcSaveContext context)
        {
            AddArrayByteLength(() =>
            {
                //do not convert to foreach
                for (var i = 0; i < unitWrapper.ports.Count; i++) ToBytes(unitWrapper.ports[i], context);
            }, (byte)unitWrapper.ports.Count);
        }


        //port to binary array
        private void ToBytes(BGCalcPortI port, BGCalcSaveContext context)
        {
            AddStringByteLength(port.Id);
            //we ABSOLUTELY need port type- cause port type can be changed in the code
            writer.AddByte((byte)port.PortType);

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
                    //if port not connected-> it means it has default value 100%, no other options 
                    if (!port.IsConnected) valueInputI.TypeCode.ValueToBytes(writer, valueInputI.DefaultValue);
                    break;
                }
                case BGCalcPortTypeEnum.ValueOut:
                {
                    AddType(port);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(port.PortType));
            }
        }

        private void AddReference(BGCalcPortI connectedPort, BGCalcSaveContext context)
        {
            if (connectedPort != null)
            {
                var connectedUnit = context.GetUnitWrapper(connectedPort.Unit);
                writer.AddByte((byte)connectedUnit.Value.index);
                writer.AddByte((byte)connectedUnit.Value.GetPortIndex(connectedPort));
            }
            else writer.AddByte(0);
        }

        private void AddType(BGCalcPortI port)
        {
            if (port.TypeCode != null)
            {
                writer.AddByte((byte)port.TypeCode.TypeCode);
                if (port.TypeCode is BGCalcTypeCodeStateful stateful) stateful.WriteState(writer);
            }
            else
            {
                writer.AddByte(0);
                writer.AddString(port.Type.AssemblyQualifiedName);
            }
        }

        public void AddArrayByteLength(Action action, byte count = 0)
        {
            writer.AddByte(count);
            //probably it makes sense to NOT invoke action if count==0????
            if (count <= 0) return;
            action();
        }

        /*
        public void AddArrayUShort(Action action, ushort count = 0)
        {
            writer.AddUShort(count);
            //probably it makes sense to NOT invoke action if count==0????
            if (count <= 0) return;
            action();
        }
        */

        public void AddStringByteLength(string value)
        {
            writer.AddByte((byte)value.Length);
            writer.AddBytesRaw(Encoding.UTF8.GetBytes(value));
        }
    }
}