/*
<copyright file="BGDnaMetaNested.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Creatable nested meta DNA
    /// </summary>
    public partial class BGDnaMetaNested : BGDnaMetaCreatable<BGMetaNested>
    {
        private readonly BGDnaMeta owner;

        public BGDnaMetaNested(string dnaName, BGDnaMeta owner) : base(null, dnaName)
        {
            this.owner = owner;
        }

        protected override BGMetaEntity New(BGRepo repo, string addon)
        {
            var meta = new BGMetaNested(repo, DnaName, repo.GetMeta(owner.DnaName)) { Addon = addon };
            meta.OwnerRelation.Addon = addon;
            return meta;
        }
    }
}