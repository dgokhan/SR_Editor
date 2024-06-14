/*
<copyright file="BGField.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Basic abstract field class. Do not extend your field from this class, use generic version (BGField<T>) instead.
    /// </summary>
    public abstract partial class BGField : BGMetaObject, IEquatable<BGField>
    {
        //=================================================================================================================
        //                      Static
        //=================================================================================================================
        //microoptimization: dictionary to hold already created field factories
        //safe-to-use in multi-threaded environment
        private static readonly Dictionary<string, Func<BGMetaEntity, BGId, string, BGField>> FieldTypeName2Factory = new Dictionary<string, Func<BGMetaEntity, BGId, string, BGField>>();

        /// <summary>
        /// field descriptor attribute
        /// </summary>
        public class FieldDescriptor : BGAttributeWithManager
        {
            /// <summary>
            /// Folder to be used in editor while creating new field
            /// </summary>
            public string Folder;

            /// <summary>
            /// deprecation warning
            /// </summary>
            public string DeprecatedNote;
        }

        //safe-to-use in multi-threaded environment
        private static readonly List<Type> AllFieldTypes = new List<Type>();

        /// <summary>
        /// all field types
        /// </summary>
        public static List<Type> FieldTypes
        {
            get
            {
                if (AllFieldTypes.Count != 0) return AllFieldTypes;

                var allSubTypes = BGUtil.GetAllSubTypes(typeof(BGField));
                foreach (var fieldType in allSubTypes) AllFieldTypes.Add(fieldType);
                return AllFieldTypes;
            }
        }

        //=================================================================================================================
        //                      Fields
        //=================================================================================================================

        /// <summary>
        /// name for GUI (performance is not critical)
        /// </summary>
        public string DisplayName => BGAttribute.GetName(GetType()) ?? GetType().Name;

        private string defaultValue;

        /// <summary>
        /// Default value as string
        /// </summary>
        public string DefaultValue
        {
            get => defaultValue;
            set
            {
                if (string.Equals(defaultValue, value)) return;
                defaultValue = value;
                Meta.Repo.Events.MetaWasChanged(Meta);
            }
        }

        /// <summary>
        /// Field name. Must be unique within meta
        /// </summary>
        public override string Name
        {
            set
            {
                if (string.Equals(Name, value)) return;
                Meta.CheckFieldName(value);

                var oldName = Name;
                //value will be checked in base method
                base.Name = value;

                Meta.FieldNameWasChanged(this, oldName);
            }
        }

        /// <summary>
        /// Full name (with meta name)
        /// </summary>
        public string FullName => MetaName + "." + Name;

        /// <summary>
        /// meta this field belong to
        /// </summary>
        public BGMetaEntity Meta { get; private set; }

        /// <summary>
        /// repository
        /// </summary>
        public BGRepo Repo => Meta.Repo;

        /// <summary>
        /// meta id
        /// </summary>
        public BGId MetaId => Meta.Id;

        /// <summary>
        /// meta name
        /// </summary>
        public string MetaName => Meta.Name;

        private bool required;

        /// <summary>
        /// if this field is required?
        /// </summary>
        public bool Required
        {
            get => required;
            set
            {
                if (required == value) return;
                required = value;
                Meta.Repo.Events.MetaWasChanged(Meta);
            }
        }


        /// <summary>
        /// field description
        /// </summary>
        public virtual string Description => "Field [" + BGAttribute.GetName(GetType()) + "]";

        /// <summary>
        /// if this field is readonly by its nature?
        /// There is an alternative way to manually mark field as readonly by using UserDefinedReadonly property
        /// </summary>
        public virtual bool ReadOnly => false;

        /// <summary>
        /// if not 0, it means:
        /// 1) field value is always present 
        /// 2) binary representation always takes the specified size in bytes
        /// </summary>
        /// <example>
        /// int field always present (can not be null) and always takes 4 bytes
        /// </example>
        public virtual int ConstantSize => 0;

        /// <summary>
        /// EmptyContent- means the field does not store any value
        /// </summary>
        public virtual bool EmptyContent => false;

        /// <inheritdoc/>
        public override int Index => Meta.GetFieldIndex(Id);

        /// <inheritdoc/>
        public override string Comment
        {
            set
            {
                var oldComment = base.Comment;
                if (string.Equals(value, oldComment)) return;
                var newCommentNull = string.IsNullOrEmpty(value);
                var commentNull = string.IsNullOrEmpty(oldComment);
                if (newCommentNull && commentNull) return;
                SetComment(value);
                Meta.Repo.Events.MetaWasChanged(Meta);
            }
        }
        
        /// <inheritdoc/>
        public override string ControllerType
        {
            set
            {
                var oldControllerType = base.ControllerType;
                if (string.Equals(value, oldControllerType)) return;
                var newControllerNull = string.IsNullOrEmpty(value);
                var controllerNull = string.IsNullOrEmpty(oldControllerType);
                if (newControllerNull && controllerNull) return;
                base.ControllerType = string.IsNullOrEmpty(value) ? null : value;
                Meta.Repo.Events.MetaWasChanged(Meta);
            }
        }

        /// <summary>
        /// Does this field supports multithreaded loading
        /// </summary>
        public virtual bool SupportMultiThreadedLoading => true;

        /// <summary>
        /// Can this field be used inside key
        /// </summary>
        public virtual bool CanBeUsedAsKey => false;

        /// <summary>
        /// Do not override it
        /// type code is used by internal database code.
        /// type code is mapped to C# type for faster objects instantiation 
        /// </summary>
        public virtual ushort TypeCode => 0;

        //database events
        protected BGRepoEvents events => Meta.Repo.Events;

        private bool userDefinedReadonly;

        /// <summary>
        /// if user marked the field as readonly
        /// </summary>
        public bool UserDefinedReadonly
        {
            get => userDefinedReadonly;
            set
            {
                if (userDefinedReadonly == value) return;
                userDefinedReadonly = value;
                Meta.Repo.Events.MetaWasChanged(Meta);
            }
        }

        /// <summary>
        /// Final value for all possible readonly options
        /// </summary>
        public bool ReadonlyFinal => Meta.UserDefinedReadonly || ReadOnly || userDefinedReadonly;

        //=================================================================================================================
        //                      Constructors
        //=================================================================================================================
        //for new fields only
        protected BGField(BGMetaEntity meta, string name) : base(meta.NewFieldId, name)
        {
            if (name == "Index") throw new Exception("'Index' name is reserved, please, use another name");

            RegisterField(meta);
        
            meta.ForEachEntity(OnEntityCreate);
        }

        //for existing fields only
        protected BGField(BGMetaEntity meta, BGId id, string name) : base(id, name) => RegisterField(meta);

        private void RegisterField(BGMetaEntity meta)
        {
            Meta = meta;
            Meta.Register(this);
        }

        //=================================================================================================================
        //                      Registration
        //=================================================================================================================
        // unregister table 
        protected void Unregister() => Meta?.Unregister(this);

        //=================================================================================================================
        //                      Methods
        //=================================================================================================================
        /// <inheritdoc />
        public override void Delete()
        {
            if (IsDeleted) return;
            base.Delete();

            Unregister();
            Unload();
            Meta = null;
        }

        /// <summary>
        /// Clone itself to another meta. Can copy values as well.
        /// </summary>
        [Obsolete("Use CloneTo(BGCloneContextField context) instead")]
        public virtual BGField CloneTo(BGMetaEntity meta, bool copyValues) => Clone(meta, Id);
        
        /// <summary>
        /// Clone itself to another meta. Can copy values as well.
        /// </summary>
        public virtual BGField CloneTo(BGCloneContextField context) => Clone(context.meta, Id);

        /// <summary>
        /// Duplicate itself to the same meta (nested field and self-referencing relations are not supported- null is returned). Can copy values as well.
        /// </summary>
        [Obsolete("use Clone(meta, meta.NewFieldId(meta));")]
        public virtual BGField Duplicate(BGMetaEntity meta) => throw new Exception("This method is obsolete");

        /// <summary>
        /// Clone this field to provided table
        /// </summary>
        public BGField Clone(BGMetaEntity meta, BGId fieldId)
        {
            var clone = CreateFieldFactory()(meta, fieldId, Name);
//            var clone = BGUtil.Create<BGField>(GetType(), true, meta, Id, Name);
            clone.System = System;
            clone.Addon = Addon;
            clone.CustomStringFormatterTypeAsString = CustomStringFormatterTypeAsString;
            clone.CustomEditorTypeAsString = CustomEditorTypeAsString;
            clone.Comment = Comment;
            clone.ControllerType = ControllerType;
            clone.DefaultValue = DefaultValue;

            var configToBytes = ConfigToBytes();
            clone.ConfigFromBytes(configToBytes == null ? new ArraySegment<byte>(Array.Empty<byte>()) : new ArraySegment<byte>(configToBytes));
            return clone;
        }

        private void SetComment(string value) => base.Comment = value;

        public bool Equals(BGField other)
        {
            return other != null && Id == other.Id;
        }

        //=================================================================================================================
        //                      Factory
        //=================================================================================================================
        /// <summary>
        /// Provide field factory which can be used for creating new fields
        /// </summary>
        protected abstract Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory();

        //=================================================================================================================
        //                      Callbacks
        //=================================================================================================================
        /// <summary>
        /// Do not call this method. 
        /// </summary>
        // callback for entity added (when entity added to meta- no matter new or existing)
        public virtual void OnEntityAdd(BGEntity entity)
        {
        }

        /// <summary>
        /// Do not call this method. 
        /// </summary>
        //callback for entity removed
        public virtual void OnEntityDelete(BGEntity entity)
        {
        }

        /// <summary>
        /// Do not call this method.
        /// </summary>
        // callback for entity name changed 
        public virtual void OnNameChange(int entityIndex)
        {
        }

        /// <summary>
        /// Do not call this method.
        /// </summary>
        // callback for entity created (For new entities only)
        public virtual void OnEntityCreate(BGEntity entity)
        {
            if (string.IsNullOrEmpty(DefaultValue)) return;

            try
            {
                FromString(entity.Index, DefaultValue);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        /// Do not call this method.
        /// </summary>
        // after field was added to meta
        public virtual void OnCreate()
        {
        }

        /// <summary>
        /// Do not call this method.
        /// </summary>
        // when field is deleted
        public virtual void OnDelete()
        {
        }


        //=================================================================================================================
        //                      Configuration
        //=================================================================================================================
        /// <inheritdoc />
        public override string ConfigToString() => null;

        /// <inheritdoc />
        public override void ConfigFromString(string config)
        {
        }

        /// <inheritdoc />
        public override byte[] ConfigToBytes() => null;

        /// <inheritdoc />
        public override void ConfigFromBytes(ArraySegment<byte> config)
        {
        }

        //=================================================================================================================
        //                      Custom Editor
        //=================================================================================================================
        private string customEditorTypeAsString;

        /// <summary>
        /// IS IT USED SOMEWHERE??
        /// </summary>
        public string CustomEditorTypeAsString
        {
            get => customEditorTypeAsString;
            set
            {
                if (BGUtil.AreEqual(customEditorTypeAsString, value)) return;
                customEditorTypeAsString = value;
                Meta.Repo.Events.MetaWasChanged(Meta);
            }
        }

        /// <summary>
        /// ??? this is very strangely used- search usage 
        /// </summary>
        public virtual bool StoredValueIsTheSameAsValueType => true;


        //=================================================================================================================
        //                      Custom String formatter
        //=================================================================================================================
        private string customStringFormatterTypeAsString;

        /// <summary>
        /// Custom string formatter type as string
        /// </summary>
        public string CustomStringFormatterTypeAsString
        {
            get => customStringFormatterTypeAsString;
            set
            {
                if (BGUtil.AreEqual(customStringFormatterTypeAsString, value)) return;
                customStringFormatterTypeAsString = value;
                OnCustomStringFormatterChange();
                Meta.Repo.Events.MetaWasChanged(Meta);
            }
        }

        //on custom string formatter changed
        protected virtual void OnCustomStringFormatterChange()
        {
        }

        /// <summary>
        /// Custom string formatter type 
        /// </summary>
        public abstract bool CustomStringFormatSupported { get; }


        /// <summary>
        /// the same as ToString, but custom formatter is used
        /// </summary>
        public abstract string ToCustomString(int entityIndex);

        /// <summary>
        /// the same as FromString, but custom formatter is used
        /// </summary>
        public abstract void FromCustomString(int entityIndex, string formattedValue);

        //=================================================================================================================
        //                      Value
        //=================================================================================================================
        /// <summary>
        /// field value type
        /// </summary>
        public abstract Type ValueType { get; }


        /// <summary>
        /// clear field value for entity with specified id
        /// </summary>
        public abstract void ClearValue(int entityIndex);

        /// <summary>
        /// iterate over each not null value within this field 
        /// </summary>
        public abstract void ForEachValue(Action<int> action);

        /// <summary>
        /// copy value from another field for entity with specified id. This method does not fire any events.
        /// </summary>
        public abstract void CopyValue(BGField fromField, BGId fromEntityId, int fromEntityIndex, BGId toEntityId);

        /// <summary>
        /// called when entity duplicated (similar to CopyValue)
        /// </summary>
        public abstract void DuplicateValue(BGId fromEntityId, int fromEntityIndex, BGId toEntityId);

        /// <summary>
        /// get value by entity id (with boxing/unboxing). Generic version from BGField<T> should normally be used  
        /// </summary>
        public abstract object GetValue(BGId entityId);

        /// <summary>
        /// set value by entity id (with boxing/unboxing). Generic version from BGField<T> should normally be used  
        /// </summary>
        public abstract void SetValue(BGId entityId, object value);

        /// <summary>
        /// get value by entity index (with boxing/unboxing). Generic version from BGField<T> should normally be used  
        /// </summary>
        public abstract object GetValue(int entityIndex);

        /// <summary>
        /// set value by entity index (with boxing/unboxing). Generic version from BGField<T> should normally be used  
        /// </summary>
        public abstract void SetValue(int entityIndex, object value);

        /// <summary>
        /// Do not call this method. swap 2 entities values.   
        /// </summary>
        public abstract void Swap(int entityIndex1, int entityIndex2);

        /// <summary>
        /// Do not call this method. Move block of entities values to new location   
        /// </summary>
        public abstract void MoveEntitiesValues(int fromIndex, int toIndex, int numberOfValues);

        /// <summary>
        /// Do not call this method. Clears all values   
        /// </summary>
        public abstract void ClearValues();

        /// <summary>
        /// Are stored values equal?   
        /// </summary>
        public abstract bool AreStoredValuesEqual(BGField field, int myEntityIndex, int otherEntityIndex);


        //=================================================================================================================
        //                      Serialization
        //=================================================================================================================
        /// <summary>
        /// serialize value for entity with specified id as byte array.This method does not fire any events.
        /// </summary>
        public abstract byte[] ToBytes(int entityIndex);

        /// <summary>
        /// restore value for entity with specified id from byte array.This method does not fire any events.
        /// </summary>
        public abstract void FromBytes(int entityIndex, ArraySegment<byte> segment);

        /// <summary>
        /// serialize value for entity with specified id as string.This method does not fire any events.
        /// </summary>
        public abstract string ToString(int entityIndex);

        /// <summary>
        /// restore value for entity with specified id from string.This method does not fire any events.
        /// </summary>
        public abstract void FromString(int entityIndex, string value);

        //=================================================================================================================
        //                      Utility
        //=================================================================================================================
        public override string ToString()
        {
            return "Field [id:" + Id + ", name:" + Name + ", type:" + GetType().FullName + "]";
        }

        //=================================================================================================================
        //                      Factory
        //=================================================================================================================
        /// <summary>
        /// Create a field using string config
        /// </summary>
        public static BGField Create(BGMetaEntity meta, string type, BGId id, string name, string config, bool system, string addon, string defaultValue, bool required)
        {
            var field = Create(meta, type, id, name, system, addon, defaultValue, required);
            field.ConfigFromString(config);
            return field;
        }

        /// <summary>
        /// Create a field using string config
        /// </summary>
        public static BGField Create(BGMetaEntity meta, string type, BGId id, string name, ArraySegment<byte> config, bool system, string addon, string defaultValue, bool required)
        {
            var field = Create(meta, type, id, name, system, addon, defaultValue, required);
            field.ConfigFromBytes(config);
            return field;
        }

        private static BGField Create(BGMetaEntity meta, string type, BGId id, string name, bool system, string addon, string defaultValue, bool required)
        {
            //------------------ create
            BGField field;
            if (FieldTypeName2Factory.TryGetValue(type, out var factory)) field = factory(meta, id, name);
            else
            {
                field = BGUtil.Create<BGField>(type, true, meta, id, name);
                FieldTypeName2Factory[type] = field.CreateFieldFactory();
            }

            //------------------ fill all fields
            field.DefaultValue = defaultValue;
            field.System = system;
            field.Addon = addon;
            field.Required = required;
            return field;
        }

        //========================================================================================
        //              Binary
        //========================================================================================
        /// <summary>
        /// Reconstruct field from binary stream
        /// </summary>
        internal static BGField FromBinary(BGBinaryReader binder, BGMetaEntity meta)
        {
            var version = binder.ReadInt();
            switch (version)
            {
                case 1:
                {
                    var fieldId = binder.ReadId();
                    var fieldName = binder.ReadString();
                    var fieldType = binder.ReadString();
                    var fieldConfig = binder.ReadByteArray();
                    var fieldSystem = binder.ReadBool();
                    var fieldAddon = binder.ReadString();
                    var fieldDefaultValue = binder.ReadString();
                    var fieldRequired = binder.ReadBool();
                    var fieldStringFormatter = binder.ReadString();
                    var fieldCustomEditor = binder.ReadString();
                    var comment = binder.ReadString();

                    var field = Create(meta, fieldType, fieldId, fieldName, fieldConfig, fieldSystem, fieldAddon, fieldDefaultValue, fieldRequired);
                    field.CustomStringFormatterTypeAsString = fieldStringFormatter;
                    field.CustomEditorTypeAsString = fieldCustomEditor;
                    field.SetComment(comment);
                    return field;
                }
                case 2:
                case 3:
                case 4:
                {
                    var typeCode = binder.ReadUShort();
                    string type = null;
                    if (typeCode == 0) type = binder.ReadString();

                    var fieldId = binder.ReadId();
                    var fieldName = binder.ReadString();
                    // var fieldType = binder.ReadString();
                    var fieldConfig = binder.ReadByteArray();
                    var fieldSystem = binder.ReadBool();
                    var fieldAddon = binder.ReadString();
                    var fieldDefaultValue = binder.ReadString();
                    var fieldRequired = binder.ReadBool();
                    var fieldStringFormatter = binder.ReadString();
                    var fieldCustomEditor = binder.ReadString();
                    var comment = binder.ReadString();

                    var field = typeCode == 0
                        ? Create(meta, type, fieldId, fieldName, fieldConfig, fieldSystem, fieldAddon, fieldDefaultValue, fieldRequired)
                        : BGFieldTypeCodeFactory.Instance.Create(meta, typeCode, fieldId, fieldName, fieldConfig, fieldSystem, fieldAddon, fieldDefaultValue, fieldRequired);

                    field.CustomStringFormatterTypeAsString = fieldStringFormatter;
                    field.CustomEditorTypeAsString = fieldCustomEditor;
                    field.SetComment(comment);

                    //v.3
                    if (version >= 3) field.UserDefinedReadonly = binder.ReadBool();

                    //v.4
                    if (version >= 4) field.ControllerType = binder.ReadString();

                    return field;
                }
                default:
                {
                    throw new BGException("Can not read field from binary array: unsupported version $", version);
                }
            }
        }

        /// <summary>
        /// field to binary stream
        /// </summary>
        internal static void ToBinary(BGBinaryWriter builder, BGField field)
        {
            //version
            builder.AddInt(4);

            //type code
            builder.AddUShort(field.TypeCode);
            if (field.TypeCode == 0) builder.AddString(field.GetType().AssemblyQualifiedName);


            builder.AddId(field.Id);
            builder.AddString(field.Name);
            // builder.AddString(field.GetType().AssemblyQualifiedName);
            builder.AddByteArray(field.ConfigToBytes());
            builder.AddBool(field.System);
            builder.AddString(field.Addon);
            builder.AddString(field.DefaultValue);
            builder.AddBool(field.Required);
            builder.AddString(field.CustomStringFormatterTypeAsString);
            builder.AddString(field.CustomEditorTypeAsString);
            builder.AddString(field.Comment);

            //v.3
            builder.AddBool(field.UserDefinedReadonly);
            
            //v.4
            builder.AddString(field.ControllerType);
        }

        //========================================================================================
        //              Events
        //========================================================================================
        /// <summary>
        /// On field value changed 
        /// </summary>
        public event EventHandler<BGEventArgsField> ValueChanged;

        public event EventHandler<BGEventArgsField> BeforeValueChanged;

        protected bool HasValueListener => ValueChanged != null;

        protected bool HasBeforeValueListener => BeforeValueChanged != null;

        // internal void SwitchTo(BGRepo repo)
        // {
        //     events = repo.Events;
        // }

        /// <summary>
        /// Fire field value changed for provided entity
        /// </summary>
        public void FireValueChanged(BGEntity entity)
        {
            if (events.ConsumeOnChange(MetaId)) return;
            FireValueChangedInternal(entity);
        }

        // Fire field value changed for provided entity ID
        protected void FireValueChanged(BGId entityId)
        {
            if (events.ConsumeOnChange(MetaId)) return;
            var entity = Meta.GetEntity(entityId);
            if (entity != null) FireValueChangedInternal(entity);
        }

        // Fire field value changed for provided entity
        private void FireValueChangedInternal(BGEntity entity)
        {
            if (ValueChanged != null)
                using (var eventArgs = BGEventArgsField.GetInstance(entity, Id))
                    ValueChanged.Invoke(this, eventArgs);

            Meta.FireValueChanged(this, entity, true);
            events.FireAnyChange();
        }

        // fire value changed event
        protected void FireValueChanged(BGEventArgsField eventArgs) => ValueChanged?.Invoke(this, eventArgs);

        // fire before value changed event
        protected void FireBeforeValueChanged(BGEventArgsField eventArgs) => BeforeValueChanged?.Invoke(this, eventArgs);

        //transfer events to events holder before database is reloaded
        internal void TransferEventsTo(BGEventsHolder eventsHolder)
        {
            if (ValueChanged != null)
            {
                eventsHolder.AddOnFieldValueChangedListeners(Id, ValueChanged.GetInvocationList());
                ValueChanged = null;
            }
            if (BeforeValueChanged != null)
            {
                eventsHolder.AddOnBeforeFieldValueChangedListeners(Id, BeforeValueChanged.GetInvocationList());
                BeforeValueChanged = null;
            }
        }

        //transfer events from events holder after database is reloaded
        internal void TransferEventsFrom(BGEventsHolder eventsHolder)
        {
            ValueChanged = null;
            var delegates = eventsHolder.GetOnFieldValueChangedListeners(Id);
            if (delegates != null && delegates.Length > 0)
            {
                foreach (var @delegate in delegates) ValueChanged += (EventHandler<BGEventArgsField>)@delegate;
            }

            BeforeValueChanged = null;
            delegates = eventsHolder.GetOnFieldBeforeValueChangedListeners(Id);
            if (delegates != null && delegates.Length > 0)
            {
                foreach (var @delegate in delegates) BeforeValueChanged += (EventHandler<BGEventArgsField>)@delegate;
            }
        }


        //========================================================================================
        //              Interfaces and classes
        //========================================================================================
        /// <summary>
        /// ?? IS IT USED ??
        /// Interface for field factory (used to create existing metas)    
        /// </summary>
        protected interface FieldFactory
        {
            BGField Create(BGMetaEntity meta, BGId id, string name);
        }
    }

    //=================================================================================================================
    //                      Generic field (base class for all fields)
    //=================================================================================================================
    /// <summary>
    /// Basic generic field with specified field value type as generic parameter T.
    /// </summary>
    public abstract partial class BGField<T> : BGField
    {
        //default separator for complex values, like Vector3= 1`2`3
        protected const char S = '`';

        //default separator for array values, like List<int>= 1|2|3
        protected const char A = '|';
        protected static readonly char[] AA = { '|' };

        private Type valueType;

        protected BGField(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        protected BGField(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //=================================================================================================================
        //                      Value
        //=================================================================================================================
        /// <inheritdoc />
        public override Type ValueType
        {
            get
            {
                if (valueType != null) return valueType;
                valueType = typeof(T);
                return valueType;
            }
        }

        /// <inheritdoc />
        public override object GetValue(BGId entityId)
        {
            return this[entityId];
        }

        /// <inheritdoc />
        public override void SetValue(BGId entityId, object value)
        {
            this[entityId] = (T)value;
        }

        /// <inheritdoc />
        public override object GetValue(int entityIndex)
        {
            return this[entityIndex];
        }

        /// <inheritdoc />
        public override void SetValue(int entityIndex, object value)
        {
            this[entityIndex] = (T)value;
        }

        /// <summary>
        /// value for a entity by its id. This is generic version of GetValue/SetValue methods, which get rid of boxing/unboxing
        /// </summary>
        public abstract T this[BGId entityId] { get; set; }

        /// <summary>
        /// value for a entity by its index. This is main method to use to get/set field value
        /// </summary>
        public abstract T this[int index] { get; set; }


        //=================================================================================================================
        //                      Custom string
        //=================================================================================================================
        private bool customStringFormatterActivationTried;
        private BGStringFormatter<T> customStringFormatter;

        public override bool CustomStringFormatSupported => StoredValueIsTheSameAsValueType && CustomStringFormatter != null;

        //try to instantiate custom string formatter object by its name 
        private BGStringFormatter<T> CustomStringFormatter
        {
            get
            {
                if (customStringFormatter != null) return customStringFormatter;

                if (string.IsNullOrEmpty(CustomStringFormatterTypeAsString)) return null;

                //there was a try to create string formatter and it failed
                if (customStringFormatterActivationTried) return null;

                //we store stringFormatterActivationTried to avoid multiple expensive GetType & Activator.CreateInstance calls, so the code below is executed only once
                customStringFormatterActivationTried = true;
                var type = BGUtil.GetType(CustomStringFormatterTypeAsString);
                if (type == null) return null;

                try
                {
                    customStringFormatter = Activator.CreateInstance(type) as BGStringFormatter<T>;
                }
                catch (Exception e)
                {
                }

                return customStringFormatter;
            }
        }

        /// <inheritdoc/>
        protected override void OnCustomStringFormatterChange()
        {
            customStringFormatter = null;
            customStringFormatterActivationTried = false;
        }

        /// <inheritdoc />
        public override string ToCustomString(int entityIndex)
        {
            try
            {
                return CustomStringFormatter.ToString(this[entityIndex]);
            }
            catch (BGStringFormatterUseDefaultException)
            {
                return ToString(entityIndex);
            }
        }

        /// <inheritdoc />
        public override void FromCustomString(int entityIndex, string formattedValue)
        {
            try
            {
                this[entityIndex] = CustomStringFormatter.FromString(formattedValue);
            }
            catch (BGStringFormatterUseDefaultException)
            {
                FromString(entityIndex, formattedValue);
            }
        }

        //Fire value changed event for provided entity using provided values (old and new ones)
        public void FireValueChanged(BGEntity entity, T oldValue, T newValue)
        {
            if (events.ConsumeOnChange(MetaId)) return;
            if (HasValueListener)
                using (var eventArgs = BGEventArgsFieldWithValue<T>.GetInstance(entity, this, oldValue, newValue))
                    FireValueChanged(eventArgs);

            Meta.FireValueChanged(this, entity, oldValue, newValue);
            events.FireAnyChange();
        }

        //Fire value changed event for provided entity using provided values (old and new ones)
        public void FireBeforeValueChanged(BGEntity entity, T oldValue, T newValue)
        {
            if (events.ConsumeOnChange(MetaId)) return;
            if (HasBeforeValueListener)
                using (var eventArgs = BGEventArgsFieldWithValue<T>.GetInstance(entity, this, oldValue, newValue))
                    FireBeforeValueChanged(eventArgs);
            Meta.FireBeforeValueChanged(this, entity, oldValue, newValue);
        }
    }
}