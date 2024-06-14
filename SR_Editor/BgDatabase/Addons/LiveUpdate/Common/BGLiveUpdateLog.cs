/*
<copyright file="BGLiveUpdateLog.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Data container for log messages and statistics
    /// </summary>
    public class BGLiveUpdateLog
    {
        /// <summary>
        /// enum for the level of logging details 
        /// </summary>
        public enum LogLevelEnum
        {
            Summary = 0,
            SummaryByMeta = 10,
            Detailed = 20,
            Full = 100
        }

        /// <summary>
        /// enum for the global status
        /// </summary>
        public enum StatusEnum
        {
            NotLoaded,
            LoadAttempted,
            LoadNotSupported
        }

        private readonly StringBuilder detailsBuilder = new StringBuilder();
        private readonly LogLevelEnum level;
        private readonly Dictionary<BGId, MetaInfo> id2MetaInfo = new Dictionary<BGId, MetaInfo>();
        private readonly List<string> errors = new List<string>();

        /// <summary>
        /// the number of tables, successfully updated 
        /// </summary>
        public int OkMetaCount { get; set; }
        
        /// <summary>
        /// the number of tables, which failed to update 
        /// </summary>
        public int InvalidMetaCount { get; set; }
        
        /// <summary>
        /// the number of cells, successfully updated 
        /// </summary>
        public int OkCellsCount { get; set; }
        
        /// <summary>
        /// the number of cells, which failed to update 
        /// </summary>
        public int InvalidCellsCount { get; set; }

        /// <summary>
        /// The target repository
        /// </summary>
        public BGRepo Repo { get; set; }

        /// <summary>
        /// Global status for update operation
        /// </summary>
        public StatusEnum Status { get; set; }

        /// <summary>
        /// Current level of logging details
        /// </summary>
        public LogLevelEnum Level => level;

        /// <summary>
        /// Global exception
        /// </summary>
        public string Exception;

        public BGLiveUpdateLog(LogLevelEnum level)
        {
            this.level = level;
        }

        /// <summary>
        /// Was the specified table was updated correctly 
        /// </summary>
        public bool IsMetaLoadedOk(BGId metaId)
        {
            if (Status != StatusEnum.LoadAttempted) return false;
            if (Exception != null) return false;
            var details = GetMetaDetails(metaId);
            if (details == null) return true;
            return !details.IsError;
        }

        /// <summary>
        /// Mark specified table as failed to updated 
        /// </summary>
        public void SetError(BGId metaId, Exception exception)
        {
            var error = exception.Message ?? exception.GetType().FullName;
            errors.Add(error);

            var metaInfo = EnsureMetaInfo(metaId);
            metaInfo.Error = error;

            if (level == LogLevelEnum.Full) AddDetail("ERROR STACKTRACE:" + exception.StackTrace);
        }

        //ensure that the meta info was added for specified meta
        private MetaInfo EnsureMetaInfo(BGId metaId)
        {
            if (id2MetaInfo.TryGetValue(metaId, out var result)) return result;
            result = new MetaInfo(level);
            id2MetaInfo.Add(metaId, result);
            return result;
        }

        /// <summary>
        /// get the object with details for the specified meta 
        /// </summary>
        public MetaInfo GetMetaDetails(BGId metaId)
        {
            return BGUtil.Get(id2MetaInfo, metaId);
        }

        /// <summary>
        /// Add a message if logging level>=Detailed
        /// </summary>
        public void AddDetail(string message, params object[] parameters)
        {
            if (level < LogLevelEnum.Detailed) return;
            detailsBuilder.AppendLine(BGUtil.Format(message, parameters));
        }

        /// <summary>
        /// Add a warning message if logging level>=Detailed
        /// </summary>
        public void AddWarning(string message, params object[] parameters)
        {
            AddDetail("WARNING:" + message, parameters);
        }

        /// <summary>
        /// Add detailed message if detail level=Full  
        /// </summary>
        public void AddFullDetail(string message, params object[] parameters)
        {
            if (level < LogLevelEnum.Full) return;
            AddDetail(message, parameters);
        }

        /// <summary>
        /// Mark cell as failed and adds the message about the failure reason if detail level>=SummaryByMeta
        /// </summary>
        public void AddCellFailed(BGId metaId, BGId entityId, BGId fieldId, string message, params object[] parameters)
        {
            InvalidCellsCount++;
            if (level < LogLevelEnum.SummaryByMeta) return;
            var metaInfo = EnsureMetaInfo(metaId);
            metaInfo.InvalidCellsCount++;
            metaInfo.AddCellError(entityId, fieldId);

            if (level < LogLevelEnum.Detailed) return;
            AddWarning(message, parameters);
        }

        /// <summary>
        /// Mark cell as successfully updated and adds the message about the failure reason if detail level>=SummaryByMeta
        /// </summary>
        public void AddCellSuccess(BGId metaId, string message, params object[] parameters)
        {
            OkCellsCount++;
            if (level < LogLevelEnum.SummaryByMeta) return;
            EnsureMetaInfo(metaId).OkCellsCount++;

            if (level < LogLevelEnum.Full) return;
            AddDetail(message, parameters);
        }

        /// <summary>
        /// Prints a log to Unity console
        /// </summary>
        public void PrintToConsole()
        {
            Debug.Log(GetLog());
        }

        /// <summary>
        /// Get the log as string 
        /// </summary>
        public string GetLog()
        {
            var result = new StringBuilder();

            result.Append("BGDatabase.LiveUpdate plugin log: ");
            result.Append(" Status: ").Append(Status);
            result.Append(", Ok meta count: ").Append(OkMetaCount);
            result.Append(", Invalid meta count: ").Append(InvalidMetaCount);
            result.Append(", Ok cells count: ").Append(OkCellsCount);
            result.Append(", Invalid cells count: ").Append(InvalidCellsCount);
            result.AppendLine();
            if (!string.IsNullOrEmpty(Exception)) result.Append("Global ERROR: " + Exception); 

            if (level >= LogLevelEnum.SummaryByMeta)
            {
                var index = 0;
                foreach (var pair in id2MetaInfo)
                {
                    var meta = Repo.GetMeta(pair.Key);
                    if (meta == null)
                    {
                        result.AppendLine(BGUtil.Format("Meta: id=$. Error: can not find meta in repo", pair.Key));
                        continue;
                    }

                    var info = pair.Value;
                    result.Append(BGUtil.Format("Meta Summary # $. Name $. Status: $ ", index++, meta.Name, info.Error != null ? "ERROR: " + info.Error : "Loaded"));
                    result.Append(", Ok cells count: ").Append(info.OkCellsCount);
                    result.Append(", Invalid cells count: ").Append(info.InvalidCellsCount);
                    result.AppendLine();
                }
            }

            if (level >= LogLevelEnum.Detailed)
            {
                result.AppendLine("Detailed log:");
                result.AppendLine(detailsBuilder.ToString());
            }

            return result.ToString();
        }

        /// <summary>
        /// Clears all internal information
        /// </summary>
        public void Clear()
        {
            Status = StatusEnum.NotLoaded;
            Exception = null;
            detailsBuilder.Length = 0;
            id2MetaInfo.Clear();
            errors.Clear();
            OkMetaCount = 0;
            InvalidMetaCount = 0;
            OkCellsCount = 0;
            InvalidCellsCount = 0;
        }

        /// <summary>
        /// log/statistics data container for specific table 
        /// </summary>
        public class MetaInfo
        {
            private readonly Dictionary<BGId, HashSet<BGId>> invalidEntityId2FieldIds = new Dictionary<BGId, HashSet<BGId>>();
            private readonly LogLevelEnum level;

            public bool IsError => !string.IsNullOrEmpty(Error);

            /// <summary>
            /// Did data updating result in error
            /// </summary>
            public string Error { get; set; }

            /// <summary>
            /// The number of successfully updated cells
            /// </summary>
            public int OkCellsCount { get; set; }
            /// <summary>
            /// The number of cells, which failed to update
            /// </summary>
            public int InvalidCellsCount { get; set; }


            public MetaInfo(LogLevelEnum level)
            {
                this.level = level;
            }

            /// <summary>
            /// Does specified entity have at least one error 
            /// </summary>
            public bool IsEntityHasInvalidValues(BGId entityId)
            {
                CheckMinLevel();
                return invalidEntityId2FieldIds.ContainsKey(entityId);
            }

            /// <summary>
            /// Returns all fields IDs which failed to update 
            /// </summary>
            public HashSet<BGId> GetInvalidFields(BGId entityId)
            {
                CheckMinLevel();
                return BGUtil.Get(invalidEntityId2FieldIds, entityId);
            }

            /// <summary>
            /// Returns all entity IDs which failed to update 
            /// </summary>
            public HashSet<BGId> GetInvalidEntities()
            {
                CheckMinLevel();
                var result = new HashSet<BGId>();
                foreach (var pair in invalidEntityId2FieldIds) result.Add(pair.Key);
                return result;
            }

            /// <summary>
            /// Was the cell updated correctly  
            /// </summary>
            public bool IsFieldInvalid(BGId entityId, BGId fieldId)
            {
                CheckMinLevel();
                if (!invalidEntityId2FieldIds.TryGetValue(entityId, out var set)) return false;
                if (set == null) return false;
                return set.Contains(fieldId);
            }

            private void CheckMinLevel()
            {
                if (level < LogLevelEnum.Detailed)
                    throw new Exception("You can not use this method with log level=" + level.ToString() + " . Assign at least 'Detailed' level in LiveUpdate addon settings page!");
            }

            /// <summary>
            /// add the error for specified cell  
            /// </summary>
            public void AddCellError(BGId entityId, BGId fieldId)
            {
                if (!invalidEntityId2FieldIds.TryGetValue(entityId, out var set))
                {
                    set = new HashSet<BGId>();
                    invalidEntityId2FieldIds.Add(entityId, set);
                }

                set.Add(fieldId);
            }
        }
    }
}