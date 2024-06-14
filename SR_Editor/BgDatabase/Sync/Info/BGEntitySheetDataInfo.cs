/*
<copyright file="BGEntitySheetDataInfo.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    public class BGEntitySheetDataInfo
    {
        private readonly BGField[] fields;
        private readonly List<RowData> rowsData = new List<RowData>();

        public int FieldsCount => fields.Length;
        public int RowsCount => rowsData.Count;

        public BGEntitySheetDataInfo(BGField[] fields)
        {
            this.fields = fields;
        }

        public BGField GetField(int index) => fields[index];
        
        public void AddRow(RowData data)
        {
            if (fields.Length != data.Data.Length) throw new Exception($"counts mismatch {fields.Length} != {data.Data.Length}");
            rowsData.Add(data);
        }

        public RowData GetRow(int index) => rowsData[index];

        public class RowData
        {
            public BGId EntityId { get; set; }

            public string[] Data { get; }
            
            public object ExtraData { get; }
            public BGEntity Entity { get; set; }

            public RowData(BGId entityId, string[] data, object extraData)
            {
                EntityId = entityId;
                Data = data ?? throw new Exception("data can not be null");
                ExtraData = extraData;
            }

            public string GetValue(int index) => Data[index];
        }
    }
}