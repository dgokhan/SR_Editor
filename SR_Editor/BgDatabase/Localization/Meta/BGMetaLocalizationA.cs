/*
<copyright file="BGMetaLocalizationA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    public abstract partial class BGMetaLocalizationA : BGMetaEntity
    {
        //do not change setter to private, it's used by reflection (BGMetaCustomToolProviderLocalization)
        public Type FieldType { get; protected set; }

        //========================================================================================
        //              Constructors
        //========================================================================================
        protected BGMetaLocalizationA(BGRepo repo, string name, Type fieldType) : base(repo, name)
        {
            var localization = repo.Addons.Get<BGAddonLocalization>();
            if (localization == null)
            {
                Unregister();
                throw new BGException("Localization addon is not enabled");
            }

            if (fieldType == null)
            {
                Unregister();
                throw new BGException("Field type can not be null");
            }

            if (!typeof(BGFieldLocaleI).IsAssignableFrom(fieldType))
            {
                Unregister();
                throw new BGException("Field type should implement BGFieldLocaleI");
            }

            FieldType = fieldType;
            localization.Locales.ForEach(locale =>
            {
                BGUtil.Create<BGField>(fieldType, false, this, locale.Name).System = true;
            });

            /*
            localization.ForEachLoadedLocale((locale, localeRepo) =>
            {
                localization.SyncMetas(locale, localeRepo);
            });
        */
            BGLocalizationReposCache.ForEachLoadedLocale((locale, localeRepo) =>
            {
                var field = GetField(locale);
                BGLocalizationSaveManager.Sync(localeRepo, field);
            });
        }

        protected BGMetaLocalizationA(BGRepo repo, BGId id, string name) : base(repo, id, name)
        {
        }

        //========================================================================================
        //              Methods
        //========================================================================================
        public override void Delete()
        {
            var repo = Repo;
            if (repo == null) return;

            base.Delete();
            BGLocalizationReposCache.ForEachLoadedLocale((locale, localeRepo) =>
            {
                if (localeRepo.HasMeta(Id)) localeRepo.GetMeta(Id).Delete();
            });
        }

        public static void ForEachMeta(BGRepo repo, Action<BGMetaLocalizationA> action)
        {
            repo.ForEachMeta(meta => action((BGMetaLocalizationA)meta), meta => meta is BGMetaLocalizationA);
        }

        //========================================================================================
        //              Configuration
        //========================================================================================

        public override string ConfigToString()
        {
            return JsonUtility.ToJson(new JsonConfig { FieldTypeName = FieldType.AssemblyQualifiedName });
        }

        public override void ConfigFromString(string config)
        {
            if (string.IsNullOrEmpty(config)) return;
            FieldType = BGUtil.GetType(JsonUtility.FromJson<JsonConfig>(config).FieldTypeName);
        }

        [Serializable]
        private struct JsonConfig
        {
            public string FieldTypeName;
        }

        /// <inheritdoc />
        public override byte[] ConfigToBytes()
        {
            var fieldTypeName = FieldType.AssemblyQualifiedName;
            var writer = new BGBinaryWriter(4 + BGBinaryWriter.GetBytesCount(fieldTypeName));
            //version
            writer.AddInt(1);
            //field type
            writer.AddString(fieldTypeName);

            return writer.ToArray();
        }

        /// <inheritdoc />
        public override void ConfigFromBytes(ArraySegment<byte> config)
        {
            var reader = new BGBinaryReader(config);
            var version = reader.ReadInt();
            switch (version)
            {
                case 1:
                {
                    FieldType = BGUtil.GetType(reader.ReadString());
                    break;
                }
                default:
                {
                    throw new BGException("Unknown version: $", version);
                }
            }
        }
    }
}