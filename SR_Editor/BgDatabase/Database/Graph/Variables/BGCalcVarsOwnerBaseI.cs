/*
<copyright file="BGCalcVarsOwnerBaseI.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// interface for variables owner
    /// </summary>
    public interface BGCalcVarsOwnerBaseI
    {
        /// <summary>
        /// on variables  change
        /// </summary>
        void OnVarsChange();
    }
}