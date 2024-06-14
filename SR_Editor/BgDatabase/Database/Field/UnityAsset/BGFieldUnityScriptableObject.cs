/*
<copyright file="BGFieldUnityScriptableObject.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// scriptable object field
    /// </summary>
    [FieldDescriptor(Name = "unityScriptableObject", Folder = "Unity Asset", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerUnityScriptableObject")]
    public class BGFieldUnityScriptableObject : BGFieldUnityAssetA<ScriptableObject>
    {
        public const ushort CodeType = 54;
        public override ushort TypeCode => CodeType;

        private Type scriptableObjectType;
        private bool allowSubclasses;

        /// <summary>
        /// The scriptable object type 
        /// </summary>
        public Type ScriptableObjectType
        {
            get => scriptableObjectType;
            set
            {
                if (scriptableObjectType == value) return;
                if (value != null && !value.IsSubclassOf(typeof(ScriptableObject))) throw new BGException("scriptableObjectType should be a subclass of ScriptableObject type!");
                scriptableObjectType = value;
                events.MetaWasChanged(Meta);
            }
        }

        /// <summary>
        /// Should subclasses also be acceptable as valid field values?
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
        public BGFieldUnityScriptableObject(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldUnityScriptableObject(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc />
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldUnityScriptableObject(meta, id, name);

        //================================================================================================
        //                                              Serialization
        //================================================================================================
        /// <inheritdoc />
        public override string ConfigToString()
        {
            return JsonUtility.ToJson(new JsonConfigSo
            {
                LoaderType = assetLoader.GetType().FullName,
                LoaderConfig = assetLoader.ConfigToString(),
                SOType = scriptableObjectType?.FullName,
                AllowSubclasses = allowSubclasses
            });
        }

        /// <inheritdoc />
        public override void ConfigFromString(string config)
        {
            if (string.IsNullOrEmpty(config))
            {
                assetLoader = new BGAssetLoaderResources();
                return;
            }

            var jsonConfig = JsonUtility.FromJson<JsonConfigSo>(config);
            assetLoader = BGUtil.Create<BGAssetLoaderA>(jsonConfig.LoaderType, false);
            assetLoader.ConfigFromString(jsonConfig.LoaderConfig);

            var className = jsonConfig.SOType;
            if (!string.IsNullOrEmpty(className)) scriptableObjectType = BGUtil.GetType(className);
            allowSubclasses = jsonConfig.AllowSubclasses;
        }


        [Serializable]
        private class JsonConfigSo : JsonConfig
        {
            public string SOType;
            public bool AllowSubclasses;
        }

        /// <inheritdoc />
        protected override void ConfigToBytes(BGBinaryWriter writer)
        {
            writer.AddString(scriptableObjectType?.AssemblyQualifiedName);
            writer.AddBool(allowSubclasses);
        }

        /// <inheritdoc />
        protected override void ConfigFromBytes(int version, BGBinaryReader reader)
        {
            switch (version)
            {
                case 1:
                {
                    var className = reader.ReadString();
                    if (!string.IsNullOrEmpty(className)) scriptableObjectType = BGUtil.GetType(className);
                    allowSubclasses = reader.ReadBool();
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