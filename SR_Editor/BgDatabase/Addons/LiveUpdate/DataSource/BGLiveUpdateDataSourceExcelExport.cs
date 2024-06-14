/*
<copyright file="BGLiveUpdateDataSourceExcelExport.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Excel file data source 
    /// </summary>
    public class BGLiveUpdateDataSourceExcelExport : BGLiveUpdateDataSourceA
    {
        //default excel parser implementation class name
        public const string ParserTypeName = "BansheeGz.BGDatabase.BGLiveUpdateExcelParser";

        private readonly List<BGMetaEntity> metas = new List<BGMetaEntity>();
        private BGMergeSettingsEntity actualSettings;
        private string url;
        private string localFileName;

        public BGLiveUpdateDataSourceExcelExport(BGLiveUpdateContext context) : base(context)
        {
        }

        //parser for received data
        private LiveUpdateExcelParser Parser
        {
            get
            {
                var type = BGUtil.GetType(ParserTypeName);
                if (type == null) return null;
                return Activator.CreateInstance(type) as LiveUpdateExcelParser;
            }
        }

        /// <summary>
        /// Get GoogleSheets excel export URL for specified spreadsheet  
        /// </summary>
        public static string GetUrl(string spreadSheetId)
        {
            return "https://docs.google.com/spreadsheets/d/" + spreadSheetId + "/export";
        }
        
        /// <inheritdoc />
        public override void Load(BGMetaEntity meta, BGMergeSettingsEntity actualSettings, BGLiveUpdateUrl url = null, bool applyLastDataOnFailure = false)
        {
            this.actualSettings = actualSettings;
            
            if (url?.URL != null) this.url = url.URL;
            
            if (applyLastDataOnFailure) localFileName = Path.ChangeExtension("BGD_LU_" + (string.IsNullOrEmpty(LocalFileID) ? "EXCEL" : LocalFileID), "xlsx");

            metas.Add(meta);
        }

        /// <inheritdoc />
        public override void Complete()
        {
            if (metas.Count != 0) loader.Load(new BGLiveUpdateLoaderA.LoadContext(url ?? GetUrl(addon.SpreadsheetId), addon.Log){LocalFileName = localFileName}, Loaded);

            base.Complete();
        }

        //called after request is completed 
        private void Loaded(BGLiveUpdateLoaderA.LoadResultBinary result)
        {
            if (result.IsError)
            {
                MarkForError(result.Error);
                return;
            }

            if (result.Result == null || result.Result.Length == 0)
            {
                MarkForError("Loaded result is null");
                return;
            }

            var parser = Parser;
            if (parser == null)
            {
                MarkForError("Can not create Excel parser of type " + ParserTypeName + ". You need to download it at addon's page https://www.bansheegz.com/BGDatabase/Addons/LiveUpdate/");
                return;
            }

            LiveUpdateExcelData data;
            try
            {
                //parse remote Excel data
                data = parser.Parse(result.Result, true, metas.ToArray());
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                MarkForError("Can not parse Excel file: " + e.Message);
                return;
            }

            if (data == null || data.sheets == null || data.sheets.Length == 0)
            {
                MarkForError("Excel file does not have any useful information");
                return;
            }

            var name2Meta = new Dictionary<string, BGMetaEntity>();
            foreach (var meta in metas) name2Meta.Add(meta.Name, meta);
            foreach (var sheet in data.sheets)
            {
                if (!name2Meta.TryGetValue(sheet.Name, out var meta)) continue;
                name2Meta.Remove(sheet.Name);

                var sheetData = sheet.Data;
                if (sheetData == null)
                {
                    Error(actualSettings, meta, "No data for meta " + meta.Name);
                    return;
                }

                var length0 = sheetData.GetLength(0);
                var length1 = sheetData.GetLength(1);
                if (length0 < 2 || length1 < 2)
                {
                    Error(actualSettings, meta, "No data for meta " + meta.Name);
                    return;
                }

                //================================ map
                var mappedData = MapFields(actualSettings, meta, GetRow(sheetData, 0));
                if (mappedData == null) continue;

                //================================ read data
                var idColumnIsMappedOk = mappedData.HasId && mappedData.IdIndex < length1;
                for (var i = 1; i < length0; i++)
                {
                    var id = idColumnIsMappedOk ? ReadId(sheetData[i, mappedData.IdIndex], mappedData.IdIndex) : BGId.Empty;

                    var rowData = new string[mappedData.Fields.Length];
                    for (var j = 0; j < mappedData.Fields.Length; j++) rowData[j] = sheetData[i, mappedData.Indexes[j]];
                    mappedData.Add(id, rowData, addon.Log, i);
                }

                //================================ process
                try
                {
                    new BGLiveUpdateDataProcessor(addon, defaultRepo).Process(mappedData);
                    addon.Log.OkMetaCount++;
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    Error(actualSettings, meta, e);
                }
            }

            foreach (var pair in name2Meta) Error(actualSettings, pair.Value, "No data for meta " + pair.Value.Name);
        }

        //get single rows data
        private string[] GetRow(string[,] sheetData, int row)
        {
            var length = sheetData.GetLength(1);
            var result = new string[length];
            for (var i = 0; i < length; i++) result[i] = sheetData[row, i];

            return result;
        }

        //mark all tables as failed
        private void MarkForError(string resultError)
        {
            foreach (var meta in metas) Error(actualSettings, meta, resultError);
        }

        /// <summary>
        /// parser for Excel data (must be public)
        /// </summary>
        public interface LiveUpdateExcelParser
        {
            LiveUpdateExcelData Parse(byte[] data, bool UseXml, BGMetaEntity[] metas);
        }

        /// <summary>
        /// data container for parsed Excel data (must be public)
        /// </summary>
        public class LiveUpdateExcelData
        {
            /// <summary>
            /// Parsed sheets data
            /// </summary>
            public LiveUpdateExcelSheet[] sheets;
        }

        /// <summary>
        /// data container for parsed Excel sheet data (must be public)
        /// </summary>
        public class LiveUpdateExcelSheet
        {
            /// <summary>
            /// Sheet name
            /// </summary>
            public string Name;
            /// <summary>
            /// Cells data
            /// </summary>
            public string[,] Data;
        }
    }
}