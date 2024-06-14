/*
<copyright file="BGCalcTypeCodeEntity.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// type code for database row type
    /// </summary>
    public class BGCalcTypeCodeEntity : BGCalcTypeCode<BGEntity>
    {
        public const byte Code = 15;

        /// <inheritdoc />
        public override bool SupportDefaultValue => true;
        
        /// <inheritdoc />
        public override byte TypeCode => Code;
        
        /// <inheritdoc />
        public override object DefaultValue => null;
        
        /// <inheritdoc />
        public override string Name => "entity";

        /// <inheritdoc />
        public override bool CanBeConvertedFrom(BGCalcTypeCode otherCode) => otherCode is BGCalcTypeCodeEntityRuntime;

        /// <inheritdoc />
        public override object ConvertFrom(BGCalcTypeCode otherCode, object value)
        {
            if (otherCode == null) return value;
            switch (otherCode)
            {
                case BGCalcTypeCodeEntityRuntime runtimeEntity:
                {
                    return value;
                }
            }

            return value;
        }

        /// <inheritdoc />
        public override void ValueToBytes(BGBinaryWriter writer, object value)
        {
            var entity = (BGEntity)value;
            if (entity == null)
            {
                writer.AddId(BGId.Empty);
                writer.AddId(BGId.Empty);
            }
            else
            {
                writer.AddId(entity.MetaId);
                writer.AddId(entity.Id);
            }
        }

        /// <inheritdoc />
        public override object ValueFromBytes(BGBinaryReader reader)
        {
            var metaId = reader.ReadId();
            var entityId = reader.ReadId();
            if (!metaId.IsEmpty && !entityId.IsEmpty)
            {
                var meta = BGRepo.I.GetMeta(metaId);
                if (meta != null) return meta.GetEntity(entityId);
            }

            return null;
        }

        /// <inheritdoc />
        public override string ValueToString(object value)
        {
            var jsonValue = new JsonValue();
            var entity = (BGEntity)value;
            if (entity != null)
            {
                jsonValue.MetaId = entity.MetaId.ToString();
                jsonValue.EntityId = entity.Id.ToString();
            }

            return JsonUtility.ToJson(jsonValue);
        }

        /// <inheritdoc />
        public override object ValueFromString(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;

            var json = JsonUtility.FromJson<JsonValue>(value);
            if (BGId.TryParse(json.MetaId, out var metaId) && BGId.TryParse(json.EntityId, out var entityId))
            {
                var meta = BGRepo.I.GetMeta(metaId);
                if (meta != null) return meta.GetEntity(entityId);
            }

            return null;
        }

        [Serializable]
        private class JsonValue
        {
            public string MetaId;
            public string EntityId;
        }
    }
}