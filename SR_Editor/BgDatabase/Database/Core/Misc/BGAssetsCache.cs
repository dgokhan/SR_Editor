using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// abstract class for Level two cache for loaded Unity assets
    /// </summary>
    public abstract class BGAssetsCache
    {
        /// <summary>
        /// Singleton object
        /// </summary>
        private static BGAssetsCache Instance;

        /// <summary>
        /// Is cache enabled?
        /// </summary>
        public static bool Enabled
        {
            get => Instance != null;
            set
            {
                if (value)
                {
                    if (Instance == null) Instance = new BGAssetsCacheDefault();
                }
                else
                {
                    if (Instance != null) Clear();
                    Instance = null;
                }
            }
        }

        /// <summary>
        /// an option to override default implementation 
        /// </summary>
        public static void SetInstance(BGAssetsCache cache) => Instance = cache;

        /// <summary>
        /// Try to get an asst from cache using provided key 
        /// </summary>
        public static bool TryToGet(string key, out Object asset) => Instance.TryToGetAsset(key, out asset);

        /// <summary>
        /// add Unity asset to the cache
        /// </summary>
        public static bool Add(string key, Object asset) => Instance.AddAsset(key, asset);

        /// <summary>
        /// Get all asset with provided key
        /// </summary>
        public static bool TryToGetAll(string key, out Object[] assets) => Instance.TryToGetAssetAll(key, out assets);

        /// <summary>
        /// add all assets to the cache
        /// </summary>
        public static bool AddAll(string key, Object[] assets) => Instance.AddAssetAll(key, assets);

        /// <summary>
        /// Clear the cache
        /// </summary>
        public static void Clear() => Instance.ClearAssets();


        protected BGAssetsCache()
        {
        }

        protected abstract bool TryToGetAsset(string key, out Object asset);
        protected abstract bool TryToGetAssetAll(string key, out Object[] assets);
        protected abstract bool AddAsset(string key, Object asset);
        protected abstract bool AddAssetAll(string key, Object[] assets);
        protected abstract void ClearAssets();

        //default cache implementation
        private class BGAssetsCacheDefault : BGAssetsCache
        {
            private readonly Dictionary<string, Object> key2Asset = new Dictionary<string, Object>();
            private readonly Dictionary<string, Object[]> key2AssetAll = new Dictionary<string, Object[]>();

            protected override bool TryToGetAsset(string key, out Object asset)
            {
                asset = null;
                if (key == null) return false;
                if (!key2Asset.TryGetValue(key, out asset)) return false;
                if (asset == null)
                {
                    key2Asset.Remove(key);
                    return false;
                }

                return true;
            }

            protected override bool TryToGetAssetAll(string key, out Object[] assets)
            {
                assets = null;
                if (key == null) return false;
                if (!key2AssetAll.TryGetValue(key, out assets)) return false;
                if (assets == null)
                {
                    key2Asset.Remove(key);
                    return false;
                }

                return true;
            }

            protected override bool AddAsset(string key, Object asset)
            {
                if (key == null || asset == null) return false;
                key2Asset[key] = asset;
                return true;
            }

            protected override bool AddAssetAll(string key, Object[] assets)
            {
                if (key == null || assets == null || assets.Length == 0) return false;
                key2AssetAll[key] = assets;
                return true;
            }

            protected override void ClearAssets()
            {
                key2Asset.Clear();
                key2AssetAll.Clear();
            }
        }
    }
}