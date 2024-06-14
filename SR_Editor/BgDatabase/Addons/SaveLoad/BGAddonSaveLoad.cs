/*
<copyright file="BGAddonSaveLoad.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Saving/Loading addon. Use this addon to save/load data during play session.
    /// See  <a href="http://www.bansheegz.com/BGDatabase/Addons/CodeGeneration/">this link</a> for more details.
    /// </summary>
    [AddonDescriptor(Name = "SaveLoad", ManagerType = "BansheeGz.BGDatabase.Editor.BGAddonManagerSaveLoad")]
    public partial class BGAddonSaveLoad : BGAddon
    {
        public const string DefaultSettingsName = "Default";

        private BGMergeSettingsEntity mergeSettings = new BGMergeSettingsEntity();
        private readonly Dictionary<string, BGMergeSettingsEntity> name2Setting = new Dictionary<string, BGMergeSettingsEntity>();

        private static readonly List<BeforeSaveReciever> saveReceivers = new List<BeforeSaveReciever>();
        private static readonly List<AfterLoadReciever> loadReceivers = new List<AfterLoadReciever>();
        
        /// <summary>
        /// Setting to use while saving/loading
        /// </summary>
        public BGMergeSettingsEntity MergeSettings => mergeSettings;

        /// <summary>
        /// additional settings
        /// </summary>
        public Dictionary<string, BGMergeSettingsEntity> Name2Setting => name2Setting;

        public BGAddonSaveLoad()
        {
            mergeSettings.OnChange += SettingsChanged;
        }

        //================================================================================================
        //                                              Configuration
        //================================================================================================

        /// <inheritdoc />
        public override string ConfigToString()
        {
            var settings = new Settings { MergeSettings = mergeSettings };
            foreach (var pair in name2Setting) settings.AdditionalSettings[pair.Key] = pair.Value;
            var configToString = JsonUtility.ToJson(settings);
            return configToString;
        }

        /// <inheritdoc />
        public override void ConfigFromString(string config)
        {
            var fromJson = JsonUtility.FromJson<Settings>(config);
            mergeSettings = fromJson.MergeSettings;
            mergeSettings.OnChange += SettingsChanged;

            ClearAdditionalSettings();
            foreach (var pair in fromJson.AdditionalSettings)
            {
                var additionalSettings = pair.Value;
                additionalSettings.OnChange += SettingsChanged;
                name2Setting[pair.Key] = additionalSettings;
            }
        }

        [Serializable]
        private class Settings
        {
            public BGMergeSettingsEntity MergeSettings = new BGMergeSettingsEntity();
            public BGHashtableForSerialization<string, BGMergeSettingsEntity> AdditionalSettings = new BGHashtableForSerialization<string, BGMergeSettingsEntity>();
        }

        /// <inheritdoc />
        public override byte[] ConfigToBytes()
        {
            var configToBytes = mergeSettings.ConfigToBytes();
            var writer = new BGBinaryWriter(4 + BGBinaryWriter.GetBytesCount(configToBytes));

            //version
            writer.AddInt(2);

            //default config
            writer.AddByteArray(configToBytes);

            //Version 2: additional configs
            writer.AddArray(() =>
            {
                foreach (var pair in name2Setting)
                {
                    writer.AddString(pair.Key);
                    writer.AddByteArray(pair.Value.ConfigToBytes());
                }
            }, name2Setting.Count);

            return writer.ToArray();
        }

        /// <inheritdoc />
        public override void ConfigFromBytes(ArraySegment<byte> config)
        {
            ClearAdditionalSettings();

            var reader = new BGBinaryReader(config);
            var version = reader.ReadInt();
            switch (version)
            {
                case 1:
                {
                    mergeSettings.ConfigFromBytes(reader.ReadByteArray());
                    break;
                }
                case 2:
                {
                    mergeSettings.ConfigFromBytes(reader.ReadByteArray());
                    reader.ReadArray(() =>
                    {
                        var settingName = reader.ReadString();
                        var additionalSettings = new BGMergeSettingsEntity();
                        additionalSettings.ConfigFromBytes(reader.ReadByteArray());
                        additionalSettings.OnChange += SettingsChanged;
                        name2Setting[settingName] = additionalSettings;
                    });
                    break;
                }
                default:
                {
                    throw new BGException("Unknown version: $", version);
                }
            }
        }

        //================================================================================================
        //                                              Methods
        //================================================================================================
        /// <inheritdoc />
        public override BGAddon CloneTo(BGRepo repo)
        {
            var clone = new BGAddonSaveLoad
            {
                Repo = repo,
                mergeSettings = (BGMergeSettingsEntity)mergeSettings.Clone()
            };
            clone.mergeSettings.OnChange += clone.SettingsChanged;

            foreach (var pair in name2Setting)
            {
                var additionalSettingClone = (BGMergeSettingsEntity)pair.Value.Clone();
                additionalSettingClone.OnChange += clone.SettingsChanged;
                clone.name2Setting[pair.Key] = additionalSettingClone;
            }

            return clone;
        }

        private void ClearAdditionalSettings()
        {
            foreach (var pair in name2Setting) pair.Value.OnChange -= SettingsChanged;
            name2Setting.Clear();
        }

        //fire settings change event
        private void SettingsChanged() => FireChange();

        //check if save-load data should be encrypted
        private void CheckEncryption(BGRepo saveRepo)
        {
            var settingsAddon = Repo.Addons.Get<BGAddonSettings>();
            if (settingsAddon == null) return;
            if (string.IsNullOrEmpty(settingsAddon.EncryptorType)) return;
            var encryptor = settingsAddon.Encryptor;
            if (encryptor == null) return;
            saveRepo.Addons.Add(new BGAddonSettings
            {
                EncryptorType = settingsAddon.EncryptorType,
                EncryptorConfig = settingsAddon.EncryptorConfig
            });
        }

        //================================================================================================
        //                                              Saving Loading
        //================================================================================================

        /// <summary>
        /// Serialize current database state as byte array, using default save/load merge setting
        /// </summary>
        public byte[] Save() => SaveInternal(new BGSaveLoadAddonSaveContext());

        /// <summary>
        /// Serialize current database state as byte array, using custom additional merge setting
        /// </summary>
        public byte[] Save(BGSaveLoadAddonSaveContext context) => SaveInternal(context);

        private byte[] SaveInternal(BGSaveLoadAddonSaveContext context)
        {
            BGMainThreadRunner.EnsureMainThread("SaveLoad add-on should be run on main thread");

            if (context == null) throw new Exception("Can not save, cause saveContext is null");
            if (string.IsNullOrEmpty(context.ConfigName)) throw new Exception("Can not save, cause saveContext.ConfigName is null or empty");

            BGMergeSettingsEntity settings;
            if (context.ConfigName == DefaultSettingsName) settings = mergeSettings;
            else if (!Name2Setting.TryGetValue(context.ConfigName, out settings)) throw new Exception($"Can not save, cause config with name {context.ConfigName} can not be found");

            if (context.FireBeforeSaveEvents)
            {
                //fire event
                foreach (var receiver in saveReceivers)
                {
                    try
                    {
                        receiver.OnBeforeSave();
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
               
                var receivers = BGInterfaceFinder.FindObjects<BeforeSaveReciever>(true);
                if (receivers != null)
                    for (var i = 0; i < receivers.Count; i++)
                        try
                        {
                            receivers[i].OnBeforeSave();
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }
            }


            if (context.MergeDataFromMTAddon)
            {
                //mt addon
                var mtAddon = Repo.Addons.Get<BGAddonMT>();
                if (mtAddon != null && mtAddon.MergeOnSave) mtAddon.Merge();
            }

            //save
            var controller = settings.NewController(null);

            BGRepo saveRepo;
            if (!(controller is BGMergeSettingsEntity.ISaveLoadAddonSavedEntityFilter saveController)) saveRepo = settings.NewRepo(Repo, true);
            else saveRepo = settings.NewRepo(Repo, true, entity => !saveController.OnSaveEntity(entity));

            CheckEncryption(saveRepo);
            return new BGRepoBinary().Write(saveRepo);
        }


        /// <summary>
        /// restore database state from byte array, using save/load merge setting
        /// </summary>
        public void Load(byte[] data) => LoadInternal(new BGSaveLoadAddonLoadContext(new BGSaveLoadAddonLoadContext.LoadRequest(DefaultSettingsName, data)));

        public void Load(BGSaveLoadAddonLoadContext loadContext) => LoadInternal(loadContext);

        private void LoadInternal(BGSaveLoadAddonLoadContext context)
        {
            BGMainThreadRunner.EnsureMainThread("SaveLoad add-on should be run on main thread");

            if (context == null) throw new Exception("Can not load, cause loadContext is null");
            if (context.Requests.Count == 0) throw new Exception("Can not load, cause load requests are empty");

            //load requests
            var toLoad = new List<Tuple<BGMergeSettingsEntity, byte[]>>();
            foreach (var request in context.Requests)
            {
                if (string.IsNullOrEmpty(request.ConfigName)) throw new Exception("Can not load, cause one of the requests has empty config name");
                Tuple<BGMergeSettingsEntity, byte[]> toAdd;
                if (request.ConfigName == DefaultSettingsName) toAdd = new Tuple<BGMergeSettingsEntity, byte[]>(mergeSettings, request.data);
                else if (!Name2Setting.TryGetValue(request.ConfigName, out var target) || target==null) throw new Exception($"Can not load, cause config with name {request.ConfigName} can not be found");
                else toAdd = new Tuple<BGMergeSettingsEntity, byte[]>(target, request.data);
                toLoad.Add(toAdd);
            }
            
            //preserve requests
            if (context.PreserveRequests != null && context.PreserveRequests.Count > 0)
            {
                foreach (var request in context.PreserveRequests)
                {
                    if (string.IsNullOrEmpty(request.ConfigName)) throw new Exception("Can not load, cause one of the reload requests has empty config name");
                    BGMergeSettingsEntity settings;
                    if (request.ConfigName == DefaultSettingsName) settings = mergeSettings;
                    else if (!Name2Setting.TryGetValue(request.ConfigName, out var target) || target==null) throw new Exception($"Can not load, cause config with name {request.ConfigName} can not be found");
                    else settings = target;
                    var data = SaveInternal(new BGSaveLoadAddonSaveContext(request.ConfigName));
                    toLoad.Add(new Tuple<BGMergeSettingsEntity, byte[]>(settings, data));
                }
            }

            events?.BeforeLoad();

            //load
            if (context.ReloadDatabase) BGRepo.Load();

            foreach (var (settings, data) in toLoad)
            {
                var loadedRepo = new BGRepoBinary().Read(data);
                new BGMergerEntity(null, loadedRepo, Repo, settings).Merge();
            }

            //fire events
            if (context.FireAfterLoadEvents)
            {
                foreach (var receiver in loadReceivers)
                {
                    try
                    {
                        receiver.OnAfterLoad();
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
                var receivers = BGInterfaceFinder.FindObjects<AfterLoadReciever>();
                if (receivers != null)
                    for (var i = 0; i < receivers.Count; i++)
                    {
                        try
                        {
                            receivers[i].OnAfterLoad();
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }
                    }
            }

            if (events != null && context.ReloadDatabase)
            {
                //renew event after database is reloaded
                var addonSaveLoad = Repo.Addons.Get<BGAddonSaveLoad>();
                addonSaveLoad.events = events;
                events.Addon = addonSaveLoad;
                events.AfterLoad();
                events = null;
            }
        }

        //================================================================================================
        //                                              Events
        //================================================================================================
        private BGSaveLoadEventsManager events;

        /// <summary>
        /// Add entity listener
        /// </summary>
        public void AddEntityListener(BGEntityPointer pointer, EventHandler<BGSaveLoadEventArgsEntityChanged> handler)
        {
            if (pointer == null) throw new Exception("entity pointer can not be null");
            events = events ?? new BGSaveLoadEventsManager(this);
            events.Add(pointer, handler);
        }

        /// <summary>
        /// Remove entity listener
        /// </summary>
        public void RemoveEntityListener(BGEntityPointer pointer, EventHandler<BGSaveLoadEventArgsEntityChanged> handler)
        {
            if (pointer == null) throw new Exception("entity pointer can not be null");
            if (events == null) return;
            events.Remove(pointer, handler);
        }

        /// <summary>
        /// Add cell listener
        /// </summary>
        public void AddCellListener(BGCellPointer pointer, EventHandler<BGSaveLoadEventArgsCellChanged> handler)
        {
            if (pointer == null) throw new Exception("cell pointer can not be null");
            events = events ?? new BGSaveLoadEventsManager(this);
            events.Add(pointer, handler);
        }

        /// <summary>
        /// Remove cell listener
        /// </summary>
        public void RemoveCellListener(BGCellPointer pointer, EventHandler<BGSaveLoadEventArgsCellChanged> handler)
        {
            if (pointer == null) throw new Exception("cell pointer can not be null");
            if (events == null) return;
            events.Remove(pointer, handler);
        }
        //================================================================================================
        //                                              static
        //================================================================================================
        public static void AddSaveReceiver(BeforeSaveReciever receiver) => saveReceivers.Add(receiver);
        public static void RemoveSaveReceiver(BeforeSaveReciever receiver) => saveReceivers.Remove(receiver);
        public static void AddLoadReceiver(AfterLoadReciever receiver) => loadReceivers.Add(receiver);
        public static void RemoveLoadReceiver(AfterLoadReciever receiver) => loadReceivers.Remove(receiver);
        //================================================================================================
        //                                              Interfaces
        //================================================================================================

        /// <summary>
        /// Implement this interface in your MonoBehaviour inherited class, if you want to receive messages before saving
        /// </summary>
        public interface BeforeSaveReciever
        {
            void OnBeforeSave();
        }

        /// <summary>
        /// Implement this interface in your MonoBehaviour inherited class, if you want to receive messages after loading
        /// </summary>
        public interface AfterLoadReciever
        {
            void OnAfterLoad();
        }
    }
}