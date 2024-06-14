/*
<copyright file="BGFieldLocaleAssetA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// uses delegate field to store values 
    /// </summary>
    public abstract partial class BGFieldLocaleAssetA<T> : BGFieldLocaleA<T>, BGFieldLocaleWithDelegateI,
        BGAssetLoaderA.WithLoaderI, BGStorableString, BGAddressablesAssetI where T : Object
    {
        private BGAssetLoaderA assetLoader;

        /// <inheritdoc />
        public BGAssetLoaderA AssetLoader
        {
            get => assetLoader;
            set
            {
                if (value == assetLoader) return;
                assetLoader = value ?? throw new BGException("Loader can not be null");
                events.MetaWasChanged(Meta);
            }
        }

        public virtual Type AssetType => ValueType;

        /// <inheritdoc />
        public override bool ReadOnly => true;

        public BGField FieldDelegate => DelegateField;

        public BGField<T> DelegateField
        {
            get
            {
                if (fieldDelegate == null) CreateDelegate();

                return (BGField<T>)fieldDelegate;
            }
        }

        public override bool StoredValueIsTheSameAsValueType => false;

        private BGField fieldDelegate;


        protected BGFieldLocaleAssetA(BGMetaEntity meta, string name) : base(meta, name)
        {
            assetLoader = new BGAssetLoaderResources();
        }

        protected BGFieldLocaleAssetA(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }


        //================================================================================================
        //                                              Delegate field
        //================================================================================================
        protected void EnsureStoreOnWrite()
        {
            if (fieldDelegate != null) return;
            CreateDelegate();
        }

        protected bool EnsureStoreOnRead()
        {
            if (fieldDelegate != null) return true;
            if (!IsMainRepo) return false;
            CreateDelegate();
            return true;
        }

        private void CreateDelegate()
        {
            EnsureStore();
            if (IsMainRepo)
            {
                try
                {
                    BGLocalizationReposCache.Load(this);
                }
                catch
                {
                    if (fieldDelegate == null) throw;
                    fieldDelegate.ValueChanged -= MyListener;
                    fieldDelegate = null;
                    throw;
                }
            }
        }

        public override void EnsureStore()
        {
            if (fieldDelegate != null) return;
            var meta = BGMetaEntity.Create(new BGRepo(), typeof(BGMetaRow).FullName, Meta.Id, Meta.Name, (string)null, true, null, false, false, false);
            fieldDelegate = Create(meta, BGLocaleFieldAttribute.Get(GetType()).DelegateFieldType.FullName, Id, Name, (string)null, true, null, null, false);
            Meta.ForEachEntity(entity => meta.NewEntity(entity.Id));

            if (fieldDelegate is BGAssetLoaderA.WithLoaderI delegateFieldWithLoader)
            {
                if (delegateFieldWithLoader.AssetLoader != AssetLoader) delegateFieldWithLoader.AssetLoader = AssetLoader;
            }

            fieldDelegate.Meta.Repo.Events.On = true;
            fieldDelegate.ValueChanged += MyListener;
        }

        private void MyListener(object sender, BGEventArgsField args)
        {
            FireValueChanged(args.Entity.Id);
        }

        public override void DestroyStore()
        {
            if (fieldDelegate != null)
            {
                fieldDelegate.ValueChanged -= MyListener;
                fieldDelegate.Delete();
            }

            fieldDelegate = null;
        }

        //for main repo fields only

        //================================================================================================
        //                                              callbacks
        //================================================================================================
        public override void OnEntityDelete(BGEntity entity)
        {
            var delegateEntity = fieldDelegate?.Meta.GetEntity(entity.Id);
            delegateEntity?.Delete();
        }

        public override void OnDelete()
        {
            if (fieldDelegate == null) return;
            fieldDelegate.Meta.Delete();
            fieldDelegate = null;
        }

        public override void OnEntityAdd(BGEntity entity)
        {
            fieldDelegate?.Meta.NewEntity(entity.Id);
        }

        public override void ClearValues()
        {
            if (fieldDelegate == null) return;
            fieldDelegate.Meta.Delete();
            fieldDelegate = null;
        }

        //================================================================================================
        //                                              Config
        //================================================================================================
        /// <inheritdoc />
        public override string ConfigToString()
        {
            return JsonUtility.ToJson(new JsonConfig { LoaderType = assetLoader.GetType().FullName, LoaderConfig = assetLoader.ConfigToString() });
        }

        /// <inheritdoc />
        public override void ConfigFromString(string config)
        {
            if (string.IsNullOrEmpty(config))
            {
                assetLoader = new BGAssetLoaderResources();
                return;
            }

            var jsonConfig = JsonUtility.FromJson<JsonConfig>(config);
            assetLoader = BGUtil.Create<BGAssetLoaderA>(jsonConfig.LoaderType, false);
            assetLoader.ConfigFromString(jsonConfig.LoaderConfig);
        }

        [Serializable]
        private struct JsonConfig
        {
            public string LoaderType;
            public string LoaderConfig;
        }

        /// <inheritdoc />
        public override byte[] ConfigToBytes()
        {
            var loaderType = assetLoader.GetType().AssemblyQualifiedName;
            var byteConfig = assetLoader.ConfigToBytes();

            var writer = new BGBinaryWriter(4 + BGBinaryWriter.GetBytesCount(loaderType) + BGBinaryWriter.GetBytesCount(byteConfig));
            //version
            writer.AddInt(1);
            //loader type
            writer.AddString(loaderType);
            //loader config
            writer.AddByteArray(byteConfig);
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
                    assetLoader = BGUtil.Create<BGAssetLoaderA>(reader.ReadString(), false);
                    assetLoader.ConfigFromBytes(reader.ReadByteArray());
                    break;
                }
                default:
                {
                    throw new BGException("Unknown version: $", version);
                }
            }
        }

        //================================================================================================
        //                                              value
        //================================================================================================

        public override bool AreStoredValuesEqual(BGField field, int myEntityIndex, int otherEntityIndex)
        {
            if (!EnsureStoreOnRead()) return true;

            var myEntityId = Meta.GetEntity(myEntityIndex).Id;


            //my delegate
            var delegateField = DelegateField;
            var delegateIndex = EnsureDelegateEntity(delegateField, myEntityId);


            var fromFieldLocale = (BGFieldLocaleWithDelegateI)field;
            var fromDelegate = fromFieldLocale.FieldDelegate;
            var fromIndex = EnsureDelegateEntity(fromDelegate, myEntityId);


            return delegateField.AreStoredValuesEqual(fromDelegate, delegateIndex, fromIndex);
        }

        public override void ClearValue(int entityIndex)
        {
            EnsureStoreOnWrite();
            var delegateField = DelegateField;
            var entity = Meta[entityIndex];
            var entityId = entity.Id;
            var index = delegateField.Meta.FindEntityIndex(entityId);
            if (events.On)
            {
                // var value = delegateField[index];
                delegateField.ClearValue(index);
                //no need to fire event- cause delegate field will fire it (MyListener method) 
                // if (!Equals(value, default(T))) FireValueChanged(entity);
            }
            else delegateField.ClearValue(index);
        }

        public override void ForEachValue(Action<int> action)
        {
            if (!EnsureStoreOnRead()) return;
            DelegateField.ForEachValue(action);
        }

        public override void CopyValue(BGField fromField, BGId fromEntityId, int fromEntityIndex, BGId toEntityId)
        {
            EnsureStoreOnWrite();
            if (fromField.IsDeleted) return;

            // var delegateField = DelegateField;
            var fromFieldTyped = (BGStorableString)fromField;
            if (fromEntityIndex < 0) fromEntityIndex = fromField.Meta.FindEntityIndex(fromEntityId);
            if (fromEntityIndex < 0) return;
            var toEntityIndex = Meta.FindEntityIndex(toEntityId);
            if (toEntityIndex < 0) return;
            var storedValue = fromFieldTyped.GetStoredValue(fromEntityIndex);
            SetStoredValue(toEntityIndex, storedValue);
        }

        public override void DuplicateValue(BGId fromEntityId, int fromEntityIndex, BGId toEntityId)
        {
            CopyValue(this, fromEntityId, fromEntityIndex, toEntityId);
        }

        //================================================================================================
        //                                              Serialization
        //================================================================================================

        public override byte[] ToBytes(int entityIndex)
        {
            var delegateField = DelegateField;
            var index = FindDelegateIndex(entityIndex, delegateField);
            if (index == -1) return null;
            return delegateField.ToBytes(index);
        }

        public override void FromBytes(int entityIndex, ArraySegment<byte> segment)
        {
            var delegateField = DelegateField;
            var index = FindDelegateIndex(entityIndex, delegateField);
            if (index == -1) return;
            delegateField.FromBytes(index, segment);
        }

        public override string ToString(int entityIndex)
        {
            var delegateField = DelegateField;
            var index = FindDelegateIndex(entityIndex, delegateField);
            if (index == -1) return null;
            return delegateField.ToString(index);
        }

        public override void FromString(int entityIndex, string value)
        {
            var delegateField = DelegateField;
            var index = FindDelegateIndex(entityIndex, delegateField);
            if (index == -1) index = EnsureDelegateEntity(delegateField, Meta[entityIndex].Id);
            delegateField.FromString(index, value);
        }

        //================================================================================================
        //                                              Value
        //================================================================================================

        public override T this[BGId entityId]
        {
            get
            {
                var index = Meta.FindEntityIndex(entityId);
                return index == -1 ? default : this[index];
            }
            set
            {
                var index = Meta.FindEntityIndex(entityId);
                if (index == -1) return;
                this[index] = value;
            }
        }

        public override T this[int index]
        {
            get
            {
                if (!EnsureStoreOnRead()) return default;
                var entity = Meta[index];
                var delegateField = DelegateField;
                var delegateEntityIndex = EnsureDelegateEntity(delegateField, entity.Id);

                return delegateField[delegateEntityIndex];
            }
            set
            {
                EnsureStoreOnWrite();
                var entity = Meta[index];
                var delegateField = DelegateField;
                var delegateEntityIndex = EnsureDelegateEntity(delegateField, entity.Id);
                if (events.On)
                {
                    // var oldValue = delegateField[delegateEntityIndex];
                    delegateField[delegateEntityIndex] = value;
                    //no need to fire event- cause delegate field will fire it (MyListener method) 
                    // if (!Equals(oldValue, value)) FireValueChanged(entity);
                }
                else delegateField[delegateEntityIndex] = value;
            }
        }

        public int EnsureDelegateEntity(BGId entityId)
        {
            return EnsureDelegateEntity(DelegateField, entityId);
        }

        //this method retrieve entity index by Id from delegate field (and create entity if it's not found)
        protected static int EnsureDelegateEntity(BGField delegateField, BGId entityId)
        {
            var delegateFieldMeta = delegateField.Meta;

            var delegateEntityIndex = delegateFieldMeta.FindEntityIndex(entityId);
            if (delegateEntityIndex == -1) delegateEntityIndex = delegateFieldMeta.NewEntity(entityId).Index;
            return delegateEntityIndex;
        }

        /// <inheritdoc/>
        [Obsolete("Use CloneTo(BGCloneContextField context) instead")]
        public override BGField CloneTo(BGMetaEntity meta, bool copyValues) => CloneTo(new BGCloneContextField(meta, copyValues));

        /// <inheritdoc/>
        public override BGField CloneTo(BGCloneContextField context)
        {
            var clone = base.CloneTo(context);
            context.OnAfterFieldCreated?.Invoke(clone);
            if (!context.copyValues) return clone;

            //try to transfer values
            var fieldDelegate = DelegateField;
            if (fieldDelegate == null || fieldDelegate.Meta == null || fieldDelegate.Meta.CountEntities == 0) return clone;
            var cloneFieldDelegate = ((BGFieldLocaleAssetA<T>)clone).DelegateField;
            if (cloneFieldDelegate == null) return clone;

            Meta.ForEachEntity(entity =>
            {
                var cloneEntity = cloneFieldDelegate.Meta.GetEntity(entity.Id) ?? cloneFieldDelegate.Meta.NewEntity(entity.Id);
                var delegateIndex = fieldDelegate.Meta.FindEntityIndex(entity.Id);
                if (delegateIndex >= 0) cloneFieldDelegate.CopyValue(fieldDelegate, entity.Id, delegateIndex, entity.Id);
            });
            return clone;
        }

        protected int FindDelegateIndex(int entityIndex, BGField<T> delegateField)
        {
            var id = Meta[entityIndex].Id;
            var index = delegateField.Meta.FindEntityIndex(id);
            return index;
        }

        /// <inheritdoc />
        public override void Swap(int entityIndex1, int entityIndex2)
        {
/*
//            no sense! 
            var delegateField = DelegateField;
            var index1 = FindDelegateIndex(entityIndex1, delegateField);
            var index2 = FindDelegateIndex(entityIndex2, delegateField);
            DelegateField.Meta.Swap(index1, index2);
*/
        }

        /// <inheritdoc />
        public override void MoveEntitiesValues(int fromIndex, int toIndex, int numberOfValues)
        {
//            no sense
        }

        //================================================================================================
        //                                              BGStorable
        //================================================================================================
        /// <inheritdoc />
        public void SetStoredValue(int entityIndex, string value)
        {
            var delegateField = DelegateField;
            var entityId = Meta[entityIndex].Id;
            var delegateIndex = delegateField.Meta.FindEntityIndex(entityId);
            if (delegateIndex == -1) delegateIndex = delegateField.Meta.NewEntity(entityId).Index;
            ((BGStorable<string>)delegateField).SetStoredValue(delegateIndex, value);
        }

        /// <inheritdoc />
        public string GetStoredValue(int entityIndex)
        {
            if (!EnsureStoreOnRead()) return null;
            var delegateField = DelegateField;
            var entityId = Meta[entityIndex].Id;
            var delegateIndex = delegateField.Meta.FindEntityIndex(entityId);
            if (delegateIndex == -1) delegateIndex = delegateField.Meta.NewEntity(entityId).Index;
            return ((BGStorable<string>)delegateField).GetStoredValue(delegateIndex);
        }

        //================================================================================================
        //                                              BGFieldUnityAssetI 
        //================================================================================================
        public string GetAssetPath(int entityIndex)
        {
            return GetStoredValue(entityIndex);
        }

        public void SetAssetPath(int entityIndex, string path)
        {
            SetStoredValue(entityIndex, path);
        }

        //================================================================================================
        //                                             BGAddressablesAssetI
        //================================================================================================
        public virtual string GetAddressablesAddress(int entityIndex)
        {
            if (!EnsureStoreOnRead()) return null;
            var delegateField = DelegateField;
            if (delegateField is BGAddressablesAssetI assetI)
            {
                var entityId = Meta[entityIndex].Id;
                var delegateIndex = delegateField.Meta.FindEntityIndex(entityId);
                if (delegateIndex == -1) delegateIndex = delegateField.Meta.NewEntity(entityId).Index;
                return assetI.GetAddressablesAddress(delegateIndex);
            }

            return GetStoredValue(entityIndex);
        }
    }
}