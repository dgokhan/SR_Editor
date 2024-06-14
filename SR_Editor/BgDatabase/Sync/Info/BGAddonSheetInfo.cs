/*
<copyright file="BGAddonSheetInfo.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    //================================================================================================
    //                                              Sheet with addon
    //================================================================================================

    /// <summary>
    /// NOT CURRENTLY USED. 
    /// </summary>
    public partial class BGAddonSheetInfo : BGSheetInfoA
    {
        private readonly Dictionary<string, int> addonType2Row = new Dictionary<string, int>();

        public int IndexType = -1;
        public int IndexConfig = -1;

        public BGAddonSheetInfo(int sheetNumber) : base(sheetNumber)
        {
        }

        public int AddonCount => addonType2Row.Count;

        public void AddAddon(string type, int rowIndex)
        {
            if (HasAddon(type)) return;

            addonType2Row[type] = rowIndex;
        }

        public bool HasAddon(string type)
        {
            return addonType2Row.ContainsKey(type);
        }

        public int GetAddonRow(string type)
        {
            if (!addonType2Row.ContainsKey(type)) return -1;
            return BGUtil.Get(addonType2Row, type);
        }

        public override void Clear()
        {
            addonType2Row.Clear();
            IndexType = -1;
            IndexConfig = -1;
        }

        public override object Clone()
        {
            var clone = new BGAddonSheetInfo(SheetNumber)
            {
                IndexType = IndexType,
                IndexConfig = IndexConfig
            };
            foreach (var pair in addonType2Row) clone.addonType2Row.Add(pair.Key, pair.Value);
            return clone;
        }

        public void ForEachAddon(Action<string, int> action)
        {
            foreach (var pair in addonType2Row) action(pair.Key, pair.Value);
        }

        public void RemoveAddon(string type)
        {
            addonType2Row.Remove(type);
        }
    }
}