/*
<copyright file="BGCalcLoader.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Text;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// graph binary loader
    /// </summary>
    public class BGCalcLoader
    {
        private readonly BGCalcGraph graph;
        private readonly BGBinaryReader reader;
        private readonly ArraySegment<byte> array;

        public BGCalcGraph Graph => graph;

        public BGBinaryReader Reader => reader;

        public BGCalcLoader(BGCalcGraph graph, ArraySegment<byte> array)
        {
            this.graph = graph;
            this.array = array;
            reader = new BGBinaryReader(array);
        }

        /// <summary>
        /// Load the graph frm binary array 
        /// </summary>
        public void Load()
        {
            graph.Clear();
            reader.Reset(array);

            graph.Batch(() =>
            {
                //version
                var version = reader.ReadUShort();
                switch (version)
                {
                    case 1:
                    {
                        //vars
                        BGCalcVarContainer.FromBytes(reader, graph);

                        //units
                        var context = new BGCalcLoadContext();
                        ReadUshortByte(() => UnitFromBytes(context));

                        //map ports
                        context.MapPorts();

                        break;
                    }
                    default:
                        throw new Exception($"Unknown graph serialization format version {version}");
                }
            }, false);
        }


        //reconstruct unit from byte array
        private void UnitFromBytes(BGCalcLoadContext context)
        {
            BGCalcUnitI unit;
            var code = reader.ReadUShort();
            if (code != 0) unit = BGCalcUnitRegistry.Create(code);
            else
            {
                var typeName = reader.ReadString();
                var type = BGUtil.GetType(typeName);
                if (type == null) throw new Exception($"Can not find type {typeName}");
                unit = (BGCalcUnitI)Activator.CreateInstance(type);
            }

            //pos
            var x = reader.ReadFloat();
            var y = reader.ReadFloat();
            unit.Position = new Vector2(x, y);

            var unitWrapper = new BGCalcLoadContext.UnitWrapper(unit);
            context.Add(unitWrapper);

            //vars
            BGCalcVarLiteContainer.FromBytes(reader, unitWrapper.unit);

            //definition
            graph.Init(unit);

            //ports
            ReadUshortByte(() => ReadPort(unitWrapper));

            graph.AddUnitNoInit(unit);
        }


        //reconstruct port from byte array
        private void ReadPort(BGCalcLoadContext.UnitWrapper unit)
        {
            var wrapper = new BGCalcLoadContext.PortWrapper();
            unit.ports.Add(wrapper);

            wrapper.portId = ReadString256();
            wrapper.portType = (BGCalcPortTypeEnum)reader.ReadByte();
            switch (wrapper.portType)
            {
                case BGCalcPortTypeEnum.ControlIn:
                    wrapper.typeCode = BGCalcTypeCodeControl.Code;
                    wrapper.typeCodeObj = BGCalcTypeCodeRegistry.Control;
                    break;
                case BGCalcPortTypeEnum.ControlOut:
                    wrapper.typeCode = BGCalcTypeCodeControl.Code;
                    wrapper.typeCodeObj = BGCalcTypeCodeRegistry.Control;
                    wrapper.UnitRef = reader.ReadByte();
                    wrapper.PortRef = reader.ReadByte();
                    break;
                case BGCalcPortTypeEnum.ValueIn:
                    ReadType(wrapper);
                    // BGCalcVarLiteContainer.FromBytes(reader, wrapper);
                    wrapper.UnitRef = reader.ReadByte();
                    if (wrapper.UnitRef != 0) wrapper.PortRef = reader.ReadByte();
                    else wrapper.Value = wrapper.typeCodeObj.ValueFromBytes(reader);
                    break;
                case BGCalcPortTypeEnum.ValueOut:
                    ReadType(wrapper);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(wrapper.portType));
            }
        }

        private string ReadString256()
        {
            var length = reader.ReadByte();
            var stringArray = reader.ReadByteArrayRaw(length);
            return stringArray.Count == 0 ? null : Encoding.UTF8.GetString(stringArray.Array, stringArray.Offset, stringArray.Count);
        }

        private void ReadType(BGCalcLoadContext.PortWrapper wrapper)
        {
            wrapper.typeCode = reader.ReadByte();
            if (wrapper.typeCode != 0)
            {
                wrapper.typeCodeObj = BGCalcTypeCodeRegistry.Get(wrapper.typeCode);
                if (wrapper.typeCodeObj is BGCalcTypeCodeStateful stateful) stateful.ReadState(reader);
            }
            else wrapper.Type = reader.ReadString();
        }

        private void ReadUshortByte(Action action)
        {
            var count = reader.ReadByte();
            for (var i = 0; i < count; i++) action();
        }
        /*
        private void ReadUshortArray(Action action)
        {
            var count = reader.ReadUShort();
            for (var i = 0; i < count; i++) action();
        }
    */
    }
}