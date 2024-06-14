/*
<copyright file="BGLocalizationDna.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    public partial class BGLocalizationDna : BGDnaCreatable
    {
        public const string FieldCurrent = "current";
        public const string FieldLocales = "Locale";

        public static BGLocalizationDna Instance
        {
            get
            {
                var addon = BGRepo.I.Addons.Get<BGAddonLocalization>();
                if (addon == null) throw new BGException("Localization addon is not activated!");
                return addon.Dna;
            }
        }

        public readonly BGDnaMetaLocalizationSettings Settings;
        public readonly BGDnaFieldRelationSingle CurrentLocale;
        public readonly BGDnaFieldNested Locale;

        public BGLocalizationDna()
        {
            Settings = new BGDnaMetaLocalizationSettings(this);
            Locale = new BGDnaFieldNested(Settings, FieldLocales);

            CurrentLocale = new BGDnaFieldRelationSingle(Settings, FieldCurrent, Locale.NestedDnaMeta);
        }
    }
}