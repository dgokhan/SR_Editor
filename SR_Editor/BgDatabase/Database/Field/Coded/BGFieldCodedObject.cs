/*
<copyright file="BGFieldCodedObject.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// object programmable field
    /// </summary>
    [FieldDescriptor(Name = "programmableObject", Folder = "Programmable", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerCodedObject")]
    public class BGFieldCodedObject : BGFieldCodedA<object>
    {
        public const ushort CodeType = 104;
        public override ushort TypeCode => CodeType;

        private string objectType;

        /// <summary>
        /// The object type 
        /// </summary>
        public string ObjectType
        {
            get => objectType;
            set
            {
                if (objectType == value) return;
                objectType = value;
                events.MetaWasChanged(Meta);
            }
        }

        //================================================================================================
        //                                              Constructors
        //================================================================================================
        public BGFieldCodedObject(BGMetaEntity meta, string name, Type delegateType) : base(meta, name, delegateType)
        {
        }

        protected internal BGFieldCodedObject(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc/>
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldCodedObject(meta, id, name);
        
        //================================================================================================
        //                                              Serialization
        //================================================================================================
        /// <inheritdoc />
        public override string ConfigToString()
        {
            return JsonUtility.ToJson(new JsonConfigObject
            {
                DelegateClass = delegateClass,
                ObjectType = objectType,
            });
        }

        /// <inheritdoc />
        public override void ConfigFromString(string config)
        {
            if (string.IsNullOrEmpty(config)) return;

            var jsonConfig = JsonUtility.FromJson<JsonConfigObject>(config);
            delegateClass = jsonConfig.DelegateClass;
            objectType = jsonConfig.ObjectType;
        }


        [Serializable]
        private class JsonConfigObject : JsonConfig
        {
            public string ObjectType;
        }

        /// <inheritdoc />
        protected override void ConfigToBytes(BGBinaryWriter writer)
        {
            writer.AddString(objectType);
        }

        /// <inheritdoc />
        protected override void ConfigFromBytes(int version, BGBinaryReader reader)
        {
            switch (version)
            {
                case 1:
                {
                    objectType = reader.ReadString();
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