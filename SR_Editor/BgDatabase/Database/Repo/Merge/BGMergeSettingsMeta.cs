/*
<copyright file="BGMergeSettingsMeta.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// NOT CURRENTLY USED
    /// Settings for merging metas. Merging metas is experimental feature
    /// </summary>
    [Serializable]
    public partial class BGMergeSettingsMeta : BGMergeSettingsA
    {
        //do not make readonly!
        [SerializeField] private HashtableId2MetaSettings id2Meta = new HashtableId2MetaSettings();

        public bool HasAny => addMissing || updateMatching || removeOrphaned;

        public int CountMeta => id2Meta.Count;

        public bool Has(BGId metaId)
        {
            return id2Meta.ContainsKey(metaId);
        }

        public MetaSettings GetSettings(BGId metaId)
        {
            return BGUtil.Get(id2Meta, metaId);
        }

        public void Remove(BGId metaId)
        {
            if (!Has(metaId)) return;
            id2Meta.Remove(metaId);
            FireOnChange();
        }

        public bool IsMetaIncluded(BGId metaId)
        {
            if (!HasAny) return false;

            return Has(metaId) ? GetSettings(metaId).Included : IncludedByDefault;
        }

        public bool IsFieldIncluded(BGField field)
        {
            if (!HasAny) return false;

            var metaId = field.MetaId;
            if (!Has(metaId)) return IncludedByDefault;

            var metaSettings = GetSettings(metaId);
            if (!metaSettings.Included) return false;
            return metaSettings.HasField(field.Id);
        }

        [Serializable]
        public class MetaSettings
        {
            public event Action OnChange;

            [SerializeField] private BGIdList fields = new BGIdList();
            [SerializeField] private bool included;

            public bool Included
            {
                get => included;
                set => included = value;
            }

            public int Count => fields.Count;

            public void AddField(BGId fieldId)
            {
                if (fields.Contains(fieldId)) return;
                fields.Add(fieldId);
                FireOnChange();
            }

            public void RemoveField(BGId fieldId)
            {
                if (!fields.Contains(fieldId)) return;
                fields.Remove(fieldId);
                FireOnChange();
            }

            public bool HasField(BGId fieldId)
            {
                return fields.Contains(fieldId);
            }

            private void FireOnChange()
            {
                OnChange?.Invoke();
            }
        }

        [Serializable]
        public class HashtableId2MetaSettings : BGHashtableIdKey<MetaSettings>
        {
        }

        public void ForEachSetting(Action<MetaSettings> action)
        {
            foreach (var pair in id2Meta) action(pair.Value);
        }

        public void Ensure(BGId metaId)
        {
            if (id2Meta.ContainsKey(metaId)) return;
            id2Meta[metaId] = new MetaSettings();
        }

        public void ForEachMeta(BGRepo repo, Action<BGMetaEntity> action)
        {
            repo.ForEachMeta(meta =>
            {
                if (!IsMetaIncluded(meta.Id)) return;
                action(meta);
            });
        }

        public void ForEachField(BGRepo repo, Action<BGField> action)
        {
            repo.ForEachMeta(meta =>
            {
                if (!IsMetaIncluded(meta.Id)) return;
                meta.ForEachField(field =>
                {
                    if (!IsFieldIncluded(field)) return;
                    action(field);
                });
            });
        }
    }
}