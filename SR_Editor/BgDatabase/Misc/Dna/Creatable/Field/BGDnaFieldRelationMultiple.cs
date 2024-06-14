/*
<copyright file="BGDnaFieldRelationMultiple.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Multiple relation field DNA
    /// </summary>
    public partial class BGDnaFieldRelationMultiple : BGDnaCreatableField<List<BGEntity>, BGFieldRelationMultiple>
    {
        private readonly BGDnaMeta metaDnaTo;

        public BGDnaFieldRelationMultiple(BGDnaMeta metaDna, string dnaName, BGDnaMeta metaDnaTo) : base(metaDna, dnaName)
        {
            this.metaDnaTo = metaDnaTo ?? throw new BGException("Related metaDna can not be null");
        }

        protected override BGField New(BGMetaEntity meta, string addon)
        {
            return new BGFieldRelationMultiple(meta, DnaName, meta.Repo.GetMeta(metaDnaTo.DnaName));
        }
    }
}