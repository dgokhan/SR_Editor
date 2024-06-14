/*
<copyright file="BGCalcLoaderString.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// graph JSON loader
    /// </summary>
    public class BGCalcLoaderString
    {
        /// <summary>
        /// reconstruct provided graph using provided JSON string 
        /// </summary>
        public void Load(BGCalcGraph graph, string json)
        {
            var graphModel = JsonUtility.FromJson<BGCalcGraphModel>(json);
            graphModel.ToGraph(graph);
        }
    }
}