/*
<copyright file="BGConfigurableI.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Mark type as having some configuration which can be represented as string
    /// </summary>
    public partial interface BGConfigurableI
    {
        /// <summary>
        /// serialize config data as string
        /// </summary>
        /// <returns>config as string</returns>
        string ConfigToString();

        /// <summary>
        /// restore config data from string
        /// </summary>
        void ConfigFromString(string config);
    }
}