/*
<copyright file="BGCalcTypeCodeEntityRuntime.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// type code for database row type for specific table 
    /// </summary>
    public class BGCalcTypeCodeEntityRuntime : BGCalcTypeCode<BGEntity>, BGCalcTypeCodeStateful
    {
        private BGId metaId;
        private BGMetaEntity meta;

        public const byte Code = 8;
        
        /// <inheritdoc />
        public override bool SupportDefaultValue => true;
        
        /// <inheritdoc />
        public override byte TypeCode => Code;
        
        /// <inheritdoc />
        public override object DefaultValue => null;

        /// <inheritdoc />
        public override string TypeTitle => Name + (Meta == null ? "" : " [" + Meta.Name + "]");

        /// <summary>
        /// database table
        /// </summary>
        public BGMetaEntity Meta
        {
            get
            {
                if (meta != null && !meta.IsDeleted) return meta;
                meta = BGRepo.I.GetMeta(metaId);
                return meta;
            }
        }

        /// <inheritdoc />
        public override string Name => "row";

        //===========================================================================
        //                                      Constructor
        //===========================================================================
        internal BGCalcTypeCodeEntityRuntime()
        {
        }

        public BGCalcTypeCodeEntityRuntime(BGMetaEntity meta)
        {
            if (meta == null) throw new Exception($"meta can not be null!");
            metaId = meta.Id;
            this.meta = meta;
        }
        //===========================================================================
        //                                      Serialization
        //===========================================================================
        /// <inheritdoc />
        public override void ValueToBytes(BGBinaryWriter writer, object value) => writer.AddId(((BGEntity)value)?.Id ?? BGId.Empty);

        /// <inheritdoc />
        public override object ValueFromBytes(BGBinaryReader reader)
        {
            var eId = reader.ReadId();
            if (eId.IsEmpty) return null;
            var meta = Meta;
            return meta?.GetEntity(eId);
        }

        /// <inheritdoc />
        public override string ValueToString(object value) => ((BGEntity)value)?.Id.ToString();

        /// <inheritdoc />
        public override object ValueFromString(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            var meta = Meta;
            if (meta == null) return null;
            if (!BGId.TryParse(value, out var id)) return null;
            return meta.GetEntity(id);
        }

        //===========================================================================
        //                                      Equals
        //===========================================================================
        protected bool Equals(BGCalcTypeCodeEntityRuntime other)
        {
            return base.Equals(other) && metaId.Equals(other.metaId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((BGCalcTypeCodeEntityRuntime)obj);
        }

        public override int GetHashCode()
        {
            unchecked { return (base.GetHashCode() * 397) ^ metaId.GetHashCode(); }
        }


        //===========================================================================
        //                                      State
        //===========================================================================
        /// <inheritdoc />
        public void ReadState(BGBinaryReader reader)
        {
            metaId = reader.ReadId();
            meta = null;
        }

        /// <inheritdoc />
        public void WriteState(BGBinaryWriter writer) => writer.AddId(metaId);

        /// <inheritdoc />
        public void ReadState(string state)
        {
            if (BGId.TryParse(state, out var id)) metaId = id;
        }

        /// <inheritdoc />
        public string WriteState() => metaId.ToString();

        //===========================================================================
        //                                      conversion
        //===========================================================================
        /// <inheritdoc />
        public override bool CanBeConvertedFrom(BGCalcTypeCode otherCode) => otherCode is BGCalcTypeCodeEntity;

        /// <inheritdoc />
        public override object ConvertFrom(BGCalcTypeCode otherCode, object value)
        {
            if (otherCode == null) return value;
            if (value == null) return null;
            switch (otherCode)
            {
                case BGCalcTypeCodeEntity entityCode:
                {
                    var e = (BGEntity)value;
                    if (metaId != e.MetaId) throw new Exception($"Can not convert an entity, cause it seems to be from another table! metaId mismatch {metaId}!={e.MetaId}");
                    return value;
                }
            }

            return value;
        }
    }
}