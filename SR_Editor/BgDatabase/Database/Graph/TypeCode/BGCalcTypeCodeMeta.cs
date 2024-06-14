/*
<copyright file="BGCalcTypeCodeMeta.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// type code for database table type
    /// </summary>
    public class BGCalcTypeCodeMeta : BGCalcTypeCode<BGMetaEntity>
    {
        public const byte Code = 12;

        /// <inheritdoc />
        public override bool SupportDefaultValue => true;
        
        /// <inheritdoc />
        public override byte TypeCode => Code;
        
        /// <inheritdoc />
        public override object DefaultValue => null;

        /// <inheritdoc />
        public override string Name => "meta";

        /// <inheritdoc />
        public override void ValueToBytes(BGBinaryWriter writer, object value) => writer.AddId(((BGMetaEntity)value)?.Id ?? BGId.Empty);

        /// <inheritdoc />
        public override object ValueFromBytes(BGBinaryReader reader)
        {
            var metaId = reader.ReadId();
            return metaId.IsEmpty ? null : BGRepo.I.GetMeta(metaId);
        }

        /// <inheritdoc />
        public override string ValueToString(object value) => ((BGMetaEntity)value)?.Id.ToString();

        /// <inheritdoc />
        public override object ValueFromString(string value) => !BGId.TryParse(value, out var metaId) ? null : BGRepo.I.GetMeta(metaId);
    }
}