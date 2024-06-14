/*
<copyright file="BGEventArgsAnyChange.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// event is fired on any database change
    /// </summary>
    public partial class BGEventArgsAnyChange : BGEventArgsA
    {
        private static readonly BGObjectPoolNTS<BGEventArgsAnyChange> pool = new BGObjectPoolNTS<BGEventArgsAnyChange>(() => new BGEventArgsAnyChange());
        protected override BGObjectPool Pool => pool;
        
        public BGRepo Repo { get; private set; }

        //to make sure instances are reused
        private BGEventArgsAnyChange()
        {
        }

        public static BGEventArgsAnyChange GetInstance(BGRepo repo)
        {
            var e = pool.Get();
            e.Repo = repo;
            return e;
        }


        /// <inheritdoc/>
        public override void Clear() => Repo = null;

        public override string ToString() => $"BGEventArgsAnyChange";
    }
}