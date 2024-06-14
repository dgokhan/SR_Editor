/*
<copyright file="BGFieldPointer.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Reference to database field
    /// </summary>
    public class BGFieldPointer : BGMetaPointer
    {
        private BGId fieldId;

        /// <summary>
        /// Referenced field ID
        /// </summary>
        public BGId FieldId
        {
            get => fieldId;
            set => fieldId = value;
        }

        public BGFieldPointer() 
        {
        }

        public BGFieldPointer(BGId metaId, BGId fieldId) : base(metaId)
        {
            this.fieldId = fieldId;
        }
        /// <summary>
        /// resolve the field for provided database or for default database if provided database is null
        /// </summary>
        public BGField GetField(BGRepo repo = null) => GetMeta(repo)?.GetField(fieldId, false);

        public override void Reset()
        {
            base.Reset();
            fieldId = BGId.Empty;
        }

        //========================================= Equality members
        protected bool Equals(BGFieldPointer other)
        {
            return base.Equals(other) && fieldId.Equals(other.fieldId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BGFieldPointer)obj);
        }

        public override int GetHashCode()
        {
            unchecked { return (base.GetHashCode() * 397) ^ fieldId.GetHashCode(); }
        }
    }
}