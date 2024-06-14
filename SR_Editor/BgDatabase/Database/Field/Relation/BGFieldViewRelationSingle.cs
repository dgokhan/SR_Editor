/*
<copyright file="BGFieldViewRelationSingle.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// view single relation Field 
    /// </summary>
    [FieldDescriptor(Name = "viewRelationSingle", Folder = "Relation", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerViewRelationSingle")]
    public class BGFieldViewRelationSingle : BGFieldManyRelationsSingle, BGFieldViewRelationI
    {
        public const ushort CodeType = 97;
        
        /// <inheritdoc/>
        public override ushort TypeCode => CodeType;
        
        private BGMetaView view;
        private BGId viewId;

        /// <summary>
        /// Related view
        /// </summary>
        public BGMetaView View
        {
            get
            {
                if (view != null) return view;
                view = Repo.GetView(viewId);
                return view;
            }
        }

        public BGId ViewId => viewId;

        //third parameter to base constructor is not used
        public BGFieldViewRelationSingle(BGMetaEntity meta, string name, BGMetaView to) : base(meta, name, new List<BGMetaEntity>(){meta})
        {
            if (to == null)
            {
                Meta.Unregister(this);
                throw new BGException("'To' view can not be null or empty");
            }

            this.view = to;
            viewId = to.Id;
        }

        internal BGFieldViewRelationSingle(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }
        
        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc/>
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldViewRelationSingle(meta, id, name);

        //================================================================================================
        //                                              Overrides
        //================================================================================================
        /// <inheritdoc/>
        public override List<BGMetaEntity> RelatedMetas => View.Metas;

        public override List<BGId> ToIds => new List<BGId>(View.Mappings.IncludedMetas);

        /// <inheritdoc/>
        public override  void RemoveRelatedMeta(BGMetaEntity metaEntity) => OnRemoveRelatedMeta(metaEntity);

        /// <inheritdoc/>
        public override void AddRelatedMeta(BGMetaEntity metaEntity)
        {
        }

        protected override void CheckMetaId(BGEntity entity)
        {
            if(!View.Mappings.IsIncluded(entity.MetaId)) throw new BGException("Can not assign entity [$] as related entity for field [$]: meta [$] is not included in view [$]!", 
                entity.FullName, FullName, entity.MetaName, View.Name);
        }
        
        //================================================================================================
        //                                              Configuration
        //================================================================================================
        /// <inheritdoc />
        public override string ConfigToString() => JsonUtility.ToJson(new JsonConfig { ViewId = viewId.ToString()});

        /// <inheritdoc />
        public override void ConfigFromString(string config)
        {
            if (string.IsNullOrEmpty(config)) return;
            var fromJson = JsonUtility.FromJson<JsonConfig>(config);
            view = null;
            BGId.TryParse(fromJson.ViewId, out viewId);
        }

        [Serializable]
        private struct JsonConfig
        {
            public string ViewId;
        }

        /// <inheritdoc />
        public override byte[] ConfigToBytes()
        {
            var writer = new BGBinaryWriter(20);
            //version
            writer.AddInt(1);
            //view Id
            writer.AddId(viewId);

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
                    view = null;
                    viewId = reader.ReadId();
                    break;
                }
                default:
                    throw new BGException("Unknown version: $", version);
            }
        }
    }
}