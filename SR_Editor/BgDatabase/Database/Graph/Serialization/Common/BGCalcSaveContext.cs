/*
<copyright file="BGCalcSaveContext.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// saving context, used while graph saving
    /// </summary>
    public class BGCalcSaveContext
    {
        // private readonly Dictionary<BGCalcUnitI, CalcUnityWrapper> unit2Wrapper = new Dictionary<BGCalcUnitI, CalcUnityWrapper>();
        private readonly List<CalcUnityWrapper> unitWrappers = new List<CalcUnityWrapper>();


        public List<CalcUnityWrapper> UnitWrappers => unitWrappers;

        public BGCalcSaveContext(BGCalcGraph graph) => graph.ForEachUnit(AddUnit);

        /// <summary>
        /// add unit to the context
        /// </summary>
        private void AddUnit(BGCalcUnitI unit)
        {
            var wrapper = new CalcUnityWrapper(unitWrappers.Count, unit);
            unitWrappers.Add(wrapper);
            // unit2Wrapper.Add(unit, wrapper);
        }

        /// <summary>
        /// get node wrapper for provided node
        /// </summary>
        public CalcUnityWrapper? GetUnitWrapper(BGCalcUnitI unit)
        {
            // if (!unit2Wrapper.TryGetValue(unit, out var wrapper)) return null;

            var count = unitWrappers.Count;
            for (var i = 0; i < count; i++)
            {
                var wrapper = unitWrappers[i];
                if (wrapper.unit == unit) return wrapper;
            }

            return null;
        }

        //======================================= Private classes
        /// <summary>
        /// helper wrapper for the unit
        /// </summary>
        public struct CalcUnityWrapper
        {
            public readonly int index;
            public readonly List<BGCalcPortI> ports;
            public readonly BGCalcUnitI unit;

            public CalcUnityWrapper(int index, BGCalcUnitI unit) : this()
            {
                this.index = index;
                this.unit = unit;
                ports = new List<BGCalcPortI>();
                AddAll(unit.InControls);
                AddAll(unit.InValues);
                AddAll(unit.OutControls);
                AddAll(unit.OutValues);
                if (ports.Count > byte.MaxValue) throw new Exception($"Maximum number of ports [{byte.MaxValue}] is exceeded!");
            }

            //add all ports that need to be serialized
            private void AddAll<T>(List<T> portsToAdd) where T : BGCalcPortI
            {
                for (var i = 0; i < portsToAdd.Count; i++)
                {
                    var port = portsToAdd[i];
                    if (!BGCalcPort.ShouldPortBeSerialized(port)) continue;
                    ports.Add(port);
                }
            }

            /// <summary>
            /// get port index for provided port
            /// </summary>
            public int GetPortIndex(BGCalcPortI port)
            {
                for (var i = 0; i < ports.Count; i++)
                {
                    var portI = ports[i];
                    if (port == portI) return i;
                }

                throw new Exception("Can not find connected port! " + port.Unit.Title + '.' + port.Name);
            }
        }

        /*
        public bool Contains(BGCalcUnitI connectedUnit)
        {
            foreach (var unitWrapper in unitWrappers)
            {
                if (unitWrapper.unit == connectedUnit) return true;
            }

            return false;
        }
    */
    }
}