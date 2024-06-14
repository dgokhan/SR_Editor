/*
<copyright file="BGCalcVarsOwnerI.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// interface for variables owner
    /// </summary>
    public interface BGCalcVarsOwnerI : BGCalcVarsOwnerBaseI
    {
        /// <summary>
        /// Get variables container
        /// </summary>
        BGCalcVarContainer GetVars(bool createIfMissing = false);
    }
}