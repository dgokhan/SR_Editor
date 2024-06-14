namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Cell pointer (table+field+row)
    /// </summary>
    public class BGCellPointer : BGFieldPointer
    {
        private BGId entityId;

        /// <summary>
        /// Referenced row ID
        /// </summary>
        public BGId EntityId
        {
            get => entityId;
            set => entityId = value;
        }

        public BGCellPointer() 
        {
            
        }
        public BGCellPointer(BGId metaId, BGId fieldId, BGId entityId) : base(metaId, fieldId)
        {
            this.entityId = entityId;
        }
        
        /// <summary>
        /// resolve the row for provided database or for default database if provided database is null
        /// </summary>
        public BGEntity GetEntity(BGRepo repo = null) => GetMeta(repo)?.GetEntity(entityId);

        public override void Reset()
        {
            base.Reset();
            entityId = BGId.Empty;
        }

        public void Reset(BGField field, BGEntity entity)
        {
            MetaId = field.MetaId;
            FieldId = field.Id;
            EntityId = entity.Id;
        }        

        //========================================= Equality members
        protected bool Equals(BGCellPointer other) => base.Equals(other) && entityId.Equals(other.entityId);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BGCellPointer)obj);
        }

        public override int GetHashCode()
        {
            unchecked { return (base.GetHashCode() * 397) ^ entityId.GetHashCode(); }
        }

    }
}