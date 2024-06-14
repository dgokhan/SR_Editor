/*
<copyright file="BGFieldCodedA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// field, which value is calculated with C# code
    /// </summary>
    public abstract class BGFieldCodedA<T> : BGFieldDictionaryBasedA<T, BGFieldCodedValue>, BGFieldCodedI, BGFieldWithCustomConfigI
    {
        /// <inheritdoc />
        public override bool ReadOnly => true;

        /// <inheritdoc/>
        public override bool CustomStringFormatSupported => false;

        /// <inheritdoc/>
        public override bool StoredValueIsTheSameAsValueType => false;

        protected string delegateClass;
        private BGCodedFieldDelegateI<T> @delegate;
        private bool delegateLoadTried = false;

        public BGCodedFieldDelegateI DelegateInstance => Delegate;

        private BGCodedFieldDelegateI<T> Delegate
        {
            get
            {
                if (@delegate != null || delegateLoadTried) return @delegate;
                delegateLoadTried = true;
                if (string.IsNullOrEmpty(delegateClass)) return null;
                var type = BGUtil.GetType(delegateClass);
                if (type == null) return null;
                try
                {
                    SetDelegate((BGCodedFieldDelegateI<T>)Activator.CreateInstance(type));
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }

                return @delegate;
            }
            set
            {
                if (value == @delegate) return;
                if (value == null)
                {
                    delegateClass = null;
                    SetDelegate(null);
                }
                else
                {
                    delegateClass = value.GetType().FullName;
                    SetDelegate(value);
                }
                events.MetaWasChanged(Meta);
            }
        }

        public string DelegateClass
        {
            get => delegateClass;
            set
            {
                if (value == delegateClass) return;
                SetDelegate(null);
                delegateLoadTried = false;
                delegateClass = value;
                events.MetaWasChanged(Meta);
            }
        }

        //================================================================================================
        //                                              Constructors
        //================================================================================================
        //for new fields
        public BGFieldCodedA(BGMetaEntity meta, string name, Type delegateType) : base(meta, name)
        {
            if (delegateType != null)
            {
                var error = GetErrorForDelegateType(delegateType);
                if (!string.IsNullOrEmpty(error))
                {
                    Meta.Unregister(this);
                    throw new BGException(error);
                }

                delegateClass = delegateType.FullName;
            }
        }

        //for existing fields
        protected internal BGFieldCodedA(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              PREVENT SET
        //================================================================================================
        /// <inheritdoc/>
        public override T this[BGId entityId]
        {
            set
            {
                //field is readonly!
            }
        }

        /// <inheritdoc/>
        public override T this[int index]
        {
            set
            {
                //field is readonly!
            }
        }

        //================================================================================================
        //                                              Methods
        //================================================================================================
        /// <inheritdoc/>
        protected internal override void Unload()
        {
            base.Unload();
            SetDelegate(null);
        }

        /// <inheritdoc/>
        //set never called - so this method never called 
        protected override BGFieldCodedValue Convert(BGEntity entity, T value) => throw new NotImplementedException();

        /// <inheritdoc/>
        protected override T Convert(BGEntity entity, BGFieldCodedValue value)
        {
            //launch value graph
            if (value != null) return value.Call<T>(this, entity);

            //launch field graph
            if (!string.IsNullOrEmpty(DelegateClass)) return GetCodedValue(entity);

            return default;
        }
        private T GetCodedValue(BGEntity entity)
        {
            var myDelegate = Delegate;
            if (myDelegate == null) throw new Exception($"Delegate can not be created, class={delegateClass}");
            using (var context = BGCodedFieldContext.Get())
            {
                context.Field = this;
                context.Entity = entity;
                return myDelegate.Get(context);
            }
        }
        private void SetDelegate( BGCodedFieldDelegateI<T> newDelegate)
        {
            if (@delegate is BGCodedFieldDelegateLifeCycleI lifeCycleEventsReceiverOld)
            {
                try
                {
                    lifeCycleEventsReceiverOld.OnUnload(new BGCodedFieldDelegateLifeCycleContext(){Field = this});
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            } 
            @delegate = newDelegate;
            
            if (@delegate is BGCodedFieldDelegateLifeCycleI lifeCycleEventsReceiverNew)
            {
                try
                {
                    lifeCycleEventsReceiverNew.OnLoad(new BGCodedFieldDelegateLifeCycleContext(){Field = this});
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }


        //================================================================================================
        //                                              Utilities
        //================================================================================================
        /// <summary>
        /// Check enum type and underlying type for possible errors
        /// </summary>
        public static string GetErrorForDelegateType(Type delegateType)
        {
            if (delegateType == null) return null;
            if (!typeof(BGCodedFieldDelegateI<T>).IsAssignableFrom(delegateType))
                return BGUtil.Format("delegateType $ can not be casted to BGCodedFieldDelegateI interface with $ generic parameter", delegateType.FullName, typeof(T).FullName);

            return null;
        }

        //================================================================================================
        //                                              Configuration
        //================================================================================================
        /// <inheritdoc />
        public override string ConfigToString()
        {
            return JsonUtility.ToJson(new JsonConfig { DelegateClass = delegateClass });
        }

        /// <inheritdoc />
        public override void ConfigFromString(string config)
        {
            var jsonSettings = JsonUtility.FromJson<JsonConfig>(config);
            delegateClass = jsonSettings.DelegateClass;
            NullifyDelegate();
        }

        [Serializable]
        protected class JsonConfig
        {
            public string DelegateClass;
        }

        /// <inheritdoc />
        public override byte[] ConfigToBytes()
        {
            var writer = new BGBinaryWriter(64);
            //version
            writer.AddInt(1);
            //NestedMetaId
            writer.AddString(delegateClass);

            //additional fields
            ConfigToBytes(writer);

            return writer.ToArray();
        }

        /// <inheritdoc />
        public override void ConfigFromBytes(ArraySegment<byte> config)
        {
            var reader = new BGBinaryReader(config);
            var version = reader.ReadInt();
            switch (version)
            {
                case 1:
                {
                    delegateClass = reader.ReadString();
                    ConfigFromBytes(version, reader);
                    NullifyDelegate();
                    break;
                }
                default:
                {
                    throw new BGException("Unknown version: $", version);
                }
            }
        }

        protected virtual void ConfigToBytes(BGBinaryWriter writer)
        {
        }
        protected virtual void ConfigFromBytes(int version, BGBinaryReader reader)
        {
        }

        protected void NullifyDelegate()
        {
            SetDelegate(null);
            delegateLoadTried = false;
        }

        //================================================================================================
        //                                              Serialization
        //================================================================================================
        protected override byte[] ValueToBytes(BGFieldCodedValue value) => value?.ToBytes();

        protected override BGFieldCodedValue ValueFromBytes(int entityIndex, ArraySegment<byte> segment)
        {
            if (segment.Count == 0) return null;
            var result = new BGFieldCodedValue(this, Meta.GetEntity(entityIndex));
            result.FromBytes(segment);
            return result;
        }

        protected override string ValueToString(BGFieldCodedValue value) => value?.ToJsonString();

        protected override BGFieldCodedValue ValueFromString(int entityIndex, string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            var result = new BGFieldCodedValue(this, Meta.GetEntity(entityIndex));
            result.FromJsonString(value);
            return result;
        }
    }
}