/*
<copyright file="BGFieldRelationMA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// relation field, referencing multiple tables 
    /// </summary>
    public abstract class BGFieldRelationMA<T, TStoreType> : BGFieldRelationA<T, TStoreType>, BGManyTablesRelationI
    {
        private readonly List<BGId> toIds = new List<BGId>();

        /*
        public BGMetaEntity To
        {
            get { return Meta.Repo.GetMeta(toId); }
        }
        */

        public virtual List<BGId> ToIds => toIds;

        protected BGFieldRelationMA(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        protected BGFieldRelationMA(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        protected BGFieldRelationMA(BGMetaEntity meta, string name, List<BGMetaEntity> to) : base(meta, name)
        {
            if (to == null || to.Count == 0)
            {
                Meta.Unregister(this);
                throw new BGException("'To' can not be null or empty");
            }

            foreach (var metaEntity in to) toIds.Add(metaEntity.Id);
        }

        //================================================================================================
        //                                              Relation
        //================================================================================================
        /// <inheritdoc/>
        public virtual List<BGMetaEntity> RelatedMetas
        {
            get
            {
                var result = new List<BGMetaEntity>();
                foreach (var toId in toIds)
                {
                    var toMeta = Meta.Repo.GetMeta(toId);
                    if (toMeta == null) continue;
                    result.Add(toMeta);
                }

                return result;
            }
        }

        /// <inheritdoc/>
        public virtual void RemoveRelatedMeta(BGMetaEntity metaEntity)
        {
            // if (toIds.Count == 1 && toIds[0] == metaEntity.Id) throw new Exception("Can not remove the last related meta!");
            if (toIds.RemoveAll(id => id == metaEntity.Id) == 0) return;
            OnRemoveRelatedMeta(metaEntity);
            events.MetaWasChanged(Meta);
        }

        /// <inheritdoc/>
        public virtual void AddRelatedMeta(BGMetaEntity metaEntity)
        {
            if (toIds.Contains(metaEntity.Id)) return;
            toIds.Add(metaEntity.Id);
            events.MetaWasChanged(Meta);
        }

        /// <summary>
        /// called when related table is removed 
        /// </summary>
        protected abstract void OnRemoveRelatedMeta(BGMetaEntity metaEntity);

        protected virtual void CheckMetaId(BGEntity entity)
        {
            var metaId = entity.MetaId;
            var found = false;
            for (var i = 0; i < toIds.Count; i++)
            {
                if (toIds[i] != metaId) continue;
                found = true;
                break;
            }

            if (!found) throw new BGException("Can not assign related entity: meta [$] is not included in field's settings!", entity.MetaName);
        }

        //================================================================================================
        //                                              Configuration
        //================================================================================================
        /// <inheritdoc />
        public override string ConfigToString()
        {
            var metas = RelatedMetas;
            var jsonConfig = new JsonConfig { ToIds = new List<string>(metas.Count) };
            foreach (var meta in metas) jsonConfig.ToIds.Add(meta.Id.ToString());
            return JsonUtility.ToJson(jsonConfig);
        }

        /// <inheritdoc />
        public override void ConfigFromString(string config)
        {
            toIds.Clear();
            if (string.IsNullOrEmpty(config)) return;
            var fromJson = JsonUtility.FromJson<JsonConfig>(config);
            if (fromJson.ToIds != null)
                foreach (var jsonToId in fromJson.ToIds)
                {
                    if (!BGId.TryParse(jsonToId, out var id)) continue;
                    toIds.Add(id);
                }
        }

        [Serializable]
        private struct JsonConfig
        {
            public List<string> ToIds;
        }

        /// <inheritdoc />
        public override byte[] ConfigToBytes()
        {
            var writer = new BGBinaryWriter(32);
            //version
            writer.AddInt(1);
            //toId
            var relatedMetas = RelatedMetas;
            writer.AddArray(() =>
            {
                foreach (var relatedMeta in relatedMetas) writer.AddId(relatedMeta.Id);
            }, relatedMetas.Count);

            return writer.ToArray();
        }

        /// <inheritdoc />
        public override void ConfigFromBytes(ArraySegment<byte> config)
        {
            toIds.Clear();
            var reader = new BGBinaryReader(config);
            var version = reader.ReadInt();
            switch (version)
            {
                case 1:
                {
                    reader.ReadArray(() =>
                    {
                        toIds.Add(reader.ReadId());
                    });
                    break;
                }
                default:
                {
                    throw new BGException("Unknown version: $", version);
                }
            }
        }
        
        //================================================================================================
        //                                              Static
        //================================================================================================
        public static BGRowRef StringToRowRef(string value)
        {
            BGRowRef rowRef = null;
            var index = value.LastIndexOf(ValueIdSeparator);
            if (index > 0 && value.Length - index == 23 && BGId.TryParse(value.Substring(index + 1, 22), out var entityId))
            {
                var index2 = value.LastIndexOf(ValueIdSeparator, index - 1);
                if (index2 > 0)
                {
                    if(index - index2 == 23 && BGId.TryParse(value.Substring(index2 + 1, 22), out var metaId)) rowRef = new BGRowRef(metaId, entityId);
                }
                else if(index==22 && BGId.TryParse(value.Substring(0, 22), out var metaId))
                {
                    rowRef = new BGRowRef(metaId, entityId);
                }
            }

            return rowRef;
        }
        public static string RowRefToString(BGRowRef rowRef, BGRepo repo)
        {
            if (rowRef == null) return "";

            var fullId = RowRefToString(rowRef);

            var entity = rowRef.GetEntity(repo);
            if (entity == null || string.IsNullOrEmpty(entity.Name)) return fullId;

            var result = entity.MetaName + '.' + entity.Name.Trim() + ValueIdSeparator + fullId;
            return result;
        }

        public static string RowRefToString(BGRowRef rowRef)
        {
            var toMetaIdString = rowRef.MetaId.ToString();
            var toEntityIdString = rowRef.EntityId.ToString();
            return toMetaIdString + ValueIdSeparator + toEntityIdString;
        }
    }
}