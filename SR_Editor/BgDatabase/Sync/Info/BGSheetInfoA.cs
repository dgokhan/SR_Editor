/*
<copyright file="BGSheetInfoA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    //================================================================================================
    //                                              Sheet abstract
    //================================================================================================
    /// <summary>
    /// Abstract class for individual sheet info
    /// </summary>
    public abstract partial class BGSheetInfoA : ICloneable
    {
        //row ID mapped to row index
        protected readonly BGIdDictionary<int> Id2Row = new BGIdDictionary<int>();
        //row index mapped to row ID
        protected readonly Dictionary<int, BGId> Row2Id = new Dictionary<int, BGId>();

        public readonly int SheetNumber;

        /// <summary>
        /// Number of mapped rows
        /// </summary>
        public int RowCount => Id2Row.Count;

        protected BGSheetInfoA(int sheetNumber)
        {
            SheetNumber = sheetNumber;
        }


        /// <summary>
        /// Clears internal state
        /// </summary>
        public virtual void Clear()
        {
            Id2Row.Clear();
            Row2Id.Clear();
        }

        /// <inheritdoc/>
        public abstract object Clone();

        /// <summary>
        /// Add row index for the row with provided ID
        /// </summary>
        public void AddRow(BGId entityId, int rowIndex)
        {
            if (HasRow(entityId)) return;

            Id2Row[entityId] = rowIndex;
            Row2Id[rowIndex] = entityId;
        }

        /// <summary>
        /// Does this container has information for the entity with provided ID
        /// </summary>
        public bool HasRow(BGId entityId) => Id2Row.ContainsKey(entityId);

        /// <summary>
        /// Get row's index for the row with provided ID
        /// </summary>
        public int GetRow(BGId entityId)
        {
            if (!Id2Row.TryGetValue(entityId, out var index)) return -1;
            return index;
        }

        /// <summary>
        /// Get row's ID for the row with provided index
        /// </summary>
        public BGId GetRowId(int index)
        {
            if (!Row2Id.TryGetValue(index, out var id)) return BGId.Empty;
            return id;
        }

        /// <summary>
        /// Remove mapping info for the row with provided ID
        /// </summary>
        public void RemoveRow(BGId entityId)
        {
            if (!Id2Row.TryGetValue(entityId, out var index)) return;
            Id2Row.Remove(entityId);
            Row2Id.Remove(index);
        }

        /// <summary>
        /// Clone internal information to provided container
        /// </summary>
        protected void Clone(BGSheetInfoA to)
        {
            foreach (var pair in Id2Row) to.Id2Row.Add(pair.Key, pair.Value);
            foreach (var pair in Row2Id) to.Row2Id.Add(pair.Key, pair.Value);
        }

        /// <summary>
        /// Iterate over all mapped rows
        /// </summary>
        public void ForEachRow(Action<BGId, int> action)
        {
            foreach (var pair in Id2Row) action(pair.Key, pair.Value);
        }
    }
}