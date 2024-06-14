/*
<copyright file="BGCalcSaverString.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// graph JSON writer
    /// </summary>
    public class BGCalcSaverString
    {
        private readonly BGCalcGraph graph;

        public BGCalcSaverString(BGCalcGraph graph) => this.graph = graph;

        /// <summary>
        /// convert graph to JSON string
        /// </summary>
        public string Save() => JsonUtility.ToJson(new BGCalcGraphModel(graph));
    }
}