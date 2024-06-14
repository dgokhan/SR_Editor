/*
<copyright file="BGCalcControlInputI.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// control flow input port
    /// </summary>
    public interface BGCalcControlInputI : BGCalcPortI
    {
        /// <summary>
        /// execute
        /// </summary>
        Func<BGCalcFlowI, BGCalcControlOutputI> Action { get; }
    }
}