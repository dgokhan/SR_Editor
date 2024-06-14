/*
<copyright file="BGLiveUpdateDataSourceChartsAPI.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Google Visualization API data source 
    /// </summary>
    public class BGLiveUpdateDataSourceChartsAPI : BGLiveUpdateDataSourceA
    {
        public BGLiveUpdateDataSourceChartsAPI(BGLiveUpdateContext context) : base(context)
        {
        }

        //return full URL for specified table
        private string GetUrl(string metaName) => "https://docs.google.com/spreadsheets/d/" + addon.SpreadsheetId + "/gviz/tq?tqx=out:csv&sheet=" + metaName;

        /// <inheritdoc />
        public override void Load(BGMetaEntity meta, BGMergeSettingsEntity actualSettings, BGLiveUpdateUrl url = null, bool applyLastDataOnFailure = false)
        {
            var loadContext = url?.URL == null
                ? new BGLiveUpdateLoaderA.LoadContext(GetUrl(meta.Name), addon.Log)
                : new BGLiveUpdateLoaderA.LoadContext(url.URL, addon.Log, url.HttpMethod, url.HttpParametersAsTuples, url.HttpHeadersAsTuples);

            if (applyLastDataOnFailure)
                loadContext.LocalFileName =
                    Path.ChangeExtension("BGD_LU_" + (string.IsNullOrEmpty(LocalFileID) ? "CHARTS_" : LocalFileID + "_") + BGAddonPartition.ToFilePath(meta.Id), "csv");

            loader.Load(loadContext, result => LoaderCallback(meta, actualSettings, result));
        }

        //called after request completion
        private void LoaderCallback(BGMetaEntity meta, BGMergeSettingsEntity actualSettings, BGLiveUpdateLoaderA.LoadResultText result)
        {
            try
            {
                //parse CSV result data
                var liveUpdateData = ParseCsv(meta, actualSettings, result);
                if (liveUpdateData != null)
                {
                    //process parsed data
                    new BGLiveUpdateDataProcessor(addon, defaultRepo).Process(liveUpdateData);
                    addon.Log.OkMetaCount++;
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                Error(actualSettings, meta, e);
            }
        }

        //parse CSV data
        private BGLiveUpdateDataProcessor.BGLiveUpdateData ParseCsv(BGMetaEntity meta, BGMergeSettingsEntity actualSettings, BGLiveUpdateLoaderA.LoadResultText result)
        {
            if (result.IsError)
            {
                Error(actualSettings, meta, result.Error);
                return null;
            }

            if (string.IsNullOrEmpty(result.Result))
            {
                Error(actualSettings, meta, "Loaded result is null");
                return null;
            }

            if (result.Result.StartsWith("<") && result.Result.IndexOf("><html", 0, 24, StringComparison.OrdinalIgnoreCase) != -1)
            {
                Error(actualSettings, meta, "It looks like the result is not a valid CSV content. Try to open Visualization API URL in your browser " +
                                            "(the URL can be found here: https://www.bansheegz.com/BGDatabase/Addons/LiveUpdate ). ");
                return null;
            }

            BGLiveUpdateDataWithOrigin data;
            using (var reader = new CsvFileReader(new MemoryStream(Encoding.UTF8.GetBytes(result.Result))))
            {
                //try to read field names
                var row = new List<string>();
                if (!reader.ReadRow(row) || row.Count == 0)
                {
                    Error(actualSettings, meta, "Loaded csv data does not have field names");
                    return null;
                }

                //==================================== mapping
                data = MapFields(actualSettings, meta, row.ToArray());
                if (data == null) return null;

                //==================================== read data
                //read data
                row.Clear();
                var rowIndex = 1;
                while (reader.ReadRow(row))
                {
                    var id = data.HasId && data.IdIndex < row.Count ? ReadId(row[data.IdIndex], data.IdIndex) : BGId.Empty;
                    var values = new string[data.Fields.Length];
                    for (var i = 0; i < data.Fields.Length; i++) values[i] = row[data.Indexes[i]];

                    data.Add(id, values, addon.Log, rowIndex);
                    row.Clear();
                    rowIndex++;
                }
            }

            return data;
        }
    }
}