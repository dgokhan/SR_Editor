/*
<copyright file="BGDnaFieldRelationSingle.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Single relation field DNA
    /// </summary>
    public partial class BGDnaFieldRelationSingle : BGDnaCreatableField<BGEntity, BGFieldRelationSingle>
    {
        private readonly BGDnaMeta metaDnaTo;

        public BGDnaFieldRelationSingle(BGDnaMeta metaDna, string dnaName, BGDnaMeta metaDnaTo) : base(metaDna, dnaName)
        {
            if (metaDnaTo == null) throw new BGException("Related metaDna can not be null");
            this.metaDnaTo = metaDnaTo;
        }

        protected override BGField New(BGMetaEntity meta, string addon)
        {
            return new BGFieldRelationSingle(meta, DnaName, meta.Repo.GetMeta(metaDnaTo.DnaName));
        }
    }
}