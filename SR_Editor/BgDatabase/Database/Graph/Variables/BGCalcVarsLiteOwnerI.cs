/*
<copyright file="BGCalcVarsLiteOwnerI.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// container with lite variables
    /// </summary>
    public interface BGCalcVarsLiteOwnerI : BGCalcVarsOwnerBaseI
    {
        /// <summary>
        /// Get variables container
        /// </summary>
        BGCalcVarLiteContainer GetVars(bool createIfMissing = false);
    }
}