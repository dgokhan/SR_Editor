/*
<copyright file="BGEntitySheetInfo.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    //================================================================================================
    //                                              Sheet with entities
    //================================================================================================
    /// <summary>
    /// Data container for one single sheet with data
    /// </summary>
    public partial class BGEntitySheetInfo : BGSheetInfoA
    {
        private readonly BGIdDictionary<int> fieldId2Column = new BGIdDictionary<int>();
        private readonly BGId metaId;
        private readonly string name;
        public string SheetName;

        private int indexId = -1;

        public BGId MetaId => metaId;

        public string Name => name;

        /// <summary>
        /// Index for ID column
        /// </summary>
        public int IndexId
        {
            get => indexId;
            set
            {
                if (indexId >= 0) return;
                indexId = value;
            }
        }

        /// <summary>
        /// Does this container have any data or empty
        /// </summary>
        public bool HasAnyData => fieldId2Column.Count > 0 || HasId;


        public BGEntitySheetInfo(BGId metaId, string name, int sheetNumber) : base(sheetNumber)
        {
            this.metaId = metaId;
            this.name = name;
        }


        /// <inheritdoc/>
        public override object Clone()
        {
            var clone = new BGEntitySheetInfo(metaId, name, SheetNumber)
            {
                indexId = indexId,
                SheetName = SheetName
            };
            Clone(clone);
            foreach (var pair in fieldId2Column) clone.fieldId2Column.Add(pair.Key, pair.Value);
            return clone;
        }


        /// <summary>
        /// Get the column index for the field with provided ID
        /// </summary>
        public int GetFieldColumn(BGId fieldId)
        {
            if (!fieldId2Column.TryGetValue(fieldId, out var index)) return -1;
            return index;
        }

        /// <summary>
        /// Does this container have data for the field with provided ID
        /// </summary>
        public bool HasField(BGId fieldId) => fieldId2Column.ContainsKey(fieldId);

        /// <summary>
        /// Assign the index for the field with provided ID (if not already assigned)
        /// </summary>
        public void AddField(BGId fieldId, int columnIndex)
        {
            if (HasField(fieldId)) return;

            fieldId2Column[fieldId] = columnIndex;
        }

        /// <summary>
        /// Iterate all assigned fields
        /// </summary>
        public void ForEachField(Action<BGId, int> action)
        {
            foreach (var pair in fieldId2Column) action(pair.Key, pair.Value);
        }


        /// <summary>
        /// Does it have ID column
        /// </summary>
        public bool HasId => IndexId >= 0;

        /// <summary>
        /// All assigned field IDs
        /// </summary>
        public List<BGId> FieldIds => new List<BGId>(fieldId2Column.Keys);

        /// <summary>
        /// All assigned rows IDs
        /// </summary>
        public List<BGId> EntityIds => new List<BGId>(Id2Row.Keys);

        /// <summary>
        /// Physical columns count
        /// </summary>
        public int PhysicalColumnCount { get; set; }
        /// <summary>
        /// Physical rows count
        /// </summary>
        public int PhysicalRowCount { get; set; }

        /// <summary>
        /// Max column index
        /// </summary>
        public int MaxColumn
        {
            get
            {
                var max = -1;
                max = Math.Max(max, indexId);
                foreach (var pair in fieldId2Column) max = Math.Max(max, pair.Value);
                return max;
            }
        }

        /// <summary>
        /// Number of assigned fields
        /// </summary>
        public int FieldsCount => fieldId2Column.Count;

        /// <summary>
        /// Set the index for the field with provided ID
        /// </summary>
        public void SetField(BGId fieldId, int column) => fieldId2Column[fieldId] = column;

        /// <summary>
        /// Set the row's index for the row with provided ID
        /// </summary>
        public void SetEntity(BGId entityId, int row)
        {
            Id2Row[entityId] = row;
            Row2Id[row] = entityId;
        }

        /// <inheritdoc/>
        public override void Clear()
        {
            base.Clear();
            fieldId2Column.Clear();
            indexId = -1;
        }

        public List<Tuple<BGField, int>> GetFieldsInfo(BGMetaEntity meta)
        {
            var result = new List<Tuple<BGField, int>>();
            foreach (var pair in fieldId2Column) result.Add(Tuple.Create<BGField, int>(meta.GetField(pair.Key), pair.Value));
            return result;
        }

        public static BGField[] GetFieldsArray(List<Tuple<BGField, int>> list)
        {
            var result = new BGField[list.Count];
            for (var i = 0; i < list.Count; i++) result[i] = list[i].Item1;

            return result;
        }
    }
}