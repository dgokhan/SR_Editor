/*
<copyright file="BGCalcValueInputI.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// interface for value input port
    /// </summary>
    public interface BGCalcValueInputI : BGCalcPortI //, BGCalcVarsLiteOwnerI
    {
        /// <summary>
        /// on state changed event
        /// </summary>
        event Action OnChange;

        /// <summary>
        /// Get connected output port
        /// </summary>
        BGCalcValueOutputI ConnectedPort { get; }
        
        /// <summary>
        /// does this port support default value?
        /// </summary>
        bool SupportDefaultValue { get; }
        
        /// <summary>
        /// default value if suported
        /// </summary>
        object DefaultValue { get; set; }

        /// <summary>
        /// Does this port have default value?
        /// </summary>
        bool HasDefaultValue { get; }
        
        
        // bool CanConnectToOutput(BGCalcValueOutputI toConnectPort);
    }
}