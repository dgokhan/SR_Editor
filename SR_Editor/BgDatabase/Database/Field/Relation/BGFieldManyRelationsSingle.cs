/*
<copyright file="BGFieldManyRelationsSingle.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// many tables single relation Field 
    /// </summary>
    [FieldDescriptor(Name = "manyTablesRelationSingle", Folder = "Relation", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerManyRelationsSingle")]
    public class BGFieldManyRelationsSingle : BGFieldRelationMA<BGEntity, BGRowRef>, BGBinaryBulkLoaderClass, BGFieldRelationSingleI
    {
        public const ushort CodeType = 43;
        /// <inheritdoc/>
        public override ushort TypeCode => CodeType;

        /*
        public override bool CanBeUsedAsKey
        {
            get { return true; }
        }
        */


        //for new
        public BGFieldManyRelationsSingle(BGMetaEntity meta, string name, List<BGMetaEntity> to) : base(meta, name, to)
        {
        }

        //for existing
        internal BGFieldManyRelationsSingle(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Callbacks
        //================================================================================================
        /// <inheritdoc/>
        public override void OnEntityDelete(BGEntity entity)
        {
            //do not use index as argument to ReverseCache, cause it's not valid at this point
            if (ReverseCache.Enabled)
            {
                var tuple = StoreGet(entity.Index);
                if (tuple != null) ReverseCache.RemoveRelated(entity, tuple.EntityId);
            }

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
                var rowRef = StoreItems[entityIndex];
                if (rowRef == null) return null;
                return rowRef.GetEntity(Repo);
            }
            set
            {
                if (value == null)
                {
                    ClearValue(entityIndex);
                    ReverseRemoveRelated(entityIndex);
                }
                else
                {
                    CheckMetaId(value);

                    var rowRef = new BGRowRef(value);
                    ReverseSetRelated(entityIndex, rowRef);
                    if (events.On)
                    {
                        var oldValue = this[entityIndex];
                        if (Equals(value, oldValue)) return;
                        var entity = Meta[entityIndex];
                        FireBeforeValueChanged(entity, oldValue, value);
                        StoreSet(entityIndex, rowRef);
                        FireValueChanged(entity, oldValue, value);
                    }
                    else StoreSet(entityIndex, rowRef);
                }
            }
        }
        
        /// <inheritdoc />
        public BGEntity GetRelatedEntity(int entityIndex) => this[entityIndex];
        
        /// <inheritdoc />
        public void SetRelatedEntity(int entityIndex, BGEntity entity) => this[entityIndex] = entity;

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
        public override void SetStoredValue(int entityIndex, BGRowRef value)
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

            var fieldRelationSingle = (BGFieldManyRelationsSingle)fromField;
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
            if (related != null) ReverseCache.MarkDirty(related.EntityId);
        }

        //================================================================================================
        //                                              Reverse cache
        //================================================================================================
        /// <inheritdoc/>
        protected override void BuildReverseCache()
        {
            StoreForEachKeyValue((index, val) =>
            {
                if (val == null) return;
                var value = ReverseCache.Ensure(val.EntityId);
                value.Add(Meta[index]);
            });
        }

        private void ReverseSetRelated(int entityIndex, BGRowRef value)
        {
            if (!ReverseCache.Enabled) return;
            var oldValue = GetStoredValue(entityIndex);
            if (Equals(value, oldValue)) return;

            if (oldValue != null) ReverseCache.RemoveRelated(entityIndex, oldValue.EntityId);
            if (value != null) ReverseCache.AddRelated(entityIndex, value.EntityId);
        }

        private void ReverseRemoveRelated(int index)
        {
            if (!ReverseCache.Enabled) return;
            var tuple = StoreGet(index);
            if (tuple != null) ReverseCache.RemoveRelated(index, tuple.EntityId);
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
                            if (oldValue == null) continue;
                            StoreSet(e.Index, null);
                            FireStoredValueChanged(e, oldValue, null);
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
                    if (value == null || value.EntityId != id) return;
                    StoreSet(index, null);
                    FireStoredValueChanged(Meta[index], value, null);
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
                            if(oldValue==null) continue;
                            StoreSet(e.Index, null);
                            try
                            {
                                FireStoredValueChanged(e, oldValue, null);
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
                    if (value == null) return;
                    if (!entityIds.Contains(value.EntityId)) return;
                    var oldValue = StoreGet(index);
                    StoreSet(index, null);
                    try
                    {
                        FireStoredValueChanged(Meta[index], oldValue,null );
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
                    if (value == null || value.EntityId != entityId) return;

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
                StoreForEachKeyValue((index, tuple) =>
                {
                    if (tuple == null || !entityIds.Contains(tuple.EntityId)) return;
                    var entity = Meta[index];
                    if (entity != null) result.Add(entity);
                });
            }

            return result;
        }

        /// <inheritdoc/>
        protected override void OnRemoveRelatedMeta(BGMetaEntity metaEntity)
        {
            Meta.ForEachEntity(entity =>
            {
                var related = this[entity.Index];
                if (related == null || related.MetaId != metaEntity.Id) return;
                this[entity.Index] = null;
            });
        }

        //================================================================================================
        //                                              Serialization
        //================================================================================================
        /// <inheritdoc />
        public override byte[] ToBytes(int entityIndex)
        {
            var rowRef = StoreItems[entityIndex];
            if (rowRef == null) return null;
            return rowRef.ToBytes();
        }

        /// <inheritdoc />
        public override void FromBytes(int entityIndex, ArraySegment<byte> segment)
        {
            if (segment.Count != 32) ClearValueNoEventForRelation(entityIndex);
            else
            {
                var rowRef = new BGRowRef(segment);
                if (ReverseCache.Enabled) ReverseSetRelated(entityIndex, rowRef);
                StoreItems[entityIndex] = rowRef;
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
                if (cellRequest.Count != 32) ClearValueNoEventForRelation(cellRequest.EntityIndex);
                else
                {
                    var rowRef = new BGRowRef(array, cellRequest.Offset);
                    if (ReverseCache.Enabled) ReverseSetRelated(cellRequest.EntityIndex, rowRef);
                    StoreItems[cellRequest.EntityIndex] = rowRef;
                }
            }
        }

        /// <inheritdoc />
        public override string ToString(int entityIndex)
        {
            var rowRef = StoreItems[entityIndex];
            return RowRefToString(rowRef, Repo);
        }

        /// <inheritdoc />
        public override void FromString(int entityIndex, string value)
        {
            if (string.IsNullOrEmpty(value)) ClearValueNoEventForRelation(entityIndex);
            else
            {
                value = value.Trim();
                var rowRef = StringToRowRef(value);

                if (ReverseCache.Enabled) ReverseSetRelated(entityIndex, rowRef);
                StoreItems[entityIndex] = rowRef;
            }
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc/>
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldManyRelationsSingle(meta, id, name);

        //================================================================================================
        //                                              Utils
        //================================================================================================
    }
}