/*
<copyright file="BGGoogleSheetServiceI.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using BansheeGz.BGDatabase.Editor;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// this is the interface for GoogleSheet delegate implementation
    /// All GoogleSheet spreadsheet processing is handled by this class   
    /// </summary>
    //=============== V1
    public interface BGGoogleSheetServiceI
    {
        void Init(Editor.BGDsGoogleSheets dataSource);
        BGGoogleSheetRefreshTokenServiceI CreateRefreshTokenService();
        void Export(BGLogger logger, BGRepo repo, BGMergeSettingsEntity modelSettingsEntity, BGMergeSettingsMeta mergeSettingsMeta, bool transferRowsOrder);
        void Import(BGLogger logger, BGRepo repo, BGMergeSettingsEntity modelSettingsEntity, BGMergeSettingsMeta mergeSettingsMeta, bool updateNewIds, bool transferRowsOrder);
    }

    public interface BGGoogleSheetRefreshTokenServiceI
    {
        string Url { get; }
        void Exchange(string code, out string token, out string refreshToken);
    }

    public interface BGGoogleSheetRefreshTokenServiceIV2 : BGGoogleSheetRefreshTokenServiceI
    {
        string GetUrl(int port);
        void Exchange(int port, string code, out string token, out string refreshToken);
    }


    //=============== V2
    public interface BGGoogleSheetServiceV2
    {
        void Export(BGGoogleSheetServiceV2ExportTask task);
        void Import(BGGoogleSheetServiceV2ImportTask task);
    }

    public class BGGoogleSheetServiceV2ImportTask : BGGoogleSheetServiceV2ExportTask
    {
        public bool updateNewIds;
    }

    public class BGGoogleSheetServiceV2ExportTask
    {
        public BGLogger logger;
        public BGRepo repo;
        public BGMergeSettingsEntity modelSettingsEntity;
        public bool transferRowsOrder;
        public BGSyncNameMapConfig nameMapConfig;
    }

    //=============== V3
    public interface BGGoogleSheetServiceV3
    {
        void Export(BGGoogleSheetServiceV3ExportTask task);
        void Import(BGGoogleSheetServiceV3ImportTask task);
    }

    public class BGGoogleSheetServiceV3ImportTask : BGGoogleSheetServiceV2ImportTask
    {
        public BGSyncIdConfig idConfig;
        public BGSyncRelationsConfig relationsConfig;
        public bool printWarnings;
    }

    public class BGGoogleSheetServiceV3ExportTask : BGGoogleSheetServiceV2ExportTask
    {
        public BGSyncIdConfig idConfig;
        public BGSyncRelationsConfig relationsConfig;
        public bool printWarnings;
    }
    
    //=============== V4
    public interface BGGoogleSheetServiceV4
    {
        void Export(BGGoogleSheetServiceV4ExportTask task);
        void Import(BGGoogleSheetServiceV4ImportTask task);
    }
    
    public class BGGoogleSheetServiceV4ImportTask : BGGoogleSheetServiceV3ImportTask
    {
        public bool skipLocking;
        public BGDsGoogleSheets.ReadFormatEnum ReadFormat;
        public string FloatingFormatCountry;
    }

    public class BGGoogleSheetServiceV4ExportTask : BGGoogleSheetServiceV3ExportTask
    {
        public bool skipLocking;
        public BGDsGoogleSheets.ReadFormatEnum ReadFormat;
        public string ReadFormatCountry;
        public BGDsGoogleSheets.WriteFormatEnum WriteFormat;
        public string WriteFormatCountry;
    }


    //================================= Data structure
    public interface BGGoogleSheetDataStructureProvider
    {
        BGSyncDataStructure Extract(BGGoogleSheetDataStructureProviderContext context);
    }

    public class BGGoogleSheetDataStructureProviderContext
    {
        public BGLogger logger;
    }
    
    //================================= Data structure
    public interface BGGoogleSheetSpreadsheetInfoProvider
    {
        BGSpreadsheetInfo GetInfo();
    }

    public class BGSpreadsheetInfo
    {
        public string name;
        public string locale;
    }
}