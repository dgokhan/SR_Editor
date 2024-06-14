/*
<copyright file="BGAssetLoaderAssetBundle.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;
// using Object = UnityEngine.Object;
#if !BG_SA
using ABundle = UnityEngine.AssetBundle;
#endif

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// loader for loading from asset bundles
    /// THIS LOADER IS NOT RECOMMENDED TO USE
    /// </summary>
    [AssetLoaderDescriptor(Name = "AssetBundle", ManagerType = "BansheeGz.BGDatabase.Editor.BGAssetLoaderManagerAssetBundle")]
    public partial class BGAssetLoaderAssetBundle : BGAssetLoaderA
    {
        private string assetBundle;

        /// <summary>
        /// target asset bundle
        /// </summary>
        public string AssetBundle
        {
            get => assetBundle;
            set => assetBundle = value;
        }

        //================================================================================================
        //                                              Load
        //================================================================================================
        public override string Name => "AssetBundle[" + assetBundle + "]";

        private AssetBundle TargetBundle
        {
            get
            {
                var bundles = ABundle.GetAllLoadedAssetBundles();
                if (bundles == null) return null;

                AssetBundle targetBundle = null;
                foreach (var bundle in bundles)
                {
                    if (!string.Equals(bundle.name, assetBundle)) continue;

                    targetBundle = bundle;
                    break;
                }

                return targetBundle;
            }
        }

        /// <inheritdoc />
        public override T Load<T>(string path)
        {
            if (string.IsNullOrEmpty(path)) return null;

            var targetBundle = TargetBundle;

            return targetBundle == null ? null : targetBundle.LoadAsset<T>(path);
        }

        /// <inheritdoc />
        public override T[] LoadAll<T>(string path)
        {
            if (string.IsNullOrEmpty(path)) return null;

            var targetBundle = TargetBundle;

            return targetBundle == null ? null : targetBundle.LoadAssetWithSubAssets<T>(path);
        }


        //================================================================================================
        //                                              Config
        //================================================================================================
        /// <inheritdoc />
        public override string ConfigToString() => JsonUtility.ToJson(new JsonConfig { AssetBundle = assetBundle });

        /// <inheritdoc />
        public override void ConfigFromString(string config) => assetBundle = JsonUtility.FromJson<JsonConfig>(config).AssetBundle;

        [Serializable]
        private struct JsonConfig
        {
            public string AssetBundle;
        }

        /// <inheritdoc />
        public override byte[] ConfigToBytes()
        {
            var writer = new BGBinaryWriter(4 + BGBinaryWriter.GetBytesCount(assetBundle));
            //version
            writer.AddInt(1);
            //loader type
            writer.AddString(assetBundle);
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
                    assetBundle = reader.ReadString();
                    break;
                }
                default:
                {
                    throw new BGException("Unknown version: $", version);
                }
            }
        }
    }
}