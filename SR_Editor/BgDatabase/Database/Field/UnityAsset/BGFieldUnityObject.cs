/*
<copyright file="BGFieldUnityObject.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

#if !BG_SA
using Object = UnityEngine.Object;
#endif



namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// any Unity asset field. Value should be casted from UnityEngine.Object to required type
    /// </summary>
    [FieldDescriptor(Name = "unityObject", Folder = "Unity Asset", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerUnityObject")]
    public partial class BGFieldUnityObject : BGFieldUnityAssetA<Object>, BGAddressablesAssetCustomLoaderI
    {
        public const ushort CodeType = 52;

        /// <inheritdoc />
        public override ushort TypeCode => CodeType;

        private const char PathSeparator = ':';

        /// <summary>
        /// Some components can have multiple instances assigned
        /// </summary>
        public enum AssetLocationEnum
        {
            Single = 0,
            Complex = 1
        }


        private string assetTypeName;
        private Type assetType;
        private bool typeLoadTried;

        private bool allowSubclasses;

        //should be new or override??
        public Type AssetType
        {
            set
            {
                if (value == assetType) return;
                if (value == null)
                {
                    assetType = null;
                    assetTypeName = null;
                }
                else
                {
                    if (!value.IsSubclassOf(typeof(Object)))
                        throw new BGException("Can not change assetType, cause submitted value type is not inherited from UnityEngine.Object, value=$", value.FullName);
                    assetType = value;
                    assetTypeName = value.AssemblyQualifiedName;
                }

                events.MetaWasChanged(Meta);
            }
            get
            {
                if (assetType != null || typeLoadTried) return assetType;
                typeLoadTried = true;
                if (string.IsNullOrEmpty(assetTypeName)) return assetType;

                var type = BGUtil.GetType(assetTypeName);
                if (type != null && type.IsSubclassOf(typeof(Object))) assetType = type;

                return assetType;
            }
        }

        /// <summary>
        /// for type constraint: should subclasses be supported? 
        /// </summary>
        public bool AllowSubclasses
        {
            get => allowSubclasses;
            set
            {
                if (allowSubclasses == value) return;
                allowSubclasses = value;
                events.MetaWasChanged(Meta);
            }
        }

        //for new field
        public BGFieldUnityObject(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldUnityObject(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc />
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldUnityObject(meta, id, name);

        //================================================================================================
        //                                              Config
        //================================================================================================
        /// <inheritdoc />
        public override string ConfigToString() => ConfigToString(new JsonConfigObject { AssetTypeName = assetTypeName, AllowSubclasses = allowSubclasses });

        /// <inheritdoc />
        public override void ConfigFromString(string config)
        {
            ConfigFromString<JsonConfigObject>(config, jsonConfig =>
            {
                assetTypeName = jsonConfig.AssetTypeName;
                allowSubclasses = jsonConfig.AllowSubclasses;
            });
        }

        [Serializable]
        protected class JsonConfigObject : JsonConfig
        {
            public string AssetTypeName;
            public bool AllowSubclasses;
        }

        /// <inheritdoc />
        public override byte[] ConfigToBytes()
        {
            var loaderType = assetLoader.GetType().AssemblyQualifiedName;
            var byteConfig = assetLoader.ConfigToBytes();

            var writer = new BGBinaryWriter(4 + BGBinaryWriter.GetBytesCount(loaderType) + BGBinaryWriter.GetBytesCount(byteConfig));
            //version
            writer.AddInt(3);
            //loader type
            writer.AddString(loaderType);
            //loader config
            writer.AddByteArray(byteConfig);

            //version 2
            writer.AddString(assetTypeName);

            //version 3
            writer.AddBool(allowSubclasses);

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
                case 2:
                {
                    assetLoader = BGUtil.Create<BGAssetLoaderA>(reader.ReadString(), false);
                    assetLoader.ConfigFromBytes(reader.ReadByteArray());

                    assetTypeName = reader.ReadString();
                    ResetType();
                    break;
                }
                case 3:
                {
                    assetLoader = BGUtil.Create<BGAssetLoaderA>(reader.ReadString(), false);
                    assetLoader.ConfigFromBytes(reader.ReadByteArray());

                    assetTypeName = reader.ReadString();
                    allowSubclasses = reader.ReadBool();
                    ResetType();
                    break;
                }
                default:
                {
                    throw new BGException("Unknown version: $", version);
                }
            }
        }

        private void ResetType()
        {
            assetType = null;
            typeLoadTried = false;
        }

        //================================================================================================
        //                                              Value
        //================================================================================================
        /// <summary>
        /// Can provided object be assigned as a value? 
        /// </summary>
        public bool CanBeAssigned(Object value)
        {
            if (value == null) return true;
            if (AssetType == null) return true;

            if (AssetType != value.GetType())
            {
                if (!allowSubclasses) return false;
                if (!value.GetType().IsSubclassOf(AssetType)) return false;
            }

            return true;
        }

        /// <inheritdoc />
        public override Object this[int entityIndex]
        {
            get
            {
                var path = GetStoredValue(entityIndex);
                if (string.IsNullOrEmpty(path)) return null;
                if (BGAssetsCache.Enabled && BGAssetsCache.TryToGet(path, out var cAsset)) return cAsset;

                Object result = null;
                if (assetLoader is BGAssetLoaderAddressables)
                {
                    //Addressables supports address[subAddress] notation
                    var addressablesAddress = GetAddressablesAddress(entityIndex);
                    if (string.IsNullOrEmpty(addressablesAddress)) return null;
                    result = assetLoader.Load<Object>(addressablesAddress);
                }
                else
                {
                    var location = new AssetLocation(path);
                    switch (location.Location)
                    {
                        case AssetLocationEnum.Single:
                            result = assetLoader.Load<Object>(location.AssetPath);
                            break;
                        case AssetLocationEnum.Complex:
                            if (assetLoader is BGAssetLoaderAddressables) result = assetLoader.Load<Object>(location.AssetPath + '[' + location.SubAssetPath + ']');
                            else
                            {
                                //Is there any alternative???
                                var assets = assetLoader.LoadAll<Object>(location.AssetPath);
                                if (assets == null) return null;
                                for (var i = 0; i < assets.Length; i++)
                                {
                                    var asset = assets[i];
                                    if (!string.Equals(asset.name, location.SubAssetPath)) continue;
                                    result = asset;
                                    break;
                                }
                            }

                            break;
                        default:
                            throw new ArgumentOutOfRangeException("location.Location");
                    }
                }

                if (result != null)
                {
                    if(BGAddressablesMonitor.Enabled) BGAddressablesMonitor.AssetWasLoaded(this, Meta.FindEntityId(entityIndex));
                    if (BGAssetsCache.Enabled) BGAssetsCache.Add(path, result);
                }

                return result;
            }
        }

        //================================================================================================
        //                                              addressables
        //================================================================================================
        /// <inheritdoc />
        public override string GetAddressablesAddress(int entityIndex)
        {
            var address = GetStoredValue(entityIndex);
            if (string.IsNullOrEmpty(address)) return null;

            var location = new AssetLocation(address);
            switch (location.Location)
            {
                case AssetLocationEnum.Single:
                    return address;
                case AssetLocationEnum.Complex:
                    if (string.IsNullOrEmpty(location.AssetPath) || string.IsNullOrEmpty(location.SubAssetPath)) return null;
                    return location.AssetPath + '[' + location.SubAssetPath + ']';
                default:
                    throw new ArgumentOutOfRangeException("location.Location");
            }
        }

        /// <inheritdoc />
        public BGAddressablesLoaderModel GetAddressablesLoaderModel(int entityIndex)
        {
            var address = GetStoredValue(entityIndex);
            if (string.IsNullOrEmpty(address)) return null;

            var location = new AssetLocation(address);
            switch (location.Location)
            {
                case AssetLocationEnum.Single:
                    return new BGAddressablesLoaderModel(address, typeof(Object));
                case AssetLocationEnum.Complex:
                    return new BGAddressablesLoaderModel(location.AssetPath + '[' + location.SubAssetPath + ']', typeof(Object));
                default:
                    throw new ArgumentOutOfRangeException("location.Location");
            }
        }
        //================================================================================================
        //                                              Nested
        //================================================================================================

        /// <summary>
        /// struct for details about asset location
        /// </summary>
        public struct AssetLocation
        {
            private readonly AssetLocationEnum location;
            private readonly string assetPath;
            private readonly string subAssetPath;
            private readonly string fullPath;

            /// <summary>
            /// is path single or complex?
            /// </summary>
            public AssetLocationEnum Location => location;

            /// <summary>
            /// asset path
            /// </summary>
            public string AssetPath => assetPath;

            /// <summary>
            /// subasset path 
            /// </summary>
            public string SubAssetPath => subAssetPath;

            /// <summary>
            /// Full asset path
            /// </summary>
            public string FullPath => fullPath;

            public AssetLocation(AssetLocationEnum location, string assetPath, string subAssetPath)
            {
                this.location = location;
                this.assetPath = assetPath;
                this.subAssetPath = subAssetPath;
                switch (location)
                {
                    case AssetLocationEnum.Single:
                    {
                        fullPath = assetPath;
                        break;
                    }
                    case AssetLocationEnum.Complex:
                        var result = "" + (int)location;
                        result += PathSeparator;
                        result += assetPath;
                        result += PathSeparator;
                        result += subAssetPath;
                        fullPath = result;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("location");
                }
            }

            public AssetLocation(string path)
            {
                fullPath = path ?? throw new ArgumentException("path is null");

                location = AssetLocationEnum.Single;
                assetPath = path;
                subAssetPath = null;

                if (path.Length <= 2 || path[1] != PathSeparator) return;

                //protocol
                var typeCode = path[0];
                switch (typeCode)
                {
                    case '0':
                    {
                        assetPath = assetPath.Substring(2);
                        break;
                    }
                    case '1':
                    {
                        var lastIndex = assetPath.LastIndexOf(PathSeparator);
                        if (lastIndex > 2 && lastIndex < assetPath.Length - 1)
                        {
                            subAssetPath = assetPath.Substring(lastIndex + 1);
                            assetPath = assetPath.Substring(2, lastIndex - 2);
                            location = AssetLocationEnum.Complex;
                        }

                        break;
                    }
                }
            }

            public override string ToString() => fullPath;
        }
    }
}