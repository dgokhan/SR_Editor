/*
<copyright file="BGLocalizationLoadedRepos.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    //loaded repos for main database
    public static class BGLocalizationReposCache
    {
        //cache for main database ONLY!
        private static readonly Dictionary<string, BGRepo> locale2Repo = new Dictionary<string, BGRepo>();
        private static readonly Dictionary<PartitionLocaleKey, BGRepo> partition2Repo = new Dictionary<PartitionLocaleKey, BGRepo>();

        //for testing ONLY
        // private static readonly Dictionary<string, byte[]> locale2RepoContent = new Dictionary<string, byte[]>();


        public static BGRepo Load(BGLocalizationStructure structure, BGLocalizationLoadingContext context)
        {
            if (context == null)
            {
                Debug.Log("Unable to load localized data, cause context is null");
                return null;
            }

            var locale = context.Locale;
            var errorIfNotFound = context.ErrorIfNotFound;

            if (string.IsNullOrEmpty(locale))
            {
                Debug.Log("Unable to load localized data, cause locale is null");
                return null;
            }

            if (structure.Find(locale) == null)
            {
                Debug.Log(BGUtil.Format("Unable to load localized data, cause locale [$] is not found", locale));
                return null;
            }

            var localeRepo = new BGRepo();

            var loader = structure.Repo.RepoLoader ?? BGRepo.DefaultRepoLoader;

            //this is used only by tests, correct?????- switch to BGRepoCustomLoaderModel?
            if (context.RepoData != null) localeRepo.Load(context.RepoData);
            else
            {
                // var repoAssetPath = GetLocalizationFilePath(locale);

                byte[] content = null;
                /*
                if (locale2RepoContent.Count > 0 && locale2RepoContent.TryGetValue(locale, out content))
                {
                    //loaded from cache (for tests only)
                }
                else
                {
                    */
                if (loader != null) content = LoadContent(structure.Repo, loader, BGAddonLocalization.GetLocalePaths(locale), errorIfNotFound, context.BasePath);
                // }

                if (content == null)
                {
                    if (errorIfNotFound) throw new BGException("Can not load localization file, loader is null");
                }
                else localeRepo.Load(content);
            }

            localeRepo.Events.On = true;

            if (!context.NotMain) locale2Repo[locale] = localeRepo;

            if (context.PushFieldsConfig != null)
            {
                Push(locale, structure.Repo, localeRepo, context.ErrorIfNotFound);
                PushPartitions(locale, structure, context, loader);
            }

            return localeRepo;
        }

        private static void PushPartitions(string locale, BGLocalizationStructure structure, BGLocalizationLoadingContext context, BGLoaderForRepo loader)
        {
            if (context.PushFieldsConfig.LoadPartitionsMode == BGLocalizationLoadingContext.LoadPartitionsModeEnum.NotLoad) return;
            var loadAll = context.PushFieldsConfig.LoadPartitionsMode == BGLocalizationLoadingContext.LoadPartitionsModeEnum.LoadAll;

            var partitionAddon = structure.Repo.Addons.Get<BGAddonPartition>();
            if (partitionAddon == null || !partitionAddon.EnabledHorizontal) return;

            var settings = new BGMergeSettingsEntity()
            {
                AddMissing = true,
                UpdateMatching = true
            };
            var partitionMeta = partitionAddon.PartitionMeta;
            partitionMeta.ForEachEntity(entity =>
            {
                BGRepo partitionRepo = null;
                var partitionLocaleKey = new PartitionLocaleKey(locale, entity.Id);
                if (context.NotMain || !partition2Repo.TryGetValue(partitionLocaleKey, out partitionRepo))
                {
                    var keys = BGUtil.Concat(BGAddonLocalization.GetLocalePaths(locale), BGAddonPartition.GetPartitionPaths(entity.Id));
                    var partitionContent = LoadContent(structure.Repo, loader, keys, context.ErrorIfNotFound, context.BasePath);
                    if (partitionContent != null)
                    {
                        partitionRepo = new BGRepo(partitionContent);
                        if (!context.NotMain) partition2Repo[partitionLocaleKey] = partitionRepo;
                    }
                }

                if (partitionRepo == null)
                {
                    if (context.ErrorIfNotFound) throw new BGException("Can not load partition $ for locale $", entity.Id, locale);
                    return;
                }

                BGAddonPartition.Merge(partitionRepo, structure.Repo, settings);
            }, entity => loadAll || partitionAddon.IsLoaded(entity.Id));
        }

        public static void Push(string locale, BGRepo targetRepo, BGRepo delegateRepo, bool errorIfNotFound)
        {
            BGMetaLocalizationA.ForEachMeta(targetRepo, meta =>
            {
                // if (!(meta is BGMetaLocalizationSingleValue)) return;
                var delegateMeta = delegateRepo[meta.Id];
                ThrowIf(errorIfNotFound && delegateMeta == null, "Can not load delegate meta for $ meta $ locale- does localization file corrupted?", meta.Name, locale);
                if (delegateMeta == null) return;
                var localeField = meta.GetField(locale, false);
                ThrowIf(errorIfNotFound && localeField == null, "Can not find $ locale field in $ meta", locale, meta.Name);
                ThrowIf(errorIfNotFound && !(localeField is BGFieldLocaleI), "Locale field $ in $ meta has wrong type- should implement BGFieldLocaleI", locale, meta.Name);
                if (!(localeField is BGFieldLocaleI localeI)) return;
                var delegateField = delegateMeta.GetField(localeField.Id, false);
                ThrowIf(errorIfNotFound && delegateField == null, "Can not load delegate field for $ field - does localization file corrupted?", locale);
                if (delegateField == null) return;

                //make sure store is inited to avoid recursive load
                localeI.EnsureStore();
                delegateField.Meta.ForEachEntity(entity =>
                {
                    localeField.CopyValue(delegateField, entity.Id, entity.Index, entity.Id);
                });
            });
        }

        private static void ThrowIf(bool condition, string error, params object[] parameters)
        {
            if (!condition) return;
            throw new BGException(error, parameters);
        }

        public static byte[] LoadContent(BGRepo repo, BGLoaderForRepo loader, string[] subPath, bool errorIfNotFound, string basePathParameter)
        {
            var basePath = basePathParameter ?? repo.RepoAssetPath ?? BGRepo.DefaultRepoAssetPath;
            var request = new BGLoaderForRepo.LoadRequest(basePath, subPath);
            var content = loader.Load(request);
            if (content == null && errorIfNotFound) throw new BGException("Can not load localization file, using $ path", request.ToPath(loader));
            return content;
        }

        public static void Unload(string locale)
        {
            var repo = BGUtil.Get(locale2Repo, locale);
            if (repo != null)
                // repo.ForEachMeta(RemoveListeners);
                locale2Repo.Remove(locale);

            var toRemove = new List<PartitionLocaleKey>();
            foreach (var pair in partition2Repo)
            {
                if (!string.Equals(pair.Key.Locale, locale)) continue;
                toRemove.Add(pair.Key);
            }

            foreach (var key in toRemove) partition2Repo.Remove(key);
        }

        public static void Unload()
        {
            // ForEachLoadedLocale((locale, repo) =>
            // {
            // repo.ForEachMeta(RemoveListeners);
            // });
            locale2Repo.Clear();
            partition2Repo.Clear();
        }

        public static bool IsLoaded(string locale) => locale2Repo.ContainsKey(locale);

        public static void ForEachLoadedLocale(Action<string, BGRepo> action)
        {
            foreach (var pair in locale2Repo) action(pair.Key, pair.Value);
        }

        /// <summary>
        /// DO NOT call this method
        /// </summary>
        public static void Load<T>(BGFieldLocaleA<T> field) //where T : Object
        {
            if (!BGRepo.DefaultRepo(field.Meta.Repo)) throw new Exception("Can not load a field, cause field's repo is not main repo");

            var delegateRepo = BGUtil.Get(locale2Repo, field.Name);
            if (delegateRepo == null)
            {
                var structure = new BGLocalizationStructure(field.Meta.Repo);
                delegateRepo = Load(structure, new BGLocalizationLoadingContext(field.Name)
                    {
                        ErrorIfNotFound = true
                    }
                );
            }

            if (delegateRepo == null) throw new BGException("Can not load localized values for $ field, cause locale repo can not be loaded", field.FullName);
            var delegateMeta = delegateRepo[field.MetaId];
            if (delegateMeta == null) throw new BGException("Can not load localized values for $ field, cause locale repo's meta is not found", field.FullName);
            var delegateField = delegateMeta.GetField(field.Id, false);
            if (delegateField == null) throw new BGException("Can not load localized values for $ field, cause locale repo meta field is not found", field.FullName);

            field.Repo.Events.WithEventsDisabled(() =>
            {
                delegateField.Meta.ForEachEntity(entity =>
                {
                    field.CopyValue(delegateField, entity.Id, entity.Index, entity.Id);
                });
            });
        }

        /*
        public static void SetLocaleRepoContent(string locale, byte[] content)
        {
            locale2RepoContent[locale] = content;
        }

        public static void ClearContentCache()
        {
            locale2RepoContent.Clear();
        }
        */

        public static BGRepo LoadPartitionRepo(PartitionLocaleKey partitionLocaleKey, BGRepo repo, BGLoaderForRepo loader)
        {
            var partitionId = partitionLocaleKey.PartitionId;
            var locale = partitionLocaleKey.Locale;
            BGRepo partitionRepo;
            if (!partition2Repo.TryGetValue(partitionLocaleKey, out partitionRepo))
            {
                var paths = BGUtil.Concat(BGAddonLocalization.GetLocalePaths(locale), BGAddonPartition.GetPartitionPaths(partitionId));
                var content = LoadContent(repo, loader, paths, true, null);
                // if (content == null) throw new BGException("Can not load partition $ for $ locale: file path is $ ", partitionEntity.Name, locale, partitionPath);
                partitionRepo = new BGRepo(content);
                partition2Repo[partitionLocaleKey] = partitionRepo;
            }

            return partitionRepo;
        }

        public static void UnloadPartitionRepo(PartitionLocaleKey partitionLocaleKey)
        {
            partition2Repo.Remove(partitionLocaleKey);
        }


        public class PartitionLocaleKey
        {
            public readonly string Locale;
            public readonly BGId PartitionId;

            public PartitionLocaleKey(string locale, BGId partitionId)
            {
                Locale = locale;
                PartitionId = partitionId;
            }

            protected bool Equals(PartitionLocaleKey other)
            {
                return Locale == other.Locale && PartitionId.Equals(other.PartitionId);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;
                return Equals((PartitionLocaleKey)obj);
            }

            public override int GetHashCode()
            {
                unchecked { return ((Locale != null ? Locale.GetHashCode() : 0) * 397) ^ PartitionId.GetHashCode(); }
            }

            public static bool operator ==(PartitionLocaleKey left, PartitionLocaleKey right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(PartitionLocaleKey left, PartitionLocaleKey right)
            {
                return !Equals(left, right);
            }
        }
    }
}