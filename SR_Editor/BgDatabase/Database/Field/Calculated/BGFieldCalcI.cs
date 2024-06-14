/*
<copyright file="BGFieldCalcI.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// interface for calculated field 
    /// </summary>
    public interface BGFieldCalcI
    {
        /// <summary>
        /// Default field graph to be used for each row
        /// </summary>
        BGCalcGraph Graph { get; set; }

        /// <summary>
        /// result type of graph calculations
        /// </summary>
        BGCalcTypeCode ResultCode { get; }
    }
}