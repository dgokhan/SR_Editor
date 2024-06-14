/*
<copyright file="BGExcelServiceI.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// this is the interface for Excel delegate implementation
    /// All Excel file processing is handled by this class   
    /// </summary>
    //================================= V1
    public interface BGExcelServiceI
    {
        byte[] Import(BGLogger logger, byte[] content, BGRepo repo, BGMergeSettingsEntity modelSettingsEntity, BGMergeSettingsMeta mergeSettingsMeta, bool updateNewIds, bool transferRowsOrder,
            bool useXml);

        byte[] Export(BGLogger logger, byte[] content, BGRepo repo, BGMergeSettingsEntity modelSettingsEntity, BGMergeSettingsMeta mergeSettingsMeta, bool transferRowsOrder, bool useXml);
    }

    //================================= V2
    public interface BGExcelServiceV2
    {
        byte[] Import(BGExcelServiceV2ImportTask task);

        byte[] Export(BGExcelServiceV2ExportTask task);
    }

    public class BGExcelServiceV2ImportTask : BGExcelServiceV2ExportTask
    {
        public bool updateNewIds;
    }

    public class BGExcelServiceV2ExportTask
    {
        public BGLogger logger;
        public byte[] content;
        public BGRepo repo;
        public BGMergeSettingsEntity modelSettingsEntity;
        public bool transferRowsOrder;
        public bool useXml;
        public BGSyncNameMapConfig nameMapConfig;
    }

    //================================= V3
    public interface BGExcelServiceV3
    {
        byte[] Import(BGExcelServiceV3ImportTask task);

        byte[] Export(BGExcelServiceV3ExportTask task);
    }

    public class BGExcelServiceV3ImportTask : BGExcelServiceV2ImportTask
    {
        public BGSyncIdConfig idConfig;
        public BGSyncRelationsConfig relationsConfig;
        public bool printWarnings;
    }

    public class BGExcelServiceV3ExportTask : BGExcelServiceV2ExportTask
    {
        public BGSyncIdConfig idConfig;
        public BGSyncRelationsConfig relationsConfig;
        public bool printWarnings;
    }

    //================================= Data structure
    public interface BGExcelDataStructureProvider
    {
        BGSyncDataStructure Extract(BGExcelDataStructureProviderContext context);
    }

    public class BGExcelDataStructureProviderContext
    {
        public byte[] content;
        public bool useXml;
        public BGLogger logger;
    }
}