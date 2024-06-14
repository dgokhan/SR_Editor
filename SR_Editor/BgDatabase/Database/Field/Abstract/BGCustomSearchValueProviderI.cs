/*
<copyright file="BGCustomSearchValueProviderI.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Field, providing custom value for search
    /// USED in Editor assembly only
    /// </summary>
    public interface BGCustomSearchValueProviderI
    {
        /// <summary>
        /// get custom value for search operator
        /// </summary>
        string GetSearchValue(int entityIndex);
    }
}