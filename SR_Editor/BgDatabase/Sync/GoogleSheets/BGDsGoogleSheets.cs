/*
<copyright file="BGDsGoogleSheets.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


using System;
using UnityEngine;

//DO NOT CHANGE NAMESPACE! we are stuck here cause class is stored inside settings
namespace BansheeGz.BGDatabase.Editor
{
    /// <summary>
    /// Data source for Google Sheets spreadsheet
    /// </summary>
    [Descriptor(Name = "GoogleSheets")]
    public partial class BGDsGoogleSheets : BGDataSource, BGSyncNameMapConfig.BGNameConfigOwner
    {
        public const string PluginPage = "https://www.bansheegz.com/BGDatabase/Downloads/EditorGoogleSheets";

        /// <summary>
        /// All possible options for different GoogleSheets Authentication 
        /// </summary>
        public enum DataSourceTypeEnum
        {
            OAuth,
            Service,
            APIKey,
            Anonymous
        }

        //delegate implementation full type name
        public const string ImplementationType = "BansheeGz.BGDatabase.BGGoogleSheetService";

        //is it used somewhere?
        public const string WrongScriptingRuntimeVersion = "To use Google Sheets you need to switch ScriptingRuntimeVersion to 4.x. " +
                                                           "Set 'File->Build Settings..->Player Settings->Other Settings->Scripting Runtime Version*' parameter to NET 4.x";

        public DataSourceTypeEnum DataSourceType;

        public string ClientId;
        public string ClientSecret;
        public string ApplicationName;
        public string SpreadSheetId;
        public string AccessToken;
        public string RefreshToken;

        public string APIKey;

        public string ClientEmail;
        public string PrivateKey;

        public ReadFormatEnum ReadFormat = ReadFormatEnum.UseSpreadsheetCulture;
        public string ReadFormatCountry;
        public WriteFormatEnum WriteFormat = WriteFormatEnum.UseSpreadsheetCulture;
        public string WriteFormatCountry;
        
        public BGSyncNameMapConfig NameMapConfig { get; set; }
        public BGSyncDisabledConfig DisabledConfig { get; set; }
        public bool NameMapConfigEnabled { get; set; }
        public BGSyncIdConfig IdConfig { get; set; }
        public bool IdConfigEnabled { get; set; }
        public BGSyncRelationsConfig RelationsConfig { get; set; }
        public bool RelationsConfigEnabled { get; set; }


        public override bool IsExportAllowed => base.IsExportAllowed && DataSourceType != DataSourceTypeEnum.APIKey && DataSourceType != DataSourceTypeEnum.Anonymous;

        /// <summary>
        /// Returns created GoogleSheet delegate  initialized  class.
        /// Initialized means that all data source parameters are passed to the delegate 
        /// </summary>
        public BGGoogleSheetServiceI Service
        {
            get
            {
                var service = NotInitiatedService;
                service?.Init(this);
                return service;
            }
        }

        /// <summary>
        /// Returns created GoogleSheet delegate class without initializing it
        /// </summary>
        public static BGGoogleSheetServiceI NotInitiatedService
        {
            get
            {
                var type = BGUtil.GetType(ImplementationType);
                if (type == null) return null;
                try
                {
                    return Activator.CreateInstance(type) as BGGoogleSheetServiceI;
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <inheritdoc/>
        public override string Error
        {
            get
            {
                string error = null;
                if (string.IsNullOrEmpty(SpreadSheetId)) return "No spreadsheet ID";
                if (CheckForSpaceChar(SpreadSheetId, "SpreadSheet Id", ref error)) return error;
                switch (DataSourceType)
                {
                    case DataSourceTypeEnum.OAuth:
                        if (string.IsNullOrEmpty(ClientId)) return "No client Id";
                        if (string.IsNullOrEmpty(ClientSecret)) return "No client secret";
                        if (string.IsNullOrEmpty(ApplicationName)) return "No application name";
                        if (string.IsNullOrEmpty(AccessToken)) return "No access token";
                        if (string.IsNullOrEmpty(RefreshToken)) return "No refresh token";
                        if (CheckForSpaceChar(ClientId, "Client id", ref error)) return error;
                        if (CheckForSpaceChar(ClientSecret, "Client secret", ref error)) return error;
                        if (CheckForSpaceChar(AccessToken, "Access Token", ref error)) return error;
                        if (CheckForSpaceChar(RefreshToken, "Refresh Token", ref error)) return error;
                        break;
                    case DataSourceTypeEnum.Service:
                        if (string.IsNullOrEmpty(ClientEmail)) return "No client email";
                        if (string.IsNullOrEmpty(PrivateKey)) return "No private key";
                        if (CheckForSpaceChar(ClientEmail, "Client Email", ref error)) return error;
                        //private keys can have space chars!
                        // if (CheckForSpaceChar(PrivateKey, "Private Key", ref error)) return error;
                        break;
                    case DataSourceTypeEnum.APIKey:
                        if (string.IsNullOrEmpty(APIKey)) return "No API key";
                        if (CheckForSpaceChar(APIKey, "API Key", ref error)) return error;
                        break;
                    case DataSourceTypeEnum.Anonymous:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("DataSourceType");
                }

                return null;
            }
        }

        private bool CheckForSpaceChar(string parameter, string parameterName, ref string error)
        {
            error = null;
            var spaceCharIndex = parameter.IndexOf(' ');
            if (spaceCharIndex == -1) return false;
            error = $"[{parameterName}] has invalid space char at position {spaceCharIndex}";
            return true;
        }

        /// <inheritdoc/>
        public override string ConfigToString()
        {
            if (NameMapConfigEnabled && NameMapConfig != null) NameMapConfig.Trim();
            return JsonUtility.ToJson(new Settings
            {
                ClientId = ClientId,
                AccessToken = AccessToken,
                ApplicationName = ApplicationName,
                SpreadSheetId = SpreadSheetId,
                ClientSecret = ClientSecret,
                RefreshToken = RefreshToken,

                DataSourceType = (int)DataSourceType,
                APIKey = APIKey,

                ClientEmail = ClientEmail,
                PrivateKey = PrivateKey,

                NameMapConfig = NameMapConfigEnabled ? NameMapConfig : null,
                NameMapConfigEnabled = NameMapConfigEnabled,
                IdConfig = IdConfigEnabled ? IdConfig : null,
                IdConfigEnabled = IdConfigEnabled,
                DisabledConfig = DisabledConfig,
                RelationsConfigEnabled = RelationsConfigEnabled,
                RelationsConfig = RelationsConfig,

                ActionsType = ActionsType,
                
                ReadFormat = ReadFormat,
                ReadFormatCountry = ReadFormatCountry,
                WriteFormat = WriteFormat,
                WriteFormatCountry = WriteFormatCountry,
            });
        }

        /// <inheritdoc/>
        public override void ConfigFromString(string config)
        {
            var settings = JsonUtility.FromJson<Settings>(config);
            ClientId = settings.ClientId;
            ClientSecret = settings.ClientSecret;
            ApplicationName = settings.ApplicationName;
            SpreadSheetId = settings.SpreadSheetId;
            AccessToken = settings.AccessToken;
            RefreshToken = settings.RefreshToken;

            DataSourceType = (DataSourceTypeEnum)settings.DataSourceType;
            APIKey = settings.APIKey;

            ClientEmail = settings.ClientEmail;
            PrivateKey = settings.PrivateKey;
            NameMapConfig = settings.NameMapConfig;
            NameMapConfigEnabled = settings.NameMapConfigEnabled;
            DisabledConfig = settings.DisabledConfig;
            IdConfig = settings.IdConfig;
            IdConfigEnabled = settings.IdConfigEnabled;
            RelationsConfigEnabled = settings.RelationsConfigEnabled;
            RelationsConfig = settings.RelationsConfig;
            ActionsType = settings.ActionsType;
            
            ReadFormat = settings.ReadFormat;
            ReadFormatCountry = settings.ReadFormatCountry;
            WriteFormat = settings.WriteFormat;
            WriteFormatCountry = settings.WriteFormatCountry;
        }


        //serializable container for different data source settings
        [Serializable]
        private class Settings
        {
            public ActionsTypeEnum ActionsType;
            public int DataSourceType;

            public string ClientId;
            public string ClientSecret;
            public string ApplicationName;
            public string SpreadSheetId;
            public string AccessToken;
            public string RefreshToken;

            public string APIKey;

            public string ClientEmail;
            public string PrivateKey;
            public bool NameMapConfigEnabled;
            public BGSyncNameMapConfig NameMapConfig;
            public BGSyncDisabledConfig DisabledConfig;
            public bool IdConfigEnabled;
            public BGSyncIdConfig IdConfig;
            public BGSyncRelationsConfig RelationsConfig;
            public bool RelationsConfigEnabled;
            public ReadFormatEnum ReadFormat;
            public string ReadFormatCountry;
            public WriteFormatEnum WriteFormat;
            public string WriteFormatCountry;

        }

        /// <summary>
        /// Try to create GoogleSheets delegate implementation class
        /// </summary>
        public BGGoogleSheetServiceI TryToCreateService(BGLogger logger)
        {
            var service = Service;
            if (service != null)
            {
                logger?.AppendLine("GoogleSheet service is created successfully");
                return service;
            }

            var error = "Error: Can not create GoogleSheet service, cause Google plugin is not installed. " +
                        "Please, download GoogleSheets plugin at https://www.bansheegz.com/BGDatabase/Downloads + and make sure your scripting runtime parameter " +
                        "'File->Build Settings..->Player Settings->Other Settings->Scripting Runtime Version*' is set to NET 4.x";

            logger?.AppendWarning(error);

            // if (interactive) BGEditorUtility.Exit(WrongScriptingRuntimeVersion);
            throw new Exception(error);
            // return false;
        }

        public enum ReadFormatEnum : byte
        {
            CurrentLocalCulture,
            InvariantCulture,
            CultureBased,
            UseSpreadsheetCulture
        }
        public enum WriteFormatEnum : byte
        {
            InvariantCulture,
            CurrentLocalCulture,
            CultureBased,
            UseSpreadsheetCulture
        }
    }
}