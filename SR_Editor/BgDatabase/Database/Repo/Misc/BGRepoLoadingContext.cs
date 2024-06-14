/*
<copyright file="BGRepoLoadingContext.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// context for repo loading
    /// </summary>
    public class BGRepoLoadingContext
    {
        /// <summary>
        /// Called before onload event
        /// </summary>
        public Action OnBeforeFiringOnLoad;
    }
}