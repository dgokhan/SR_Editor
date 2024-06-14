/*
<copyright file="BGLiveUpdateDataSourceGoogleSheetsAPI.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.IO;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// GoogleSheets API data source
    /// </summary>
    public class BGLiveUpdateDataSourceGoogleSheetsAPI : BGLiveUpdateDataSourceA
    {
        public BGLiveUpdateDataSourceGoogleSheetsAPI(BGLiveUpdateContext context) : base(context)
        {
        }

        //get GoogleSheets API URL for specified table
        protected string GetUrl(string metaName)
        {
            return "https://sheets.googleapis.com/v4/spreadsheets/" + addon.SpreadsheetId + "/values/" + metaName + "?key=" + addon.ApiKey;
        }

        /// <inheritdoc />
        public override void Load(BGMetaEntity meta, BGMergeSettingsEntity actualSettings, BGLiveUpdateUrl url = null, bool applyLastDataOnFailure = false)
        {
            var loadContext = url?.URL == null 
                ? new BGLiveUpdateLoaderA.LoadContext(GetUrl(meta.Name), addon.Log) 
                : new BGLiveUpdateLoaderA.LoadContext(url.URL, addon.Log, url.HttpMethod, url.HttpParametersAsTuples, url.HttpHeadersAsTuples );

            if (applyLastDataOnFailure)
                loadContext.LocalFileName =
                    Path.ChangeExtension("BGD_LU_" + (string.IsNullOrEmpty(LocalFileID) ? "GH_" : LocalFileID + "_") + BGAddonPartition.ToFilePath(meta.Id), "json");

            loader.Load(loadContext, result => LoaderCallback(meta, actualSettings, result));
        }

        //called after request is completed
        private void LoaderCallback(BGMetaEntity meta, BGMergeSettingsEntity actualSettings, BGLiveUpdateLoaderA.LoadResultText result)
        {
            try
            {
                //parse remote JSON data
                var liveUpdateData = ParseJson(meta, actualSettings, result);
                if (liveUpdateData != null)
                {
                    //process data
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

        //parse remote JSON data
        private BGLiveUpdateDataProcessor.BGLiveUpdateData ParseJson(BGMetaEntity meta, BGMergeSettingsEntity actualSettings, BGLiveUpdateLoaderA.LoadResultText result)
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

            var json = JSON.Parse(result.Result);
            if (json == null)
            {
                Error(actualSettings, meta, "Parsed JSON object is null");
                return null;
            }

            var valuesNode = json["values"];
            if (valuesNode == null || !valuesNode.IsArray)
            {
                Error(actualSettings, meta, "Values are null");
                return null;
            }

            var valuesArray = valuesNode as JSONArray;
            if (valuesArray == null || valuesArray.Count < 2)
            {
                Error(actualSettings, meta, "No values in json feed");
                return null;
            }

            //==================================== mapping
            var fieldNamesJson = valuesArray[0];
            string[] fieldNames = null;
            if (fieldNamesJson != null && fieldNamesJson.Count > 0)
            {
                fieldNames = new string[fieldNamesJson.Count];
                for (var i = 0; i < fieldNamesJson.Count; i++) fieldNames[i] = fieldNamesJson[i];
            }

            var data = MapFields(actualSettings, meta, fieldNames);
            if (data == null) return null;


            //==================================== read data
            LogDetail("==== Reading $ rows..", valuesArray.Count - 1);
            for (var i = 1; i < valuesArray.Count; i++)
            {
                var values = valuesArray[i];
                if (!values.IsArray)
                {
                    LogDetail("WARNING! json values with index $ are not array!", i);
                    continue;
                }

                var row = values as JSONArray;
                if (row == null)
                {
                    LogDetail("WARNING! json values with index $ can not be cast to JSONArray!", i);
                    continue;
                }

                var count = row.Count;
                if (count == 0)
                {
                    LogDetail("WARNING! json values array with index $ has no values! skipping this row..", i);
                    continue;
                }

                //read id data
                var id = BGId.Empty;
                if (Get(row, data.IdIndex, out var idString)) id = ReadId(idString, data.IdIndex);

                var fieldValues = new string[data.Fields.Length];
                for (var index = 0; index < data.Fields.Length; index++)
                {
                    var field = data.Fields[index];
                    if (!Get(row, data.Indexes[index], out var fieldValue))
                    {
                        LogDetail("WARNING! can not get field value (field=$), index $!", field.Name, i);
                        continue;
                    }

                    fieldValues[index] = fieldValue;
                }

                data.Add(id, fieldValues, addon.Log, i);
            }

            LogDetail("==== Rows are read.");
            LogDetail("======== Meta $ loaded.", meta.Name);
            return data;
        }

        //get string data from JSONArray for specified index
        private static bool Get(JSONArray row, int index, out string val)
        {
            val = null;
            if (index < 0 || index >= row.Count) return false;
            var valString = row[index];
            if (valString == null || !valString.IsString) return false;
            val = (valString as JSONString).Value;
            return true;
        }
    }
}