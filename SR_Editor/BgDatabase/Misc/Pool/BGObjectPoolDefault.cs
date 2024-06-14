/*
<copyright file="BGObjectPoolDefault.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Generic default object pool with static instance  
    /// </summary>
    public class BGObjectPoolDefault<T> : BGObjectPool<T> where T : new()
    {
        private static readonly BGObjectPoolDefault<T> I = new BGObjectPoolDefault<T>(); 
            
        private BGObjectPoolDefault() : base(() => new T())
        {
        }
    }
}