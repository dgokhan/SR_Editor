/*
<copyright file="BGCalcTypeCodeCell.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// type code for database cell type
    /// </summary>
    public class BGCalcTypeCodeCell : BGCalcTypeCode<BGCalcCell>
    {
        public const byte Code = 16;

        /// <inheritdoc />
        public override bool SupportDefaultValue => true;
        
        /// <inheritdoc />
        public override byte TypeCode => Code;
        
        /// <inheritdoc />
        public override object DefaultValue => null;
        
        /// <inheritdoc />
        public override string Name => "cell";

        /// <inheritdoc />
        public override void ValueToBytes(BGBinaryWriter writer, object value)
        {
            var cell = (BGCalcCell)value;
            if (cell?.Field == null || cell.Entity == null)
            {
                writer.AddId(BGId.Empty);
                writer.AddId(BGId.Empty);
                writer.AddId(BGId.Empty);
            }
            else
            {
                writer.AddId(cell.Field.MetaId);
                writer.AddId(cell.Field.Id);
                writer.AddId(cell.Entity.Id);
            }
        }

        /// <inheritdoc />
        public override object ValueFromBytes(BGBinaryReader reader)
        {
            var metaId = reader.ReadId();
            var fieldId = reader.ReadId();
            var entityId = reader.ReadId();
            if (!metaId.IsEmpty && !fieldId.IsEmpty && !entityId.IsEmpty)
            {
                var meta = BGRepo.I.GetMeta(metaId);
                var field = meta?.GetField(fieldId, false);
                if (field != null)
                {
                    var entity = meta.GetEntity(entityId);
                    if (entity != null) return new BGCalcCell(field, entity);
                }
            }

            return null;
        }

        /// <inheritdoc />
        public override string ValueToString(object value)
        {
            var cell = (BGCalcCell)value;
            if (cell?.Field != null && cell.Entity != null)
            {
                var jsonCell = new JsonCell
                {
                    MetaId = cell.Field.MetaId.ToString(),
                    FieldId = cell.Field.Id.ToString(),
                    EntityId = cell.Entity.Id.ToString()
                };
                return JsonUtility.ToJson(jsonCell);
            }

            return null;
        }

        /// <inheritdoc />
        public override object ValueFromString(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            var json = JsonUtility.FromJson<JsonCell>(value);
            if (json != null)
                if (BGId.TryParse(json.MetaId, out var metaId) && BGId.TryParse(json.FieldId, out var fieldId) && BGId.TryParse(json.EntityId, out var entityId))
                {
                    var meta = BGRepo.I.GetMeta(metaId);
                    var field = meta?.GetField(fieldId, false);
                    if (field != null)
                    {
                        var entity = meta.GetEntity(entityId);
                        if (entity != null) return new BGCalcCell(field, entity);
                    }
                }

            return null;
        }

        [Serializable]
        private class JsonCell
        {
            public string MetaId;
            public string FieldId;
            public string EntityId;
        }
    }
}