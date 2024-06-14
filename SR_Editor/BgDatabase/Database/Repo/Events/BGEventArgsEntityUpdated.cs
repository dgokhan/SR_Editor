/*
<copyright file="BGEventArgsEntityUpdated.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// event for single entity any field value changed 
    /// </summary>
    public partial class BGEventArgsEntityUpdated : BGEventArgsEntity
    {
        private static readonly BGObjectPoolNTS<BGEventArgsEntityUpdated> pool = new BGObjectPoolNTS<BGEventArgsEntityUpdated>(() => new BGEventArgsEntityUpdated());
        protected override BGObjectPool Pool => pool;

        public BGId FieldId { get; protected set; }

        //to make sure instances are reused
        protected BGEventArgsEntityUpdated()
        {
        }

        public static BGEventArgsEntityUpdated GetInstance(BGEntity entity, BGId fieldId)
        {
            var e = pool.Get();
            e.Fill(entity);
            e.FieldId = fieldId;
            return e;
        }
        public override string ToString() => $"BGEventArgsEntityUpdated: filedId [{FieldId}], entity [{Entity}]";
    }

    /// <summary>
    /// event for single entity any field value changed with old new values 
    /// </summary>
    public abstract class BGEventArgsEntityUpdatedWithValue : BGEventArgsEntityUpdated
    {
        /// <summary>
        /// There are 2 possible situation- event contains actual field values or so-called "stored" value- the value which is actually stored inside database
        /// The reason for this- is that for some fields it's expensive to retrieve field's value(for example for asset fields and reference fields),
        /// so such fields are passing a "stored" value instead of fields's value in events (for performance reason) 
        /// </summary>
        public bool IsStoredValue { get; protected set; }

        /// <summary>
        /// old value
        /// </summary>
        public abstract object GetOldValue();
        /// <summary>
        /// new value
        /// </summary>
        public abstract object GetNewValue();
        /// <summary>
        /// database field
        /// </summary>
        public abstract BGField GetField();
        
        public override string ToString() => $"BGEventArgsEntityUpdatedWithValue: field [{GetField()}], entity [{Entity}], oldValue [{GetOldValue()}], newValue [{GetNewValue()}]";
    }
    
    /// <summary>
    /// event for single entity any field stored value changed with old new values 
    /// </summary>
    public partial class BGEventArgsEntityUpdatedWithValue<T, TStoreType> : BGEventArgsEntityUpdatedWithValue
    {
        private static readonly BGObjectPoolNTS<BGEventArgsEntityUpdatedWithValue<T, TStoreType>> pool = 
            new BGObjectPoolNTS<BGEventArgsEntityUpdatedWithValue<T, TStoreType>>(() => new BGEventArgsEntityUpdatedWithValue<T, TStoreType>());
        protected override BGObjectPool Pool => pool;

        
        /// <summary>
        /// old value
        /// </summary>
        public TStoreType OldValue { get; private set; }
        /// <summary>
        /// new value
        /// </summary>
        public TStoreType NewValue { get; private set; }
        /// <summary>
        /// database field
        /// </summary>
        public BGField<T> Field { get; private set; }

        /// <inheritdoc/>
        public override object GetOldValue() => OldValue;
        /// <inheritdoc/>
        public override object GetNewValue() => NewValue;
        /// <inheritdoc/>
        public override BGField GetField() => Field;

        public static BGEventArgsEntityUpdatedWithValue<T, TStoreType> GetInstance(BGEntity entity, BGField<T> field, TStoreType oldValue, TStoreType newValue)
        {
            var instance = pool.Get();
            instance.Field = field;
            instance.FieldId = field.Id;
            instance.Entity = entity;
            instance.OldValue = oldValue;
            instance.NewValue = newValue;
            return instance;
        }
        public override void Clear()
        {
            base.Clear();
            Field = null;
            OldValue = default;
            NewValue = default;
        }
    }
    
    /// <summary>
    /// event for single entity any field value changed with old new values 
    /// </summary>
    public partial class BGEventArgsEntityUpdatedWithValue<T> : BGEventArgsEntityUpdatedWithValue
    {
        private static readonly BGObjectPoolNTS<BGEventArgsEntityUpdatedWithValue<T>> pool = 
            new BGObjectPoolNTS<BGEventArgsEntityUpdatedWithValue<T>>(() => new BGEventArgsEntityUpdatedWithValue<T>());
        protected override BGObjectPool Pool => pool;

        
        /// <summary>
        /// old value
        /// </summary>
        public T OldValue { get; private set; }
        /// <summary>
        /// new value
        /// </summary>
        public T NewValue { get; private set; }
        /// <summary>
        /// database field
        /// </summary>
        public BGField<T> Field { get; private set; }

        /// <inheritdoc/>
        public override object GetOldValue() => OldValue;
        /// <inheritdoc/>
        public override object GetNewValue() => NewValue;
        /// <inheritdoc/>
        public override BGField GetField() => Field;

        public static BGEventArgsEntityUpdatedWithValue<T> GetInstance(BGEntity entity, BGField<T> field, T oldValue, T newValue)
        {
            var instance = pool.Get();
            instance.Field = field;
            instance.FieldId = field.Id;
            instance.Entity = entity;
            instance.OldValue = oldValue;
            instance.NewValue = newValue;
            return instance;
        }
        public override void Clear()
        {
            base.Clear();
            Field = null;
            OldValue = default;
            NewValue = default;
        }
    }
}