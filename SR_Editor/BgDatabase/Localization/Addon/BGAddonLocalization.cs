/*
<copyright file="BGAddonLocalization.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    [AddonDescriptor(Name = "Localization", ManagerType = "BansheeGz.BGDatabase.Editor.BGAddonManagerLocalization")]
    public partial class BGAddonLocalization : BGAddon
    {
        public const string LocaleFileKey = "locale";

        //================================================================================================
        //                                              Properties
        //================================================================================================

        public override BGRepo Repo
        {
            protected set
            {
                if (base.Repo == value) return;
                base.Repo = value;
                structure = null;
                currentLocaleManager = null;
            }
        }

        private BGLocalizationStructure structure;

        public BGLocalizationStructure Structure
        {
            get
            {
                if (structure != null) return structure;
                structure = new BGLocalizationStructure(Repo);
                return structure;
            }
        }

        private BGLocalizationCurrentLocale currentLocaleManager;

        private BGLocalizationCurrentLocale CurrentLocaleManager
        {
            get
            {
                if (currentLocaleManager != null) return currentLocaleManager;
                currentLocaleManager = new BGLocalizationCurrentLocale(Structure);
                return currentLocaleManager;
            }
        }

        public BGMetaEntity LocalesMeta => Structure.LocalesMeta;

        /// <summary>
        /// Get localization addon DNA. this method is relatively costly
        /// </summary>
        public BGLocalizationDna Dna => Structure.Dna;


        /*
        public string GetLocalizationFilePath(string locale)
        {
            return LoadedRepos.GetLocalizationFilePath(locale);
        }
        */

        public string CurrentLocale
        {
            get => CurrentLocaleManager.CurrentLocale;
            set => CurrentLocaleManager.CurrentLocale = value;
        }

        public List<BGEntity> Locales => Structure.Locales;

        public int CountLocales => Structure.CountLocales;

        public override int OnMainDatabaseLoadOrder => 64;

        //================================================================================================
        //                                              Load
        //================================================================================================

        [Obsolete("use Load(BGLocalizationLoadingContext context) instead")]
        private BGRepo Load(string locale, byte[] repoData = null, bool errorIfNotFound = false)
        {
            return Load(new BGLocalizationLoadingContext(locale)
            {
                RepoData = repoData,
                ErrorIfNotFound = errorIfNotFound
            });
        }

        private BGRepo Load(BGLocalizationLoadingContext context)
        {
            if (!BGRepo.DefaultRepo(Repo)) return null;
            return BGLocalizationReposCache.Load(Structure, context);
        }

        public void UnloadLocalesExceptCurrent()
        {
            var currentLocale = CurrentLocale;
            var locales = Locales;
            foreach (var locale in locales)
            {
                if (string.Equals(currentLocale, locale.Name)) continue;
                BGLocalizationReposCache.Unload(locale.Name);
            }
        }


        //================================================================================================
        //                                              Value
        //================================================================================================


        //================================================================================================
        //                                              Config
        //================================================================================================

        //================================================================================================
        //                                              Syncing
        //================================================================================================

        //================================================================================================
        //                                              Methods
        //================================================================================================

        public BGEntity Find(string locale)
        {
            return Structure.Find(locale);
        }

        public void Add(string locale)
        {
            //check name
            if (string.IsNullOrEmpty(locale)) throw new BGException("Locale name is null");
            if (locale.Length > 8) throw new BGException("Locale name is not valid (8 characters max, no more)");
            if (Find(locale) != null) throw new BGException("Locale with name $ already exists", locale);
            var error = BGMetaObject.CheckName(locale);
            if (error != null) throw new BGException(error);

            foreach (var @char in locale)
                if (!char.IsLetter(@char))
                    throw new BGException("Locale name should have letters only");

            BGMetaLocalizationA.ForEachMeta(Repo, meta =>
            {
                if (meta.HasField(locale)) throw new BGException($"Meta [{meta.Name}] already has a field with name [{locale}]! Change field name or locale name to avoid naming collision!");
            });

            var loader = Repo.RepoLoader ?? BGRepo.DefaultRepoLoader;
            if (loader is BGLoaderForRepoResources || loader is BGLoaderForRepoStreamingAssets)
            {
                var data = loader.Load(new BGLoaderForRepo.LoadRequest(Repo.RepoAssetPath ?? BGRepo.DefaultRepoAssetPath, GetLocalePaths(locale)));
                if (data != null)
                    throw new BGException("Can not create a data file for $ locale: you already have a file for this locale under "
                                          + (loader is BGLoaderForRepoResources ? "Resources" : "StreamingAssets") + " folder!" +
                                          " Please, backup and remove existing file first", locale);
            }


            //add
            var dna = Dna;
            var localeEntity = dna.Locale.NestedDnaMeta.MetaAs<BGMetaNested>().NewEntity(dna.Settings.Meta[0]);
            localeEntity.Name = locale;

            BGMetaLocalizationA.ForEachMeta(Repo, meta =>
            {
                BGUtil.Create<BGField>(meta.FieldType, false, meta, locale).System = true;
            });
            var delegateRepo = Load(new BGLocalizationLoadingContext(locale));
            //sync
            BGMetaLocalizationA.ForEachMeta(structure.Repo, meta =>
            {
                var field = meta.GetField(locale);
                BGLocalizationSaveManager.Sync(delegateRepo, field);
            });
        }

        public static string[] GetLocalePaths(string locale)
        {
            return new[] { LocaleFileKey, locale };
        }

        public void Delete(string locale)
        {
            BGLocalizationReposCache.Unload(locale);
            var model = Find(locale);
            if (model == null) return;

            model.Delete();
            BGMetaLocalizationA.ForEachMeta(Repo, meta =>
            {
                var field = meta.GetField(locale, false);
                if (field == null) return;

                field.Delete();
            });
        }

        public override BGAddon CloneTo(BGRepo repo)
        {
            var clone = new BGAddonLocalization { Repo = repo };
            return clone;
        }

        public override void OnDelete(BGRepo repo)
        {
            var toRemove = new List<BGMetaEntity>();
            BGMetaLocalizationA.ForEachMeta(repo, meta => toRemove.Add(meta));
            foreach (var meta in toRemove) meta.Delete();
            new BGLocalizationDna().Delete(repo);
            BGLocalizationReposCache.Unload();
        }

        public override void OnMainDatabaseLoad()
        {
            //unload all
            BGLocalizationReposCache.Unload();

            if (Application.isPlaying) return;

            if (Application.isEditor)
            {
                //load all
                var locales = Locales;
                for (var i = 0; i < locales.Count; i++)
                {
                    var locale = locales[i];
                    Load(new BGLocalizationLoadingContext(locale.Name)
                    {
                        ErrorIfNotFound = true,
                        PushFieldsConfig = new BGLocalizationLoadingContext.PushFieldValuesConfig()
                        {
                            LoadPartitionsMode = BGLocalizationLoadingContext.LoadPartitionsModeEnum.LoadAll
                        }
                    });
                }
            }
        }

        public interface LocaleChangeReceiver
        {
            void LocaleChanged();
        }

        public override void OnBeforeAdd(BGRepo repo)
        {
            base.OnBeforeAdd(repo);

            if (repo.HasMeta(BGDnaMetaLocalizationSettings.MetaName))
                throw new BGException("Can not activate addon: meta with name $ already exists! Please, rename this meta before enabling localization addon", BGDnaMetaLocalizationSettings.MetaName);
            if (repo.HasMeta(BGLocalizationDna.FieldLocales))
                throw new BGException("Can not activate addon: meta with name $ already exists! Please, rename this meta before enabling localization addon", BGLocalizationDna.FieldLocales);

            var dna = new BGLocalizationDna();
            dna.Create(repo, "Localization");
            dna.Settings.Meta.NewEntity().Name = "Localization Settings";
        }

        //New fast access to current locale for default repo
        private static BGLocalizationStructure structureStatic;
        private static BGLocalizationCurrentLocale currentLocaleManagerStatic;

        private static BGLocalizationCurrentLocale CurrentLocaleManagerStatic
        {
            get
            {
                if (currentLocaleManagerStatic != null) return currentLocaleManagerStatic;
                if (structureStatic == null) structureStatic = new BGLocalizationStructure(BGRepo.I);
                currentLocaleManagerStatic = new BGLocalizationCurrentLocale(structureStatic);
                return currentLocaleManagerStatic;
            }
        }

        public static string DefaultRepoCurrentLocale
        {
            get => CurrentLocaleManagerStatic.CurrentLocale;
            set => CurrentLocaleManagerStatic.CurrentLocale = value;
        }

        public static event Action LocaleForDefaultRepoChanged; 
        public static void FireLocaleChangedEvent() => LocaleForDefaultRepoChanged?.Invoke();
    }
}