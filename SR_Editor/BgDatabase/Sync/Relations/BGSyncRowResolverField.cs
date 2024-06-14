/*
<copyright file="BGSyncRowResolverField.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    public class BGSyncRowResolverField : BGSyncRowResolver
    {
        private readonly MetaData metaData1;
        private readonly MetaData metaData2;
        private readonly BGId fieldId;

        public BGId MetaId => metaData1.Meta?.Id ?? metaData2.Meta.Id;
        public string MetaName => metaData1.Meta?.Name ?? metaData2.Meta.Name;

        public BGSyncRowResolverField(BGMetaEntity meta1, BGMetaEntity meta2, BGId fieldId, BGLogger logger, bool printWarnings)
        {
            metaData1 = new MetaData(meta1, fieldId, logger, printWarnings);
            metaData2 = new MetaData(meta2, fieldId, logger, printWarnings);
            this.fieldId = fieldId;
        }


        public BGRowRef FromString(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            value = value.Trim();
            var rowRef = metaData1.StringToRowRef(value);
            return rowRef ?? metaData2.StringToRowRef(value);
        }

        public string ToString(BGId rowId)
        {
            if (rowId.IsEmpty) return null;
            var rowRef = metaData1.RowIdToString(rowId);
            return rowRef ?? metaData2.RowIdToString(rowId);
        }

        public override string ToString() => "Resolver by field, table=" + MetaName + ", ID field=" + fieldId;

        private class MetaData
        {
            private readonly BGMetaEntity meta;
            private readonly BGId fieldId;
            private readonly BGLogger logger;
            private readonly bool printWarnings;


            private Dictionary<object, BGId> value2Entity;
            private bool value2EntityInited;
            private Dictionary<BGId, object> entity2Value;
            private bool entity2ValueInited;

            public BGMetaEntity Meta => meta;
            public BGId MetaId => meta?.Id ?? BGId.Empty;

            public MetaData(BGMetaEntity meta, BGId fieldId, BGLogger logger, bool printWarnings)
            {
                this.meta = meta;
                this.fieldId = fieldId;
                this.logger = logger;
                this.printWarnings = printWarnings;
            }

            public BGRowRef StringToRowRef(string value)
            {
                if (!value2EntityInited) InitValueToRow();
                if (value2Entity != null && value2Entity.TryGetValue(value.Trim(), out var e1)) return new BGRowRef(meta.Id, e1);
                return null;
            }

            public string RowIdToString(BGId rowId)
            {
                if (!entity2ValueInited) InitRowToValue();
                if (entity2Value != null && entity2Value.TryGetValue(rowId, out var value)) return value?.ToString();
                return null;
            }

            private void InitValueToRow()
            {
                value2EntityInited = true;
                var field = meta?.GetField(fieldId, false);
                if (field == null) return;

                value2Entity = new Dictionary<object, BGId>();
                for (var i = 0; i < meta.CountEntities; i++)
                {
                    var entity = field.Meta.GetEntity(i);
                    var value = field.GetValue(i);
                    if (value == null) continue;
                    try
                    {
                        value2Entity.Add(value, entity.Id);
                    }
                    catch (ArgumentException e)
                    {
                        BGSyncUtil.AppendWarning(logger, printWarnings,
                            "RowResolver: duplicate ID value is detected! Row ID=$, field=$, duplicate ID value=$", entity.Id, field.FullName, value);
                    }
                }
            }

            private void InitRowToValue()
            {
                entity2ValueInited = true;
                var field = meta?.GetField(fieldId, false);
                if (field == null) return;

                entity2Value = new Dictionary<BGId, object>();
                for (var i = 0; i < meta.CountEntities; i++)
                {
                    var entity = field.Meta.GetEntity(i);
                    var value = field.GetValue(i);
                    entity2Value[entity.Id] = value;
                }
            }
        }
    }
}