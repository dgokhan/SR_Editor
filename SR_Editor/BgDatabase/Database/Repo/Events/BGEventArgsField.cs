/*
<copyright file="BGEventArgsField.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// field event args. 
    /// </summary>
    public partial class BGEventArgsField : BGEventArgsA
    {
        private static readonly BGObjectPoolNTS<BGEventArgsField> pool = new BGObjectPoolNTS<BGEventArgsField>(() => new BGEventArgsField());
        protected override BGObjectPool Pool => pool;

        public BGId FieldId { get; protected set; }
        public BGEntity Entity { get; protected  set; }

        //to make sure instances are reused
        protected BGEventArgsField()
        {
        }

        public static BGEventArgsField GetInstance(BGEntity entity, BGId fieldId)
        {
            var e = pool.Get();
            e.FieldId = fieldId;
            e.Entity = entity;
            return e;
        }

        public override void Clear() => Entity = null;

        public override string ToString() => $"BGEventArgsField: fieldId [{FieldId}, entity [{Entity}]]";
    }

    /// <summary>
    /// field event args with old new values 
    /// </summary>
    public abstract partial class BGEventArgsFieldWithValue : BGEventArgsField
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
    /// field event args with old new stored values 
    /// </summary>
    public partial class BGEventArgsFieldWithValue<T, TStoreType> : BGEventArgsFieldWithValue
    {
        private static readonly BGObjectPoolNTS<BGEventArgsFieldWithValue<T, TStoreType>> pool = 
            new BGObjectPoolNTS<BGEventArgsFieldWithValue<T, TStoreType>>(() => new BGEventArgsFieldWithValue<T, TStoreType>());
        protected override BGObjectPool Pool => pool;


        /// <summary>
        /// old stored value 
        /// </summary>
        public TStoreType OldValue { get; private set; }
        /// <summary>
        /// new stored value 
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

        private BGEventArgsFieldWithValue()
        {
        }

        public static BGEventArgsFieldWithValue<T, TStoreType> GetInstance(BGEntity entity, BGField<T> field, TStoreType oldValue, TStoreType newValue)
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
    /// field event args with old new values 
    /// </summary>
    public partial class BGEventArgsFieldWithValue<T> : BGEventArgsFieldWithValue
    {
        private static readonly BGObjectPoolNTS<BGEventArgsFieldWithValue<T>> pool = 
            new BGObjectPoolNTS<BGEventArgsFieldWithValue<T>>(() => new BGEventArgsFieldWithValue<T>());
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

        private BGEventArgsFieldWithValue()
        {
        }

        public static BGEventArgsFieldWithValue<T> GetInstance(BGEntity entity, BGField<T> field, T oldValue, T newValue)
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