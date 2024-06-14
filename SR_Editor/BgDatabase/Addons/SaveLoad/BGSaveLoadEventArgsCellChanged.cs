namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Cell changed event arguments
    /// </summary>
    public class BGSaveLoadEventArgsCellChanged : BGEventArgsA
    {
        private static readonly BGObjectPool<BGSaveLoadEventArgsCellChanged> pool = new BGObjectPool<BGSaveLoadEventArgsCellChanged>(() => new BGSaveLoadEventArgsCellChanged());
        protected override BGObjectPool Pool => pool;


        private BGMetaEntity meta;
        private BGEntity entity;
        private BGField field;
        private object oldValue;
        private object newValue;

        public BGMetaEntity Meta => meta;

        public BGEntity Entity => entity;

        public BGField Field => field;

        public object OldValue => oldValue;

        public object NewValue => newValue;

        private BGSaveLoadEventArgsCellChanged()
        {
        }

        public override void Clear()
        {
            meta = null;
            entity = null;
            field = null;
            oldValue = null;
            newValue = null;
        }

        public override string ToString()
        {
            return "BGSaveLoadEventArgsCellChanged: " + (entity == null ? "[no entity]" : entity.FullName) + ", field: " + (field == null ? "[no field]" : field.Name) + " [" + oldValue + "->" + newValue + "]";
        }

        public static BGSaveLoadEventArgsCellChanged Get(BGMetaEntity meta, BGField field, BGEntity entity, object oldValue, object newValue)
        {
            var instance = pool.Get(); 
            instance.Clear();
            instance.meta = meta;
            instance.field = field;
            instance.entity = entity;
            instance.oldValue = oldValue;
            instance.newValue = newValue;

            return instance;
        }
    }
}