/*
<copyright file="BGCalcVarsProvider.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// interface for variables provider
    /// </summary>
    public interface BGCalcVarsProvider
    {
        /// <summary>
        /// Try to get a value for variable with provided ID
        /// </summary>
        bool TryGet(BGId variableId, out object value);
    }
}