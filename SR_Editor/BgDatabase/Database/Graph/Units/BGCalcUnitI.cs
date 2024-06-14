/*
<copyright file="BGCalcUnitI.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// interface for graph node
    /// </summary>
    public interface BGCalcUnitI : BGCalcVarsLiteOwnerI
    {
        /// <summary>
        /// owner graph
        /// </summary>
        BGCalcGraph Graph { get; set; }

        /// <summary>
        /// node title
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Type code is used for fast instantiation of builtin units. DO NOT OVERRIDE IT.
        /// </summary>
        ushort TypeCode { get; }

        /// <summary>
        /// incoming control ports 
        /// </summary>
        List<BGCalcControlInputI> InControls { get; }

        /// <summary>
        /// outgoing control ports 
        /// </summary>
        List<BGCalcControlOutputI> OutControls { get; }

        /// <summary>
        /// incoming values ports 
        /// </summary>
        List<BGCalcValueInputI> InValues { get; }

        /// <summary>
        /// outgoing values ports 
        /// </summary>
        List<BGCalcValueOutputI> OutValues { get; }

        /// <summary>
        /// all ports 
        /// </summary>
        List<BGCalcPortI> Ports { get; }
        
        /// <summary>
        /// all ports count
        /// </summary>
        int PortsCount { get; }

        /// <summary>
        /// Return variable label if the var is supposed to be a public var  
        /// </summary>
        string GetPublicVarLabel(byte varId);

        /// <summary>
        /// Position on the graph canvas
        /// </summary>
        Vector2 Position { get; set; }

        /// <summary>
        /// Called to initialize unit
        /// </summary>
        void Definition();

        /// <summary>
        /// Find all ports, complying to provided filter 
        /// </summary>
        List<BGCalcPortI> FindPorts(Predicate<BGCalcPortI> filter);

        /// <summary>
        /// Find the first port, complying to provided filter 
        /// </summary>
        BGCalcPortI FindPort(Predicate<BGCalcPortI> filter);
        
        /// <summary>
        /// Find the port by its ID 
        /// </summary>
        BGCalcPortI FindPort(string id);
        
        /// <summary>
        /// Remove the port  
        /// </summary>
        void RemovePort(BGCalcPortI port);

        bool IsEqual(BGCalcUnitI other);
    }
}