/*
<copyright file="BGCalcDefinitionContext.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// IS IT USED???
    /// </summary>
    public struct BGCalcDefinitionContext
    {
        public readonly BGCalcGraph graph;

        public BGCalcDefinitionContext(BGCalcGraph graph)
        {
            this.graph = graph;
        }
    }
}