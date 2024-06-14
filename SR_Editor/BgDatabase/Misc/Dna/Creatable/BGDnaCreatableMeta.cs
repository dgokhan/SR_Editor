/*
<copyright file="BGDnaCreatableMeta.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// DNA for creatable meta
    /// </summary>
    public abstract partial class BGDnaCreatableMeta : BGDnaMeta, BGDnaCreatable.CreatableI
    {
#pragma warning disable 0649
        public bool Singleton;
        public bool UniqueName;
        public bool EmptyName;
#pragma warning restore 0649

        public BGDnaCreatableMeta(BGDna dna, string dnaName) : base(dna, dnaName)
        {
        }

        //================================================================================================
        //                                              Create
        //================================================================================================
        /// <inheritdoc />
        public virtual void Create(BGRepo repo, string addon)
        {
            Meta = New(repo, addon);
            Meta.Addon = addon;
            Meta.Singleton = Singleton;
            Meta.UniqueName = UniqueName;
            Meta.EmptyName = EmptyName;
        }

        //create meta from its DNA
        protected abstract BGMetaEntity New(BGRepo repo, string addon);
    }

    /// <summary>
    /// basic generic creatable meta
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract partial class BGDnaMetaCreatable<T> : BGDnaCreatableMeta where T : BGMetaEntity
    {
        protected BGDnaMetaCreatable(BGDna dna, string dnaName) : base(dna, dnaName)
        {
        }

        protected override BGMetaEntity New(BGRepo repo, string addon)
        {
            return BGUtil.Create<BGMetaEntity>(typeof(T), true, repo, DnaName);
        }
    }
}