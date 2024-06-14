/*
<copyright file="BGDatabasePreloaderGo.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    [AddComponentMenu("BansheeGz/BGDatabasePreloaderGo")]
    public class BGDatabasePreloaderGo : MonoBehaviour
    {
        // serializable
        [SerializeField] private List<PreloaderMetaSetting> tableSettings = new List<PreloaderMetaSetting>();
        [SerializeField] private List<PreloaderKeySetting> keys = new List<PreloaderKeySetting>();
        [SerializeField] private List<PreloaderIndexSetting> indexes = new List<PreloaderIndexSetting>();
        [SerializeField] private List<PreloaderReverseRelationSetting> reverseRelations = new List<PreloaderReverseRelationSetting>();
        [SerializeField] private bool doNotInitializeKeys;
        [SerializeField] private bool printLoadingTime;

        public bool DoNotInitializeKeys
        {
            get => doNotInitializeKeys;
            set => doNotInitializeKeys = value;
        }

        public bool PrintLoadingTime
        {
            get => printLoadingTime;
            set => printLoadingTime = value;
        }

        // Not serializable
        private Dictionary<BGId, PreloaderMetaSetting> metaId2settings;

        private Dictionary<BGId, PreloaderMetaSetting> MetaId2settings
        {
            get
            {
                if (metaId2settings != null) return metaId2settings;
                metaId2settings = new Dictionary<BGId, PreloaderMetaSetting>();
                foreach (var metaSetting in TableSettings)
                {
                    var meta = metaSetting.Meta;
                    if (meta == null) continue;
                    metaId2settings[meta.Id] = metaSetting;
                }

                return metaId2settings;
            }
        }

        private Dictionary<BGId, PreloaderKeySetting> keyId2settings;

        private Dictionary<BGId, PreloaderKeySetting> KeyId2settings
        {
            get
            {
                if (keyId2settings != null) return keyId2settings;
                keyId2settings = new Dictionary<BGId, PreloaderKeySetting>();
                foreach (var keySetting in keys)
                {
                    var key = keySetting.Key;
                    if (key == null) continue;
                    keyId2settings[key.Id] = keySetting;
                }

                return keyId2settings;
            }
        }

        private Dictionary<BGId, PreloaderIndexSetting> indexId2settings;

        private Dictionary<BGId, PreloaderIndexSetting> IndexId2settings
        {
            get
            {
                if (indexId2settings != null) return indexId2settings;
                indexId2settings = new Dictionary<BGId, PreloaderIndexSetting>();
                foreach (var indexSetting in indexes)
                {
                    var index = indexSetting.Index;
                    if (index == null) continue;
                    indexId2settings[index.Id] = indexSetting;
                }

                return indexId2settings;
            }
        }

        private Dictionary<BGId, PreloaderReverseRelationSetting> reverseRelationsId2settings;

        private Dictionary<BGId, PreloaderReverseRelationSetting> ReverseRelationsId2settings
        {
            get
            {
                if (reverseRelationsId2settings != null) return reverseRelationsId2settings;
                reverseRelationsId2settings = new Dictionary<BGId, PreloaderReverseRelationSetting>();
                foreach (var relationSetting in reverseRelations)
                {
                    var field = relationSetting.Field;
                    if (!(field is BGAbstractRelationI)) continue;
                    reverseRelationsId2settings[field.Id] = relationSetting;
                }

                return reverseRelationsId2settings;
            }
        }


        private HashSet<BGId> referencedMetas;

        private HashSet<BGId> ReferencedMetas
        {
            get
            {
                if (referencedMetas != null) return referencedMetas;
                referencedMetas = new HashSet<BGId>();
                BGRepo.I.ForEachMeta(meta =>
                {
                    meta.ForEachField(field =>
                    {
                        if (!(field is BGAbstractRelationI relation)) return;
                        switch (relation)
                        {
                            case BGRelationI relationI:
                            {
                                var relatedMetaId = relationI.ToId;
                                if (relatedMetaId.IsEmpty) return;
                                referencedMetas.Add(relatedMetaId);
                                break;
                            }
                            case BGManyTablesRelationI manyTablesRelationI:
                            {
                                var relatedMetaIds = manyTablesRelationI.ToIds;
                                if (relatedMetaIds == null || relatedMetaIds.Count == 0) return;
                                foreach (var metaId in relatedMetaIds) referencedMetas.Add(metaId);
                                break;
                            }
                        }
                    });
                });
                return referencedMetas;
            }
        }

        public List<PreloaderKeySetting> Keys => keys;

        public List<PreloaderIndexSetting> Indexes => indexes;

        public List<PreloaderReverseRelationSetting> ReverseRelations => reverseRelations;

        public List<PreloaderMetaSetting> TableSettings => tableSettings;

        //============================ Unity messages
        private void Awake()
        {
            if (PrintLoadingTime) BGUtil.Measure("[BGDatabase] database loaded in (mls)", Load);
            else Load();
        }

        //============================ methods
        public void Load()
        {
            //load database
            var db = BGRepo.I;

            if (!doNotInitializeKeys)
            {
                db.ForEachMeta(meta =>
                {
                    var metaSettings = GetMetaSetting(meta.Id);

                    //id key
                    if (IsUsedByRelations(meta) || (metaSettings?.IdIndex ?? false)) meta.GetEntity(BGId.Empty);

                    //name key
                    if (metaSettings?.NameIndex ?? false) meta.GetEntity("");

                    meta.ForEachField(field =>
                    {
                        if (!(field is BGAbstractRelationI relation)) return;
                        //reverse relation key, used by nested meta to get the list of related entities  
                        if (IsUsedByNestedMeta(field)) relation.GetRelatedIn(BGId.Empty);
                    });
                });

                //user-defined keys
                foreach (var key in keys)
                {
                    var dbKey = key.Key;
                    if (dbKey == null) continue;
                    if (key.IncludePartialKeys) dbKey.BuildAll();
                    else dbKey.Build();
                }

                //user-defined indexes
                foreach (var index in indexes)
                {
                    var dbIndex = index.Index;
                    if (dbIndex == null) continue;
                    dbIndex.Build();
                }

                //reverse relation
                foreach (var relation in reverseRelations)
                {
                    var dbRelation = relation.Field;
                    if (!(dbRelation is BGAbstractRelationI aRelation)) continue;
                    aRelation.GetRelatedIn(BGId.Empty);
                }
            }
        }

        public bool IsUsedByRelations(BGMetaEntity meta) => ReferencedMetas.Contains(meta.Id);
        public bool IsUsedByNestedMeta(BGField field) => field.Meta is BGMetaNested metaNested && metaNested.OwnerRelationId == field.Id;

        public PreloaderMetaSetting GetMetaSetting(BGId metaId) => MetaId2settings.TryGetValue(metaId, out var result) ? result : null;

        public PreloaderMetaSetting AddMetaSetting(BGMetaEntity meta)
        {
            var metaSetting = new PreloaderMetaSetting() { Meta = meta };
            tableSettings.Add(metaSetting);
            MetaId2settings[meta.Id] = metaSetting;
            return metaSetting;
        }

        public void RemoveMetaSetting(BGId metaId)
        {
            var idStr = metaId.ToString();
            tableSettings.RemoveAll(setting => idStr == setting.MetaId.ToString());
            MetaId2settings.Remove(metaId);
        }

        public PreloaderKeySetting GetKeySetting(BGId keyId) => KeyId2settings.TryGetValue(keyId, out var result) ? result : null;

        public PreloaderKeySetting AddKeySetting(BGKey key)
        {
            var keySetting = new PreloaderKeySetting() { Key = key };
            keys.Add(keySetting);
            KeyId2settings[key.Id] = keySetting;
            return keySetting;
        }

        public void RemoveKeySetting(BGId keyId)
        {
            var idStr = keyId.ToString();
            keys.RemoveAll(setting => idStr == setting.KeyId);
            KeyId2settings.Remove(keyId);
        }

        public PreloaderIndexSetting GetIndexSetting(BGId indexId) => IndexId2settings.TryGetValue(indexId, out var result) ? result : null;

        public PreloaderIndexSetting AddIndexSetting(BGIndex index)
        {
            var indexSetting = new PreloaderIndexSetting() { Index = index };
            indexes.Add(indexSetting);
            IndexId2settings[index.Id] = indexSetting;
            return indexSetting;
        }

        public void RemoveIndexSetting(BGId indexId)
        {
            var idStr = indexId.ToString();
            indexes.RemoveAll(setting => idStr == setting.IndexId);
            IndexId2settings.Remove(indexId);
        }

        public PreloaderReverseRelationSetting GetReverseRelationSetting(BGId indexId) => ReverseRelationsId2settings.TryGetValue(indexId, out var result) ? result : null;

        public PreloaderReverseRelationSetting AddReverseRelationSetting(BGField field)
        {
            var relationSetting = new PreloaderReverseRelationSetting() { Field = field };
            reverseRelations.Add(relationSetting);
            ReverseRelationsId2settings[field.Id] = relationSetting;
            return relationSetting;
        }

        public void RemoveReverseRelationSetting(BGId relationId)
        {
            var idStr = relationId.ToString();
            reverseRelations.RemoveAll(setting => idStr == setting.FieldId);
            ReverseRelationsId2settings.Remove(relationId);
        }

        //============================ nested classes
        [Serializable]
        public class PreloaderMetaSetting : BGMetaReference
        {
            public bool IdIndex;
            public bool NameIndex;
        }

        [Serializable]
        public class PreloaderKeySetting : BGKeyReference
        {
            public bool IncludePartialKeys;
        }

        [Serializable]
        public class PreloaderIndexSetting : BGIndexReference
        {
        }

        [Serializable]
        public class PreloaderReverseRelationSetting : BGFieldReference
        {
        }
    }
}