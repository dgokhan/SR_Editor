/*
<copyright file="BGFieldUnityAssetA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Text;
using UnityEngine;
#if !BG_SA
using Object = UnityEngine.Object;
#endif

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Abstract field referencing unity asset
    /// </summary>
    public abstract partial class BGFieldUnityAssetA<T> : BGFieldCachedA<T, string>, BGBinaryBulkLoaderClass,
        BGAssetLoaderA.WithLoaderI, BGStorableString, BGAddressablesAssetI where T : Object
    {
        //================================================================================================
        //                                              Fields
        //================================================================================================

        protected BGAssetLoaderA assetLoader;

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

        /// <inheritdoc />
        public override bool StoredValueIsTheSameAsValueType => false;

        protected BGFieldUnityAssetA(BGMetaEntity meta, string name) : base(meta, name)
        {
            assetLoader = new BGAssetLoaderResources();
        }

        protected BGFieldUnityAssetA(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Config
        //================================================================================================
        /// <inheritdoc />
        public override string ConfigToString() => ConfigToString(new JsonConfig());

        /// <inheritdoc />
        public override void ConfigFromString(string config) => ConfigFromString<JsonConfig>(config, null);

        protected virtual string ConfigToString(JsonConfig config)
        {
            config.LoaderType = assetLoader.GetType().FullName;
            config.LoaderConfig = assetLoader.ConfigToString();
            return JsonUtility.ToJson(config);
        }


        protected virtual void ConfigFromString<T>(string config, Action<T> callback) where T : JsonConfig
        {
            if (string.IsNullOrEmpty(config)) assetLoader = new BGAssetLoaderResources();
            else
            {
                var jsonConfig = JsonUtility.FromJson<T>(config);
                assetLoader = BGUtil.Create<BGAssetLoaderA>(jsonConfig.LoaderType, false);
                assetLoader.ConfigFromString(jsonConfig.LoaderConfig);
                if (callback != null) callback(jsonConfig);
            }
        }

        [Serializable]
        protected class JsonConfig
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
                    assetLoader = BGUtil.Create<BGAssetLoaderA>(reader.ReadString(), false);
                    assetLoader.ConfigFromBytes(reader.ReadByteArray());
                    //additional fields
                    ConfigFromBytes(version, reader);
                    break;
                }
                default:
                {
                    throw new BGException("Unknown version: $", version);
                }
            }
        }

        /// <summary>
        /// flush additional parameters to config
        /// </summary>
        protected virtual void ConfigToBytes(BGBinaryWriter writer)
        {
        }

        /// <summary>
        /// restore additional parameters from config
        /// </summary>
        protected virtual void ConfigFromBytes(int version, BGBinaryReader reader)
        {
        }

        //================================================================================================
        //                                              Value
        //================================================================================================
        /// <inheritdoc />
        public override T this[int entityIndex]
        {
            get
            {
                var path = GetStoredValue(entityIndex);
                if (string.IsNullOrEmpty(path)) return null;

                if (BGAssetsCache.Enabled && BGAssetsCache.TryToGet(path, out var asset) && asset is T tAsset) return tAsset;

                var result = assetLoader.Load<T>(path);
                if (result != null)
                {
                    if(BGAddressablesMonitor.Enabled) BGAddressablesMonitor.AssetWasLoaded(this, Meta.FindEntityId(entityIndex));
                    if (BGAssetsCache.Enabled) BGAssetsCache.Add(path, result);
                }
                return result;
            }
            set
            {
                //it's not possible to set value here, use SetStoredValue instead
            }
        }

        /// <inheritdoc />
        public override void OnEntityDelete(BGEntity entity)
        {
            if(BGAddressablesMonitor.Enabled && BGAddressablesMonitor.UnloadOnRowDelete) BGAddressablesMonitor.UnloadAsset(this, entity.Id, entity.Index);
            base.OnEntityDelete(entity);
        }

        //================================================================================================
        //                                              BGFieldUnityAssetI
        //================================================================================================
        /// <inheritdoc />
        public virtual string GetAssetPath(int entityIndex) => GetStoredValue(entityIndex);

        /// <inheritdoc />
        public void SetAssetPath(int entityIndex, string path) => SetStoredValue(entityIndex, path);

        //================================================================================================
        //                                             BGAddressablesAssetI
        //================================================================================================
        /// <inheritdoc />
        public virtual string GetAddressablesAddress(int entityIndex) => GetStoredValue(entityIndex);

        //================================================================================================
        //                                              Serialization
        //================================================================================================
        /// <inheritdoc />
        public override byte[] ToBytes(int entityIndex)
        {
            var value = GetStoredValue(entityIndex);
            return value == null ? null : Encoding.UTF8.GetBytes(value);
        }

        /// <inheritdoc />
        public override void FromBytes(int entityIndex, ArraySegment<byte> segment)
        {
            if (segment.Count == 0) StoreItems[entityIndex] = null;
            else StoreItems[entityIndex] = Encoding.UTF8.GetString(segment.Array, segment.Offset, segment.Count);
        }

        /// <inheritdoc />
        public void FromBytes(BGBinaryBulkRequestClass request)
        {
            var array = request.Array;
            var requests = request.CellRequests;
            var length = requests.Length;
            var encoding = Encoding.UTF8;
            for (var i = 0; i < length; i++)
            {
                var cellRequest = requests[i];
                try
                {
                    StoreItems[cellRequest.EntityIndex] = encoding.GetString(array, cellRequest.Offset, cellRequest.Count);
                }
                catch (Exception e)
                {
                    request.OnError?.Invoke(e);
                }
            }
        }

        /// <inheritdoc />
        public override string ToString(int entityIndex) => GetStoredValue(entityIndex);

        /// <inheritdoc />
        public override void FromString(int entityIndex, string value)
        {
            if (string.IsNullOrEmpty(value)) StoreItems[entityIndex] = null;
            else StoreItems[entityIndex] = value;
        }
    }
}