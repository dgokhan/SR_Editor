/*
<copyright file="BGFieldUnitySprite.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;
using UnityEngine.U2D;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// sprite field
    /// </summary>
    [FieldDescriptor(Name = "unitySprite", Folder = "Unity Asset", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerUnitySprite")]
    public partial class BGFieldUnitySprite : BGFieldUnityAssetA<Sprite>, BGAddressablesAssetCustomLoaderI
    {
        public const ushort CodeType = 55;

        /// <inheritdoc />
        public override ushort TypeCode => CodeType;

        /// <summary>
        /// sprite origin
        /// </summary>
        public enum LocationEnum
        {
            Single = 0,
            Multiple = 1,
            SpriteAtlas = 2
        }

        /// <summary>
        /// which location should be acceptable
        /// </summary>
        [Flags]
        public enum LocationConstraintEnum
        {
            None = 0,
            Single = 1,
            Multiple = 2,
            SpriteAtlas = 4
        }

        private const char PathSeparator = ':';

        //sprite array type
        private static readonly Type spriteArray = typeof(Sprite).MakeArrayType();

        private LocationConstraintEnum locationConstraint;

        /// <summary>
        /// constraints for sprites location
        /// </summary>
        public LocationConstraintEnum LocationConstraint
        {
            get => locationConstraint;
            set
            {
                if (locationConstraint == value) return;
                locationConstraint = value;
                Meta.Repo.Events.MetaWasChanged(Meta);
            }
        }

        //for new field
        public BGFieldUnitySprite(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldUnitySprite(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc />
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldUnitySprite(meta, id, name);

        //================================================================================================
        //                                              Config
        //================================================================================================
        /// <inheritdoc />
        public override string ConfigToString()
        {
            var config = new SpriteConfig
            {
                LoaderType = assetLoader.GetType().FullName,
                LoaderConfig = assetLoader.ConfigToString(),
                LocationConstraint = (int)locationConstraint
            };
            return JsonUtility.ToJson(config);
        }

        /// <inheritdoc />
        public override void ConfigFromString(string config)
        {
            if (string.IsNullOrEmpty(config)) assetLoader = new BGAssetLoaderResources();
            else
            {
                var jsonConfig = JsonUtility.FromJson<SpriteConfig>(config);
                assetLoader = BGUtil.Create<BGAssetLoaderA>(jsonConfig.LoaderType, false);
                assetLoader.ConfigFromString(jsonConfig.LoaderConfig);
                locationConstraint = (LocationConstraintEnum)jsonConfig.LocationConstraint;
            }
        }

        [Serializable]
        private class SpriteConfig : JsonConfig
        {
            public int LocationConstraint;
        }

        /// <inheritdoc />
        public override byte[] ConfigToBytes()
        {
            var loaderType = assetLoader.GetType().AssemblyQualifiedName;
            var byteConfig = assetLoader.ConfigToBytes();

            var writer = new BGBinaryWriter(4 + BGBinaryWriter.GetBytesCount(loaderType) + BGBinaryWriter.GetBytesCount(byteConfig));
            //version
            writer.AddInt(2);
            //loader type
            writer.AddString(loaderType);
            //loader config
            writer.AddByteArray(byteConfig);

            //v.2
            writer.AddInt((int)locationConstraint);

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
                    ReadLoader(reader);
                    break;
                }
                case 2:
                {
                    ReadLoader(reader);
                    locationConstraint = (LocationConstraintEnum)reader.ReadInt();
                    break;
                }
                default:
                {
                    throw new BGException("Unknown version: $", version);
                }
            }
        }

        private void ReadLoader(BGBinaryReader reader)
        {
            assetLoader = BGUtil.Create<BGAssetLoaderA>(reader.ReadString(), false);
            assetLoader.ConfigFromBytes(reader.ReadByteArray());
        }


        //================================================================================================
        //                                              BGAddressablesAssetCustomLoaderI
        //================================================================================================
        /// <inheritdoc />
        public BGAddressablesLoaderModel GetAddressablesLoaderModel(int entityIndex)
        {
            var address = GetStoredValue(entityIndex);
            if (string.IsNullOrEmpty(address)) return null;

            var location = new SpriteLocation(address);
            switch (location.Location)
            {
                case LocationEnum.Single:
                    return new BGAddressablesLoaderModel(address, typeof(Sprite));
                case LocationEnum.Multiple:
                    return new BGAddressablesLoaderModel(location.AssetPath, spriteArray);
                case LocationEnum.SpriteAtlas:
                    return new BGAddressablesLoaderModel(location.AssetPath, typeof(SpriteAtlas));
                default:
                    throw new ArgumentOutOfRangeException("location.Location");
            }
        }

        /// <inheritdoc />
        public override string GetAddressablesAddress(int entityIndex)
        {
            var address = GetStoredValue(entityIndex);
            if (string.IsNullOrEmpty(address)) return null;

            var location = new SpriteLocation(address);
            switch (location.Location)
            {
                case LocationEnum.Single:
                    return address;
                case LocationEnum.Multiple:
                case LocationEnum.SpriteAtlas:
                    if (string.IsNullOrEmpty(location.AssetPath) || string.IsNullOrEmpty(location.SubAssetPath)) return null;
                    return location.AssetPath + '[' + location.SubAssetPath + ']';
                default:
                    throw new ArgumentOutOfRangeException("location.Location");
            }
        }

        //================================================================================================
        //                                              Value
        //================================================================================================
        /// <inheritdoc />
        public override Sprite this[int entityIndex]
        {
            get
            {
                var path = GetStoredValue(entityIndex);
                if (string.IsNullOrEmpty(path)) return null;

                if (BGAssetsCache.Enabled && BGAssetsCache.TryToGet(path, out var cAsset) && cAsset is Sprite s) return s;

                Sprite result = null;
                //Addressables supports address[subAddress] notation
                if (assetLoader is BGAssetLoaderAddressables)
                {
                    var addressablesAddress = GetAddressablesAddress(entityIndex);
                    if (string.IsNullOrEmpty(addressablesAddress)) return null;
                    var sprite = assetLoader.Load<Sprite>(addressablesAddress);
                    if (sprite != null && sprite.name.EndsWith("(Clone)")) sprite.name = sprite.name.Replace("(Clone)", "");
                    result = sprite;
                }
                else
                {
                    var location = new SpriteLocation(path);
                    switch (location.Location)
                    {
                        case LocationEnum.Single:
                            result = assetLoader.Load<Sprite>(location.AssetPath);
                            break;
                        case LocationEnum.Multiple:
                            //Is there any alternative???
                            var sprites = assetLoader.LoadAll<Sprite>(location.AssetPath);
                            if (sprites == null) return null;
                            for (var i = 0; i < sprites.Length; i++)
                            {
                                var sprite = sprites[i];
                                if (!string.Equals(sprite.name, location.SubAssetPath)) continue;
                                result = sprite;
                                break;
                            }

                            break;
                        case LocationEnum.SpriteAtlas:
                            //this code produces sprite clones!! is there any alternatives?
                            var atlas = assetLoader.Load<SpriteAtlas>(location.AssetPath);
                            if (atlas == null) return null;
                            result = atlas.GetSprite(location.SubAssetPath);
                            if (result != null && result.name.EndsWith("(Clone)")) result.name = result.name.Replace("(Clone)", "");
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

        /// <summary>
        /// data container for all information regarding single sprite location 
        /// </summary>
        public struct SpriteLocation
        {
            private readonly LocationEnum location;
            private readonly string assetPath;
            private readonly string subAssetPath;
            private readonly string fullPath;

            /// <summary>
            /// where sprite is located?
            /// </summary>
            public LocationEnum Location => location;

            /// <summary>
            /// asset path
            /// </summary>
            public string AssetPath => assetPath;

            /// <summary>
            /// subasset path
            /// </summary>
            public string SubAssetPath => subAssetPath;

            /// <summary>
            /// full asset path
            /// </summary>
            public string FullPath => fullPath;

            public SpriteLocation(LocationEnum location, string assetPath, string subAssetPath)
            {
                this.location = location;
                this.assetPath = assetPath;
                this.subAssetPath = subAssetPath;
                switch (location)
                {
                    case LocationEnum.Single:
                    {
                        fullPath = assetPath;
                        break;
                    }
                    case LocationEnum.Multiple:
                    case LocationEnum.SpriteAtlas:
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

            public SpriteLocation(string path)
            {
                fullPath = path ?? throw new ArgumentException("path is null");

                location = LocationEnum.Single;
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
                    case '2':
                    {
                        var lastIndex = assetPath.LastIndexOf(PathSeparator);
                        if (lastIndex > 2 && lastIndex < assetPath.Length - 1)
                        {
                            subAssetPath = assetPath.Substring(lastIndex + 1);
                            assetPath = assetPath.Substring(2, lastIndex - 2);
                            location = typeCode == '1' ? LocationEnum.Multiple : LocationEnum.SpriteAtlas;
                        }

                        break;
                    }
                }
            }

            public override string ToString()
            {
                return fullPath;
            }
        }
    }
}