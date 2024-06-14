/*
<copyright file="BGCalcValueOutputI.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// interface for value output port
    /// </summary>
    public interface BGCalcValueOutputI : BGCalcPortI //, BGCalcVarOwnerI
    {
        /// <summary>
        /// Get port value
        /// </summary>
        Func<BGCalcFlowI, object> GetValue { get; }
    }
}