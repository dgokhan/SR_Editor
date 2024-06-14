/*
<copyright file="BGEventArgsAnyEntityUpdated.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// any entity updated event args. 
    /// </summary>
    public partial class BGEventArgsAnyEntityUpdated : BGEventArgsAnyEntity
    {
        private static readonly BGObjectPoolNTS<BGEventArgsAnyEntityUpdated> pool = new BGObjectPoolNTS<BGEventArgsAnyEntityUpdated>(() => new BGEventArgsAnyEntityUpdated());
        protected override BGObjectPool Pool => pool;
        
        public BGId FieldId { get; protected set; }

        //to make sure instances are reused
        protected BGEventArgsAnyEntityUpdated()
        {
        }

        public static BGEventArgsAnyEntityUpdated GetInstance(BGEntity entity, BGId fieldId)
        {
            var e = pool.Get();
            e.Entity = entity;
            e.FieldId = fieldId;
            return e;
        }

        public override string ToString() => $"BGEventArgsAnyEntityUpdated: fieldId [{FieldId}], entity [{Entity}]";
    }

    /// <summary>
    /// any entity updated event args with old new values 
    /// </summary>
    public abstract class BGEventArgsAnyEntityUpdatedWithValue : BGEventArgsAnyEntityUpdated
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
        
        public override string ToString() => $"BGEventArgsAnyEntityUpdatedWithValue: field [{GetField()}], entity [{Entity}], oldValue [{GetOldValue()}], newValue [{GetNewValue()}]";
    }
    
    /// <summary>
    /// any entity updated event args with old new stored values
    /// Stored values are values which are store inside database, not the actual values
    /// For example Unity asset fields store asset path inside database 
    /// </summary>
    public partial class BGEventArgsAnyEntityUpdatedWithValue<T, TStoreType> : BGEventArgsAnyEntityUpdatedWithValue
    {
        private static readonly BGObjectPoolNTS<BGEventArgsAnyEntityUpdatedWithValue<T, TStoreType>> pool = 
            new BGObjectPoolNTS<BGEventArgsAnyEntityUpdatedWithValue<T, TStoreType>>(() => new BGEventArgsAnyEntityUpdatedWithValue<T, TStoreType>());
        protected override BGObjectPool Pool => pool;

        // private static readonly BGEventArgsAnyEntityUpdatedWithValue<T, TStoreType> instance = new BGEventArgsAnyEntityUpdatedWithValue<T, TStoreType>(){IsStoredValue = true};
        
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

        public static BGEventArgsAnyEntityUpdatedWithValue<T, TStoreType> GetInstance(BGEntity entity, BGField<T> field, TStoreType oldValue, TStoreType newValue)
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
    /// any entity updated event args with old new values as generic T
    /// </summary>
    public partial class BGEventArgsAnyEntityUpdatedWithValue<T> : BGEventArgsAnyEntityUpdatedWithValue
    {
        private static readonly BGObjectPoolNTS<BGEventArgsAnyEntityUpdatedWithValue<T>> pool = 
            new BGObjectPoolNTS<BGEventArgsAnyEntityUpdatedWithValue<T>>(() => new BGEventArgsAnyEntityUpdatedWithValue<T>());
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

        public static BGEventArgsAnyEntityUpdatedWithValue<T> GetInstance(BGEntity entity, BGField<T> field, T oldValue, T newValue)
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