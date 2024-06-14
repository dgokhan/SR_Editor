/*
<copyright file="BGFieldUnityAssetArrayA.cs" company="BansheeGz">
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
    /// Abstract field referencing unity asset array
    /// </summary>
    public abstract partial class BGFieldUnityAssetArrayA<T> : BGFieldCachedA<T[], string>, BGBinaryBulkLoaderClass,
        BGAssetLoaderA.WithLoaderI, BGStorableString, BGAddressablesAssetI where T : Object
    {
        //================================================================================================
        //                                              Fields
        //================================================================================================

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

        /// <inheritdoc />
        public override bool StoredValueIsTheSameAsValueType => false;

        protected BGFieldUnityAssetArrayA(BGMetaEntity meta, string name) : base(meta, name)
        {
            assetLoader = new BGAssetLoaderResources();
        }

        protected BGFieldUnityAssetArrayA(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Config
        //================================================================================================
        /// <inheritdoc />
        public override string ConfigToString() => JsonUtility.ToJson(new JsonConfig { LoaderType = assetLoader.GetType().FullName, LoaderConfig = assetLoader.ConfigToString() });

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
        //                                              Value
        //================================================================================================
        /// <inheritdoc />
        public override T[] this[int entityIndex]
        {
            get
            {
                var path = GetStoredValue(entityIndex);
                if (string.IsNullOrEmpty(path)) return null;
                if (BGAssetsCache.Enabled && BGAssetsCache.TryToGetAll(path, out var cAsset)) return (T[])cAsset;

                var result = assetLoader.LoadAll<T>(path);
                if (result != null)
                {
                    if(BGAddressablesMonitor.Enabled) BGAddressablesMonitor.AssetWasLoaded(this, Meta.FindEntityId(entityIndex));
                    if (BGAssetsCache.Enabled) BGAssetsCache.AddAll(path, result);
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

        //================================================================================================
        //                                             BGFieldUnityAssetI
        //================================================================================================
        /// <inheritdoc />
        public string GetAssetPath(int entityIndex) => GetStoredValue(entityIndex);

        /// <inheritdoc />
        public void SetAssetPath(int entityIndex, string path) => SetStoredValue(entityIndex, path);

        //================================================================================================
        //                                             BGAddressablesAssetI
        //================================================================================================
        /// <inheritdoc />
        public string GetAddressablesAddress(int entityIndex) => GetStoredValue(entityIndex);
    }
}