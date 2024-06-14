/*
<copyright file="BGEventArgsEntity.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// entity event args. 
    /// </summary>
    public partial class BGEventArgsEntity : BGEventArgsA
    {
        private static readonly BGObjectPoolNTS<BGEventArgsEntity> pool = new BGObjectPoolNTS<BGEventArgsEntity>(() => new BGEventArgsEntity());
        protected override BGObjectPool Pool => pool;

        public BGEntity Entity { get; protected set; }

        //to make sure instances are reused
        protected BGEventArgsEntity()
        {
        }

        public static BGEventArgsEntity GetInstance(BGEntity entity)
        {
            var e = pool.Get();
            e.Fill(entity);
            return e;
        }

        protected void Fill(BGEntity entity) => Entity = entity;

        /// <inheritdoc/>
        public override void Clear() => Entity = null;

        public override string ToString() => $"BGEventArgsEntity: entity [{Entity}]";
    }
}