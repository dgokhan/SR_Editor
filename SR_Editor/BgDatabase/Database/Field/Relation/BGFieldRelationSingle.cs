/*
<copyright file="BGFieldRelationSingle.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// single relation Field 
    /// </summary>
    [FieldDescriptor(Name = "relationSingle", Folder = "Relation", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerRelationSingle")]
    public partial class BGFieldRelationSingle : BGFieldRelationSA<BGEntity, BGId>, BGBinaryBulkLoaderClass, BGFieldRelationSingleI
    {
        public const ushort CodeType = 46;
        public override ushort TypeCode => CodeType;

        public override bool CanBeUsedAsKey => true;


        //for new
        public BGFieldRelationSingle(BGMetaEntity meta, string name, BGMetaEntity to) : base(meta, name, to)
        {
        }

        //for existing
        internal BGFieldRelationSingle(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Callbacks
        //================================================================================================
        /// <inheritdoc/>
        public override void OnEntityDelete(BGEntity entity)
        {
            //do not use index as argument to ReverseCache, cause it's not valid at this point
            if (ReverseCache.Enabled) ReverseCache.RemoveRelated(entity, StoreGet(entity.Index));
            base.OnEntityDelete(entity);
        }

        //================================================================================================
        //                                              Value
        //================================================================================================
        /// <inheritdoc />
        public override BGEntity this[int entityIndex]
        {
            get
            {
                //micro-optimization code copied from base class
                if (entityIndex >= StoreCount) ThrowIndexOutOfBoundOnRead(entityIndex);
                var entityId = StoreItems[entityIndex];
                return entityId.IsEmpty ? null : To[entityId];
            }
            set
            {
                if (value != null && value.Meta.Id != RelatedMeta.Id) throw new BGException("Can not assign related entity: meta is wrong! Expected $, actual $", RelatedMeta.Name, value.Meta.Name);
                var valueId = value?.Id ?? BGId.Empty;

                ReverseSetRelated(entityIndex, valueId);
                if (events.On)
                {
                    var oldValue = this[entityIndex];
                    if (Equals(value, oldValue)) return;
                    var entity = Meta[entityIndex];
                    FireBeforeValueChanged(entity, oldValue, value);
                    StoreSet(entityIndex, valueId);
                    FireValueChanged(entity, oldValue, value);
                }
                else StoreSet(entityIndex, valueId);
            }
        }

        /// <inheritdoc />
        public BGEntity GetRelatedEntity(int entityIndex) => this[entityIndex];
        
        /// <inheritdoc />
        public void SetRelatedEntity(int entityIndex, BGEntity entity) => this[entityIndex] = entity;
        
        /*
        /// <inheritdoc />
        public override void CopyValue(BGField fieldFrom, BGId entityId, int entityIndex)
        {
            var cached = (BGFieldRelationSingle) fieldFrom;

            BGId relatedId;
            if (cached.Store.TryGetValue(entityId,out relatedId)) Store[entityId] = relatedId;
            else ClearValueNoEvent(entityId);
        }
*/

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
        public override void SetStoredValue(int entityIndex, BGId value)
        {
            ReverseSetRelated(entityIndex, value);
            base.SetStoredValue(entityIndex, value);
        }

        /// <inheritdoc/>
        public override void CopyValue(BGField fromField, BGId fromEntityId, int fromEntityIndex, BGId toEntityId)
        {
            if (fromEntityIndex == -1 || fromField.IsDeleted) return;
            var index = Meta.FindEntityIndex(toEntityId);
            if (index == -1) return;

            var fieldRelationSingle = (BGFieldRelationSingle)fromField;
            var value = fieldRelationSingle.StoreGet(fromEntityIndex);
            ReverseSetRelated(index, value);
            StoreSet(index, value);
        }

        /// <inheritdoc/>
        public override void Swap(int entityIndex1, int entityIndex2)
        {
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
            if (!related.IsEmpty) ReverseCache.MarkDirty(related);
        }

        //================================================================================================
        //                                              Reverse cache
        //================================================================================================
        /// <inheritdoc/>
        protected override void BuildReverseCache()
        {
            StoreForEachKeyValue((index, val) =>
            {
                if (val.IsEmpty) return;
                var value = ReverseCache.Ensure(val);
                value.Add(Meta[index]);
            });
        }

        private void ReverseSetRelated(int entityIndex, BGId value)
        {
            if (!ReverseCache.Enabled) return;
            var oldValue = GetStoredValue(entityIndex);
            if (value == oldValue) return;

            ReverseCache.RemoveRelated(entityIndex, oldValue);
            ReverseCache.AddRelated(entityIndex, value);
        }

        private void ReverseRemoveRelated(int index)
        {
            ReverseCache.RemoveRelated(index, StoreGet(index));
        }

        //================================================================================================
        //                                              Relation
        //================================================================================================
        /// <inheritdoc />
        public override void ClearToValue(BGId id)
        {
            if (ReverseCache.Enabled)
            {
                var list = ReverseCache.Get(id);
                if (list != null)
                    try
                    {
                        foreach (var e in list)
                        {
                            var oldValue = StoreGet(e.Index);
                            if (oldValue.IsEmpty) continue;
                            StoreSet(e.Index , BGId.Empty);
                            FireStoredValueChanged(e, oldValue, BGId.Empty);
                        }
                    }
                    finally
                    {
                        ReverseCache.Remove(id);
                    }
            }
            else
                StoreForEachKeyValue((index, value) =>
                {
                    if (value != id) return;
                    var oldValue = StoreGet(index);
                    if (oldValue.IsEmpty) return;
                    StoreSet(index, BGId.Empty);
                    FireStoredValueChanged(Meta[index], oldValue, BGId.Empty);
                });
        }

        /// <inheritdoc />
        public override void ClearToValue(HashSet<BGId> entityIds)
        {
            if (entityIds == null || entityIds.Count == 0) return;

            Exception exception = null;
            if (ReverseCache.Enabled)
                foreach (var id in entityIds)
                {
                    var list = ReverseCache.Get(id);
                    if (list != null)
                    {
                        foreach (var e in list)
                        {
                            var oldValue = StoreGet(e.Index);
                            if (oldValue.IsEmpty) continue;
                            StoreSet(e.Index, BGId.Empty);
                            try
                            {
                                FireStoredValueChanged(e, oldValue, BGId.Empty);
                            }
                            catch (Exception ex)
                            {
                                if (exception == null) exception = ex;
                            }
                        }

                        ReverseCache.Remove(id);
                    }
                }
            else
                StoreForEachKeyValue((index, value) =>
                {
                    if (!entityIds.Contains(value)) return;
                    var oldValue = StoreGet(index);
                    if (oldValue.IsEmpty) return;
                    StoreSet(index, BGId.Empty);
                    try
                    {
                        FireStoredValueChanged(Meta[index], oldValue, BGId.Empty);
                    }
                    catch (Exception e)
                    {
                        if (exception == null) exception = e;
                    }
                });

            if (exception != null) throw exception;
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
                    if (value != entityId) return;

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
                else
                {
                    result.Clear();
                    result.AddRange(set);
                }
            }
            else
            {
                result = result ?? new List<BGEntity>();
                result.Clear();
                StoreForEachKeyValue((index, id) =>
                {
                    if (id.IsEmpty || !entityIds.Contains(id)) return;
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
            var relatedId = StoreItems[entityIndex];
            return relatedId == BGId.Empty ? null : relatedId.ToByteArray();
        }

        /// <inheritdoc />
        public override void FromBytes(int entityIndex, ArraySegment<byte> segment)
        {
            if (segment.Count != 16) ClearValueNoEventForRelation(entityIndex);
            else
            {
                var id1 = new BGId(segment.Array, segment.Offset);
                if (ReverseCache.Enabled) ReverseSetRelated(entityIndex, id1);
                StoreItems[entityIndex] = id1;
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
                try
                {
                    //will it be ever enabled at this stage? probably not
                    if (ReverseCache.Enabled) ReverseSetRelated(cellRequest.EntityIndex, new BGId(array, cellRequest.Offset));
                    StoreItems[cellRequest.EntityIndex] = new BGId(array, cellRequest.Offset);
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
            var toEntityId = StoreItems[entityIndex];
            if (toEntityId == BGId.Empty) return "";
            var meta = To;
            return IdToString(toEntityId, meta?[toEntityId]);
        }

        /// <inheritdoc />
        public override void FromString(int entityIndex, string value)
        {
            if (string.IsNullOrEmpty(value)) ClearValueNoEventForRelation(entityIndex);
            else
            {
                var id1 = IdFromString(value);
                if (ReverseCache.Enabled) ReverseSetRelated(entityIndex, id1);
                StoreItems[entityIndex] = id1;
            }
        }

        public static string IdToString(BGId entityId, BGEntity entity)
        {
            var toEntityIdString = entityId.ToString();
            if (entity == null || string.IsNullOrEmpty(entity.Name)) return toEntityIdString;
            var result = entity.Name.Trim() + ValueIdSeparator + toEntityIdString;
            return result;
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc/>
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldRelationSingle(meta, id, name);
    }
}