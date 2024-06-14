/*
<copyright file="BGLocalizationStructure.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    public class BGLocalizationStructure
    {
        private readonly BGRepo repo;
        private BGFieldRelationSingle currentLocaleField;
        private BGMetaEntity localizationSettingsMeta;
        private BGMetaNested localesMeta;
        private BGLocalizationDna dna;

        public BGRepo Repo => repo;

        public BGLocalizationStructure(BGRepo repo)
        {
            this.repo = repo;
        }

        public BGMetaEntity LocalizationSettingsMeta
        {
            get
            {
                if (localizationSettingsMeta == null || localizationSettingsMeta.IsDeleted) localizationSettingsMeta = repo[BGDnaMetaLocalizationSettings.MetaName];
                return localizationSettingsMeta;
            }
        }

        public BGFieldRelationSingle CurrentLocaleField
        {
            get
            {
                if (currentLocaleField == null || currentLocaleField.IsDeleted)
                {
                    var meta = LocalizationSettingsMeta;
                    if (meta == null) return null;
                    currentLocaleField = meta.GetFieldAs<BGFieldRelationSingle>(BGLocalizationDna.FieldCurrent, false);
                }

                return currentLocaleField;
            }
        }

        public BGMetaEntity LocalesMeta
        {
            get
            {
                if (localesMeta == null || localesMeta.IsDeleted)
                {
                    var settingsMeta = LocalizationSettingsMeta;
                    var nested = settingsMeta?.GetFieldAs<BGFieldNested>(BGLocalizationDna.FieldLocales);
                    if (nested == null) return null;
                    localesMeta = nested.NestedMeta;
                }

                return localesMeta;
            }
        }

        public BGLocalizationDna Dna
        {
            get
            {
                if (dna != null && !dna.IsObsolete) return dna;

                dna = new BGLocalizationDna();
                dna.Bind(repo);
                return dna;
            }
        }

        public List<BGEntity> Locales
        {
            get
            {
                var localeList = new List<BGEntity>();
                var localesMeta = LocalesMeta;
                if (localesMeta == null) return localeList;
                localesMeta.EntitiesToList(localeList);
                return localeList;


                //this is slow!!
//                var dna = Dna;
//                var meta = dna.Locale.NestedDnaMeta;
//                meta.Meta.EntitiesToList(localeList);
//                return localeList;
            }
        }

        public int CountLocales
        {
            get
            {
                var localesMeta = LocalesMeta;
                if (localesMeta == null) return 0;
                return localesMeta.CountEntities;
                //this is slow!!
//                var dna = Dna;
//                var meta = dna.Locale.NestedDnaMeta;
//                return meta.Meta.CountEntities;
            }
        }

        public BGEntity Find(string locale)
        {
            if (string.IsNullOrEmpty(locale)) return null;
            var locales = Locales;
            foreach (var l in locales)
                if (string.Equals(l.Name, locale))
                    return l;
            return null;
        }
    }
}