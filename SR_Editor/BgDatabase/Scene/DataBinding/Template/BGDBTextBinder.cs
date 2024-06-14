/*
<copyright file="BGDBTextBinder.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Abstract base binder implementation for template data binders
    /// </summary>
    public abstract class BGDBTextBinder
    {
        /// <summary>
        /// Bind the value using provided context
        /// </summary>
        public abstract void Bind(BGDBTextBinderContext context);
    }
}