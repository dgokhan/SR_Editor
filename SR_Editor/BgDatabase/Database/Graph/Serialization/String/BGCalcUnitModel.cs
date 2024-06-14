/*
<copyright file="BGCalcUnitModel.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// serializable graph node model
    /// </summary>
    [Serializable]
    public class BGCalcUnitModel
    {
        public ushort TypeCode;
        public string TypeName;
        public float PosX;
        public float PosY;

        public List<BGCalcVarLiteModel> variables;
        public List<BGCalcUnitPortModel> ports;

        private readonly BGCalcUnitI unit;
        private readonly BGCalcSaveContext context;

        public BGCalcUnitI Unit => unit;
        public bool IsStartUnit => TypeCode == BGCalcUnitGraphStart.Code;

        public BGCalcUnitModel(BGCalcUnitI unit, BGCalcSaveContext context, bool processPorts = true)
        {
            this.unit = unit;
            this.context = context;
            TypeCode = unit.TypeCode;
            if (TypeCode == 0) TypeName = unit.GetType().AssemblyQualifiedName;
            PosX = unit.Position.x;
            PosY = unit.Position.y;
            if (unit.GetVars()?.Variables.Count > 0)
            {
                variables = new List<BGCalcVarLiteModel>();
                foreach (var variable in unit.GetVars().Variables) variables.Add(new BGCalcVarLiteModel(variable));
            }

            if (processPorts) ProcessPorts();
        }

        public void ProcessPorts(Func<BGCalcPortI, bool> filter = null)
        {
            var unitPorts = unit.FindPorts(null);
            if (unitPorts?.Count > 0)
            {
                if (unitPorts.Count > byte.MaxValue) throw new Exception($"Can not serialize graph, cause the number of ports={unitPorts.Count} in {unit.Title} exceeds maximum {byte.MaxValue}");
                ports = new List<BGCalcUnitPortModel>();
                foreach (var port in unitPorts)
                {
                    if (!BGCalcPort.ShouldPortBeSerialized(port) || filter != null && !filter(port)) continue;
                    ports.Add(new BGCalcUnitPortModel(port, context));
                }
            }
        }

        /// <summary>
        /// reconstruct graph node using data from this model
        /// </summary>
        public BGCalcUnitI ToUnit(BGCalcGraph graph, BGCalcLoadContext context)
        {
            /*BGCalcUnitI unit;
            var code = reader.ReadInt();
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
            ReadVars(unitWrapper.unit);

            //definition
            graph.Init(unit);

            //ports
            reader.ReadArray(() => ReadPort(unitWrapper));

            graph.AddUnit(unit);*/
            //create a unit
            BGCalcUnitI unit;
            if (TypeCode != 0) unit = BGCalcUnitRegistry.Create(TypeCode);
            else
            {
                var type = BGUtil.GetType(TypeName);
                if (type == null) throw new Exception($"Can not find type {TypeName}");
                unit = (BGCalcUnitI)Activator.CreateInstance(type);
            }

            unit.Position = new Vector2(PosX, PosY);

            //wrapper
            var unitWrapper = new BGCalcLoadContext.UnitWrapper(unit);
            context.Add(unitWrapper);
            if (unit.TypeCode == BGCalcUnitVoid.Code) return unit;

            //vars
            if (variables?.Count > 0)
                foreach (var variable in variables)
                    variable.ToVar(unit);

            //definition
            graph.Init(unit);

            //ports
            if (ports?.Count > 0)
                foreach (var port in ports)
                    port.ToPort(unitWrapper);


            graph.AddUnitNoInit(unit);

            return unit;
        }
    }
}