/*
<copyright file="BGEntityPointer.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Reference to database row
    /// </summary>
    public class BGEntityPointer: BGMetaPointer
    {
        private readonly BGId entityId;

        /// <summary>
        /// Referenced entity ID
        /// </summary>
        public BGId EntityId => entityId;

        public BGEntityPointer(BGId metaId, BGId entityId) : base(metaId) => this.entityId = entityId;

        /// <summary>
        /// resolve the row for provided database or for default database if provided database is null
        /// </summary>
        public BGEntity GetEntity(BGRepo repo = null) => GetMeta(repo)?.GetEntity(entityId);

        //========================================= Equality members
        protected bool Equals(BGEntityPointer other)
        {
            return base.Equals(other) && entityId.Equals(other.entityId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BGEntityPointer)obj);
        }

        public override int GetHashCode()
        {
            unchecked { return (base.GetHashCode() * 397) ^ entityId.GetHashCode(); }
        }

    }
}