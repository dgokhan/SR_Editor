/*
<copyright file="BGAddonMT.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// addon for multithreading support. 
    /// See  <a href="http://www.bansheegz.com/BGDatabase/Addons/MultiThreading/">this link</a> for more details.
    /// </summary>
    [AddonDescriptor(Name = "MultiThreading", ManagerType = "BansheeGz.BGDatabase.Editor.BGAddonManagerMT")]
    public class BGAddonMT : BGAddon
    {
        public enum MetaModeEnum
        {
            Copy
        }

        //================================================================================================
        //                                              Fields
        //================================================================================================
        private bool multithreadedUpdates;
        private readonly BGIdDictionary<MetaSetting> id2MetaSetting = new BGIdDictionary<MetaSetting>();

        public bool MultithreadedUpdates
        {
            get => multithreadedUpdates;
            set
            {
                if (multithreadedUpdates == value) return;
                multithreadedUpdates = value;
                FireChange();
            }
        }

        private bool mergeOnSave;

        public bool MergeOnSave
        {
            get => mergeOnSave;
            set
            {
                if (mergeOnSave == value) return;
                mergeOnSave = value;
                FireChange();
            }
        }

        public BGMergeSettingsEntity MergeSettings = new BGMergeSettingsEntity();

        public BGAddonMT()
        {
            MergeSettings.OnChange += SettingsChanged;
        }

        //================================================================================================
        //                                              Methods
        //================================================================================================
        public BGMTService CreateService()
        {
            var metaList = new List<BGMTMeta>();
            var index = 0;

            Repo.ForEachMeta(meta =>
            {
                MetaSetting settings;
                if (!id2MetaSetting.TryGetValue(meta.Id, out settings)) return;

                var mtMeta = new BGMTMeta(meta, index++);
                metaList.Add(mtMeta);
            });

            return new BGMTService(multithreadedUpdates, new BGMTRepo(metaList.ToArray()));
        }

        public bool HasMeta(BGId metaId)
        {
            return id2MetaSetting.ContainsKey(metaId);
        }

        public void AddMeta(BGId metaId)
        {
            id2MetaSetting[metaId] = new MetaSetting { MetaId = metaId };
        }

        public void RemoveMeta(BGId metaId)
        {
            id2MetaSetting.Remove(metaId);
        }

        //================================================================================================
        //                                              Config
        //================================================================================================
        /// <inheritdoc />
        public override string ConfigToString()
        {
            var settings = new Settings { MultithreadedUpdates = multithreadedUpdates, MergeOnSave = mergeOnSave, MergeSettings = MergeSettings };
            if (id2MetaSetting.Count > 0)
            {
                settings.MetaIds = new MetaId[id2MetaSetting.Count];
                var index = 0;
                foreach (var pair in id2MetaSetting)
                {
                    var val = pair.Value;
                    settings.MetaIds[index++] = new MetaId
                    {
                        Mode = val.Mode,
                        Id = val.MetaId.ToString()
                    };
                }
            }

            return JsonUtility.ToJson(settings);
        }

        /// <inheritdoc />
        public override void ConfigFromString(string config)
        {
            var settings = JsonUtility.FromJson<Settings>(config);
            multithreadedUpdates = settings.MultithreadedUpdates;
            mergeOnSave = settings.MergeOnSave;
            MergeSettings = settings.MergeSettings;
            MergeSettings.OnChange += SettingsChanged;

            id2MetaSetting.Clear();
            if (settings.MetaIds != null && settings.MetaIds.Length > 0)
                for (var i = 0; i < settings.MetaIds.Length; i++)
                {
                    var metaId = settings.MetaIds[i];
                    var id = BGId.Parse(metaId.Id);
                    id2MetaSetting[id] = new MetaSetting
                    {
                        Mode = metaId.Mode,
                        MetaId = id
                    };
                }
        }

        [Serializable]
        private class Settings
        {
            public bool MultithreadedUpdates;
            public MetaId[] MetaIds;
            public bool MergeOnSave;
            public BGMergeSettingsEntity MergeSettings = new BGMergeSettingsEntity();
        }

        [Serializable]
        private class MetaId
        {
            public string Id;
            public MetaModeEnum Mode;
        }

        /// <inheritdoc />
        public override byte[] ConfigToBytes()
        {
            var writer = new BGBinaryWriter(6);

            //version
            writer.AddInt(1);

            //fields
            writer.AddBool(multithreadedUpdates);
            writer.AddArray(() =>
            {
                foreach (var metaSetting in id2MetaSetting)
                {
                    var metaId = metaSetting.Value;
                    writer.AddId(metaId.MetaId);
                    writer.AddInt((int)metaId.Mode);
                }
            }, id2MetaSetting.Count);

            writer.AddBool(mergeOnSave);
            writer.AddByteArray(MergeSettings.ConfigToBytes());
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
                    multithreadedUpdates = reader.ReadBool();
                    id2MetaSetting.Clear();

                    reader.ReadArray(() =>
                    {
                        var metaSetting = new MetaSetting
                        {
                            MetaId = reader.ReadId(),
                            Mode = (MetaModeEnum)reader.ReadInt()
                        };
                        id2MetaSetting[metaSetting.MetaId] = metaSetting;
                    });

                    mergeOnSave = reader.ReadBool();
                    MergeSettings.ConfigFromBytes(reader.ReadByteArray());
                    break;
                }
                default:
                {
                    throw new BGException("Unknown version: $", version);
                }
            }
        }


        /// <inheritdoc />
        public override BGAddon CloneTo(BGRepo repo)
        {
            var clone = new BGAddonMT
            {
                Repo = repo,
                mergeOnSave = mergeOnSave,
                multithreadedUpdates = multithreadedUpdates,
                MergeSettings = (BGMergeSettingsEntity)MergeSettings.Clone()
            };
            foreach (var pair in id2MetaSetting) clone.id2MetaSetting.Add(pair.Key, pair.Value);
            return clone;
        }

        private void SettingsChanged()
        {
            FireChange();
        }

        //================================================================================================
        //                                              Merge
        //================================================================================================
        /// <summary>
        /// Merge changes from MT environment back to main database
        /// </summary>
        public void Merge()
        {
            Merge(MergeSettings);
        }

        /// <summary>
        /// Merge changes from MT environment back to main database
        /// </summary>
        public void Merge(BGMergeSettingsEntity mergeSettings)
        {
            var fromRepo = Repo.MTService.RepoReadOnly;
            Repo.ForEachMeta(meta =>
            {
                var metaId = meta.Id;
                var b = !HasMeta(metaId);
                var b1 = !mergeSettings.IsMetaIncluded(metaId);
                if (b || b1) return;

                var fromMeta = fromRepo[meta.Id];
                if (fromMeta == null) return;

                var addingMissing = mergeSettings.IsAddingMissing(metaId);
                var removingOrphaned = mergeSettings.IsRemovingOrphaned(metaId);
                var toRemoveList = removingOrphaned ? new HashSet<BGEntity>() : null;
                var updatingMatching = mergeSettings.IsUpdatingMatching(metaId);
                var fieldsList = new List<BGField>();
                var fromFieldsIndexes = new List<int>();
                var updatingField = new List<bool>();
                if (updatingMatching || addingMissing)
                    meta.ForEachField(field =>
                    {
                        var fromField = fromMeta.GetField(field.Id, false);
                        if (fromField == null) return;

                        fieldsList.Add(field);
                        fromFieldsIndexes.Add(fromField.Index);
                        updatingField.Add(mergeSettings.IsFieldIncluded(field));
                    });


                meta.ForEachEntity(entity =>
                {
                    var fromEntity = fromMeta[entity.Id];
                    if (fromEntity == null)
                    {
                        //orphaned
                        toRemoveList?.Add(entity);
                    }
                    else
                    {
                        //matching
                        if (updatingMatching) CopyFields(fieldsList, fromFieldsIndexes, entity, fromEntity.Value, updatingField);
                    }
                });

                if (toRemoveList != null && toRemoveList.Count > 0) meta.DeleteEntities(toRemoveList);

                //missing
                if (addingMissing)
                    fromMeta.ForEachEntity(entity =>
                    {
                        var fromEntityId = entity.Id;
                        if (meta.HasEntity(fromEntityId)) return;

                        CopyFields(fieldsList, fromFieldsIndexes, meta.NewEntity(fromEntityId), entity, null);
                    });
            });
        }

        private void CopyFields(List<BGField> fieldsList, List<int> fromFieldsIndexes, BGEntity entity, BGMTEntity fromEntity, List<bool> updatingList)
        {
            if (fieldsList.Count == 0) return;
            for (var i = 0; i < fieldsList.Count; i++)
            {
                var field = fieldsList[i];
                var fromFieldIndex = fromFieldsIndexes[i];
                if (updatingList != null && !updatingList[i]) continue;

                fromEntity.Meta.GetField(fromFieldIndex).CopyTo(field, entity, fromEntity);
            }
        }

        //================================================================================================
        //                                              Nested
        //================================================================================================
        public class MetaSetting
        {
            public BGId MetaId;
            public MetaModeEnum Mode;
        }
    }
}