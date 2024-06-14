/*
<copyright file="BGAddonLiveUpdate.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Net;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Live update addon. If enabled, it tries to update repo data each time repo is loaded with data from Google sheet's spreadsheet
    /// See  <a href="http://www.bansheegz.com/BGDatabase/Addons/LiveUpdate/">this link</a> for more details.
    /// </summary>
    [AddonDescriptor(Name = "LiveUpdate", ManagerType = "BansheeGz.BGDatabase.Editor.BGAddonManagerLiveUpdate")]
    public partial class BGAddonLiveUpdate : BGAddon
    {
        //the data format types
        public enum DataSourceTypeEnum
        {
            GoogleSheetsAPI,
            VisualizationAPI,
            ExcelExport
        }

        //min max and default timeouts for web client
        public const float MinTimeout = 1;
        public const float MaxTimeout = 30;
        private const float DefaultTimeout = 5;

        //the flag to indicate that loading is disabled
        public static bool SuppressLoading { get; set; }

        //================================================================================================
        //                                              Fields
        //================================================================================================

        public event Action OnLoadComplete;


        private BGMergeSettingsEntity mergeSettings = new BGMergeSettingsEntity();

        private BGLiveUpdateSourceTypeEnum sourceType;
        private BGLiveUpdateUrls urls;

        /// <summary>
        /// Merge setting for merging of Google sheets data
        /// </summary>
        public BGMergeSettingsEntity MergeSettings => mergeSettings;

        private string spreadsheetId;

        /// <summary>
        /// Spreadsheet id to use
        /// </summary>
        public string SpreadsheetId
        {
            get => spreadsheetId;
            set
            {
                if (string.Equals(spreadsheetId, value)) return;
                spreadsheetId = value;
                FireChange();
            }
        }

        private string apiKey;

        /// <summary>
        /// API key to use
        /// </summary>
        public string ApiKey
        {
            get => apiKey;
            set
            {
                if (string.Equals(apiKey, value)) return;
                apiKey = value;
                FireChange();
            }
        }

        private bool manualLoad;

        public bool ManualLoad
        {
            get => manualLoad;
            set
            {
                if (manualLoad == value) return;
                manualLoad = value;
                FireChange();
            }
        }

        private float timeout = DefaultTimeout;

        public float Timeout
        {
            get => timeout;
            set
            {
                if (Math.Abs(timeout - value) < 0.001) return;
                if (value < MinTimeout || value > MaxTimeout) return;
                timeout = value;
                FireChange();
            }
        }

        private bool inBuildOnly;

        public bool InBuildOnly
        {
            get => inBuildOnly;
            set
            {
                if (inBuildOnly == value) return;
                inBuildOnly = value;
                FireChange();
            }
        }

        private BGLiveUpdateLog.LogLevelEnum logLevel;

        public BGLiveUpdateLog.LogLevelEnum LogLevel
        {
            get => logLevel;
            set
            {
                if (logLevel == value) return;
                logLevel = value;
                FireChange();
            }
        }

        private bool printLogOnLoad;

        public bool PrintLogOnLoad
        {
            get => printLogOnLoad;
            set
            {
                if (printLogOnLoad == value) return;
                printLogOnLoad = value;
                FireChange();
            }
        }

        public BGLiveUpdateSourceTypeEnum SourceType
        {
            get => sourceType;
            set
            {
                if (sourceType == value) return;
                sourceType = value;
                FireChange();
            }
        }


        private BGLiveUpdateLog log;

        public BGLiveUpdateLog Log
        {
            get
            {
                if (log != null) return log;
                log = new BGLiveUpdateLog(logLevel);
                return log;
            }
        }


        private bool IsPlaying => Application.isPlaying || BGUtil.TestIsRunning;

        public bool IsLoadingOnStartInEditor => !ManualLoad && !InBuildOnly;

        private BGLiveUpdateValueResolver valueResolver;
        private string valueResolverType;
        private bool valueResolverLoadTried;

        private DataSourceTypeEnum dataSourceType;

        public DataSourceTypeEnum DataSourceType
        {
            get => dataSourceType;
            set
            {
                if (dataSourceType == value) return;
                dataSourceType = value;
                FireChange();
            }
        }

        private bool forceAsynchronous;

        public bool ForceAsynchronous
        {
            get => forceAsynchronous;
            set
            {
                if (forceAsynchronous == value) return;
                forceAsynchronous = value;
                FireChange();
            }
        }

        public string ValueResolverType
        {
            get => valueResolverType;
            set
            {
                if (string.Equals(valueResolverType, value)) return;
                valueResolverType = value;
                valueResolverLoadTried = false;
                valueResolver = null;
                FireChange();
            }
        }

        public BGLiveUpdateUrls Urls
        {
            get
            {
                if (urls == null) urls = new BGLiveUpdateUrls(this);
                return urls;
            }
        }

        public BGLiveUpdateValueResolver ValueResolver
        {
            get
            {
                if (valueResolver == null && !string.IsNullOrEmpty(valueResolverType) && !valueResolverLoadTried)
                {
                    valueResolverLoadTried = true;
                    try
                    {
                        valueResolver = BGUtil.Create<BGLiveUpdateValueResolver>(valueResolverType, false);
                        if (valueResolver == null) throw new Exception("Can not create value resolver with type " + valueResolverType);
                    }
                    catch (Exception e)
                    {
                        Debug.Log("Can not create value resolver with type " + valueResolverType);
                        Debug.LogException(e);
                    }
                }

                return valueResolver;
            }
            set => valueResolver = value;
        }

        private bool IsAsynchronous => Application.platform == RuntimePlatform.WebGLPlayer;

        public string Error
        {
            get
            {
                switch (SourceType)
                {
                    case BGLiveUpdateSourceTypeEnum.GoogleSheets:
                    {
                        if (string.IsNullOrEmpty(SpreadsheetId)) return "SpreadsheetId is not set";
                        if (DataSourceType == DataSourceTypeEnum.ExcelExport)
                        {
                            var type = BGUtil.GetType(BGLiveUpdateDataSourceExcelExport.ParserTypeName);
                            if (type == null) return "ExcelExport DataSource type requires additional setup. See docs page for more details: https://www.bansheegz.com/BGDatabase/Addons/LiveUpdate/";
                        }

                        break;
                    }
                    case BGLiveUpdateSourceTypeEnum.WebServer:
                        if (Urls.urls == null || Urls.urls.Count == 0) return "URLs are empty! Add at least one URL";
                        else
                            for (var i = 0; i < Urls.urls.Count; i++)
                            {
                                var url = Urls.urls[i];
                                if (string.IsNullOrEmpty(url.URL)) return "URL is not set for [" + i + "] record (zero-based)";
                                if (string.IsNullOrEmpty(url.MetaId)) return "Meta is not set for [" + i + "] record (zero-based)";
                                if (!BGRepo.I.HasMeta(BGId.Parse(url.MetaId))) return "Repo does not have meta with id [" + url.MetaId + "]! record # " + i + " (zero-based)";
                            }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException("addon.SourceType");
                }

                if (!MergeSettings.HasAny(BGRepo.I)) return "No tables are included to merge settings";
                return null;
            }
        }
        
        private bool applyLastOnFailure;

        public bool ApplyLastOnFailure
        {
            get => applyLastOnFailure;
            set
            {
                if (applyLastOnFailure == value) return;
                applyLastOnFailure = value;
                FireChange();
            }
        }

        public BGAddonLiveUpdate()
        {
            mergeSettings.OnChange += SettingsChanged;
        }

        //================================================================================================
        //                                              Configuration
        //================================================================================================

        /// <inheritdoc />
        public override string ConfigToString()
        {
            var configToString = JsonUtility.ToJson(new Settings
            {
                SpreadsheetId = spreadsheetId,
                ApiKey = apiKey,
                MergeSettings = mergeSettings,
                ManualLoad = manualLoad,
                Timeout = timeout,
                InBuildOnly = inBuildOnly,
                PrintLogOnLoad = printLogOnLoad,
                LogLevel = (int)logLevel,
                ValueResolverType = valueResolverType,
                DataSourceType = (int)dataSourceType,
                ForceAsynchronous = forceAsynchronous,
                Urls = urls,
                sourceType = sourceType,
                ApplyLastOnFailure = applyLastOnFailure
            });
            return configToString;
        }

        /// <inheritdoc />
        public override void ConfigFromString(string config)
        {
            var settings = JsonUtility.FromJson<Settings>(config);
            apiKey = settings.ApiKey;
            spreadsheetId = settings.SpreadsheetId;
            mergeSettings = settings.MergeSettings;
            manualLoad = settings.ManualLoad;
            timeout = settings.Timeout < MinTimeout || settings.Timeout > MaxTimeout ? DefaultTimeout : settings.Timeout;
            inBuildOnly = settings.InBuildOnly;
            printLogOnLoad = settings.PrintLogOnLoad;
            logLevel = (BGLiveUpdateLog.LogLevelEnum)settings.LogLevel;
            valueResolverType = settings.ValueResolverType;
            dataSourceType = (DataSourceTypeEnum)settings.DataSourceType;
            forceAsynchronous = settings.ForceAsynchronous;
            urls = settings.Urls;
            if (urls != null) urls.Addon = this;
            sourceType = settings.sourceType;
            applyLastOnFailure = settings.ApplyLastOnFailure;
            mergeSettings.OnChange += SettingsChanged;
        }

        [Serializable]
        private class Settings
        {
            public BGMergeSettingsEntity MergeSettings = new BGMergeSettingsEntity();
            public string SpreadsheetId;
            public string ApiKey;
            public bool ManualLoad;
            public float Timeout;
            public bool InBuildOnly;
            public bool PrintLogOnLoad;
            public int LogLevel;
            public string ValueResolverType;
            public int DataSourceType;
            public bool ForceAsynchronous;
            public BGLiveUpdateSourceTypeEnum sourceType;
            public BGLiveUpdateUrls Urls;
            public bool ApplyLastOnFailure;
        }

        public override byte[] ConfigToBytes()
        {
            var mergeSettingArray = mergeSettings.ConfigToBytes();
            var writer = new BGBinaryWriter(4 + BGBinaryWriter.GetBytesCount(apiKey) + BGBinaryWriter.GetBytesCount(spreadsheetId) + BGBinaryWriter.GetBytesCount(mergeSettingArray));

            //version 
            writer.AddInt(8);

            //fields
            writer.AddString(apiKey);
            writer.AddString(spreadsheetId);
            writer.AddByteArray(mergeSettingArray);

            //version 2
            writer.AddBool(manualLoad);

            //version 3
            writer.AddFloat(timeout);
            writer.AddBool(inBuildOnly);

            //version 4
            writer.AddInt((int)logLevel);
            writer.AddBool(printLogOnLoad);

            //version 5
            writer.AddString(valueResolverType);

            //version 6
            writer.AddInt((int)dataSourceType);
            writer.AddBool(forceAsynchronous);

            //version 7  
            writer.AddInt((int)sourceType);
            writer.AddByteArray(Urls.ConfigToBytes());
            
            //version 8
            writer.AddBool(applyLastOnFailure);

            return writer.ToArray();
        }

        public override void ConfigFromBytes(ArraySegment<byte> config)
        {
            var reader = new BGBinaryReader(config);
            var version = reader.ReadInt();
            switch (version)
            {
                case 1:
                {
                    apiKey = reader.ReadString();
                    spreadsheetId = reader.ReadString();
                    mergeSettings.ConfigFromBytes(reader.ReadByteArray());
                    break;
                }

                case 2:
                {
                    apiKey = reader.ReadString();
                    spreadsheetId = reader.ReadString();
                    mergeSettings.ConfigFromBytes(reader.ReadByteArray());
                    manualLoad = reader.ReadBool();
                    break;
                }

                case 3:
                {
                    apiKey = reader.ReadString();
                    spreadsheetId = reader.ReadString();
                    mergeSettings.ConfigFromBytes(reader.ReadByteArray());
                    manualLoad = reader.ReadBool();
                    timeout = reader.ReadFloat();
                    inBuildOnly = reader.ReadBool();
                    break;
                }

                case 4:
                {
                    apiKey = reader.ReadString();
                    spreadsheetId = reader.ReadString();
                    mergeSettings.ConfigFromBytes(reader.ReadByteArray());
                    manualLoad = reader.ReadBool();
                    timeout = reader.ReadFloat();
                    inBuildOnly = reader.ReadBool();

                    logLevel = (BGLiveUpdateLog.LogLevelEnum)reader.ReadInt();
                    printLogOnLoad = reader.ReadBool();


                    break;
                }
                case 5:
                {
                    apiKey = reader.ReadString();
                    spreadsheetId = reader.ReadString();
                    mergeSettings.ConfigFromBytes(reader.ReadByteArray());
                    manualLoad = reader.ReadBool();
                    timeout = reader.ReadFloat();
                    inBuildOnly = reader.ReadBool();
                    logLevel = (BGLiveUpdateLog.LogLevelEnum)reader.ReadInt();
                    printLogOnLoad = reader.ReadBool();

                    valueResolverType = reader.ReadString();

                    break;
                }
                case 6:
                {
                    apiKey = reader.ReadString();
                    spreadsheetId = reader.ReadString();
                    mergeSettings.ConfigFromBytes(reader.ReadByteArray());
                    manualLoad = reader.ReadBool();
                    timeout = reader.ReadFloat();
                    inBuildOnly = reader.ReadBool();
                    logLevel = (BGLiveUpdateLog.LogLevelEnum)reader.ReadInt();
                    printLogOnLoad = reader.ReadBool();
                    valueResolverType = reader.ReadString();

                    dataSourceType = (DataSourceTypeEnum)reader.ReadInt();
                    forceAsynchronous = reader.ReadBool();

                    break;
                }
                case 7:
                case 8:
                {
                    apiKey = reader.ReadString();
                    spreadsheetId = reader.ReadString();
                    mergeSettings.ConfigFromBytes(reader.ReadByteArray());
                    manualLoad = reader.ReadBool();
                    timeout = reader.ReadFloat();
                    inBuildOnly = reader.ReadBool();
                    logLevel = (BGLiveUpdateLog.LogLevelEnum)reader.ReadInt();
                    printLogOnLoad = reader.ReadBool();
                    valueResolverType = reader.ReadString();

                    dataSourceType = (DataSourceTypeEnum)reader.ReadInt();
                    forceAsynchronous = reader.ReadBool();

                    sourceType = (BGLiveUpdateSourceTypeEnum)reader.ReadInt();
                    Urls.ConfigFromBytes(reader.ReadByteArray());
                    urls.Addon = this;

                    if (version == 8) applyLastOnFailure = reader.ReadBool();
                    
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
            var clone = new BGAddonLiveUpdate
            {
                Repo = repo,
                mergeSettings = (BGMergeSettingsEntity)mergeSettings.Clone(),
                apiKey = apiKey,
                spreadsheetId = spreadsheetId,
                manualLoad = manualLoad,
                timeout = timeout,
                inBuildOnly = inBuildOnly,
                logLevel = logLevel,
                printLogOnLoad = printLogOnLoad,
                valueResolverType = valueResolverType,
                dataSourceType = dataSourceType,
                forceAsynchronous = forceAsynchronous,
                sourceType = sourceType,
                applyLastOnFailure = applyLastOnFailure,
            };
            clone.urls = urls?.CloneTo(clone);
            clone.mergeSettings.OnChange += clone.SettingsChanged;
            return clone;
        }

        private void SettingsChanged()
        {
            FireChange();
        }

        //================================================================================================
        //                                              Load
        //================================================================================================
        private static bool loadTried;
        private static BGRepo remoteRepo;
        private static BGMergeSettingsEntity actualSettings;

        public static BGMergeSettingsEntity ActualSettings => actualSettings;

        /// <inheritdoc />
        public override void OnLoad()
        {
            if (!BGRepo.DefaultRepo(Repo)) return;

            if (manualLoad) return;
            Load();
        }

        /// <summary>
        /// Load remote data
        /// </summary>
        public void Load(bool allowToCacheLoadResult = false)
        {
            if (SuppressLoading) return;
            if (!IsPlaying) return;

            try
            {
                if (manualLoad && allowToCacheLoadResult && remoteRepo != null) ; //already loaded
                else LoadInternal();

                if (!IsAsynchronous) Merge();
            }
            catch (Exception e)
            {
                Log.Exception = e.Message ?? "Unknown error";
                Debug.LogException(e);
            }
            finally
            {
                if (OnLoadComplete != null && !IsAsynchronous) OnLoadComplete();
            }
        }

        //merge remote repo with database
        private void Merge()
        {
            if (remoteRepo == null /*|| !IsPlaying*/) return;
            new BGMergerEntity(null, remoteRepo, Repo, actualSettings).Merge();

            //fire batch update cause merger copy values silently
            var events = Repo.Events;
            if (events.On && !events.IsInBatch)
            {
                var batchEvent = Repo.Events.EnsureBatch();
                try
                {
                    Repo.ForEachMeta(meta =>
                    {
                        if (!actualSettings.IsMetaIncluded(meta.Id)) return;
                        batchEvent.AddMetaWithUpdatedEntities(meta.Id);
                    });
                    Repo.Events.FireBatchEvent();
                }
                finally
                {
                    //this call may be excessive, but we do it just in case to make sure batch event is released 
                    Repo.Events.ClearBatch();
                }
            }
        }

        //called is async request is used after result is received
        private void AsyncComplete()
        {
            Merge();
            if (printLogOnLoad && (IsAsynchronous || forceAsynchronous)) Log.PrintToConsole();
            OnLoadComplete?.Invoke();
        }

        //try to load data if not loaded
        private void LoadInternal()
        {
            if (inBuildOnly && Application.isEditor) return;

            if (loadTried && !manualLoad) return;

            Log.Clear();
            Log.Status = BGLiveUpdateLog.StatusEnum.LoadAttempted;
            log.Repo = Repo;

            loadTried = true;
            var error = Error;
            if (!string.IsNullOrEmpty(error))
            {
                Log.Exception = error;
                Debug.LogError(error);
                return;
            }

            var old = ServicePointManager.ServerCertificateValidationCallback;
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;

                ProcessLoad();
            }
            finally
            {
                ServicePointManager.ServerCertificateValidationCallback = old;
            }
        }

        //actual data loading
        private void ProcessLoad()
        {
            var timeOut = timeout < MinTimeout || timeout > MaxTimeout ? (int)(DefaultTimeout * 1000) : (int)(timeout * 1000);

            //shared context
            var context = new BGLiveUpdateContext(Repo, this, timeOut, IsAsynchronous || forceAsynchronous, AsyncComplete);
            BGLiveUpdateDataSourceA dataSource;
            switch (sourceType)
            {
                case BGLiveUpdateSourceTypeEnum.GoogleSheets:
                    switch (dataSourceType)
                    {
                        case DataSourceTypeEnum.GoogleSheetsAPI:
                            dataSource = new BGLiveUpdateDataSourceGoogleSheetsAPI(context);
                            break;
                        case DataSourceTypeEnum.VisualizationAPI:
                            dataSource = new BGLiveUpdateDataSourceChartsAPI(context);
                            break;
                        case DataSourceTypeEnum.ExcelExport:
                            dataSource = new BGLiveUpdateDataSourceExcelExport(context);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(dataSourceType));
                    }
                    break;
                case BGLiveUpdateSourceTypeEnum.WebServer:
                    dataSource = new BGLiveUpdateDataSourceUrls(context, urls);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sourceType));
            }

            //new empty repo without rows, using merge settings
            var loadedRepo = mergeSettings.NewRepo(Repo, false);
            //actual settings can be modified if some requests fail to keep the default data  
            actualSettings = (BGMergeSettingsEntity)mergeSettings.Clone();
            //iterate over each table, included into LiveUpdate addon merge settings 
            loadedRepo.ForEachMeta(meta =>
            {
                if (!actualSettings.IsMetaIncluded(meta.Id)) return;

                //request data loading
                dataSource.Load(meta, actualSettings, null, applyLastOnFailure);
            });

            remoteRepo = loadedRepo;

            //this call may be used by some loaders to actually load the data
            dataSource.Complete();

            if (printLogOnLoad && !context.isAsynchronous) Log.PrintToConsole();
        }

        /// <summary>
        /// Reset current state
        /// </summary>
        public static void Reset()
        {
            remoteRepo = null;
            loadTried = false;
            actualSettings = null;
        }

        /// <summary>
        /// Load data for default repo 
        /// </summary>
        public static void LoadDefault(bool allowToCacheLoadResult = false)
        {
            var addon = BGRepo.I.Addons.Get<BGAddonLiveUpdate>();
            if (addon == null) throw new Exception("Can not invoke LiveUpdate addon on default repo, cause addon is not enabled!");
            addon.Load(allowToCacheLoadResult);
        }
    }
}