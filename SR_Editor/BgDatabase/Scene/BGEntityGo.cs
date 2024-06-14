/*
<copyright file="BGEntityGo.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;
using UnityEngine.Events;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// MonoBehaviour Component for connecting GameObject to the database.
    /// Also it's the base class for MonoBehaviour generated classes
    /// </summary>
//    [DisallowMultipleComponent]
    [AddComponentMenu("BansheeGz/BGEntityGo")]
    public partial class BGEntityGo : MonoBehaviour, ISerializationCallbackReceiver
    {
        //Unity event to be fired on entity change
        public EntityChangedEvent OnEntityChange = new EntityChangedEvent();

        //======================================================================================
        //                                    Entity
        //======================================================================================
        //row ID
        [HideInInspector] [SerializeField] private string entityIdString;

        private BGId entityId;

        /// <summary>
        /// referenced entity Id
        /// </summary>
        public BGId EntityId
        {
            get
            {
                if (!initWithFirst) return entityId;

                //get first
                var entity = GetFirst();
                return entity?.Id ?? BGId.Empty;
            }
            set
            {
                if (initWithFirst) return;
                if (value == entityId) return;

                if (value != BGId.Empty && !SetUpEntity(value)) return;

                SetEntityId(value);

                EntityChanged();
                OnEntityChange?.Invoke(this);
            }
        }

        private BGEntity entity;

        /// <summary>
        /// referenced entity 
        /// </summary>
        public BGEntity Entity
        {
            get
            {
                if (initWithFirst) return GetFirst();

                if ((entity == null || entity.Id != entityId || entity.IsDeleted || entity.Meta.IsDeleted) && entityId != BGId.Empty) SetUpEntity(entityId);
                return entity;
            }
            set
            {
                if (initWithFirst) return;
                entity = value;
                if (entity != null)
                {
                    SetEntityId(entity.Id);
                    SetMetaId(entity.MetaId);
                }
                else SetEntityId(BGId.Empty);

                EntityChanged();
                OnEntityChange?.Invoke(this);
            }
        }

        //change entity ID
        private void SetEntityId(BGId value)
        {
            entityId = value;
            if (entityId == BGId.Empty)
            {
                entityIdString = null;
                entity = null;
            }
            else entityIdString = value.ToString();
        }

        //======================================================================================
        //                                    Meta
        //======================================================================================
        //referenced table ID
        [HideInInspector] [SerializeField] private string metaIdString;

        private BGId metaId;

        /// <summary>
        /// Referenced Meta Id
        /// </summary>
        public BGId MetaId
        {
            get => metaId;
            set
            {
                if (MetaConstraint != null) return;
                if (Equals(value, metaId)) return;

                SetMetaId(value);
                SetEntityId(BGId.Empty);
            }
        }

        /// <summary>
        /// Referenced Meta 
        /// </summary>
        public virtual BGMetaEntity Meta
        {
            get => MetaConstraint ?? BGRepo.I.GetMeta(metaId);
            set
            {
                if (MetaConstraint != null) return;

                if (value == null)
                {
                    SetMetaId(BGId.Empty);
                    SetEntityId(BGId.Empty);
                }
                else
                {
                    if (metaId == value.Id) return;

                    SetMetaId(value.Id);
                    SetEntityId(BGId.Empty);

                    if (initWithFirst) SetUpFirst();
                }
            }
        }

        /// <summary>
        /// meta constraint is used to limit table selection to the single table
        /// </summary>
        public virtual BGMetaEntity MetaConstraint => null;

        //change referenced meta
        private void SetMetaId(BGId metaId)
        {
            this.metaId = metaId;
            metaIdString = metaId == BGId.Empty ? null : metaId.ToString();
        }

        //======================================================================================
        //                                    Init with first
        //======================================================================================
        //if true- the first row will be used
        [HideInInspector] [SerializeField] private bool initWithFirst;

        /// <summary>
        /// init with first entity
        /// </summary>
        public bool InitWithFirst
        {
            get => initWithFirst;
            set
            {
                if (initWithFirst == value) return;

                initWithFirst = value;
                SetUpFirst();
            }
        }

        //init entity ID with first row ID
        private void SetUpFirst()
        {
            if (!initWithFirst) return;
            var meta = Meta;
            if (meta != null && meta.CountEntities > 0) SetEntityId(meta[0].Id);
        }

        //get first row
        private BGEntity GetFirst()
        {
            var meta = Meta;
            if (meta == null) return null;
            return meta.CountEntities == 0 ? null : meta[0];
        }


        //======================================================================================
        //                                    On entity change callback
        //======================================================================================

        /// <summary>
        /// entity changed callback
        /// </summary>
        public virtual void EntityChanged()
        {
        }

        //======================================================================================
        //                                    Unity Callbacks
        //======================================================================================

        //reserved methods
        public virtual void Awake()
        {
        }

        public virtual void Start()
        {
        }

        public virtual void OnDestroy()
        {
        }

        //======================================================================================
        //                                    Serialization
        //======================================================================================
        /// <inheritdoc />
        public void OnBeforeSerialize()
        {
        }

        /// <inheritdoc />
        public void OnAfterDeserialize()
        {
            try
            {
                metaId = new BGId(metaIdString);
            }
            catch (Exception)
            {
                // ignored
            }

            try
            {
                entityId = new BGId(entityIdString);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        //======================================================================================
        //                                    Utility
        //======================================================================================
        /// <summary>
        /// get field value shortcut 
        /// </summary>
        public T Get<T>(string name) => Entity.Get<T>(name);

        /// <summary>
        /// set field value shortcut 
        /// </summary>
        public void Set<T>(string name, T value) => Entity.Set(name, value);

        /// <summary>
        /// get field value shortcut 
        /// </summary>
        public T Get<T>(BGId id) => Entity.Get<T>(id);

        /// <summary>
        /// set field value shortcut 
        /// </summary>
        public void Set<T>(BGId id, T value) => Entity.Set(id, value);

        //======================================================================================
        //                                    Private
        //======================================================================================
        //new entity id was provided
        private bool SetUpEntity(BGId newEntityId)
        {
            //try to get it from current meta first, cause it's (probably) much faster
            var meta = Meta;
            entity = meta?[newEntityId];
            if (entity != null) return true;

            //meta probably changed or null - try to get it without meta- by id directly
            entity = BGRepo.I.GetEntity(newEntityId);
            if (entity == null) return true;

            //entity was found
            var metaConstraint = MetaConstraint;
            if (metaConstraint != null && entity.MetaId != metaConstraint.Id)
            {
                entity = null;
                return false;
            }

            SetMetaId(entity.MetaId);
            return true;
        }

        /// <summary>
        /// attached entity was changed
        /// </summary>
        [Serializable]
        public class EntityChangedEvent : UnityEvent<BGEntityGo>
        {
            public void Invoke(BGEntityGo bgEntityGo)
            {
            }
        }
    }
}