/*
<copyright file="BGDnaFieldLocalizationString.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    public partial class BGDnaFieldLocalizationString : BGDnaFieldLocalizedA<string, BGFieldLocalizedText>
    {
        public BGDnaFieldLocalizationString(BGDnaMeta metaDna, string dnaName, BGDnaMetaLocalization localeDnaMeta) : base(metaDna, dnaName, localeDnaMeta)
        {
        }

        protected override BGField New(BGMetaEntity meta, string addon)
        {
            return new BGFieldLocalizedString(meta, DnaName, meta.Repo.GetMeta<BGMetaLocalization>(LocaleDnaMeta.DnaName));
        }
    }
}