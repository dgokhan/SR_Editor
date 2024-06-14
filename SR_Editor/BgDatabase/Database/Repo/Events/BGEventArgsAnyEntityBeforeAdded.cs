/*
<copyright file="BGEventArgsAnyEntityBeforeAdded.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// event is fired before entity is added
    /// </summary>
    public class BGEventArgsAnyEntityBeforeAdded : BGEventArgsA
    {
        private static readonly BGObjectPoolNTS<BGEventArgsAnyEntityBeforeAdded> pool = new BGObjectPoolNTS<BGEventArgsAnyEntityBeforeAdded>(() => new BGEventArgsAnyEntityBeforeAdded());
        protected override BGObjectPool Pool => pool;
        
        public BGMetaEntity Meta { get; protected set; }

        //to make sure instances are reused
        protected BGEventArgsAnyEntityBeforeAdded()
        {
        }

        public static BGEventArgsAnyEntityBeforeAdded GetInstance(BGMetaEntity meta)
        {
            var e = pool.Get();
            e.Meta = meta;
            return e;
        }

        /// <inheritdoc/>
        public override void Clear() => Meta = null;

        public override string ToString() => $"BGEventArgsAnyEntityBeforeAdded: meta [{Meta}]]";
    }
}