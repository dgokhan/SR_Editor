/*
<copyright file="BGModdingRepoProtection.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Data container for data protection for all tables
    /// </summary>
    public class BGModdingRepoProtection
    {
        /// <summary>
        /// Data protection level. Inherited means that parent rule is used. High meaning the parent rule override  
        /// </summary>
        public enum FieldSettingEnum
        {
            Inherited,
            Enabled,
            EnabledHigh,
            Disabled,
            DisabledHigh
        }

        private readonly Dictionary<BGId, BGModdingMetaProtection> meta2Protection = new Dictionary<BGId, BGModdingMetaProtection>();

        private readonly BGRepo repo;

        public event Action Changed;

        public BGModdingRepoProtection(BGRepo repo)
        {
            this.repo = repo;
        }

        //================================================================================================
        //                                              Config
        //================================================================================================
        // Convert parameters to JSON serializable object 
        internal DataProtectionJson ConfigToJsonObject()
        {
            var result = new DataProtectionJson();
            foreach (var pair in meta2Protection)
            {
                var item = pair.Value;
                var metaConfig = new DataProtectionMetaJson
                {
                    MetaId = pair.Key.ToString(),
                    DisableAdd = item.addDisabled,
                    DisableDelete = item.deleteDisabled,
                    DisableEdit = item.editDisabled
                };

                ListToJson(item.Fields, metaConfig.FieldsEdit);

                ListToJson(item.RowsEdit, metaConfig.RowsEdit);
                ListToJson(item.RowsDelete, metaConfig.RowsDelete);

                DictionaryToJson(item.Cells, metaConfig.CellsEdit);

                result.list.Add(metaConfig);
            }

            return result;
        }

        //convert non-serializable dictionary to the list with serializable objects 
        private void ListToJson(Dictionary<BGId, bool> ids, List<DataProtectionObjectJson> jsonList)
        {
            foreach (var keys in ids) jsonList.Add(new DataProtectionObjectJson { Id = keys.Key.ToString(), Disabled = keys.Value });
        }

        //convert non-serializable dictionary to the list with serializable objects
        private void ListToJson(Dictionary<BGId, FieldSettingEnum> ids, List<DataProtectionFieldJson> jsonList)
        {
            foreach (var keys in ids) jsonList.Add(new DataProtectionFieldJson { Id = keys.Key.ToString(), Disabled = keys.Value });
        }

        //convert non-serializable dictionary to the list with serializable objects
        private void DictionaryToJson(Dictionary<BGId, Dictionary<BGId, bool>> dictionary, List<DataProtectionCellJson> targetList)
        {
            foreach (var fieldPair in dictionary)
            {
                if (fieldPair.Value.Count == 0) continue;

                var rowsIds = new List<DataProtectionObjectJson>(fieldPair.Value.Count);
                foreach (var pair in fieldPair.Value) rowsIds.Add(new DataProtectionObjectJson { Id = pair.Key.ToString(), Disabled = pair.Value });
                if (rowsIds.Count == 0) continue;
                targetList.Add(new DataProtectionCellJson
                {
                    Id = fieldPair.Key.ToString(),
                    RowsData = rowsIds
                });
            }
        }

        // Convert JSON object to internal attributes
        internal void ConfigJsonObject(DataProtectionJson config)
        {
            ClearMetas();
            if (config == null || config.list == null) return;


            foreach (var jsonMetaConfig in config.list)
            {
                if (!TryParse(jsonMetaConfig.MetaId, out var metaId)) continue;
                var meta = repo.GetMeta(metaId);
                if (meta == null) continue;
                var metaDataProtection = new BGModdingMetaProtection();
                Add(metaId, metaDataProtection);

                metaDataProtection.addDisabled = jsonMetaConfig.DisableAdd;
                metaDataProtection.editDisabled = jsonMetaConfig.DisableEdit;
                metaDataProtection.deleteDisabled = jsonMetaConfig.DisableDelete;

                ListFromJson(metaDataProtection.Fields, jsonMetaConfig.FieldsEdit);

                ListFromJson(metaDataProtection.RowsEdit, jsonMetaConfig.RowsEdit);
                ListFromJson(metaDataProtection.RowsDelete, jsonMetaConfig.RowsDelete);

                DictionaryFromJson(metaDataProtection.Cells, jsonMetaConfig.CellsEdit);
            }
        }

        //clears the list with individual table settings
        private void ClearMetas()
        {
            if (meta2Protection.Count > 0)
                foreach (var pair in meta2Protection)
                    pair.Value.Changed -= FireEvent;

            meta2Protection.Clear();
        }

        //convert the list with JSON data to target dictionary 
        private void ListFromJson(Dictionary<BGId, bool> targetDict, List<DataProtectionObjectJson> jsonData)
        {
            foreach (var fieldIdStr in jsonData)
            {
                if (!TryParse(fieldIdStr.Id, out var id)) continue;

                targetDict[id] = fieldIdStr.Disabled;
            }
        }

        //convert the list with JSON data to target dictionary
        private void ListFromJson(Dictionary<BGId, FieldSettingEnum> targetDict, List<DataProtectionFieldJson> jsonData)
        {
            foreach (var fieldIdStr in jsonData)
            {
                if (!TryParse(fieldIdStr.Id, out var id)) continue;

                targetDict[id] = fieldIdStr.Disabled;
            }
        }

        //convert the list with JSON data to target dictionary
        private void DictionaryFromJson(Dictionary<BGId, Dictionary<BGId, bool>> fieldId2RowId2Disabled, List<DataProtectionCellJson> jsonData)
        {
            foreach (var jsonCell in jsonData)
            {
                if (jsonCell.RowsData == null || jsonCell.RowsData.Count == 0) continue;
                if (!TryParse(jsonCell.Id, out var fieldId)) continue;

                foreach (var rowData in jsonCell.RowsData)
                {
                    if (!TryParse(rowData.Id, out var rowId)) continue;

                    var rowId2Disabled = EnsureDict(fieldId2RowId2Disabled, fieldId);
                    rowId2Disabled[rowId] = rowData.Disabled;
                }
            }
        }

        //ensures the dictionary for specified field is exists
        private Dictionary<BGId, bool> EnsureDict(Dictionary<BGId, Dictionary<BGId, bool>> fieldId2RowId2Disabled, BGId fieldId)
        {
            if (fieldId2RowId2Disabled.TryGetValue(fieldId, out var targetDict)) return targetDict;
            targetDict = new Dictionary<BGId, bool>();
            fieldId2RowId2Disabled.Add(fieldId, targetDict);
            FireEvent();
            return targetDict;
        }

        //can be replaced with BGId.TryParse ? 
        private bool TryParse(string idValue, out BGId id)
        {
            id = BGId.Parse(idValue);
            return !id.IsEmpty;
        }

        [Serializable]
        internal class DataProtectionJson
        {
            [SerializeField] internal List<DataProtectionMetaJson> list = new List<DataProtectionMetaJson>();
        }

        [Serializable]
        internal class DataProtectionMetaJson
        {
            public string MetaId;
            public bool DisableAdd;
            public bool DisableDelete;
            public bool DisableEdit;

            //field overrides
            public readonly List<DataProtectionFieldJson> FieldsEdit = new List<DataProtectionFieldJson>();

            //rows overrides
            public readonly List<DataProtectionObjectJson> RowsEdit = new List<DataProtectionObjectJson>();
            public readonly List<DataProtectionObjectJson> RowsDelete = new List<DataProtectionObjectJson>();

            //cell overrides
            public readonly List<DataProtectionCellJson> CellsEdit = new List<DataProtectionCellJson>();
        }

        [Serializable]
        internal class DataProtectionObjectJson
        {
            public string Id;
            public bool Disabled;
        }

        [Serializable]
        internal class DataProtectionFieldJson
        {
            public string Id;
            public FieldSettingEnum Disabled;
        }

        [Serializable]
        internal class DataProtectionCellJson
        {
            public string Id;
            public List<DataProtectionObjectJson> RowsData = new List<DataProtectionObjectJson>();
        }

        /// <summary>
        /// stores attributes in binary array  
        /// </summary>
        public void ConfigToBytes(BGBinaryWriter writer, int version)
        {
            switch (version)
            {
                case 2:
                {
                    writer.AddArray(() =>
                    {
                        foreach (var pair in meta2Protection)
                        {
                            writer.AddId(pair.Key);
                            var metaConfig = pair.Value;
                            writer.AddBool(metaConfig.addDisabled);
                            writer.AddBool(metaConfig.deleteDisabled);
                            writer.AddBool(metaConfig.editDisabled);

                            AddArray(writer, metaConfig.Fields);

                            AddArray(writer, metaConfig.RowsEdit);
                            AddArray(writer, metaConfig.RowsDelete);

                            AddDict(writer, metaConfig.Cells);
                        }
                    }, meta2Protection.Count);
                    break;
                }
                default:
                {
                    throw new BGException("unsupported version: $", version);
                }
            }
        }

        //add dictionary data to binary writer 
        private void AddDict(BGBinaryWriter writer, Dictionary<BGId, Dictionary<BGId, bool>> fieldId2RowsIds)
        {
            writer.AddArray(() =>
            {
                foreach (var pair in fieldId2RowsIds)
                {
                    writer.AddId(pair.Key);
                    var rowIds = pair.Value;
                    writer.AddArray(() =>
                    {
                        foreach (var pair2 in rowIds)
                        {
                            writer.AddId(pair2.Key);
                            writer.AddBool(pair2.Value);
                        }
                    }, rowIds.Count);
                }
            }, fieldId2RowsIds.Count);
        }

        //add dictionary data to binary writer
        private void AddArray(BGBinaryWriter writer, Dictionary<BGId, bool> collection)
        {
            writer.AddArray(() =>
            {
                foreach (var pair in collection)
                {
                    writer.AddId(pair.Key);
                    writer.AddBool(pair.Value);
                }
            }, collection.Count);
        }

        //add dictionary data to binary writer
        private void AddArray(BGBinaryWriter writer, Dictionary<BGId, FieldSettingEnum> collection)
        {
            writer.AddArray(() =>
            {
                foreach (var pair in collection)
                {
                    writer.AddId(pair.Key);
                    writer.AddInt((int)pair.Value);
                }
            }, collection.Count);
        }

        /// <summary>
        /// Restore internal state from binary array 
        /// </summary>
        public void ConfigFromBytes(BGBinaryReader reader, int version)
        {
            switch (version)
            {
                case 2:
                {
                    ClearMetas();

                    reader.ReadArray(() =>
                    {
                        var metaConfig = new BGModdingMetaProtection();
                        var metaId = reader.ReadId();
                        Add(metaId, metaConfig);

                        metaConfig.addDisabled = reader.ReadBool();
                        metaConfig.deleteDisabled = reader.ReadBool();
                        metaConfig.editDisabled = reader.ReadBool();

                        ReadArray(reader, metaConfig.Fields);

                        ReadArray(reader, metaConfig.RowsEdit);
                        ReadArray(reader, metaConfig.RowsDelete);

                        ReadDict(reader, metaConfig.Cells);
                    });
                    break;
                }

                default:
                {
                    throw new BGException("Unknown version: $", version);
                }
            }
        }

        //adds table config
        private void Add(BGId metaId, BGModdingMetaProtection moddingMetaConfig)
        {
            meta2Protection[metaId] = moddingMetaConfig;
            moddingMetaConfig.Changed += FireEvent;
        }

        //reads dictionary from binary array
        private void ReadDict(BGBinaryReader reader, Dictionary<BGId, Dictionary<BGId, bool>> targetDict)
        {
            reader.ReadArray(() =>
            {
                var fieldId = reader.ReadId();
                if (!targetDict.TryGetValue(fieldId, out var rowId2Disabled))
                {
                    rowId2Disabled = new Dictionary<BGId, bool>();
                    targetDict[fieldId] = rowId2Disabled;
                }

                reader.ReadArray(() =>
                {
                    var id = reader.ReadId();
                    rowId2Disabled[id] = reader.ReadBool();
                });
            });
        }

        //reads dictionary from binary array
        private void ReadArray(BGBinaryReader reader, Dictionary<BGId, bool> dict)
        {
            reader.ReadArray(() =>
            {
                var id = reader.ReadId();
                dict[id] = reader.ReadBool();
            });
        }

        //reads dictionary from binary array
        private void ReadArray(BGBinaryReader reader, Dictionary<BGId, FieldSettingEnum> dict)
        {
            reader.ReadArray(() =>
            {
                var id = reader.ReadId();
                dict[id] = (FieldSettingEnum)reader.ReadInt();
            });
        }

        //================================================================================================
        //                                              Methods
        //================================================================================================

        /// <summary>
        /// Create a clone of current object 
        /// </summary>
        public BGModdingRepoProtection CloneTo(BGRepo toRepo)
        {
            var clone = new BGModdingRepoProtection(toRepo);
            foreach (var pair in meta2Protection) clone.Add(pair.Key, pair.Value.Clone());
            return clone;
        }

        /// <summary>
        /// Is there any data for specified table
        /// </summary>
        public bool Has(BGId metaId)
        {
            return meta2Protection.ContainsKey(metaId);
        }

        /// <summary>
        /// Get protection data for specified table
        /// </summary>
        public BGModdingMetaProtection Get(BGId metaId)
        {
            return BGUtil.Get(meta2Protection, metaId);
        }

        /// <summary>
        /// Ensure the protection data exists for specified table
        /// </summary>
        public BGModdingMetaProtection Ensure(BGId metaId)
        {
            var dataProtection = Get(metaId);
            if (dataProtection != null) return dataProtection;

            dataProtection = new BGModdingMetaProtection();
            Add(metaId, dataProtection);
            FireEvent();
            return dataProtection;
        }

        /// <summary>
        /// Remove protection data for specified table
        /// </summary>
        public bool Remove(BGId metaId)
        {
            if (!meta2Protection.TryGetValue(metaId, out var protection)) return false;
            protection.Changed -= FireEvent;
            meta2Protection.Remove(metaId);
            FireEvent();
            return true;
        }

        //fire protection data changes event
        private void FireEvent()
        {
            Changed?.Invoke();
        }

        //================================================================================================
        //                                              Nested classes
        //================================================================================================

        /// <summary>
        /// Removes non-existent tables/fields data
        /// </summary>
        public void Trim()
        {
            //remove object, which has been deleted
            var toRemoveMetas = new HashSet<BGId>();
            foreach (var pair in meta2Protection)
            {
                var metaId = pair.Key;
                var metaSettings = pair.Value;
                var meta = repo.GetMeta(metaId);
                if (meta == null) toRemoveMetas.Add(metaId);
                else
                {
                    RemoveIf(metaSettings.fields, (id, v) => !meta.HasField(id) || v == FieldSettingEnum.Inherited, id => metaSettings.SetFieldEdit(id, FieldSettingEnum.Inherited));
                    RemoveIf(metaSettings.rowsEdit, (id, v) => !meta.HasEntity(id), id => metaSettings.RemoveRowsEdit(id));
                    RemoveIf(metaSettings.rowsDelete, (id, v) => !meta.HasEntity(id), id => metaSettings.RemoveRowsDelete(id));

                    var toRemoveFields = new HashSet<BGId>();
                    foreach (var cellsPair in metaSettings.cells)
                    {
                        var fieldId = cellsPair.Key;
                        if (!meta.HasField(fieldId)) toRemoveFields.Add(fieldId);
                        else
                        {
                            var toRemoveEntities = new HashSet<BGId>();
                            foreach (var pair2 in cellsPair.Value)
                                if (!meta.HasEntity(pair2.Key))
                                    toRemoveEntities.Add(pair2.Key);

                            foreach (var entityId in toRemoveEntities) cellsPair.Value.Remove(entityId);
                        }
                    }

                    foreach (var id in toRemoveFields) metaSettings.RemoveCellField(id);
                }
            }

            foreach (var metaId in toRemoveMetas) Remove(metaId);
        }

        //removes data using callbacks actions
        private static void RemoveIf<T>(Dictionary<BGId, T> id2value, Func<BGId, T, bool> toRemovePredicate, Action<BGId> remove)
        {
            var toRemove = new HashSet<BGId>();
            foreach (var pair in id2value)
            {
                var id = pair.Key;
                if (toRemovePredicate(id, pair.Value)) toRemove.Add(id);
            }

            foreach (var id in toRemove) remove(id);
        }

        /// <summary>
        /// Removes data protection for specified cell 
        /// </summary>
        public bool Remove(BGId metaId, BGId fieldId, BGId entityId)
        {
            if (!meta2Protection.TryGetValue(metaId, out var settings)) return false;

            var removed = false;
            if (settings.cells.TryGetValue(fieldId, out var row2Disabled)) removed = row2Disabled.Remove(entityId);

            if (removed) FireEvent();
            return removed;
        }

        /// <summary>
        /// Add disabled state for specified cell 
        /// </summary>
        public void AddDisabled(BGId metaId, BGId fieldId, BGId entityId)
        {
            AddDisabled(metaId, fieldId, entityId, true);
        }

        /// <summary>
        /// Add enabled state for specified cell 
        /// </summary>
        public void AddEnabled(BGId metaId, BGId fieldId, BGId entityId)
        {
            AddDisabled(metaId, fieldId, entityId, false);
        }

        //modify enabled/disabled state for specified cell
        private void AddDisabled(BGId metaId, BGId fieldId, BGId entityId, bool disabled)
        {
            var metaSetting = Ensure(metaId);
            var row2Disabled = EnsureDict(metaSetting.cells, fieldId);
            if (row2Disabled.TryGetValue(entityId, out var value) && value == disabled) return;

            row2Disabled[entityId] = disabled;
            FireEvent();
        }

        /// <summary>
        /// Get protection state for specified cell
        /// </summary>
        public bool? Get(BGId metaId, BGId fieldId, BGId entityId)
        {
            var settings = Get(metaId);
            return settings?.Get(fieldId, entityId);
        }

        /// <summary>
        /// Get 'delete' protection state for specified row
        /// </summary>
        public bool? GetRowDelete(BGId metaId, BGId entityId)
        {
            var settings = Get(metaId);
            return settings?.GetRowDelete(entityId);
        }

        /// <summary>
        /// Get 'edit' protection state for specified row
        /// </summary>
        public bool? GetRowEdit(BGId metaId, BGId entityId)
        {
            var settings = Get(metaId);
            return settings?.GetRowEdit(entityId);
        }

        /// <summary>
        /// Add 'delete' protection state for specified row
        /// </summary>
        public bool AddRowDelete(BGId metaId, BGId entityId, bool disabled)
        {
            var metaSetting = Ensure(metaId);
            return metaSetting.AddRowDeleteDisabled(entityId, disabled);
        }

        /// <summary>
        /// Add 'edit' protection state for specified row
        /// </summary>
        public bool AddRowEdit(BGId metaId, BGId entityId, bool disabled)
        {
            var metaSetting = Ensure(metaId);
            return metaSetting.AddRowEditDisabled(entityId, disabled);
        }

        /// <summary>
        /// Remove 'edit' protection state for specified row
        /// </summary>
        public bool RemoveRowEdit(BGId metaId, BGId entityId)
        {
            var metaSetting = Get(metaId);
            if (metaSetting == null) return false;
            return metaSetting.RemoveRowEdit(entityId);
        }

        /// <summary>
        /// Remove 'delete' protection state for specified row
        /// </summary>
        public bool RemoveRowDelete(BGId metaId, BGId entityId)
        {
            var metaSetting = Get(metaId);
            if (metaSetting == null) return false;
            return metaSetting.RemoveRowDelete(entityId);
        }

        /// <summary>
        /// Get 'edit' protection state for specified field
        /// </summary>
        public FieldSettingEnum GetFieldEdit(BGId metaId, BGId fieldId)
        {
            var settings = Get(metaId);
            if (settings == null) return FieldSettingEnum.Inherited;
            return settings.GetFieldEdit(fieldId);
        }

        /// <summary>
        /// Is 'adding' disabled for specified table
        /// </summary>
        public bool IsAddDisabled(BGId metaId)
        {
            var settings = Get(metaId);
            if (settings == null) return false;
            return settings.addDisabled;
        }

        /// <summary>
        /// Is 'deleting' disabled for specified row
        /// </summary>
        public bool IsDeleteDisabled(BGId metaId, BGId entityId)
        {
            var settings = Get(metaId);
            if (settings == null) return false;
            var rowSetting = settings.GetRowDelete(entityId);
            if (rowSetting == null) return settings.DeleteDisabled;
            return rowSetting.Value;
        }

        /// <summary>
        /// Is 'edit' disabled for specified cell
        /// </summary>
        public bool IsEditDisabled(BGId metaId, BGId fieldId, BGId entityId)
        {
            //no meta setting-> true
            if (!meta2Protection.TryGetValue(metaId, out var moddingMetaSettings)) return false;

            //cell
            bool? cellSetting = null;
            if (moddingMetaSettings.cells.TryGetValue(fieldId, out var row2Disabled))
            {
                if (row2Disabled.TryGetValue(entityId, out var disabled)) cellSetting = disabled;
            }

            if (cellSetting != null) return cellSetting.Value;

            //field
            if (moddingMetaSettings.fields.TryGetValue(fieldId, out var fieldSetting))
                switch (fieldSetting)
                {
                    case FieldSettingEnum.EnabledHigh:
                    {
                        return false;
                    }
                    case FieldSettingEnum.DisabledHigh:
                    {
                        return true;
                    }
                }

            //row
            if (moddingMetaSettings.rowsEdit.TryGetValue(entityId, out var rowDisabled)) return rowDisabled;

            switch (fieldSetting)
            {
                case FieldSettingEnum.Inherited:
                    return moddingMetaSettings.EditDisabled;
                case FieldSettingEnum.Enabled:
                    return false;
                case FieldSettingEnum.Disabled:
                    return true;
            }

            //this should never happen
            throw new Exception("unexpected error at the end of IsEditDisabled");
        }
    }
}