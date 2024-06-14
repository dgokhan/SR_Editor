/*
<copyright file="BGFieldManyRelationsMultiple.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// many tables multiple relation Field 
    /// </summary>
    [FieldDescriptor(Name = "manyTablesRelationMultiple", Folder = "Relation", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerManyRelationsMultiple")]
    public class BGFieldManyRelationsMultiple : BGFieldRelationMA<List<BGEntity>, List<BGRowRef>>, BGBinaryBulkLoaderClass, BGFieldRelationMultipleI
    {
        public const ushort CodeType = 42;

        /// <inheritdoc/>
        public override ushort TypeCode => CodeType;

        //reusable list to get rid of GC
        //safe-to-use in multi-threaded environment
        //these lists are used during READ phase of FromString (which can not be multi-threaded) serialization only!
        private static readonly List<BGRowRef> TempList = new List<BGRowRef>();


        //reusable lists to get rid of GC
//        private readonly BGIdDictionary<List<BGEntity>> from2ToList = new BGIdDictionary<List<BGEntity>>();

        private bool allowDuplicates;

        /// <summary>
        /// Whether this relation field allows duplicate entities in the value
        /// </summary>
        public bool AllowDuplicates
        {
            get => allowDuplicates;
            set
            {
                if (allowDuplicates == value) return;
                if (!value)
                    //no duplicates are allowed anymore
                    Meta.Repo.Events.WithEventsDisabled(() =>
                    {
                        Meta.ForEachEntity(entity =>
                        {
                            var val = this[entity.Index];
                            if (val == null || val.Count < 2) return;
                            var distinctValues = val.Distinct().ToList();
                            if (val.Count == distinctValues.Count) return;
                            this[entity.Index] = distinctValues;
                        });
                    });

                allowDuplicates = value;
                ReverseCache.AllowDuplicates = value;
                events.MetaWasChanged(Meta);
            }
        }

        //for new
        public BGFieldManyRelationsMultiple(BGMetaEntity meta, string name, List<BGMetaEntity> to) : base(meta, name, to)
        {
        }

        //for existing
        internal BGFieldManyRelationsMultiple(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Callbacks
        //================================================================================================
        /*
        /// <summary>
        /// Add toId entity to the list, referenced by entityId  
        /// </summary>
        public void AddRelated(int entityIndex, BGId toId)
        {
            var toList = EnsureList(entityIndex);
            if (!allowDuplicates && toList.Contains(toId)) return;
            ReverseCache.AddRelated(entityIndex, toId);
            toList.Add(toId);
            Meta.Repo.Events.FieldWasChanged(MetaId, Id, Meta[entityIndex].Id);
        }

        /// <summary>
        /// remove toId entity from the list, referenced by entityId  
        /// </summary>
        public void RemoveRelated(int entityIndex, BGId toId)
        {
            var list = Store[entityIndex];
            if (list == null) return;
            if (!list.Remove(toId)) return;
            var entity = Meta[entityIndex];
            ReverseCache.RemoveRelated(entity, toId);
            Meta.Repo.Events.FieldWasChanged(MetaId, Id, entity.Id);
        }
        */

        private List<BGRowRef> EnsureList(int entityIndex)
        {
            var result = StoreGet(entityIndex);
            if (result != null) return result;

            result = new List<BGRowRef>();
            StoreSet(entityIndex, result);
            return result;
        }

        /// <inheritdoc/>
        public override void OnEntityDelete(BGEntity entity)
        {
            if (ReverseCache.Enabled)
            {
                var related = StoreGet(entity.Index);
                if (related != null && related.Count > 0)
                    //entity Index is not valid at this point- so we need to pass entity.Id
                    for (var i = 0; i < related.Count; i++)
                        ReverseCache.RemoveRelated(entity, related[i].EntityId);
            }

            base.OnEntityDelete(entity);
        }

        /// <inheritdoc/>
        protected override void OnRemoveRelatedMeta(BGMetaEntity metaEntity)
        {
            Meta.ForEachEntity(entity =>
            {
                var related = this[entity.Index];
                if (related == null || related.Count == 0) return;
                for (var i = related.Count - 1; i >= 0; i--)
                {
                    var relatedEntity = related[i];
                    if (relatedEntity.MetaId == metaEntity.Id) related.RemoveAt(i);
                }

                this[entity.Index] = related;
            });
        }

        //================================================================================================
        //                                              Configuration
        //================================================================================================
        /// <inheritdoc />
        public override string ConfigToString()
        {
            var metas = RelatedMetas;
            var jsonConfig = new JsonConfig { ToIds = new List<string>(metas.Count), AllowDuplicates = allowDuplicates };
            foreach (var meta in metas) jsonConfig.ToIds.Add(meta.Id.ToString());
            return JsonUtility.ToJson(jsonConfig);
        }

        /// <inheritdoc />
        public override void ConfigFromString(string config)
        {
            ToIds.Clear();
            if (string.IsNullOrEmpty(config)) return;
            var fromJson = JsonUtility.FromJson<JsonConfig>(config);
            allowDuplicates = fromJson.AllowDuplicates;
            if (fromJson.ToIds != null)
                foreach (var jsonToId in fromJson.ToIds)
                {
                    if (!BGId.TryParse(jsonToId, out var id)) continue;
                    ToIds.Add(id);
                }
        }

        [Serializable]
        private struct JsonConfig
        {
            public List<string> ToIds;
            public bool AllowDuplicates;
        }

        /// <inheritdoc />
        public override byte[] ConfigToBytes()
        {
            var writer = new BGBinaryWriter(32);
            //version
            writer.AddInt(1);

            //duplicates
            writer.AddBool(allowDuplicates);

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
            ToIds.Clear();
            var reader = new BGBinaryReader(config);
            var version = reader.ReadInt();
            switch (version)
            {
                case 1:
                {
                    allowDuplicates = reader.ReadBool();
                    reader.ReadArray(() =>
                    {
                        ToIds.Add(reader.ReadId());
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
        //                                              Value
        //================================================================================================
        /// <inheritdoc />
        public override void CopyValue(BGField fromField, BGId fromEntityId, int fromEntityIndex, BGId toEntityId)
        {
            if (fromEntityIndex == -1 || fromField.IsDeleted) return;
            var index = Meta.FindEntityIndex(toEntityId);
            if (index == -1) return;

//            Store[index] = ((BGFieldRelationMultiple) fieldFrom).Store[entityIndex];

            var otherField = (BGFieldManyRelationsMultiple)fromField;
            var value = otherField.StoreGet(fromEntityIndex);
            ReverseSetRelated(index, value);
            if (value != null && value.Count > 0)
            {
                var myValue = StoreGet(index);
                if (myValue == null) StoreSet(index, new List<BGRowRef>(value));
                else
                {
                    myValue.Clear();
                    myValue.AddRange(value);
                }
            }
            else ClearValueNoEvent(index);
        }

        /// <inheritdoc/>
        protected override bool AreStoredValuesEqual(List<BGRowRef> myValue, List<BGRowRef> otherValue)
        {
            var myEmpty = myValue == null || myValue.Count == 0;
            var otherEmpty = otherValue == null || otherValue.Count == 0;
            if (myEmpty && otherEmpty) return true;
            if (myEmpty || otherEmpty) return false;
            if (myValue.Count != otherValue.Count) return false;
            for (var i = 0; i < myValue.Count; i++)
            {
                var myId = myValue[i];
                var otherId = otherValue[i];
                if (!Equals(myId, otherId)) return false;
            }

            return true;
        }

/*
        /// <inheritdoc />
        public override bool ClearValue(BGId entityId)
        {
//            from2ToList.Remove(entityId);
            return base.ClearValue(entityId);
        }
*/

        /// <inheritdoc />
        public override List<BGEntity> this[int entityIndex]
        {
            get
            {
                //micro-optimization code copied from base class
                if (entityIndex >= StoreCount) ThrowIndexOutOfBoundOnRead(entityIndex);
                var entityIdList = StoreItems[entityIndex];
                if (entityIdList == null || entityIdList.Count == 0) return null;

                var result = new List<BGEntity>();

                //do not change to foreach!
                for (var i = 0; i < entityIdList.Count; i++)
                {
                    var rowRef = entityIdList[i];
                    var entity = rowRef.GetEntity(Repo);
                    if (entity != null) result.Add(entity);
                }

                return result;
            }
            set
            {
                if (events.On)
                {
                    var oldValue = this[entityIndex];
                    if (BGUtil.ListsValuesEqual(value, oldValue)) return;
                    var entity = Meta[entityIndex];
                    FireBeforeValueChanged(entity, oldValue, value);
                    SetEntityList(entityIndex, value);
                    FireValueChanged(entity, oldValue, value);
                }
                else SetEntityList(entityIndex, value);
            }
        }

        /// <inheritdoc />
        public List<BGEntity> GetRelatedEntity(int entityIndex) => this[entityIndex];

        /// <inheritdoc />
        public void SetRelatedEntity(int entityIndex, List<BGEntity> entityList) => this[entityIndex] = entityList; 

        private void SetEntityList(int entityIndex, List<BGEntity> value)
        {
            if (value == null || value.Count == 0) ClearValue(entityIndex);
            else
            {
                //check entities metas
                for (var i = 0; i < value.Count; i++)
                {
                    var entity = value[i];
                    if (entity == null) continue;
                    CheckMetaId(entity);
                }

                var thisEntity = Meta[entityIndex];
                var result = StoreGet(entityIndex);
                if (result == null)
                {
                    result = new List<BGRowRef>(value.Count);
                    StoreSet(entityIndex, result);
                }
                else
                {
                    if (ReverseCache.Enabled)
                        foreach (var relatedId in result)
                            ReverseCache.RemoveRelated(thisEntity, relatedId.EntityId);

                    result.Clear();
                }

                var distinctValue = allowDuplicates ? value : value.Distinct();
                foreach (var entity in distinctValue)
                {
                    if (entity == null) continue;

                    ReverseCache.AddRelated(thisEntity, entity.Id);
                    result.Add(new BGRowRef(entity));
                }

                /*
                //do not change to foreach!
                for (var i = 0; i < distinctValue.Length; i++)
                {
                    var entity = distinctValue[i];
                    if (entity != null) result.Add(entity.Id);
                }
            */
            }
        }

        /// <inheritdoc/>
        public override void ClearValue(int entityIndex)
        {
            ReverseRemoveRelated(entityIndex);
            base.ClearValue(entityIndex);
        }

        private void ClearValueNoEventForRelation(int index)
        {
            ReverseRemoveRelated(index);
            ClearValueNoEvent(index);
        }

        /// <inheritdoc/>
        public override void Swap(int entityIndex1, int entityIndex2)
        {
            //why we need to do it? it looks pointless
            if (ReverseCache.Enabled)
            {
                MarkReverseDirty(entityIndex1);
                MarkReverseDirty(entityIndex2);
            }

            base.Swap(entityIndex1, entityIndex2);
        }

        private void MarkReverseDirty(int index)
        {
            var related = StoreGet(index);
            if (related == null || related.Count == 0) return;
            foreach (var relatedId in related)
            {
                if (relatedId == null) continue;
                ReverseCache.MarkDirty(relatedId.EntityId);
            }
        }


        /// <inheritdoc/>
        public override void SetStoredValue(int entityIndex, List<BGRowRef> value)
        {
            ReverseSetRelated(entityIndex, value);
            base.SetStoredValue(entityIndex, value);
        }

        //================================================================================================
        //                                              Reverse Relation cache
        //================================================================================================
        private void ReverseRemoveRelated(int entityIndex)
        {
            if (!ReverseCache.Enabled) return;

            var related = StoreGet(entityIndex);
            if (related == null || related.Count == 0) return;
            var entity = Meta[entityIndex];
            for (var i = 0; i < related.Count; i++) ReverseCache.RemoveRelated(entity, related[i].EntityId);
        }

        private void ReverseSetRelated(int entityIndex, List<BGRowRef> value)
        {
            if (!ReverseCache.Enabled) return;

            ReverseRemoveRelated(entityIndex);

            if (value == null || value.Count == 0) return;
            var entity = Meta.GetEntity(entityIndex);
            foreach (var tuple in value) ReverseCache.AddRelated(entity, tuple.EntityId);
        }

        /// <inheritdoc/>
        protected override void BuildReverseCache()
        {
            StoreForEachKeyValue((index, valList) =>
            {
                if (valList == null || valList.Count == 0) return;
                var entity = Meta[index];
                foreach (var val in valList)
                {
                    if (val == null) continue;
                    var value = ReverseCache.Ensure(val.EntityId);
                    value.Add(entity);
                }
            });
        }

        //================================================================================================
        //                                              Relation
        //================================================================================================
        /// <inheritdoc />
        public override void ClearToValue(BGId entityId)
        {
            Exception exception = null;
            if (ReverseCache.Enabled) ClearToValue(entityId, ref exception);
            else
                StoreForEachKeyValue((index, value) =>
                {
                    if (value == null || value.RemoveAll(rowRef => rowRef.EntityId == entityId) == 0) return;
                    try
                    {
                        // value value is NOT an error- we can not create new lists just for the event's sake   
                        FireStoredValueChanged(Meta[index], value, value);
                    }
                    catch (Exception e)
                    {
                        if (exception == null) exception = e;
                    }
                });

            if (exception != null) throw exception;
        }

        /// <inheritdoc/>
        public override void ClearToValue(HashSet<BGId> entityIds)
        {
            Exception exception = null;
            if (ReverseCache.Enabled)
                foreach (var entityId in entityIds)
                    ClearToValue(entityId, ref exception);
            else
                StoreForEachKeyValue((index, value) =>
                {
                    if (value == null) return;

                    if (value.RemoveAll(tuple => entityIds.Contains(tuple.EntityId)) == 0) return;

                    try
                    {
                        // value value is NOT an error- we can not create new lists just for the event's sake   
                        FireStoredValueChanged(Meta[index], value, value);
                    }
                    catch (Exception e)
                    {
                        if (exception != null) exception = e;
                    }
                });

            if (exception != null) throw exception;
        }

        private void ClearToValue(BGId entityId, ref Exception exception)
        {
            var list = ReverseCache.Get(entityId);
            if (list != null)
            {
                foreach (var e in list)
                {
                    var value = StoreGet(e.Index);
                    if (value == null || value.RemoveAll(tuple => tuple.EntityId == entityId) == 0) continue;
                    try
                    {
                        // value value is NOT an error- we can not create new lists just for the event's sake   
                        FireStoredValueChanged(e, value, value);
                    }
                    catch (Exception ex)
                    {
                        if (exception == null) exception = ex;
                    }
                }

                ReverseCache.Remove(entityId);
            }
        }

        /// <inheritdoc />
        public override List<BGEntity> GetRelatedIn(BGId entityId, List<BGEntity> result = null)
        {
            ReverseCache.Enable(true);
            if (ReverseCache.Enabled)
            {
                var list = ReverseCache.Get(entityId);
                if (result != null)
                {
                    result.Clear();
                    if (list != null) result.AddRange(list);
                }
                else result = list == null ? new List<BGEntity>() : new List<BGEntity>(list);
            }
            else
            {
                result = result ?? new List<BGEntity>();
                result.Clear();

                StoreForEachKeyValue((index, value) =>
                {
                    if (value?.Find(tuple => tuple.EntityId == entityId) == null) return;

                    var entity = Meta[index];
                    if (entity != null) result.Add(entity);
                });
            }

            return result;
        }

        /// <inheritdoc/>
        public override List<BGEntity> GetRelatedIn(HashSet<BGId> entityIds, List<BGEntity> result = null)
        {
            if (entityIds == null || entityIds.Count == 0)
            {
                result?.Clear();
                return result;
            }

            ReverseCache.Enable(true);
            if (ReverseCache.Enabled)
            {
                var set = new HashSet<BGEntity>();
                foreach (var entityId in entityIds)
                {
                    var list = ReverseCache.Get(entityId);
                    if (list == null) continue;

                    foreach (var entity in list) set.Add(entity);
                }

                if (result == null) result = new List<BGEntity>(set);
                else result.AddRange(set);
            }
            else
            {
                result = result ?? new List<BGEntity>();
                result.Clear();

                StoreForEachKeyValue((index, value) =>
                {
                    if (value == null) return;

                    var found = false;
                    for (var i = 0; i < value.Count; i++)
                    {
                        var tuple = value[i];
                        if (entityIds.Contains(tuple.EntityId))
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found) return;

                    var entity = Meta[index];
                    if (entity != null) result.Add(entity);
                });
            }

            return result;
        }

        //================================================================================================
        //                                              Serialization
        //================================================================================================
        /// <inheritdoc />
        public override byte[] ToBytes(int entityIndex)
        {
            var list = StoreItems[entityIndex];
            if (list == null || list.Count == 0) return null;

            var result = new byte[list.Count * 32];
            for (var i = 0; i < list.Count; i++)
            {
                var tuple = list[i];
                var start = i * 32;
                tuple.MetaId.ToByteArray(result, start);
                tuple.EntityId.ToByteArray(result, start + 16);
            }

            return result;
        }

        /// <inheritdoc />
        public override void FromBytes(int entityIndex, ArraySegment<byte> segment)
        {
            if (segment.Count == 0) ClearValueNoEventForRelation(entityIndex);
            else
            {
                var result = StoreItems[entityIndex] ?? new List<BGRowRef>();
                for (var i = 0; i < segment.Count; i += 32)
                {
                    var startIndex = segment.Offset + i;
                    var metaId = new BGId(segment.Array, startIndex);
                    var entityId = new BGId(segment.Array, startIndex + 16);
                    result.Add(new BGRowRef(metaId, entityId));
                }

                if (ReverseCache.Enabled) ReverseSetRelated(entityIndex, result);
                StoreItems[entityIndex] = result;
            }
        }

        /// <inheritdoc />
        public void FromBytes(BGBinaryBulkRequestClass request)
        {
            var array = request.Array;
            var requests = request.CellRequests;
            var length = requests.Length;
            for (var i = 0; i < length; i++)
            {
                var cellRequest = requests[i];
                var entityIndex = cellRequest.EntityIndex;
                var offset = cellRequest.Offset;
                try
                {
                    const int valueSize = BGFieldId.Size * 2;
                    //remove? too expensive
                    if (cellRequest.Count % valueSize != 0)
                        throw new BGException("Can not convert byte array to value. Wrong byte array size $. Should be dividable by $", cellRequest.Count, valueSize);
                    var count = cellRequest.Count / valueSize;
                    if (count == 0) StoreItems[entityIndex] = default;
                    else
                    {
                        var list = StoreItems[entityIndex];
                        if (list == null) list = new List<BGRowRef>(count);
                        else
                        {
                            list.Clear();
                            if (list.Capacity < count) list.Capacity = count;
                        }

                        StoreItems[entityIndex] = list;

                        var upperLimit = offset + valueSize * count;
                        for (var cursor = offset; cursor < upperLimit; cursor += valueSize)
                        {
                            var metaId = new BGId(array, cursor);
                            var entityId = new BGId(array, cursor + BGFieldId.Size);
                            list.Add(new BGRowRef(metaId, entityId));
                        }
                        
                        if (ReverseCache.Enabled) ReverseSetRelated(entityIndex, list);
                    }
                }
                catch (Exception e)
                {
                    request.OnError?.Invoke(e);
                }
            }
        }

        /// <inheritdoc />
        public override string ToString(int entityIndex)
        {
            var ids = StoreItems[entityIndex];
            if (ids == null || ids.Count == 0) return "";

            var builder = new StringBuilder();
            foreach (var rowRef in ids)
            {
                if (rowRef == null) continue;
                if (builder.Length != 0) builder.Append(A);
                builder.Append(RowRefToString(rowRef, Repo).Replace("" + A, ""));
            }

            var result = builder.ToString();
            builder.Length = 0;
            return result;
        }

        /// <inheritdoc />
        public override void FromString(int entityIndex, string value)
        {
            if (string.IsNullOrEmpty(value)) ClearValueNoEventForRelation(entityIndex);
            else
            {
                var allRelated = value.Split(AA, StringSplitOptions.RemoveEmptyEntries);
                TempList.Clear();
                foreach (var related in allRelated)
                {
                    var rowRef = StringToRowRef(related);
                    if (rowRef != null) TempList.Add(rowRef);
                }

                if (TempList.Count == 0) ClearValueNoEventForRelation(entityIndex);
                else
                {
                    if (ReverseCache.Enabled) ReverseSetRelated(entityIndex, TempList);
                    var targetList = EnsureList(entityIndex);
                    targetList.Clear();
                    targetList.AddRange(TempList);
                }

                TempList.Clear();
            }
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc/>
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldManyRelationsMultiple(meta, id, name);
    }
}