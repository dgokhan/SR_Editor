/*
<copyright file="BGMetaPointer.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Table reference
    /// </summary>
    public class BGMetaPointer
    {
        private BGId metaId;

        /// <summary>
        /// Referenced table ID
        /// </summary>
        public BGId MetaId
        {
            get => metaId;
            set => metaId = value;
        }

        public BGMetaPointer()
        {
        }
        public BGMetaPointer(BGId metaId)
        {
            this.metaId = metaId;
        }

        /// <summary>
        /// Resolve table for provided database, if database is not null, otherwise default database is used
        /// </summary>
        public BGMetaEntity GetMeta(BGRepo repo = null)
        {
            repo = repo ?? BGRepo.I;
            return repo.GetMeta(metaId);
        }
        /// <summary>
        /// reset internal state
        /// </summary>
        public virtual void Reset() => metaId = BGId.Empty;
        
        //========================================= Equality members
        protected bool Equals(BGMetaPointer other)
        {
            return metaId.Equals(other.metaId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BGMetaPointer)obj);
        }

        public override int GetHashCode()
        {
            return metaId.GetHashCode();
        }

    }
}