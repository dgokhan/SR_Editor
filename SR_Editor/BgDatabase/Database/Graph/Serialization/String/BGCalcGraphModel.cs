/*
<copyright file="BGCalcGraphModel.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// graph model to use with JSON serializer
    /// </summary>
    [Serializable]
    public class BGCalcGraphModel
    {
        public int Version;
        public List<BGCalcVarModel> variables;
        public List<BGCalcUnitModel> units;

        public BGCalcGraphModel()
        {
        }

        public BGCalcGraphModel(BGCalcGraph graph)
        {
            Version = 1;
            if (graph.GetVars()?.Variables.Count > 0)
            {
                variables = new List<BGCalcVarModel>();
                foreach (var variable in graph.GetVars().Variables) variables.Add(new BGCalcVarModel(variable));
            }

            if (graph.UnitsCount > 0)
            {
                if (graph.UnitsCount > byte.MaxValue) throw new Exception($"Can not serialize graph, cause the number of units={graph.UnitsCount} exceeds maximum {byte.MaxValue}");
                var context = new BGCalcSaveContext(graph);
                units = new List<BGCalcUnitModel>();
                graph.ForEachUnit(unit => units.Add(new BGCalcUnitModel(unit, context)));
            }
        }

        /// <summary>
        /// reconstruct provided graph from graph model 
        /// </summary>
        public void ToGraph(BGCalcGraph graph)
        {
            //vars
            if (variables?.Count > 0)
                foreach (var variable in variables)
                    variable.ToVar(graph);

            //units
            var context = new BGCalcLoadContext();
            if (units?.Count > 0)
                foreach (var unit in units)
                    unit.ToUnit(graph, context);

            //map ports
            context.MapPorts();
        }
    }
}