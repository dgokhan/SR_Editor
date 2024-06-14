/*
<copyright file="BGLocalizationPartitionLoadHandler.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// This is called after loadinging/unloading partition at runtime
    /// </summary>
    public class BGLocalizationPartitionLoadHandler : BGAddonPartition.OnLoadHandler
    {
        public void OnLoad(BGEntity partitionEntity)
        {
            if (!Application.isPlaying && Application.isEditor) return;
            var repo = partitionEntity.Repo;
            var localizationAddon = repo.Addons.Get<BGAddonLocalization>();
            if (localizationAddon == null) return;
            var loader = repo.RepoLoader ?? BGRepo.DefaultRepoLoader;
            // var repoAssetPath = repo.RepoAssetPath ?? BGRepo.DefaultRepoAssetPath;

            //settings
            var settings = new BGMergeSettingsEntity()
            {
                AddMissing = true,
                UpdateMatching = true
            };

            BGLocalizationReposCache.ForEachLoadedLocale((locale, localeRepo) =>
            {
                var partitionRepo = BGLocalizationReposCache.LoadPartitionRepo(new BGLocalizationReposCache.PartitionLocaleKey(locale, partitionEntity.Id), repo, loader);

                //transfer all data to main database
                BGAddonPartition.Merge(partitionRepo, BGRepo.I, settings);
                // BGLocalizationReposCache.Push(locale, BGRepo.I, partitionRepo, true);
            });
        }

        public void OnUnload(BGEntity partitionEntity)
        {
            if (!Application.isPlaying && Application.isEditor) return;
            var repo = partitionEntity.Repo;
            var localizationAddon = repo.Addons.Get<BGAddonLocalization>();
            if (localizationAddon == null) return;
            // var partitions = new BGPartitionsModel(repo);

            BGLocalizationReposCache.ForEachLoadedLocale((locale, localeRepo) =>
            {
                BGLocalizationReposCache.UnloadPartitionRepo(new BGLocalizationReposCache.PartitionLocaleKey(locale, partitionEntity.Id));
                var provider = new BGMetaPartitionModelLocalizationProvider(repo);
                localeRepo.ForEachMeta(localeMeta =>
                {
                    var repoMeta = repo[localeMeta.Id];
                    if (repoMeta == null) return;
                    DeleteEntities(partitionEntity.Index, provider, repoMeta);
                });
            });
        }

        public void UpdateMergeSettings(BGMergeSettingsEntity settings)
        {
            var locale = BGAddonLocalization.DefaultRepoCurrentLocale;
            //merge only current locale 
            settings.AddMissingFieldFilter = field => !(field is BGFieldLocaleI) || string.Equals(locale, field.Name);
        }

        private static void DeleteEntities(int partition, BGMetaPartitionModelLocalizationProvider provider, BGMetaEntity localeMeta)
        {
            var model = provider.Get(localeMeta);
            if (model == null) return;
            var toRemove = new List<BGEntity>();
            for (var i = 0; i < localeMeta.CountEntities; i++)
            {
                var entity = localeMeta.GetEntity(i);
                var index = model.GetPartitionIndex(entity);
                if (index == null) continue;
                if (index.Value != partition) continue;
                toRemove.Add(entity);
            }

            localeMeta.DeleteEntities(toRemove);
        }

        /*private class Controller : BGMergeSettingsEntity.IAddMissingReceiver
        {
            private readonly string locale;

            public Controller(string locale)
            {
                this.locale = locale;
            }

            public bool OnBeforeAdd(BGEntity newEntity)
            {
                BGLocalizationReposCache.Push(locale, BGRepo.I, newEntity.Repo, );
                return false;
            }
        }*/
    }
}