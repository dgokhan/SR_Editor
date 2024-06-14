/*
<copyright file="BGDnaMetaLocalizationSettings.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    public partial class BGDnaMetaLocalizationSettings : BGDnaMetaCreatable<BGMetaLocalizationSettings>
    {
        public const string MetaName = "Localization";

        public BGDnaMetaLocalizationSettings(BGDna dna) : base(dna, "Localization")
        {
        }

        protected override BGMetaEntity New(BGRepo repo, string addon)
        {
            return new BGMetaLocalizationSettings(repo, addon);
        }
    }
}