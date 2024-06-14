/*
<copyright file="BGEventArgsA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// abstract event args
    /// </summary>
    public abstract partial class BGEventArgsA : EventArgs, IDisposable
    {
        /// <summary>
        /// Clear internal state
        /// </summary>
        public abstract void Clear();
        
        protected abstract BGObjectPool Pool { get; }

        public void Dispose()
        {
            Clear();
            Pool.Return(this);
        }
    }
}