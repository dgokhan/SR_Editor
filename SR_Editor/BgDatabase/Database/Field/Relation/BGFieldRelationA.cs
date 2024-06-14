/*
<copyright file="BGFieldRelationA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// abstract relation Field 
    /// </summary>
    public abstract partial class BGFieldRelationA<T, TStoreType> : BGFieldCachedA<T, TStoreType>, BGAbstractRelationI, BGFieldWithCustomConfigI
    {
        public const char ValueIdSeparator = '_';

        protected readonly ReverseRelationCache ReverseCache;

        public BGMetaEntity From => Meta;

        /// <inheritdoc/>
        public override bool StoredValueIsTheSameAsValueType => false;

        protected BGFieldRelationA(BGMetaEntity meta, string name) : base(meta, name) => ReverseCache = new ReverseRelationCache(this);

        protected BGFieldRelationA(BGMetaEntity meta, BGId id, string name) : base(meta, id, name) => ReverseCache = new ReverseRelationCache(this);

        //================================================================================================
        //                                              reverse cache
        //================================================================================================
        /// <summary>
        /// build internal cache to quickly retrieve values in reverse relational direction 
        /// </summary>
        protected abstract void BuildReverseCache();

        //================================================================================================
        //                                              Value
        //================================================================================================
        /// <inheritdoc/>
        public override void ClearValues()
        {
            ReverseCache.Enable(false);
            base.ClearValues();
        }

        /*
        public override BGField Duplicate(BGMetaEntity meta)
        {
            var cloneRelation = base.Duplicate(meta) as BGFieldRelationA<T, TStoreType>;
            //if self referencing
            if (cloneRelation != null && cloneRelation.toId == Meta.Id) cloneRelation.toId = meta.Id;
            return cloneRelation;
        }
        */

        /// <inheritdoc/>
        public override void MoveEntitiesValues(int fromIndex, int toIndex, int numberOfValues)
        {
            //it is hard to implement properly
            ReverseCache.MarkDirty();
            base.MoveEntitiesValues(fromIndex, toIndex, numberOfValues);
        }

        //================================================================================================
        //                                              Relation
        //================================================================================================
        /// <inheritdoc />
        public abstract List<BGEntity> GetRelatedIn(BGId entityId, List<BGEntity> result = null);

        /// <inheritdoc />
        public abstract List<BGEntity> GetRelatedIn(HashSet<BGId> entityIds, List<BGEntity> result = null);

        /// <inheritdoc />
        public abstract void ClearToValue(BGId entityId);

        /// <inheritdoc />
        public abstract void ClearToValue(HashSet<BGId> entityIds);


        //================================================================================================
        //                                              Nested classes
        //================================================================================================
        /// <summary>
        /// This cache for fast values retrieval in reverse relation direction, i.e.
        /// when target entity is provided- the list of all rows, which reference target entity is returned 
        /// </summary>
        protected class ReverseRelationCache
        {
            private Dictionary<BGId, ReverseRelationCacheValueI> reverseEntityId2Related;

            private readonly BGFieldRelationA<T, TStoreType> relation;

            /// <summary>
            /// Is cache enabled
            /// </summary>
            public bool Enabled => reverseEntityId2Related != null;

            private bool allowDuplicates;

            /// <summary>
            /// Can result list contains duplicated?
            /// </summary>
            public bool AllowDuplicates
            {
                set
                {
                    if (allowDuplicates == value) return;
                    allowDuplicates = value;
                    Enable(false);
                }
            }


            public ReverseRelationCache(BGFieldRelationA<T, TStoreType> relation) => this.relation = relation;

            /// <summary>
            /// enable this cache
            /// </summary>
            public void Enable(bool enabled)
            {
                if (enabled)
                {
                    if (reverseEntityId2Related != null) return;
                    // var relatedMeta = relation.RelatedMeta;
                    // if (relatedMeta == null) return;

                    reverseEntityId2Related = new Dictionary<BGId, ReverseRelationCacheValueI>(16);
                    relation.BuildReverseCache();
                    //this is required in multi-threaded environment to ensure all lazily initiated collections are created 
                    foreach (var pair in reverseEntityId2Related) pair.Value.Flush();
                }
                else reverseEntityId2Related = null;
            }

            /// <summary>
            /// remove related row by its ID
            /// </summary>
            public void RemoveRelated(BGEntity entity, BGId relatedId)
            {
                if (reverseEntityId2Related == null) return;
                if (relatedId.IsEmpty) return;

                if (reverseEntityId2Related.TryGetValue(relatedId, out var value)) value.Remove(entity);
            }

            /// <summary>
            /// remove related row by its ID
            /// </summary>
            public void RemoveRelated(int entityIndex, BGId relatedId)
            {
                if (reverseEntityId2Related == null) return;
                if (relatedId.IsEmpty) return;

                if (reverseEntityId2Related.TryGetValue(relatedId, out var value)) value.Remove(relation.Meta[entityIndex]);
            }

            /// <summary>
            /// add related row by its ID
            /// </summary>
            public void AddRelated(int entityIndex, BGId relatedId)
            {
                if (reverseEntityId2Related == null) return;
                if (relatedId.IsEmpty) return;
                Ensure(relatedId).Add(relation.Meta.GetEntity(entityIndex));
            }

            /// <summary>
            /// add related row by its ID
            /// </summary>
            public void AddRelated(BGEntity entity, BGId relatedId)
            {
                if (reverseEntityId2Related == null) return;
                if (entity == null) return; //throw new BGException("Can not update reverse relation cache- entity is null");

                Ensure(relatedId).Add(entity);
            }

            /// <summary>
            /// get result list by target entity
            /// </summary>
            public List<BGEntity> Get(BGId relatedId)
            {
                if (reverseEntityId2Related == null) return null;
                return !reverseEntityId2Related.TryGetValue(relatedId, out var value) ? null : value.List;
            }

            /// <summary>
            /// remove related entity
            /// </summary>
            public void Remove(BGId relatedId) => reverseEntityId2Related?.Remove(relatedId);

            /// <summary>
            /// mark cache as dirty for provided related entity
            /// </summary>
            public void MarkDirty(BGId relatedId)
            {
                if (reverseEntityId2Related == null) return;
                if (!reverseEntityId2Related.TryGetValue(relatedId, out var value)) return;
                value.MarkDirty();
            }

            /// <summary>
            /// Mark all data as dirty
            /// </summary>
            public void MarkDirty()
            {
                if (reverseEntityId2Related == null) return;
                foreach (var pair in reverseEntityId2Related) pair.Value.MarkDirty();
            }

            /// <summary>
            /// ensures the value is added for provided target entity ID 
            /// </summary>
            public ReverseRelationCacheValueI Ensure(BGId entityId)
            {
                if (reverseEntityId2Related.TryGetValue(entityId, out var value)) return value;
                value = allowDuplicates ? (ReverseRelationCacheValueI)new ReverseRelationCacheValueMulti() : new ReverseRelationCacheValue();
                reverseEntityId2Related[entityId] = value;
                return value;
            }
        }

        /// <summary>
        /// interface for the value for related entities list
        /// </summary>
        protected interface ReverseRelationCacheValueI
        {
            /// <summary>
            /// get result
            /// </summary>
            List<BGEntity> List { get; }
            /// <summary>
            /// add entity to the result list
            /// </summary>
            void Add(BGEntity entity);
            /// <summary>
            /// remove entity from result list
            /// </summary>
            void Remove(BGEntity entity);
            /// <summary>
            /// WE need to get rid of this method
            /// </summary>
            void MarkDirty();
            /// <summary>
            /// WE need to get rid of this  method
            /// </summary>
            void Flush();
        }

        /// <summary>
        /// Reverse relational value without duplicates default implementation
        /// </summary>
        protected class ReverseRelationCacheValue : ReverseRelationCacheValueI
        {
            //can these 2 collections be replaced somehow?
            private readonly HashSet<BGEntity> set = new HashSet<BGEntity>();
            private readonly List<BGEntity> list = new List<BGEntity>();
            private bool isDirty;

            /// <inheritdoc/>
            public List<BGEntity> List
            {
                get
                {
                    if (!isDirty) return list;

                    Flush();

                    return list;
                }
            }

            internal ReverseRelationCacheValue()
            {
            }

            /// <inheritdoc/>
            public void Add(BGEntity entity)
            {
                if (entity == null) return;
                if (!set.Add(entity)) return;
                MarkDirty();
            }

            /// <inheritdoc/>
            public void Remove(BGEntity entity)
            {
                if (entity == null) return;
                if (!set.Remove(entity)) return;
                MarkDirty();
            }

            /// <inheritdoc/>
            public void MarkDirty()
            {
                isDirty = true;
                list.Clear();
            }

            /// <inheritdoc/>
            public void Flush()
            {
                if (!isDirty) return;
                isDirty = false;
                list.Clear();
                list.AddRange(set);
                list.Sort();
            }
        }

        /// <summary>
        /// Reverse relational value with duplicates default implementation
        /// </summary>
        protected class ReverseRelationCacheValueMulti : ReverseRelationCacheValueI
        {
            //can these 2 collections be replaced somehow?
            private readonly Dictionary<BGEntity, int> id2Count = new Dictionary<BGEntity, int>();
            private readonly List<BGEntity> list = new List<BGEntity>();
            private bool isDirty;

            /// <inheritdoc/>
            public List<BGEntity> List
            {
                get
                {
                    if (!isDirty) return list;

                    Flush();

                    return list;
                }
            }

            internal ReverseRelationCacheValueMulti()
            {
            }

            /// <inheritdoc/>
            public void Add(BGEntity entity)
            {
                if (entity == null) return;
                if (!id2Count.TryGetValue(entity, out var count)) id2Count[entity] = 1;
                else id2Count[entity] = count + 1;
                MarkDirty();
            }

            /// <inheritdoc/>
            public void Remove(BGEntity entity)
            {
                if (entity == null) return;
                if (id2Count.TryGetValue(entity, out var count))
                {
                    if (count <= 1) id2Count.Remove(entity);
                    else id2Count[entity] = count - 1;
                }

                MarkDirty();
            }

            /// <inheritdoc/>
            public void MarkDirty()
            {
                isDirty = true;
                list.Clear();
            }

            /// <inheritdoc/>
            public void Flush()
            {
                if (!isDirty) return;
                isDirty = false;
                list.Clear();
                list.AddRange(id2Count.Keys);
                list.Sort();
            }
        }
    }
}