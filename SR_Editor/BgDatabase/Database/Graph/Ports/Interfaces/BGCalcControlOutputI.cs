/*
<copyright file="BGCalcControlOutputI.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// control flow output port
    /// </summary>
    public interface BGCalcControlOutputI : BGCalcPortI
    {
        /// <summary>
        /// get connected input control port
        /// </summary>
        BGCalcControlInputI ConnectedPort { get; }
    }
}