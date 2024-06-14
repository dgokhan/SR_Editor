/*
<copyright file="BGMergeSettingsEntity.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Settings for merging entities
    /// </summary>
    [Serializable]
    public partial class BGMergeSettingsEntity : BGMergeSettingsA, BGConfigurableBinaryI, ICloneable
    {
        //do not make readonly!
        [SerializeField] private HashtableId2MetaSettings id2Meta = new HashtableId2MetaSettings();

        [SerializeField] private string controllerType;

        // [NonSerialized] private object controller;

        /// <summary>
        /// row controller full type name 
        /// </summary>
        public string ControllerType
        {
            get => controllerType;
            set
            {
                if (string.Equals(controllerType, value, StringComparison.Ordinal)) return;
                controllerType = value;
                FireOnChange();
            }
        }

        /*
        /// <summary>
        /// for injecting row controller instance instead of type 
        /// </summary>
        public object Controller
        {
            get => controller;
            set
            {
                if (controller == value) return;
                controller = value;
                FireOnChange();
            }
        }
        */

        /// <summary>
        /// create new controller is controller type is defined 
        /// </summary>
        public object NewController(BGLogger logger)
        {
            if (string.IsNullOrEmpty(controllerType)) return null;

            try
            {
                var newController = BGUtil.Create<object>(controllerType, false);
                return newController;
            }
            catch (Exception e)
            {
                Debug.Log($"[WARNING!] BGDatabase: Controller object can not be created using {controllerType} type! See the next line for error details!");
                Debug.LogException(e);
                logger?.AppendLine("Controller Type is set up, however the object can not be created (the error is $). Skipping..", e.Message);
            }

            return null;
        }

        /// <summary>
        /// get settings for the table with provided ID 
        /// </summary>
        public MetaSettings GetSettings(BGId metaId) => BGUtil.Get(id2Meta, metaId);

        /// <summary>
        /// does settings have settings for the table with provided ID 
        /// </summary>
        public bool Has(BGId metaId) => id2Meta.ContainsKey(metaId);

        /// <summary>
        /// Remove settings for the table with provided ID
        /// </summary>
        public void Remove(BGId metaId)
        {
            if (!Has(metaId)) return;
            id2Meta.Remove(metaId);
            FireOnChange();
        }

        /// <summary>
        /// Does settings have any parameter for provided database? 
        /// </summary>
        public bool HasAny(BGRepo repo)
        {
            if (mode == BGMergeModeEnum.Transfer) return true;
            if (IncludedByDefault && repo.CountMeta > 0) return true;

            if (id2Meta == null) return false;

            foreach (var pair in id2Meta)
                if (repo.HasMeta(pair.Key))
                    return true;

            return false;
        }

        /// <summary>
        /// ensure table settings are added for the table with provided ID
        /// </summary>
        public MetaSettings Ensure(BGId metaId, bool copyFlags = false)
        {
            if (id2Meta.ContainsKey(metaId)) return id2Meta[metaId];
            var mergeMetaSettings = new MetaSettings();
            if (copyFlags)
            {
                mergeMetaSettings.AddMissing = addMissing;
                mergeMetaSettings.UpdateMatching = updateMatching;
                mergeMetaSettings.RemoveOrphaned = removeOrphaned;
            }

            mergeMetaSettings.OnChange += FireOnChange;

            id2Meta[metaId] = mergeMetaSettings;
            FireOnChange();
            return mergeMetaSettings;
        }

        /// <summary>
        /// is table included into settings? 
        /// </summary>
        public bool IsMetaIncluded(BGId metaId)
        {
            if (mode == BGMergeModeEnum.Transfer) return true;
            return Has(metaId) ? GetSettings(metaId).Included : IncludedByDefault;
        }

        /// <summary>
        /// is field included into settings? 
        /// </summary>
        public bool IsFieldIncluded(BGField field)
        {
            if (mode == BGMergeModeEnum.Transfer) return true;
            var metaId = field.MetaId;
            if (!Has(metaId)) return IncludedByDefault;

            var metaSettings = GetSettings(metaId);
            if (!metaSettings.Included) return false;
            return !metaSettings.UseIncludedFields || metaSettings.HasField(field.Id);
        }

        public object Clone()
        {
            var clone = new BGMergeSettingsEntity
            {
                Mode = Mode,
                addMissing = addMissing,
                updateMatching = updateMatching,
                removeOrphaned = removeOrphaned,
                controllerType = controllerType
            };
            foreach (var pair in id2Meta) clone.id2Meta[pair.Key] = (MetaSettings)pair.Value.Clone();
            return clone;
        }

        /// <summary>
        /// trim settings if provided database does not have tables/fields, remove unused settings 
        /// </summary>
        public void RemoveNotExistent(BGRepo repo, BGMergerEntity.ParseResultI parseResult)
        {
            repo.ForEachMeta(meta =>
            {
                var metaIncluded = IsMetaIncluded(meta.Id);
                if (!metaIncluded) return;

                if (!parseResult.HasEntitySheet(meta.Id))
                {
                    ExcludeMeta(meta.Id);
                    //no meta no pain
                    return;
                }

                meta.ForEachField(field =>
                {
                    var filedIncluded = IsFieldIncluded(field);
                    if (!filedIncluded) return;

                    if (!parseResult.HasFieldInEntitySheet(meta.Id, field.Id)) ExcludeField(field);
                });
            });
        }

        /// <summary>
        /// make sure to excluded provided field from settings 
        /// </summary>
        public void ExcludeField(BGField field)
        {
            var metaId = field.MetaId;
            if (!IsMetaIncluded(metaId)) return;

            var has = Has(metaId);
            MetaSettings settings = null;
            if (updateMatching && !has)
            {
                //if metas are included by default- we need to create custom settings to exclude particular field
                settings = Ensure(metaId, true);
                has = true;
            }

            //no settings no pain
            if (!has) return;

            if (settings == null) settings = GetSettings(metaId);

            //updating field has sense for UpdateMatching mode only
            if (!settings.UpdateMatching) return;

            if (settings.UseIncludedFields) settings.RemoveField(field.Id);
            else
            {
                settings.UseIncludedFields = true;
                //add all fields
                field.Meta.ForEachField(f => settings.AddField(f.Id));
                settings.RemoveField(field.Id);
            }

            FireOnChange();
        }

        /// <summary>
        /// make sure to excluded provided table  from settings 
        /// </summary>
        public void ExcludeMeta(BGId metaId)
        {
            if (IncludedByDefault) Ensure(metaId).Exclude();
            else Remove(metaId);
            FireOnChange();
        }

        /// <summary>
        /// Should new entities be added for provided table?  
        /// </summary>
        public bool IsAddingMissing(BGId metaId)
        {
            if (mode == BGMergeModeEnum.Transfer) return true;
            return Has(metaId) ? GetSettings(metaId).AddMissing : addMissing;
        }

        /// <summary>
        /// Should old entities be removed for provided table?  
        /// </summary>
        public bool IsRemovingOrphaned(BGId metaId)
        {
            if (mode == BGMergeModeEnum.Transfer) return true;
            return Has(metaId) ? GetSettings(metaId).RemoveOrphaned : removeOrphaned;
        }

        /// <summary>
        /// Should matching entities be updated for provided table?  
        /// </summary>
        public bool IsUpdatingMatching(BGId metaId)
        {
            if (mode == BGMergeModeEnum.Transfer) return true;
            return Has(metaId) ? GetSettings(metaId).UpdateMatching : updateMatching;
        }

        /// <summary>
        /// remove unused settings if provide database does not have table/fields
        /// </summary>
        public void ComplyTo(BGRepo repo)
        {
            //ensure all meta ids are actually exists in the database
            var metaIdList = new List<BGId>();

            foreach (var pair in id2Meta)
            {
                var metaId = pair.Key;
                if (!repo.HasMeta(metaId)) metaIdList.Add(metaId);
                else
                {
                    var meta = repo.GetMeta(metaId);
                    //ensure all fields exist
                    var pair1 = pair;
                    pair.Value.ForEachField(fieldId =>
                    {
                        if (!meta.HasField(fieldId)) pair1.Value.RemoveField(fieldId);
                    });
                }
            }

            foreach (var metaId in metaIdList) Remove(metaId);

            FireOnChange();
        }

        /// <summary>
        /// iterate over each setting
        /// </summary>
        public void ForEachSetting(Action<MetaSettings> action)
        {
            foreach (var pair in id2Meta) action(pair.Value);
        }

        /// <summary>
        /// construct new database, using provided database and this merge settings as a source 
        /// </summary>
        public BGRepo NewRepo(BGRepo repo, bool copyValues) => new BGRepo(repo, IsMetaIncluded, IsFieldIncluded, copyValues);

        /// <summary>
        /// construct new database, using provided database and this merge settings as a source and entity filter to filter out rows
        /// </summary>
        public BGRepo NewRepo(BGRepo repo, bool copyValues, Predicate<BGEntity> entityFilter) => new BGRepo(repo, IsMetaIncluded, IsFieldIncluded, copyValues, entityFilter);

        public Predicate<BGField> AddMissingFieldFilter { get; set; }

        /// <summary>
        /// Count how much metas included 
        /// </summary>
        public int CountIncluded(BGRepo repo)
        {
            var count = 0;
            repo.ForEachMeta(meta =>
            {
                if (IsMetaIncluded(meta.Id)) count++;
            });
            return count;
        }

        //================================================================================================
        //                                              Configuration
        //================================================================================================
        /// <inheritdoc/>
        public byte[] ConfigToBytes()
        {
            //its kind a hard to calculate exact number of bytes for this 
            var writer = new BGBinaryWriter(1024);
            //version
            writer.AddInt(2);

            //controller
            writer.AddString(controllerType);

            //fields
            writer.AddByte((byte)Mode);
            writer.AddBool(addMissing);
            writer.AddBool(updateMatching);
            writer.AddBool(removeOrphaned);

            writer.AddArray(() =>
            {
                foreach (var pair in id2Meta)
                {
                    writer.AddId(pair.Key);
                    writer.AddByteArray(pair.Value.ConfigToBytes());
                }
            }, id2Meta.Count);

            return writer.ToArray();
        }

        /// <inheritdoc/>
        public void ConfigFromBytes(ArraySegment<byte> config)
        {
            var reader = new BGBinaryReader(config);
            var version = reader.ReadInt();
            switch (version)
            {
                case 1:
                {
                    ReadFromBytes(reader);

                    break;
                }
                case 2:
                {
                    controllerType = reader.ReadString();
                    ReadFromBytes(reader);

                    break;
                }
                default:
                {
                    throw new BGException("Unknown version: $", version);
                }
            }
        }

        private void ReadFromBytes(BGBinaryReader reader)
        {
            mode = (BGMergeModeEnum)reader.ReadByte();
            addMissing = reader.ReadBool();
            updateMatching = reader.ReadBool();
            removeOrphaned = reader.ReadBool();

            ForEachSetting(settings => settings.OnChange -= FireOnChange);
            id2Meta.Clear();
            reader.ReadArray(() =>
            {
                var metaSetting = new MetaSettings();

                var id = reader.ReadId();
                metaSetting.ConfigFromBytes(reader.ReadByteArray());
                id2Meta[id] = metaSetting;

                metaSetting.OnChange += FireOnChange;
            });
        }

        //================================================================================================
        //                                              Custom settings for meta
        //================================================================================================

        /// <summary>
        /// merge settings for a single table
        /// </summary>
        [Serializable]
        public class MetaSettings : ICloneable, BGConfigurableBinaryI
        {
            public event Action OnChange;

            [SerializeField] private bool useIncludedFields;

            [SerializeField] private bool addMissing;
            [SerializeField] private bool updateMatching;
            [SerializeField] private bool removeOrphaned;
            [SerializeField] private BGIdList fields = new BGIdList();

            /// <summary>
            /// is included field settings on?
            /// </summary>
            public bool UseIncludedFields
            {
                get => useIncludedFields;
                set
                {
                    if (useIncludedFields == value) return;
                    useIncludedFields = value;
                    FireOnChange();
                }
            }

            /// <summary>
            /// Should new entities be added?
            /// </summary>
            public bool AddMissing
            {
                get => addMissing;
                set
                {
                    if (addMissing == value) return;
                    addMissing = value;
                    FireOnChange();
                }
            }

            /// <summary>
            /// Should matching entities be updated?
            /// </summary>
            public bool UpdateMatching
            {
                get => updateMatching;
                set
                {
                    if (updateMatching == value) return;
                    updateMatching = value;
                    FireOnChange();
                }
            }

            /// <summary>
            /// Should orphaned entities be removed?
            /// </summary>
            public bool RemoveOrphaned
            {
                get => removeOrphaned;
                set
                {
                    if (removeOrphaned == value) return;
                    removeOrphaned = value;
                    FireOnChange();
                }
            }

            /// <summary>
            /// Is included into settings?
            /// </summary>
            public bool Included => addMissing || updateMatching || removeOrphaned;

            /// <summary>
            /// number of included fields if UseIncludedFields==true
            /// </summary>
            public int CountFields => fields?.Count ?? 0;

            /// <summary>
            /// add the field to included fields list 
            /// </summary>
            public void AddField(BGId fieldId)
            {
                if (fields == null) fields = new BGIdList();
                if (fields.Contains(fieldId)) return;
                fields.Add(fieldId);
                FireOnChange();
            }

            /// <summary>
            /// remove the field from included fields list 
            /// </summary>
            public void RemoveField(BGId fieldId)
            {
                if (fields == null) return;
                if (!fields.Contains(fieldId)) return;
                fields.Remove(fieldId);
                FireOnChange();
            }

            /// <summary>
            /// get the field by it's index 
            /// </summary>
            public BGId GetField(int index)
            {
                if (fields == null) throw new BGException("Can not get field with index $ : fields are null", index);
                return fields[index];
            }

            public object Clone()
            {
                return new MetaSettings
                {
                    addMissing = addMissing,
                    updateMatching = updateMatching,
                    removeOrphaned = removeOrphaned,
                    useIncludedFields = useIncludedFields,
                    fields = new BGIdList(fields)
                };
            }

            /// <summary>
            /// copy parameters from provided settings object 
            /// </summary>
            public void CopyFrom(MetaSettings source)
            {
                if (source == null) return;
                addMissing = source.addMissing;
                updateMatching = source.updateMatching;
                removeOrphaned = source.removeOrphaned;
                useIncludedFields = source.useIncludedFields;
                fields = source.fields == null ? new BGIdList() : new BGIdList(source.fields);
            }

            /// <summary>
            /// does included fields have a field with provided ID 
            /// </summary>
            public bool HasField(BGId fieldId) => fields != null && fields.Contains(fieldId);

            /// <summary>
            /// exclude from merge settings 
            /// </summary>
            public void Exclude()
            {
                if (!Included) return;
                AddMissing = UpdateMatching = removeOrphaned = false;
                FireOnChange();
            }

            /// <summary>
            /// iterate over included fields
            /// </summary>
            public void ForEachField(Action<BGId> action)
            {
                if (fields == null) return;
                for (var i = fields.Count - 1; i >= 0; i--) action(fields[i]);
            }

            private void FireOnChange() => OnChange?.Invoke();

            //================================================================================================
            //                                              Configuration
            //================================================================================================
            /// <inheritdoc/>
            public byte[] ConfigToBytes()
            {
                var fieldsCount = fields?.Count ?? 0;
                var writer = new BGBinaryWriter(4 + 4 + fieldsCount * 16);
                //version
                writer.AddInt(1);

                //fields
                writer.AddBool(addMissing);
                writer.AddBool(updateMatching);
                writer.AddBool(removeOrphaned);
                writer.AddBool(useIncludedFields);

                writer.AddArray(() =>
                {
                    for (var i = 0; i < fieldsCount; i++) writer.AddId(fields[i]);
                }, fieldsCount);

                return writer.ToArray();
            }

            /// <inheritdoc/>
            public void ConfigFromBytes(ArraySegment<byte> config)
            {
                var reader = new BGBinaryReader(config);
                var version = reader.ReadInt();
                switch (version)
                {
                    case 1:
                    {
                        addMissing = reader.ReadBool();
                        updateMatching = reader.ReadBool();
                        removeOrphaned = reader.ReadBool();
                        useIncludedFields = reader.ReadBool();

                        if (fields == null) fields = new BGIdList();
                        fields.Clear();
                        reader.ReadArray(() =>
                        {
                            fields.Add(reader.ReadId());
                        });

                        break;
                    }
                    default:
                    {
                        throw new BGException("Unknown version: $", version);
                    }
                }
            }
        }

        [Serializable]
        public class HashtableId2MetaSettings : BGHashtableIdKey<MetaSettings>
        {
        }

        //================================================================================================
        //                                              Row level control
        //================================================================================================
        /// <summary>
        /// Implement this interface to be able to control if entity should be updated. Return true to cancel the update.
        /// If you cancel this update, no fields will be updated and IUpdateMatchingFieldReceiver receiver wont be called as well
        /// </summary>
        public interface IUpdateMatchingReceiver
        {
            /// <summary>
            /// This method is called before updating entity. Return true to cancel the update.
            /// </summary>
            bool OnBeforeUpdate(BGEntity from, BGEntity to);
        }

        /// <summary>
        /// Implement this interface to be able to control if some field should be updated.
        /// This interface is the same as IUpdateMatchingReceiver, but it's called for each field being updated
        /// Return true to cancel the update
        /// </summary>
        public interface IUpdateMatchingFieldReceiver
        {
            /// <summary>
            /// This method is called before updating entity's field. Return true to cancel the update.
            /// </summary>
            bool OnBeforeFieldUpdate(BGField fromField, BGField toField, BGEntity from, BGEntity to);
        }


        /// <summary>
        /// Implement this interface to be able to control if entity adding should take place. Return true to cancel the add
        /// </summary>
        public interface IAddMissingReceiver
        {
            /// <summary>
            /// This method is called before adding entity. Return true to cancel the add.
            /// </summary>
            bool OnBeforeAdd(BGEntity fromEntity);
        }

        /// <summary>
        /// Implement this interface to be able to control if entity deleting should take place. Return true to cancel the delete
        /// </summary>
        public interface IRemoveOrphanedReceiver
        {
            /// <summary>
            /// This method is called before removing entity. Return true to cancel the delete.
            /// </summary>
            bool OnBeforeDelete(BGEntity toEntity);
        }

        /// <summary>
        /// This interface will allow you to process OnBeforeMerge and OnAfterMerge events.  OnBeforeMerge allows you to cancel merging
        /// </summary>
        public interface IMergeReceiver
        {
            /// <summary>
            /// This method is called before merging. Return true only if you want to cancel the whole merging operation. 
            /// </summary>
            bool OnBeforeMerge(BGRepo from, BGRepo to);

            /// <summary>
            /// This method is called after merging 
            /// </summary>
            void OnAfterMerge(BGRepo from, BGRepo to);
        }

        /// <summary>
        /// Implement this interface to be able to control if entity should be included into data, saved by SaveLoad addon. Return true to cancel the saving
        /// </summary>
        public interface ISaveLoadAddonSavedEntityFilter
        {
            /// <summary>
            /// This method is called on SaveLoad addon Save method for each included row. Return true to cancel row inclusion
            /// </summary>
            bool OnSaveEntity(BGEntity entity);
        }
    }
}