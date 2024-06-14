/*
<copyright file="BGCalcTypeCodeField.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// type code for database field type
    /// </summary>
    public class BGCalcTypeCodeField : BGCalcTypeCode<BGField>
    {
        public const byte Code = 14;

        /// <inheritdoc />
        public override bool SupportDefaultValue => true;
        
        /// <inheritdoc />
        public override byte TypeCode => Code;
        
        /// <inheritdoc />
        public override object DefaultValue => null;
        
        /// <inheritdoc />
        public override string Name => "field";

        /// <inheritdoc />
        public override void ValueToBytes(BGBinaryWriter writer, object value)
        {
            var field = (BGField)value;
            if (field == null)
            {
                writer.AddId(BGId.Empty);
                writer.AddId(BGId.Empty);
            }
            else
            {
                writer.AddId(field.MetaId);
                writer.AddId(field.Id);
            }
        }

        /// <inheritdoc />
        public override object ValueFromBytes(BGBinaryReader reader)
        {
            var metaId = reader.ReadId();
            var fieldId = reader.ReadId();
            if (!metaId.IsEmpty && !fieldId.IsEmpty)
            {
                var meta = BGRepo.I.GetMeta(metaId);
                if (meta != null) return meta.GetField(fieldId, false);
            }

            return null;
        }

        /// <inheritdoc />
        public override string ValueToString(object value)
        {
            var jsonValue = new JsonValue();
            var field = (BGField)value;
            if (field != null)
            {
                jsonValue.MetaId = field.MetaId.ToString();
                jsonValue.FieldId = field.Id.ToString();
            }

            return JsonUtility.ToJson(jsonValue);
        }

        /// <inheritdoc />
        public override object ValueFromString(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;

            var json = JsonUtility.FromJson<JsonValue>(value);
            if (BGId.TryParse(json.MetaId, out var metaId) && BGId.TryParse(json.FieldId, out var fieldId))
            {
                var meta = BGRepo.I.GetMeta(metaId);
                if (meta != null) return meta.GetField(fieldId, false);
            }

            return null;
        }

        [Serializable]
        private class JsonValue
        {
            public string MetaId;
            public string FieldId;
        }
    }
}