/*
<copyright file="BGLiveUpdateDataSourceA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// abstract LiveUpdate plugin data source 
    /// </summary>
    public abstract class BGLiveUpdateDataSourceA
    {
        protected BGRepo defaultRepo;
        protected BGAddonLiveUpdate addon;
        protected BGLiveUpdateLoaderA loader;

        public string LocalFileID;

        protected BGLiveUpdateDataSourceA(BGLiveUpdateContext context)
        {
            defaultRepo = context.Repo;
            addon = context.addon;

            if (context.loader != null) loader = context.loader;
            else
            {
                if (context.isAsynchronous) loader = new BGLiveUpdateLoaderUnityWebRequest(context.timeOut, context.asyncComplete);
                else loader = new BGLiveUpdateLoaderWebClient(context.timeOut);
            }
        }


        /// <summary>
        /// Loads data for specified table 
        /// </summary>
        public abstract void Load(BGMetaEntity meta, BGMergeSettingsEntity actualSettings, BGLiveUpdateUrl url = null, bool applyLastDataOnFailure = false);

        /// <summary>
        /// Called after all requests are completed
        /// </summary>
        public virtual void Complete()
        {
            loader.Complete();
        }

        // Called when error occurs 
        protected void Error(BGMergeSettingsEntity actualSettings, BGMetaEntity meta, Exception ex)
        {
            var error = ex.Message ?? "unknown error: " + ex.GetType().FullName;
            addon.Log.InvalidMetaCount++;
            addon.Log.SetError(meta.Id, new BGException(error));
            Debug.LogError("Error while loading meta " + error);
            //if meta can not be loaded- exclude it from merging
            actualSettings.ExcludeMeta(meta.Id);
        }

        // Called when error occurs 
        protected void Error(BGMergeSettingsEntity actualSettings, BGMetaEntity meta, string error)
        {
            error = error ?? "unknown error";
            addon.Log.InvalidMetaCount++;
            var exception = new BGException("Error while loading meta '" + meta.Name + "': " + error);
            addon.Log.SetError(meta.Id, exception);
            if (!BGUtil.TestIsRunning) Debug.LogException(exception);
            //if meta can not be loaded- exclude it from merging
            actualSettings.ExcludeMeta(meta.Id);
        }

        //shortcut for AddDetail log method 
        protected void LogDetail(string message, params object[] parameters) => addon.Log.AddDetail(message, parameters);

        // Map field names to the actual database fields
        protected BGLiveUpdateDataWithOrigin MapFields(BGMergeSettingsEntity actualSettings, BGMetaEntity meta, string[] fieldNames)
        {
            LogDetail("==== Mapping for '$' table started, reading headers (field names)..", meta.Name);
            if (fieldNames == null || fieldNames.Length == 0)
            {
                Error(actualSettings, meta, "No field names");
                return null;
            }

            var fieldId2Column = new BGIdDictionary<KeyValuePair<int, BGField>>();
            var idColumn = -1;
            var idFound = false;
            for (var i = 0; i < fieldNames.Length; i++)
            {
                var fieldName = fieldNames[i];
                if (string.IsNullOrEmpty(fieldName) || string.IsNullOrEmpty(fieldName.Trim())) continue;

                var field = MapField(actualSettings, meta, fieldName, i, ref idFound, id => fieldId2Column.ContainsKey(id));
                if (idFound && idColumn < 0)
                {
                    idColumn = i;
                    continue;
                }

                if (field == null) continue;
                fieldId2Column.Add(field.Id, new KeyValuePair<int, BGField>(i, field));
                LogDetail("Index $. Header $. Field mapped ok.", i, fieldName);
            }

            /*
             // NO MORE REQUIRED
            //no id column
            if (idColumn < 0)
            {
                LogDetail("No _id column is found. This column is required! Aborting..");
                return null;
            }
            */
            if (idColumn < 0) LogDetail("no _id column- all rows will be considered as new ones!");

            //no data
            if (fieldId2Column.Count == 0)
            {
                LogDetail("No field was mapped! Fields are resolved by name (from the first row). You need at least one field to be mapped properly. Aborting..");
                return null;
            }

            var fields = new BGField[fieldId2Column.Count];
            var indexes = new int[fieldId2Column.Count];
            var ind = 0;
            foreach (var pair in fieldId2Column)
            {
                indexes[ind] = pair.Value.Key;
                fields[ind] = pair.Value.Value;
                ind++;
            }

            var data = new BGLiveUpdateDataWithOrigin(meta, fields, indexes, idColumn);
            LogDetail("==== Mapping for '$' table ended.", meta.Name);
            return data;
        }


        // Map one single field name to the actual database field
        private BGField MapField(BGMergeSettingsEntity actualSettings, BGMetaEntity meta, string fieldName, int index, ref bool idFound, Func<BGId, bool> checkDuplicate)
        {
            if ("_id".Equals(fieldName) && !idFound)
            {
                idFound = true;
                LogDetail("Index $. Id column mapped ok.", index);
            }
            else
            {
                var field = meta.GetField(fieldName, false);

                //no field
                if (field == null)
                {
                    LogDetail("Index $. Header $. Field with such name can not be found or not included in settings . Skipping..", index, fieldName);
                    return null;
                }

                //duplicate
                if (checkDuplicate(field.Id))
                {
                    LogDetail("Index $. Header $. Duplicate (the same name was already mapped). Skipping..", index, fieldName);
                    return null;
                }

                //not included in settings
                if (!actualSettings.IsFieldIncluded(field))
                {
                    LogDetail("Index $. Header $. Field is not included into settings. Skipping..", index, fieldName);
                    return null;
                }

                return field;
            }

            return null;
        }

        //read BGId from string value
        protected BGId ReadId(string idString, int index)
        {
            if (index < 0) return BGId.Empty;
            if (idString == null || idString.Trim().Equals(string.Empty))
                // LogDetail("id value is null for index $!", index);
                return BGId.Empty;

            try
            {
                return new BGId(idString);
            }
            catch (Exception)
            {
                //invalid id- return
                LogDetail("id value is invalid ($) for index $! This row will be considered as new one", idString, index);
                return BGId.Empty;
            }
        }


        /// <summary>
        /// LiveUpdate remote data with additional fields indexes and ID index 
        /// </summary>
        public class BGLiveUpdateDataWithOrigin : BGLiveUpdateDataProcessor.BGLiveUpdateData
        {
            private readonly int[] indexes;
            private readonly int idIndex = -1;

            public int[] Indexes => indexes;

            public int IdIndex => idIndex;

            public bool HasId => idIndex >= 0;


            public BGLiveUpdateDataWithOrigin(BGMetaEntity meta, BGField[] fields, int[] indexes, int idIndex) : base(meta, fields)
            {
                this.indexes = indexes;
                this.idIndex = idIndex;
            }
        }
    }
}