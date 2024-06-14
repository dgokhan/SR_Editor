/*
<copyright file="BGEventArgsAnyEntity.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// event is fired on any entity update
    /// </summary>
    public partial class BGEventArgsAnyEntity : BGEventArgsA
    {
        private static readonly BGObjectPoolNTS<BGEventArgsAnyEntity> pool = new BGObjectPoolNTS<BGEventArgsAnyEntity>(() => new BGEventArgsAnyEntity());
        protected override BGObjectPool Pool => pool;
        
        public BGEntity Entity { get; protected set; }

        //to make sure instances are reused
        protected BGEventArgsAnyEntity()
        {
        }

        public static BGEventArgsAnyEntity GetInstance(BGEntity entity)
        {
            var e = pool.Get();
            e.Entity = entity;
            return e;
        }

        /// <inheritdoc/>
        public override void Clear() => Entity = null;

        public override string ToString() => $"BGEventArgsAnyEntity: entity [{Entity}]]";
    }
}