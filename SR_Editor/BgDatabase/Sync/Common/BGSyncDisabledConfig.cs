/*
<copyright file="BGSyncDisabledConfig.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// data container for disabled tables and fields
    /// </summary>
    [Serializable]
    public class BGSyncDisabledConfig
    {
        [SerializeField] public List<string> IgnoreTables = new List<string>();
        [SerializeField] public List<MetaMap> IgnoreFields = new List<MetaMap>();

        /// <summary>
        /// data container for single table
        /// </summary>
        [Serializable]
        public class MetaMap
        {
            [SerializeField] public string MetaName;
            [SerializeField] public List<string> Fields;

            public MetaMap(string name) => MetaName = name;

            /// <summary>
            ///  Does settings has the field with provided name
            /// </summary>
            public bool HasField(string fieldName)
            {
                if (Fields == null) return false;
                return Fields.Find(s => string.Equals(s, fieldName)) != null;
            }
            
            /// <summary>
            ///  remove provided field from config
            /// </summary>
            public void RemoveField(string fieldName)
            {
                if (Fields == null) return;
                Fields.RemoveAll(s => string.Equals(s, fieldName));
            }
        }

        /// <summary>
        /// Does config has the table with provided name
        /// </summary>
        public bool HasTable(string tableName)
        {
            if (IgnoreTables == null) return false;
            return IgnoreTables.Find(s => string.Equals(tableName, s)) != null;
        }

        /// <summary>
        /// Get config for meta with provided name
        /// </summary>
        public MetaMap GetTableWithFields(string tableName)
        {
            if (IgnoreFields == null) return null;
            return IgnoreFields.Find(s => string.Equals(tableName, s.MetaName));
        }

        /// <summary>
        /// Does config have table config for meta with provided name
        /// </summary>
        public bool HasTableWithFields(string tableName) => GetTableWithFields(tableName) != null;

        /// <summary>
        /// Set the sheet with provided name as enabled/disabled
        /// </summary>
        public void SetDisabled(string sheetName, bool disabled)
        {
            if (disabled)
            {
                if (HasTable(sheetName)) return;
                IgnoreTables = IgnoreTables ?? new List<string>();
                IgnoreTables.Add(sheetName);
            }
            else
            {
                if (!HasTable(sheetName)) return;
                IgnoreTables.RemoveAll(s => string.Equals(s, sheetName));
            }
        }

        /// <summary>
        /// Does config have data for provided sheet and provided field
        /// </summary>
        public bool HasField(string sheetName, string fieldName)
        {
            var metaMap = GetTableWithFields(sheetName);
            return metaMap?.HasField(fieldName) ?? false;
        }

        /// <summary>
        /// Set provided sheet+field as enabled/disabled
        /// </summary>
        public void SetDisabled(string sheetName, string fieldName, bool disabled)
        {
            if (disabled)
            {
                if (HasField(sheetName, fieldName)) return;
                var metaMap = GetTableWithFields(sheetName);
                if (metaMap == null)
                {
                    metaMap = new MetaMap(sheetName);
                    IgnoreFields = IgnoreFields ?? new List<MetaMap>();
                    IgnoreFields.Add(metaMap);
                }

                metaMap.Fields = metaMap.Fields ?? new List<string>();
                metaMap.Fields.Add(fieldName);
            }
            else
            {
                if (!HasField(sheetName, fieldName)) return;
                var table = GetTableWithFields(sheetName);
                table.RemoveField(fieldName);
            }
        }
    }
}