/*
<copyright file="BGFieldRelationMultiple.cs" company="BansheeGz">
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
    /// multiple relation Field 
    /// </summary>
    [FieldDescriptor(Name = "relationMultiple", Folder = "Relation", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerRelationMultiple")]
    public partial class BGFieldRelationMultiple : BGFieldRelationSA<List<BGEntity>, List<BGId>>, BGBinaryBulkLoaderClass, BGFieldRelationMultipleI
    {
        public const ushort CodeType = 45;

        /// <inheritdoc/>
        public override ushort TypeCode => CodeType;

        //reusable list to get rid of GC
        //safe-to-use in multi-threaded environment
        //these lists are used during READ phase of FromString (which can not be multi-threaded) serialization only!
        private static readonly List<BGId> TempList = new List<BGId>();


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
        public BGFieldRelationMultiple(BGMetaEntity meta, string name, BGMetaEntity to) : base(meta, name, to)
        {
        }

        //for existing
        internal BGFieldRelationMultiple(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
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

        private List<BGId> EnsureList(int entityIndex)
        {
            var result = StoreGet(entityIndex);
            if (result != null) return result;

            result = new List<BGId>();
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
                        ReverseCache.RemoveRelated(entity, related[i]);
            }

            base.OnEntityDelete(entity);
        }

        //================================================================================================
        //                                              Configuration
        //================================================================================================
        /// <inheritdoc />
        public override string ConfigToString()
        {
            return JsonUtility.ToJson(new JsonConfig { ToId = toId.ToString(), AllowDuplicates = allowDuplicates });
        }

        /// <inheritdoc />
        public override void ConfigFromString(string config)
        {
            var jsonConfig = JsonUtility.FromJson<JsonConfig>(config);
            toId = new BGId(jsonConfig.ToId);
            allowDuplicates = jsonConfig.AllowDuplicates;
            ReverseCache.AllowDuplicates = allowDuplicates;
        }

        [Serializable]
        private struct JsonConfig
        {
            public string ToId;
            public bool AllowDuplicates;
        }

        /// <inheritdoc />
        public override byte[] ConfigToBytes()
        {
            var writer = new BGBinaryWriter(20);
            //version
            writer.AddInt(2);
            //toId
            writer.AddId(toId);

            //version 2
            writer.AddBool(allowDuplicates);

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
                case 2:
                {
                    toId = reader.ReadId();
                    allowDuplicates = reader.ReadBool();
                    ReverseCache.AllowDuplicates = allowDuplicates;
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

            var otherField = (BGFieldRelationMultiple)fromField;
            var value = otherField.StoreGet(fromEntityIndex);
            ReverseSetRelated(index, value);
            if (value != null && value.Count > 0)
            {
                var myValue = StoreGet(index);
                if (myValue == null) StoreSet(index, new List<BGId>(value));
                else
                {
                    myValue.Clear();
                    myValue.AddRange(value);
                }
            }
            else ClearValueNoEvent(index);
        }

        /// <inheritdoc/>
        protected override bool AreStoredValuesEqual(List<BGId> myValue, List<BGId> otherValue)
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
                if (myId != otherId) return false;
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
                    var entityId = entityIdList[i];
                    var entity = To[entityId];
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
                    if (entity.Meta.Id != RelatedMeta.Id)
                        throw new BGException("Can not assign related entities: one of the entities has wrong meta! Expected $, actual $", RelatedMeta.Name, entity.Meta.Name);
                }

                var thisEntity = Meta[entityIndex];
                var result = StoreGet(entityIndex);
                if (result == null)
                {
                    result = new List<BGId>(value.Count);
                    StoreSet(entityIndex, result);
                }
                else
                {
                    if (ReverseCache.Enabled)
                        foreach (var relatedId in result)
                            ReverseCache.RemoveRelated(thisEntity, relatedId);

                    result.Clear();
                }

                var distinctValue = allowDuplicates ? value : value.Distinct();
                foreach (var entity in distinctValue)
                {
                    if (entity == null) continue;

                    ReverseCache.AddRelated(thisEntity, entity.Id);
                    result.Add(entity.Id);
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
                if (relatedId.IsEmpty) continue;
                ReverseCache.MarkDirty(relatedId);
            }
        }


        /// <inheritdoc/>
        public override void SetStoredValue(int entityIndex, List<BGId> value)
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
            for (var i = 0; i < related.Count; i++) ReverseCache.RemoveRelated(entity, related[i]);
        }

        private void ReverseSetRelated(int entityIndex, List<BGId> value)
        {
            if (!ReverseCache.Enabled) return;

            ReverseRemoveRelated(entityIndex);

            if (value == null || value.Count == 0) return;
            var entity = Meta.GetEntity(entityIndex);
            foreach (var id in value) ReverseCache.AddRelated(entity, id);
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
                    if (val.IsEmpty) continue;
                    var value = ReverseCache.Ensure(val);
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
                    if (value == null || value.RemoveAll(id1 => id1 == entityId) == 0) return;
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

                    if (value.RemoveAll(entityIds.Contains) == 0) return;

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
                    if (value == null || value.RemoveAll(id1 => id1 == entityId) == 0) continue;
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
                    if (value == null || !value.Contains(entityId)) return;

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
                        var eId = value[i];
                        if (entityIds.Contains(eId))
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
            var result = StoreItems[entityIndex];
            if (result == null || result.Count == 0) return null;

            var list = new List<byte>();
            for (var i = 0; i < result.Count; i++)
            {
                var id = result[i];
                list.AddRange(id.ToByteArray());
            }

            return list.ToArray();
        }

        /// <inheritdoc />
        public override void FromBytes(int entityIndex, ArraySegment<byte> segment)
        {
            if (segment.Count == 0) ClearValueNoEventForRelation(entityIndex);
            else
            {
                var result = StoreItems[entityIndex] ?? new List<BGId>();
                for (var i = 0; i < segment.Count; i = i + 16) result.Add(new BGId(segment.Array, segment.Offset + i));

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
                    const int valueSize = BGFieldId.Size;
                    //remove? too expensive
                    if (cellRequest.Count % valueSize != 0)
                        throw new BGException("Can not convert byte array to value. Wrong byte array size $. Should be dividable by $", cellRequest.Count, valueSize);
                    var count = cellRequest.Count / valueSize;
                    if (count == 0) ClearValueNoEventForRelation(entityIndex);
                    else
                    {
                        var list = StoreItems[entityIndex];
                        if (list == null) list = new List<BGId>(count);
                        else
                        {
                            list.Clear();
                            if (list.Capacity < count) list.Capacity = count;
                        }

                        StoreItems[entityIndex] = list;

                        var upperLimit = offset + valueSize * count;
                        for (var cursor = offset; cursor < upperLimit; cursor += valueSize) list.Add(new BGId(array, cursor));

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

            var meta = To;
            var builder = new StringBuilder();
            foreach (var id in ids)
            {
                if (builder.Length != 0) builder.Append(A);
                builder.Append(IdToString(id, meta?[id]));
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
                var ids = value.Split(AA, StringSplitOptions.RemoveEmptyEntries);
                TempList.Clear();
                foreach (var id in ids) TempList.Add(IdFromString(id));

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

        public static string IdToString(BGId entityId, BGEntity entity)
        {
            var toEntityIdString = entityId.ToString();
            var entityName = GetEntityName(entity);
            if (string.IsNullOrEmpty(entityName)) return toEntityIdString;
            return entityName + ValueIdSeparator + toEntityIdString;
        }

        public static string GetEntityName(BGEntity entity)
        {
            var entityName = entity?.Name;
            if (entityName == null) return null;
            entityName = entityName.Trim();
            var separatorIndex = entityName.IndexOf(A);
            if (separatorIndex >= 0) entityName = entityName.Replace("" + A, "");
            if (string.IsNullOrEmpty(entityName)) return null;
            return entityName;
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldRelationMultiple(meta, id, name);
    }
}