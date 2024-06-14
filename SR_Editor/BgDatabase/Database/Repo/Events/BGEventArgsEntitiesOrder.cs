/*
<copyright file="BGEventArgsEntitiesOrder.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// event for rows order changed
    /// </summary>
    public class BGEventArgsEntitiesOrder : BGEventArgsA
    {
        private static readonly BGObjectPoolNTS<BGEventArgsEntitiesOrder> pool = new BGObjectPoolNTS<BGEventArgsEntitiesOrder>(() => new BGEventArgsEntitiesOrder());
        protected override BGObjectPool Pool => pool;

        public BGMetaEntity Meta { get; private set; }

        //to make sure instances are reused
        private BGEventArgsEntitiesOrder()
        {
        }

        public static BGEventArgsEntitiesOrder GetInstance(BGMetaEntity meta)
        {
            var e = pool.Get();
            e.Meta = meta;
            return e;
        }

        /// <inheritdoc/>
        public override void Clear() => Meta = null;

        public override string ToString() => $"BGEventArgsEntitiesOrder: meta [{Meta}]";
    }
}