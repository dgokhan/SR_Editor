/*
<copyright file="BGLiveUpdateDataSourceUrls.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Data source for loading data from Web servers
    /// </summary>
    public class BGLiveUpdateDataSourceUrls : BGLiveUpdateDataSourceA
    {
        private readonly BGLiveUpdateContext context;
        private readonly BGLiveUpdateUrls urls;

        private BGLiveUpdateDataSourceGoogleSheetsAPI jsonDataSource;

        private BGLiveUpdateDataSourceChartsAPI csvDataSource;
        // private BGLiveUpdateDataSourceExcelExport excelDataSource;

        private readonly Dictionary<BGId, BGLiveUpdateUrl> meta2Url = new Dictionary<BGId, BGLiveUpdateUrl>();
        private readonly BGLiveUpdateContext contextClone;

        public BGLiveUpdateDataSourceUrls(BGLiveUpdateContext context, BGLiveUpdateUrls urls) : base(context)
        {
            this.context = context;
            contextClone = context.Clone();
            contextClone.loader = loader;
            this.urls = urls;
            if (urls?.urls != null)
                foreach (var url in urls.urls)
                {
                    if (string.IsNullOrEmpty(url.URL)) continue;
                    if (string.IsNullOrEmpty(url.MetaId)) continue;
                    var id = BGId.Parse(url.MetaId);
                    if (!context.Repo.HasMeta(id)) continue;
                    meta2Url[id] = url;
                }
        }

        /// <inheritdoc />
        public override void Load(BGMetaEntity meta, BGMergeSettingsEntity actualSettings, BGLiveUpdateUrl notUsedUrl = null, bool applyLastDataOnFailure = false)
        {
            if (!meta2Url.TryGetValue(meta.Id, out var url)) return;
            
            // resolve delegate data source (it depends on data format)
            BGLiveUpdateDataSourceA delegateDs;
            switch (url.URLType)
            {
                case BGLiveUpdateUrlTypeEnum.Json:
                    delegateDs = jsonDataSource = jsonDataSource ?? new BGLiveUpdateDataSourceGoogleSheetsAPI(contextClone);
                    if (applyLastDataOnFailure) delegateDs.LocalFileID = "WSJ";
                    break;
                case BGLiveUpdateUrlTypeEnum.Csv:
                    delegateDs = csvDataSource = csvDataSource ?? new BGLiveUpdateDataSourceChartsAPI(contextClone);
                    if (applyLastDataOnFailure) delegateDs.LocalFileID = "WSC";
                    break;
                /*
                case BGLiveUpdateUrlTypeEnum.Excel:
                    delegateDs = excelDataSource = excelDataSource ?? new BGLiveUpdateDataSourceExcelExport(contextClone);
                    break;
                */
                default:
                    throw new ArgumentOutOfRangeException("url.URLType");
            }

            //load data
            delegateDs.Load(meta, actualSettings, url, applyLastDataOnFailure);
        }

        /*
        public override void Complete()
        {
            if (excelDataSource != null) excelDataSource.Complete();
            else base.Complete(); 
        }
    */
    }
}