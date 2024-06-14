/*
<copyright file="BGFieldRelationSA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// relation field, referencing one single table 
    /// </summary>
    public abstract class BGFieldRelationSA<T, TStoreType> : BGFieldRelationA<T, TStoreType>, BGRelationI
    {
        internal BGId toId;

        public BGMetaEntity To => Meta.Repo.GetMeta(toId);

        /// <inheritdoc />
        public BGId ToId => toId;

        protected BGFieldRelationSA(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        protected BGFieldRelationSA(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        protected BGFieldRelationSA(BGMetaEntity meta, string name, BGMetaEntity to) : base(meta, name)
        {
            if (to == null)
            {
                Meta.Unregister(this);
                throw new BGException("'To' can not be null");
            }

            toId = to.Id;
        }

        //================================================================================================
        //                                              Relation
        //================================================================================================
        /// <inheritdoc />
        public BGMetaEntity RelatedMeta => To;

        //================================================================================================
        //                                              Configuration
        //================================================================================================
        /// <inheritdoc />
        public override string ConfigToString()
        {
            return JsonUtility.ToJson(new JsonConfig { ToId = toId.ToString() });
        }

        /// <inheritdoc />
        public override void ConfigFromString(string config)
        {
            toId = new BGId(JsonUtility.FromJson<JsonConfig>(config).ToId);
        }

        [Serializable]
        private struct JsonConfig
        {
            public string ToId;
        }

        /// <inheritdoc />
        public override byte[] ConfigToBytes()
        {
            var writer = new BGBinaryWriter(20);
            //version
            writer.AddInt(1);
            //toId
            writer.AddId(toId);

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
                    toId = reader.ReadId();
                    break;
                }
                default:
                {
                    throw new BGException("Unknown version: $", version);
                }
            }
        }

        //================================================================================================
        //                                              Utils
        //================================================================================================
        public static BGId IdFromString(string value)
        {
            var index = value.LastIndexOf(ValueIdSeparator);
            BGId val;
            if (index < 0) val = new BGId(value.Trim());
            else
            {
                var trim = value.Substring(index + 1).Trim();
                val = new BGId(trim);
            }

//            var val = index < 0 ? new Guid(value.Trim()) : new Guid(value.Substring(index+1).Trim());
            return val;
        }
    }
}