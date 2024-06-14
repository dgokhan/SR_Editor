/*
<copyright file="BGFieldLocalizedA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// This class uses value from related entity (which has BGFieldLocale with delegate)
    /// This field->BGFieldLocale->delegate
    /// </summary>
    public abstract partial class BGFieldLocalizedA<T> : BGFieldCachedA<T, BGId>, BGFieldLocalizedI, BGCustomSearchValueProviderI
    {
        private BGId toId;

        public BGId ToId => toId;

        public BGMetaLocalization To => Meta.Repo.GetMeta<BGMetaLocalization>(toId);

        //================================================================================================
        //                                              Constructors
        //================================================================================================

        //for new
        public BGFieldLocalizedA(BGMetaEntity meta, string name, BGMetaLocalization to) : base(meta, name)
        {
            if (to == null)
            {
                Unregister();
                throw new BGException("'To' can not be null");
            }

            Type targetType;
            try
            {
                targetType = BGLocalizedFieldAttribute.Get(GetType()).TargetFieldType;
            }
            catch
            {
                Unregister();
                throw;
            }

            if (to.FieldType != targetType)
            {
                Unregister();
                throw new BGException("'To' meta has incompatible target field type. expected $, found $", targetType.FullName, to.FieldType.FullName);
            }

            toId = to.Id;
//            meta.ForEachEntity(OnEntityCreate);
        }

        //for existing
        protected BGFieldLocalizedA(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Relation
        //================================================================================================
        public BGMetaEntity RelatedMeta => To;

        private List<BGEntity> result;

        public List<BGEntity> GetRelatedIn(BGId entityId, List<BGEntity> result = null)
        {
            //is this method useless?
            result = result ?? new List<BGEntity>();
            result.Clear();

            StoreForEachKeyValue((index, toEntityId) =>
            {
                if (toEntityId != entityId) return;

                var entity = Meta[index];
                if (entity != null) result.Add(entity);
            });
            return result;
        }

        public List<BGEntity> GetRelatedIn(HashSet<BGId> entityIds, List<BGEntity> result = null)
        {
            if (entityIds == null || entityIds.Count == 0) return result;
            result = result ?? new List<BGEntity>();
            result.Clear();

            StoreForEachKeyValue((index, id) =>
            {
                if (id.IsEmpty || !entityIds.Contains(id)) return;
                var entity = Meta[index];
                if (entity != null) result.Add(entity);
            });
            return result;
        }

        public void ClearToValue(BGId entityId)
        {
            //is this method useless?
            var index = -1;
#if !BG_SA
            try
            {
                StoreForEachKeyValue((i, to) =>
                {
                    if (to != entityId) return;

                    index = i;
                    throw new ExitGUIException();
                });
            }
            catch (ExitGUIException)
            {
                StoreItems[index] = BGId.Empty;
            }
#endif
        }

        public void ClearToValue(HashSet<BGId> entityIds)
        {
            //probably no need for this??
        }

        public BGEntity GetTo(int entityIndex)
        {
            var metaTo = To;
            if (metaTo == null) return null;

            var toEntityId = StoreGet(entityIndex);
            if (toEntityId.IsEmpty)
            {
                //if no related id is present- we'll create new one
                var entity = Meta[entityIndex];
                if (entity == null) return null; //is this possible?
                OnEntityCreate(entity);
                toEntityId = StoreGet(entityIndex);
            }


            return metaTo[toEntityId] ?? metaTo.NewEntity(toEntityId);
        }

        public override void DuplicateValue(BGId fromEntityId, int fromEntityIndex, BGId toEntityId)
        {
            var toEntity = Meta[toEntityId];
            if (toEntity == null) return;

            var relatedFromEntity = GetRelatedEntity(fromEntityIndex);
            if (relatedFromEntity == null) return;

            var relatedToEntity = GetTo(toEntity.Index);
            if (relatedToEntity == null) return;

            //name
            relatedToEntity.Name = Meta.Name + "." + toEntity.Name + "." + Name;

            //other fields
            relatedToEntity.Meta.ForEachField(field =>
            {
                if (field is BGFieldEntityName) return;
                field.CopyValue(field, relatedFromEntity.Id, relatedFromEntity.Index, relatedToEntity.Id);
            });
        }
        //================================================================================================
        //                                              Configuration
        //================================================================================================

        public override string ConfigToString()
        {
            return JsonUtility.ToJson(new JsonConfig { ToId = toId.ToString() });
        }

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
            //NestedMetaId
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
        //                                              Value
        //================================================================================================

        public override T this[int entityIndex]
        {
            get
            {
                var relatedEntity = GetTo(entityIndex);
                if (relatedEntity == null) return default;

                var localizationLocale = BGAddonLocalization.DefaultRepoCurrentLocale;
                if (string.IsNullOrEmpty(localizationLocale)) return default;

                var item = relatedEntity.Get<T>(localizationLocale);
                return item;
            }
            set
            {
                var relatedEntity = GetTo(entityIndex);
                if (relatedEntity == null) return;

                var localizationLocale = BGAddonLocalization.DefaultRepoCurrentLocale;
                if (string.IsNullOrEmpty(localizationLocale)) return;

                relatedEntity.Set(localizationLocale, value);
            }
        }

/*
        public override void ForEachValue(Action<int> action)
        {
            var toMeta = To;
            if (toMeta == null)
            {
                Store.ForEachKey(action);
            }
            else
            {
                Meta.ForEachEntity(entity =>
                {
                    var related = GetTo(entity.Id);
                    action(related.Index);
                });
            }
        }
*/
        public override void ClearValue(int entityIndex)
        {
            var relatedEntity = GetTo(entityIndex);
            if (relatedEntity == null) return;

            var localizationLocale = BGAddonLocalization.DefaultRepoCurrentLocale;
            if (string.IsNullOrEmpty(localizationLocale)) return;

            var relatedField = relatedEntity.Meta.GetField(localizationLocale, false);
            relatedField?.ClearValue(relatedEntity.Index);
        }

        //================================================================================================
        //                                              Serialization
        //================================================================================================

        public override byte[] ToBytes(int entityIndex)
        {
            return StoreItems[entityIndex].ToByteArray();
        }

        public override void FromBytes(int entityIndex, ArraySegment<byte> segment)
        {
            if (segment.Count != 16) StoreItems[entityIndex] = BGId.Empty;
            else StoreItems[entityIndex] = new BGId(segment.Array, segment.Offset);
        }

        public override string ToString(int entityIndex)
        {
            var toEntityId = StoreItems[entityIndex];
            if (toEntityId == BGId.Empty) return "";
            var meta = To;
            return BGFieldRelationSingle.IdToString(toEntityId, meta?[toEntityId]);

            // return Store[entityIndex].ToString();
        }

        public override void FromString(int entityIndex, string value)
        {
            if (string.IsNullOrEmpty(value)) StoreItems[entityIndex] = BGId.Empty;
            else
            {
                var id1 = BGFieldRelationSA<BGEntity, BGId>.IdFromString(value);
                StoreItems[entityIndex] = id1;
            }

            /*
            if (value == null || value.Length != 22) Store[entityIndex] = BGId.Empty;
            else Store[entityIndex] = new BGId(value);
        */
        }

        //================================================================================================
        //                                              Custom search value
        //================================================================================================
        public string GetSearchValue(int entityIndex)
        {
            var val = this[entityIndex];
            if (val == null) return null;
            return ValueToSearchString(val, entityIndex);
        }

        protected abstract string ValueToSearchString(T val, int entityIndex);

        //================================================================================================
        //                                              Callbacks
        //================================================================================================
        public override void OnEntityCreate(BGEntity entity)
        {
            base.OnEntityCreate(entity);
            var toLocale = To;
            if (toLocale == null) return;
            var toEntity = toLocale.NewEntity();
            toEntity.Name = entity.Meta.Name + "." + entity.Name + "." + Name;
            StoreSet(entity.Index, toEntity.Id);
        }

        public override void OnEntityDelete(BGEntity entity)
        {
            var relatedEntity = GetRelatedEntity(entity.Index);
            relatedEntity?.Delete();
            base.OnEntityDelete(entity);
        }

        public override void OnNameChange(int entityIndex)
        {
            base.OnNameChange(entityIndex);
            var relatedEntity = GetRelatedEntity(entityIndex);
            if (relatedEntity != null)
            {
                var entity = Meta[entityIndex];
                if (entity != null) relatedEntity.Name = Meta.Name + "." + entity.Name + "." + Name;
            }
        }

        public override void Delete()
        {
            var toLocale = To;
            if (toLocale != null)
            {
                var toRemove = new List<BGEntity>();
                StoreForEachKeyValue((from, to) =>
                {
                    var entity = toLocale[to];
                    if (entity == null) return;
                    toRemove.Add(entity);
                });

                foreach (var entity in toRemove) entity.Delete();
            }

            base.Delete();
        }

        public BGEntity GetRelatedEntity(int entityIndex)
        {
            var id = StoreGet(entityIndex);
            if (id == BGId.Empty) return null;
            var toMeta = To;
            return toMeta?[id];
        }

        public void SetRelatedEntity(int entityIndex, BGEntity entity) => throw new NotImplementedException();
    }
}