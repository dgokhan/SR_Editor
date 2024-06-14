/*
<copyright file="BGSyncNameMapConfig.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Names mapping config
    /// </summary>
    [Serializable]
    public class BGSyncNameMapConfig : BGConfigurableBinaryI
    {
        private const string DisallowedCharacters = "\\/*?:[]";

        [SerializeField] private List<MetaMap> metas = new List<MetaMap>();

        /// <summary>
        /// number of tables with names mapping config
        /// </summary>
        public int CountMetas
        {
            get
            {
                if (metas == null) return 0;
                var count = 0;
                foreach (var meta in metas)
                    if (meta.HasMapping)
                        count++;
                return count;
            }
        }

        /// <summary>
        /// number of fields with names mapping config
        /// </summary>
        public int CountFields
        {
            get
            {
                if (metas == null) return 0;
                var count = 0;
                foreach (var meta in metas) count += meta.CountFields;
                return count;
            }
        }

        /// <summary>
        /// get the first error for configuration weeoe 
        /// </summary>
        public string GetError(BGRepo repo)
        {
            if (metas == null) return null;
            //mapping name conflicts with meta name
            foreach (var metaConfig in metas)
            {
                if (!metaConfig.HasMapping) continue;
                if (metaConfig.Name.Length > 31) return "Sheet name [" + metaConfig.Name + "] length exceeds maximum number of characters (31)";
                // if (metaConfig.Name.ToLower().Equals("history")) return "Sheet can not be named as [history],  it's reserved word";
                foreach (var character in DisallowedCharacters)
                    if (metaConfig.Name.IndexOf(character) != -1)
                        return "Meta name [" + metaConfig.Name + "] contains a prohibited character (" + character + ")";

                var meta = repo.GetMeta(BGId.Parse(metaConfig.Id));
                if (meta == null) continue;
                var duplicateMeta = repo.FindMeta(m => m.Id != meta.Id && string.Equals(metaConfig.Name, m.Name));
                if (duplicateMeta != null) return "Mapped name [" + metaConfig.Name + "], used for [" + meta.Name + "] meta conflicts with [" + duplicateMeta.Name + "] meta name";

                foreach (var metaConfig2 in metas)
                {
                    if (metaConfig == metaConfig2 || !metaConfig2.HasMapping) continue;
                    if (!repo.HasMeta(BGId.Parse(metaConfig.Id)) || !repo.HasMeta(BGId.Parse(metaConfig2.Id))) continue;
                    if (string.Equals(metaConfig.Name, metaConfig2.Name)) return "The same name [" + metaConfig.Name + "] is used by multiple metas";
                }
            }

            //mapped field names
            foreach (var metaConfig in metas)
            {
                var meta = repo.GetMeta(BGId.Parse(metaConfig.Id));
                if (meta == null) continue;
                if (metaConfig.Fields != null)
                    foreach (var configField in metaConfig.Fields)
                    {
                        if (!configField.HasMapping) continue;
                        var field = meta.GetField(BGId.Parse(configField.Id), false);
                        if (field == null) continue;
                        var duplicateField = meta.FindField(f => f.Id != field.Id && string.Equals(configField.Name, f.Name));
                        if (duplicateField != null) return "Mapped name [" + configField.Name + "], used for [" + field.FullName + "] field conflicts with [" + duplicateField.FullName + "] field";
                        foreach (var configField2 in metaConfig.Fields)
                        {
                            if (configField == configField2 || !configField2.HasMapping) continue;
                            if (!meta.HasField(BGId.Parse(configField.Id)) || !meta.HasField(BGId.Parse(configField2.Id))) continue;
                            if (string.Equals(configField.Name, configField2.Name)) return "The same field name [" + configField.Name + "] is used by multiple fields of [" + meta.Name + "] meta";
                        }
                    }
            }


            return null;
        }

        /// <summary>
        /// Trim configuration for non-existent tables/fields 
        /// </summary>
        public void Trim(BGRepo repo = null)
        {
            if (metas == null) return;
            for (var i = metas.Count - 1; i >= 0; i--)
            {
                var metaMap = metas[i];
                if (!metaMap.Trim(repo ?? BGRepo.I)) metas.RemoveAt(i);
            }
        }

        /// <summary>
        /// Get mapped name for a provided table
        /// </summary>
        public string GetName(BGMetaEntity meta)
        {
            var metaMap = GetMetaMap(meta.Id.ToString());
            if (metaMap != null && metaMap.HasMapping) return metaMap.Name;
            return meta.Name;
        }

        /// <summary>
        /// Get mapped name for a provided field
        /// </summary>
        public string GetName(BGField field)
        {
            var metaMap = GetMetaMap(field.MetaId.ToString());
            if (metaMap != null) return metaMap.GetFieldName(field);
            return field.Name;
        }

        /// <summary>
        /// Get database table ID for a given sheet name
        /// </summary>
        public BGId GetDatabaseMetaId(string sheetName)
        {
            if (metas == null) return BGId.Empty;
            foreach (var meta in metas)
                if (string.Equals(meta.Name, sheetName))
                    return BGId.Parse(meta.Id);

            return BGId.Empty;
        }

        /// <summary>
        /// Has a table with provided ID names mapping config? 
        /// </summary>
        public bool HasMetaConfig(BGId metaId)
        {
            var metaMap = GetMetaMap(metaId.ToString());
            return metaMap != null && metaMap.HasMapping;
        }

        /// <summary>
        /// Has a table with provided sheet name mapping config? 
        /// </summary>
        private bool HasMetaConfig(string sheetName) => !GetDatabaseMetaId(sheetName).IsEmpty;


        /// <summary>
        /// Get field id by tableID and header name 
        /// </summary>
        public BGId GetDatabaseFieldId(BGId metaId, string headerName)
        {
            var map = GetMetaMap(metaId.ToString());
            if (map == null) return BGId.Empty;
            return map.GetFieldId(headerName);
        }

        /// <summary>
        /// Does the field has names mapping config? 
        /// </summary>
        private bool HasFieldConfig(BGId metaId, string fieldName)
        {
            var map = GetMetaMap(metaId.ToString());
            if (map == null) return false;
            return !map.GetFieldId(fieldName).IsEmpty;
        }

        /// <summary>
        /// Does the field has names mapping config? 
        /// </summary>
        private bool HasFieldConfig(BGId metaId, BGId fieldId)
        {
            var map = GetMetaMap(metaId.ToString());
            if (map == null) return false;
            return map.HasFieldMapping(fieldId);
        }

        /// <summary>
        /// make sure table config is added 
        /// </summary>
        public MetaMap EnsureMetaMap(string metaId) => GetMetaMap(metaId) ?? AddMetaMap(metaId);

        /// <summary>
        /// add table config  
        /// </summary>
        private MetaMap AddMetaMap(string metaId)
        {
            var metaMap = new MetaMap(metaId);
            metas.Add(metaMap);
            return metaMap;
        }

        /// <summary>
        /// get table config 
        /// </summary>
        public MetaMap GetMetaMap(string metaId)
        {
            if (metas != null)
                foreach (var metaMap in metas)
                    if (string.Equals(metaMap.Id, metaId))
                        return metaMap;

            return null;
        }

        /// <summary>
        /// get table config 
        /// </summary>
        public MetaMap GetMetaMapByName(string name)
        {
            if (metas != null)
                foreach (var metaMap in metas)
                    if (string.Equals(metaMap.Name, name))
                        return metaMap;

            return null;
        }

        /// <summary>
        /// Resolve the table by sheet's name 
        /// </summary>
        public BGMetaEntity Map(BGRepo repo, string sheetName)
        {
            if (HasMetaConfig(sheetName))
            {
                var metaId = GetDatabaseMetaId(sheetName);
                return repo.GetMeta(metaId);
            }

            var meta = repo[sheetName];
            if (meta != null && HasMetaConfig(meta.Id)) return null;
            return meta;
        }

        /// <summary>
        /// Resolve the field by sheet's header name 
        /// </summary>
        public BGField Map(BGMetaEntity meta, string headerName)
        {
            if (HasFieldConfig(meta.Id, headerName))
            {
                var fieldId = GetDatabaseFieldId(meta.Id, headerName);
                return meta.GetField(fieldId, false);
            }

            var field = meta.GetField(headerName, false);
            if (field != null && HasFieldConfig(meta.Id, field.Id)) return null;
            return field;
        }

        /// <summary>
        /// Clear the config
        /// </summary>
        public void Clear() => metas?.Clear();

        //===================================================================================================================
        //                                    Binary serialization
        //===================================================================================================================
        /// <inheritdoc/>
        public byte[] ConfigToBytes()
        {
            var writer = new BGBinaryWriter();

            //version
            writer.AddInt(1);

            writer.AddArray(() =>
            {
                foreach (var metaMap in metas)
                {
                    writer.AddString(metaMap.Id);
                    writer.AddString(metaMap.Name);
                    writer.AddArray(() =>
                    {
                        if (metaMap.Fields == null || metaMap.Fields.Count == 0) return;
                        foreach (var mapField in metaMap.Fields)
                        {
                            writer.AddString(mapField.Id);
                            writer.AddString(mapField.Name);
                        }
                    }, metaMap.Fields?.Count ?? 0);
                }
            }, metas.Count);

            return writer.ToArray();
        }

        /// <inheritdoc/>
        public void ConfigFromBytes(ArraySegment<byte> config)
        {
            if (config.Count < 8) return;

            metas.Clear();
            var reader = new BGBinaryReader(config);

            var version = reader.ReadInt();

            switch (version)
            {
                case 1:
                {
                    reader.ReadArray(() =>
                    {
                        var metaId = reader.ReadString();
                        var metaName = reader.ReadString();

                        var metaMap = new MetaMap(metaId) { Name = metaName };
                        metas.Add(metaMap);

                        reader.ReadArray(() =>
                        {
                            var fieldId = reader.ReadString();
                            var fieldName = reader.ReadString();

                            metaMap.Fields = metaMap.Fields ?? new List<NameMap>();
                            metaMap.Fields.Add(new NameMap(fieldId) { Name = fieldName });
                        });
                    });
                    break;
                }
                default:
                {
                    throw new Exception("Unsupported version " + version);
                }
            }
        }


        //===================================================================================================================
        //                                    helper classes
        //===================================================================================================================
        /// <summary>
        /// serializable ID=>name config
        /// </summary>
        [Serializable]
        public class NameMap
        {
            public string Id;
            public string Name;

            public NameMap(string id)
            {
                Id = id;
            }

            /// <summary>
            /// Has mapping config?
            /// </summary>
            public bool HasMapping => !string.IsNullOrEmpty(Name);

            public override string ToString() => $"{Id}/{Name}";
        }

        /// <summary>
        /// serializable names mapping config for single table
        /// </summary>
        [Serializable]
        public class MetaMap : NameMap
        {
            public List<NameMap> Fields;

            public MetaMap(string id) : base(id)
            {
            }

            /// <summary>
            /// number of fields in the configuration
            /// </summary>
            public int CountFields
            {
                get
                {
                    if (Fields == null) return 0;
                    var count = 0;
                    foreach (var field in Fields)
                        if (field.HasMapping)
                            count++;
                    return count;
                }
            }

            /// <summary>
            /// returns the mapped name for provided field
            /// </summary>
            public string GetFieldName(BGField field)
            {
                var fieldMap = GetFieldMap(field.Id.ToString());
                if (fieldMap != null && fieldMap.HasMapping) return fieldMap.Name;
                return field.Name;
            }

            /// <summary>
            /// returns the field's mapping
            /// </summary>
            public NameMap GetFieldMap(string fieldId)
            {
                if (Fields == null) return null;

                foreach (var nameMap in Fields)
                    if (string.Equals(nameMap.Id, fieldId))
                        return nameMap;

                return null;
            }

            /// <summary>
            /// ensures the filed mapping is added
            /// </summary>
            public NameMap EnsureFieldMap(string fieldId) => GetFieldMap(fieldId) ?? AddFieldMap(fieldId);

            /// <summary>
            /// add field config
            /// </summary>
            private NameMap AddFieldMap(string fieldId)
            {
                if (Fields == null) Fields = new List<NameMap>();
                var nameMap = new NameMap(fieldId);
                Fields.Add(nameMap);
                return nameMap;
            }

            /// <summary>
            /// Remove all mappings for non-existent tables/fields
            /// return true if has any mapping 
            /// </summary>
            public bool Trim(BGRepo repo)
            {
                var meta = repo.GetMeta(BGId.Parse(Id));
                if (meta == null) return false;
                var hasValue = HasMapping;
                if (Fields != null)
                    for (var i = Fields.Count - 1; i >= 0; i--)
                    {
                        var field = Fields[i];
                        hasValue = hasValue || field.HasMapping;
                        if (!field.HasMapping || !meta.HasField(BGId.Parse(field.Id))) Fields.RemoveAt(i);
                    }

                return hasValue;
            }

            /// <summary>
            /// Return field ID by mapped name
            /// </summary>
            public BGId GetFieldId(string fieldName)
            {
                if (Fields == null) return BGId.Empty;
                foreach (var field in Fields)
                    if (string.Equals(field.Name, fieldName))
                        return BGId.Parse(field.Id);

                return BGId.Empty;
            }

            /// <summary>
            /// If field has name mapping config
            /// </summary>
            public bool HasFieldMapping(BGId fieldId)
            {
                if (Fields == null) return false;
                var fieldIdStr = fieldId.ToString();
                foreach (var field in Fields)
                    if (string.Equals(field.Id, fieldIdStr))
                        return true;

                return false;
            }
        }


        /// <summary>
        /// Names mappings configuration owner
        /// </summary>
        public interface BGNameConfigOwner
        {
            /// <summary>
            /// Get/set names mapping config
            /// </summary>
            BGSyncNameMapConfig NameMapConfig { get; set; }

            /// <summary>
            /// Is names mapping config is enabled
            /// </summary>
            bool NameMapConfigEnabled { get; set; }
        }
    }
}