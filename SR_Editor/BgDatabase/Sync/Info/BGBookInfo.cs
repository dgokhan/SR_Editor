/*
<copyright file="BGBookInfo.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Data container for spreadsheet or Excel file.
    /// It has all information for every sheet
    /// </summary>
    public partial class BGBookInfo : ICloneable, BGMergerEntity.ParseResultI
    {
        public const string IdHeader = "_id";

        private readonly BGIdDictionary<BGEntitySheetInfo> metaId2EntitySheet = new BGIdDictionary<BGEntitySheetInfo>();
        private readonly List<BGEntitySheetInfo> entitySheets = new List<BGEntitySheetInfo>();
        private BGMetaSheetInfo metaSheet;
        private BGFieldSheetInfo fieldSheet;
        private BGAddonSheetInfo addonSheet;

        //================================================================================================
        //                                              Meta
        //================================================================================================

        /// <summary>
        /// NOT CURRENTLY USED. 
        /// </summary>
        public BGMetaSheetInfo MetaSheet
        {
            get => metaSheet;
            set => metaSheet = value;
        }

        //================================================================================================
        //                                              Field
        //================================================================================================

        /// <summary>
        /// NOT CURRENTLY USED. 
        /// </summary>
        public BGFieldSheetInfo FieldSheet
        {
            get => fieldSheet;
            set => fieldSheet = value;
        }

        //================================================================================================
        //                                              Addon
        //================================================================================================
        /// <summary>
        /// NOT CURRENTLY USED. 
        /// </summary>
        public BGAddonSheetInfo AddonSheet
        {
            get => addonSheet;
            set => addonSheet = value;
        }

        //================================================================================================
        //                                              Entity sheets
        //================================================================================================

        /// <summary>
        /// Number of sheets
        /// </summary>
        public int EntitySheetCount => metaId2EntitySheet.Count;

        /// <summary>
        /// Does the sheet with provided table ID have a field with provided field ID
        /// </summary>
        public bool HasFieldInEntitySheet(BGId metaId, BGId fieldId)
        {
            var sheetInfo = GetEntitySheet(metaId);
            return sheetInfo != null && sheetInfo.HasField(fieldId);
        }

        /// <summary>
        /// Does this container have information for the table with provided ID 
        /// </summary>
        public bool HasEntitySheet(BGId metaId) => metaId2EntitySheet.ContainsKey(metaId);

        /// <summary>
        /// Get the sheet for the table with provided ID 
        /// </summary>
        public BGEntitySheetInfo GetEntitySheet(BGId metaId) => !metaId2EntitySheet.TryGetValue(metaId, out var result) ? null : result;

        /// <summary>
        /// Get the sheet with provided index
        /// </summary>
        public BGEntitySheetInfo GetEntitySheet(int index) => entitySheets[index];

        /// <summary>
        /// Add the sheet data for the table with provided ID 
        /// </summary>
        public void AddEntitySheet(BGId metaId, BGEntitySheetInfo entitySheet)
        {
            metaId2EntitySheet[metaId] = entitySheet;
            entitySheets.Add(entitySheet);
        }

/*
        public BGEntitySheetInfo EnsureEntitySheet(BGId metaId)
        {
            if (!metaId2EntitySheet.ContainsKey(metaId)) metaId2EntitySheet[metaId] = new BGEntitySheetInfo(metaId, 0);
            return metaId2EntitySheet[metaId];
        }
*/

        /// <summary>
        /// Iterate the sheets with data. Action is called for each sheet
        /// </summary>
        public void ForEachEntitySheet(Action<BGEntitySheetInfo> action)
        {
            foreach (var sheet in entitySheets) action(sheet);
        }

        //================================================================================================
        //                                              Methods
        //================================================================================================

        /// <summary>
        /// Creates a clone object
        /// </summary>
        public object Clone()
        {
            var clone = new BGBookInfo();
            foreach (var sheet in entitySheets) clone.AddEntitySheet(sheet.MetaId, sheet.Clone() as BGEntitySheetInfo);
            if (metaSheet != null) clone.metaSheet = (BGMetaSheetInfo)metaSheet.Clone();
            if (fieldSheet != null) clone.fieldSheet = (BGFieldSheetInfo)fieldSheet.Clone();
            if (addonSheet != null) clone.addonSheet = (BGAddonSheetInfo)addonSheet.Clone();
            return clone;
        }

        //clear information 
        protected virtual void Clear()
        {
            metaId2EntitySheet.Clear();
            entitySheets.Clear();
        }
    }
}