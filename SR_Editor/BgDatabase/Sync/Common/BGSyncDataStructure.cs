/*
<copyright file="BGSyncDataStructure.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// data container for database structure information (used by data structure extraction wizard)
    /// </summary>
    public class BGSyncDataStructure
    {
        /// <summary>
        /// Which meta/fields are disabled
        /// </summary>
        public BGSyncDisabledConfig DisabledConfig
        {
            set
            {
                if (value == null)
                {
                    foreach (var metaData in metas) metaData.DisabledConfig = null;
                }
                else
                {
                    foreach (var metaData in metas)
                    {
                        if (value.HasTable(metaData.metaName)) metaData.disabled = true;
                        metaData.DisabledConfig = value;
                    }
                }
            }
        }

        /// <summary>
        /// information about individual tables  
        /// </summary>
        public readonly List<BGSyncMetaData> metas = new List<BGSyncMetaData>();

        /// <summary>
        /// number of errors
        /// </summary>
        public int ErrorsCount
        {
            get
            {
                var count = 0;
                foreach (var meta in metas)
                {
                    if (meta.disabled) continue;
                    if (meta.Error != null) count++;
                    foreach (var field in meta.fields)
                        if (field.Error != null)
                            count++;
                }

                return count;
            }
        }

        private BGSyncNameMapConfig namesConfig;

        /// <summary>
        /// Names mapping configuration
        /// </summary>
        public BGSyncNameMapConfig NameConfig
        {
            get
            {
                var result = namesConfig;
                foreach (var metaData in metas)
                {
                    if (metaData.disabled) continue;
                    if (!string.Equals(metaData.metaName, metaData.sheetName))
                    {
                        var metaMap = EnsureMetaMap(ref result, metaData.metaName);
                        if (metaMap == null) continue;
                        metaMap.Name = metaData.sheetName;
                    }

                    foreach (var fieldData in metaData.fields)
                    {
                        if (fieldData.disabled) continue;
                        if (!string.Equals(fieldData.fieldName, fieldData.headerName))
                        {
                            var metaMap = EnsureMetaMap(ref result, metaData.metaName);
                            if (metaMap == null) continue;
                            var field = BGRepo.I.GetMeta(metaData.metaName).GetField(fieldData.fieldName, false);
                            if (field == null) continue;
                            metaMap.EnsureFieldMap(field.Id.ToString()).Name = fieldData.headerName;
                        }
                    }
                }

                return result;
            }
        }

        public void SetNamesConfig(BGSyncNameMapConfig namesConfig, BGRepo repo)
        {
            this.namesConfig = namesConfig;
            if (this.namesConfig == null) return;

            foreach (var metaData in metas)
            {
                var metaInfo = this.namesConfig.GetMetaMapByName(metaData.sheetName);
                if (metaInfo == null)
                {
                    if (metaData.metaName != null)
                    {
                        var m = repo.GetMeta(metaData.metaName);
                        if (m != null) metaInfo = this.namesConfig.GetMetaMap(m.Id.ToString());
                    }
                }
                if (metaInfo == null) continue;
                if (!BGId.TryParse(metaInfo.Id, out var metaId)) continue;
                var meta = repo.GetMeta(metaId);
                if (meta == null) continue;
                metaData.metaName = meta.Name;
                var fieldsNamesConfig = metaInfo.Fields;
                if (BGUtil.IsEmpty(fieldsNamesConfig)) continue;
                foreach (var fieldMap in fieldsNamesConfig)
                {
                    if (!fieldMap.HasMapping) continue;
                    if (!BGId.TryParse(fieldMap.Id, out var fieldId)) continue;
                    var field = meta.GetField(fieldId, false);
                    if (field == null) continue;
                    var fieldData = metaData.GetFieldByHeaderName(fieldMap.Name);
                    if (fieldData != null) fieldData.fieldName = field.Name;
                }
            }
        }

        /// <summary>
        /// Get merge settings for provided repo
        /// </summary>
        public BGMergeSettingsEntity GetSetting(BGRepo repo)
        {
            var settings = new BGMergeSettingsEntity();
            foreach (var metaData in metas)
            {
                if (metaData.disabled) continue;
                var meta = repo.GetMeta(metaData.metaName);
                if (meta == null) continue;
                var metaSetting = settings.Ensure(meta.Id);
                metaSetting.AddMissing = true;
                metaSetting.UpdateMatching = true;
                metaSetting.UseIncludedFields = true;
                foreach (var fieldData in metaData.fields)
                {
                    if (fieldData.disabled) continue;
                    var field = meta.GetField(fieldData.fieldName, false);
                    if (field == null) continue;
                    metaSetting.AddField(field.Id);
                }
            }

            return settings;
        }

        //ensures the table config is added
        private static BGSyncNameMapConfig.MetaMap EnsureMetaMap(ref BGSyncNameMapConfig result, string metaName)
        {
            var meta = BGRepo.I.GetMeta(metaName);
            if (meta == null) return null;
            if (result == null) result = new BGSyncNameMapConfig();
            return result.EnsureMetaMap(meta.Id.ToString());
        }

        /// <summary>
        /// Data container for individual table
        /// </summary>
        public class BGSyncMetaData
        {
            private readonly BGSyncDataStructure structure;
            public readonly List<BGSyncFieldData> fields = new List<BGSyncFieldData>();
            public readonly string sheetName;
            public string metaName;
            public bool disabled;
            public int idColumn = -1;

            public BGSyncDataStructure Structure => structure;

            private BGSyncDisabledConfig disabledConfig;

            /// <summary>
            /// Error message if any
            /// </summary>
            public string Error
            {
                get
                {
                    if (string.IsNullOrEmpty(metaName)) return "Meta name not set";
                    var checkName = BGMetaObject.CheckName(metaName);
                    if (checkName != null) return checkName;
                    foreach (var meta in structure.metas)
                    {
                        if (meta == this) continue;
                        if (string.Equals(meta.metaName, metaName)) return "Duplicate name: " + metaName + ". Name must be unique";
                    }

                    return null;
                }
            }

            /// <summary>
            /// Error message if any
            /// </summary>
            public string ErrorIncludingFields
            {
                get
                {
                    var error = Error;
                    if (error != null) return error;
                    foreach (var field in fields)
                    {
                        if (!string.IsNullOrEmpty(field.Error))
                        {
                            var fieldName = string.Equals(field.headerName, field.fieldName) ? field.headerName : field.headerName + " (" + field.fieldName + ")";
                            return $"[{fieldName}]: {field.Error}";
                        }
                    }

                    return null;
                }
            }

            /// <summary>
            /// information about disabled config
            /// </summary>
            public BGSyncDisabledConfig DisabledConfig
            {
                get => disabledConfig;
                set
                {
                    disabledConfig = value;
                    if (value == null)
                    {
                        foreach (var fieldData in fields) fieldData.DisabledConfig = null;
                    }
                    else
                    {
                        foreach (var fieldData in fields) fieldData.DisabledConfig = value;
                    }
                }
            }

            public BGSyncMetaData(BGSyncDataStructure structure, string sheetName)
            {
                this.structure = structure;
                this.sheetName = metaName = sheetName;
            }

            /// <summary>
            /// Mark table as disabled 
            /// </summary>
            public void SetDisabled(bool disabled)
            {
                this.disabled = disabled;
                disabledConfig?.SetDisabled(sheetName, disabled);
            }

            public BGSyncFieldData GetFieldByHeaderName(string headerName)
            {
                foreach (var field in fields)
                {
                    if (field.headerName == headerName) return field;
                }

                return null;
            }

            public override string ToString() => metaName==sheetName ? sheetName : $"{sheetName} ({metaName})";
        }

        /// <summary>
        /// Data container for one single field 
        /// </summary>
        public class BGSyncFieldData : BGObjectI
        {
            private readonly BGSyncMetaData meta;
            public string headerName;
            public string fieldName;
            public Type fieldType;
            public bool disabled;
            private readonly BGId id;

            public BGId Id => id;

            public BGSyncMetaData Meta => meta;
            private BGSyncDisabledConfig disabledConfig;

            public Action addHandlerTest;
            public object addHandler;

            /// <summary>
            /// error message if any
            /// </summary>
            public string Error
            {
                get
                {
                    if (disabled) return null;
                    if (fieldType == null) return "Field type not set";
                    if (!typeof(BGField).IsAssignableFrom(fieldType)) return "Field type is not assignable to BGField";
                    if (string.IsNullOrEmpty(fieldName)) return "Field name not set";
                    var checkName = BGMetaObject.CheckName(fieldName);
                    if (checkName != null) return checkName;
                    foreach (var f in meta.fields)
                    {
                        if (f == this) continue;
                        if (string.Equals(f.fieldName, fieldName)) return "Duplicate name: " + fieldName + ". Name must be unique";
                    }

                    if (addHandlerTest != null)
                    {
                        try
                        {
                            addHandlerTest();
                        }
                        catch (Exception e)
                        {
                            return "Error: " + e.Message;
                        }
                    }

                    return null;
                }
            }

            public BGSyncDisabledConfig DisabledConfig
            {
                get => disabledConfig;
                set
                {
                    disabledConfig = value;
                    if (value != null)
                    {
                        if (value.HasField(meta.sheetName, fieldName)) disabled = true;
                    }
                }
            }

            public BGSyncFieldData(BGSyncMetaData meta, string headerName)
            {
                id = BGId.NewId;
                this.meta = meta;
                this.headerName = fieldName = headerName;
            }

            /// <summary>
            /// Set table as disabled
            /// </summary>
            public void SetDisabled(bool disabled)
            {
                this.disabled = disabled;
                disabledConfig?.SetDisabled(meta.sheetName, fieldName, disabled);
            }
            
            public override string ToString() => headerName==fieldName ? headerName : $"{headerName} ({fieldName})";

        }
    }
}