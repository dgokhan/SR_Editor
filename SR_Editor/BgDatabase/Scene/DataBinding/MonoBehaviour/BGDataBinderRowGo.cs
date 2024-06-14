/*
<copyright file="BGDataBinderRowGo.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// This binder is used to inject multiple values from one single row.
    /// It uses the same naming convention to resolve fields/properties as BGDataBindingDatabaseGo binder does. 
    /// </summary>
    [AddComponentMenu("BansheeGz/BGDataBinderRowGo")]
    public class BGDataBinderRowGo : BGDataBinderGoA
    {
        //static

        //serializable
        [SerializeField] [HideInInspector] private Component targetComponent;

        [SerializeField] [HideInInspector] private long metaIdKey1;
        [SerializeField] [HideInInspector] private long metaIdKey2;
        [SerializeField] [HideInInspector] private long entityIdKey1;
        [SerializeField] [HideInInspector] private long entityIdKey2;

        [SerializeField] [HideInInspector] private bool ignoreBaseTypes;
        [SerializeField] [HideInInspector] private bool liveUpdate;


        //not serializable
        private BGMetaEntity meta;
        private BGEntity entity;
        private bool listenersWasAdded;

        /// <inheritdoc/>
        public override string Error
        {
            get
            {
                if (targetComponent == null) return "Target component is not set";
                var entity = Entity;
                if (entity == null)
                {
                    var meta = Meta;
                    if (meta == null) return metaIdKey1 == 0 && metaIdKey2 == 0 ? "Meta is not defined" : "Can not find meta with id " + new BGId(metaIdKey1, metaIdKey2);
                    return entityIdKey1 == 0 && entityIdKey2 == 0 ? "Entity is not defined" : "Can not find entity with id " + new BGId(entityIdKey1, entityIdKey2);
                }

                return null;
            }
        }

        /// <summary>
        /// target Unity component
        /// </summary>
        public Component TargetComponent
        {
            get => targetComponent;
            set => targetComponent = value;
        }

        /// <summary>
        /// ignore fields/properties from base classes
        /// </summary>
        public bool IgnoreBaseTypes
        {
            get => ignoreBaseTypes;
            set => ignoreBaseTypes = value;
        }

        /// <summary>
        /// is live-update on
        /// </summary>
        public bool LiveUpdate
        {
            get => liveUpdate;
            set => liveUpdate = value;
        }

        /// <summary>
        /// Source table ID
        /// </summary>
        public BGId MetaId
        {
            get
            {
                var meta = Meta;
                return meta?.Id ?? BGId.Empty;
            }
        }

        /// <summary>
        /// Source table 
        /// </summary>
        public BGMetaEntity Meta
        {
            get
            {
                var needToLookup = false;
                if (meta == null) needToLookup = true;
                else
                {
                    if (meta.IsDeleted) needToLookup = true;
                    else
                    {
                        meta.Id.ToLongKeys(out var key1, out var key2);
                        if (metaIdKey1 != key1 || metaIdKey2 != key2) needToLookup = true;
                    }
                }

                if (needToLookup) meta = BGRepo.I[new BGId(metaIdKey1, metaIdKey2)];
                return meta;
            }
        }

        /// <summary>
        /// Source row ID
        /// </summary>
        public BGId EntityId
        {
            get
            {
                var entity = Entity;
                return entity?.Id ?? BGId.Empty;
            }
        }

        /// <summary>
        /// Source row
        /// </summary>
        public BGEntity Entity
        {
            get
            {
                if (entity != null && !entity.Meta.IsDeleted)
                {
                    entity.Id.ToLongKeys(out var key1, out var key2);
                    if (entityIdKey1 == key1 && entityIdKey2 == key2) return entity;
                }

                var meta = Meta;
                if (meta != null) entity = meta[new BGId(entityIdKey1, entityIdKey2)];
                return entity;
            }
            set
            {
                if (value == null)
                {
                    entity = null;
                    entityIdKey1 = 0;
                    entityIdKey2 = 0;
                }
                else
                {
                    if (entity.Id == new BGId(entityIdKey1, entityIdKey2)) return;

                    entity = value;
//                    var metaHasChanged = entity.Meta.Id != new BGId(metaIdKey1, metaIdKey2);  
                    entity.Id.ToLongKeys(out entityIdKey1, out entityIdKey2);
                    entity.Meta.Id.ToLongKeys(out metaIdKey1, out metaIdKey2);
                    meta = entity.Meta;
                    Bind();
                }
            }
        }

        //is live-update enabled
        protected bool IsLiveUpdateOn => liveUpdate && (Application.isPlaying || BGUtil.TestIsRunning) && Error == null;


        //-------------------------- Autoconfig
        private void Reset()
        {
            AutoConfig();
        }

        //try to find target component in auto mode
        private void AutoConfig()
        {
            var components = GetComponents<Component>();
            var componentsList = new List<Component>();
            for (var i = 0; i < components.Length; i++)
            {
                var component = components[i];
                if (component == this) continue;
                componentsList.Add(component);
            }

            var metaList = BGRepo.I.FindMetas();
            for (var i = 0; i < metaList.Count; i++)
            {
                var meta = metaList[i];
                for (var j = 0; j < components.Length; j++)
                {
                    var component = components[j];
                    if (string.Equals(meta.Name, component.GetType().Name))
                    {
                        targetComponent = component;
                        meta.Id.ToLongKeys(out metaIdKey1, out metaIdKey2);
                        goto breakHere;
                    }
                }
            }

            breakHere: ;
        }


        //-------------------------- Binding
        /// <inheritdoc/>
        protected override void FirstBind()
        {
            if (HasError) return;
            Bind();
            AddListeners();
        }

        /// <inheritdoc/>
        public override void Bind()
        {
            if (HasError) return;

            if (!bindedOnce)
            {
                bindedOnce = true;
                FirstBind();
            }
            else
            {
                var meta = Meta;
                var e = Entity;

                BGDBAutoMapRegistry.Instance.GetAutoMappedConfig(meta, targetComponent.GetType(), !ignoreBaseTypes).Bind(e, targetComponent, false,
                    s => LogError("BGDatabase Error: Can not bind a value using BGDataBinderRowGo binder. " + s));
            }
            FireOnBind();
        }

        /// <inheritdoc/>
        public override void ReverseBind()
        {
            if (HasError) return;

            var meta = Meta;
            var e = Entity;

            BGDBAutoMapRegistry.Instance.GetAutoMappedConfig(meta, targetComponent.GetType(), !ignoreBaseTypes).ReverseBind(e, targetComponent,
                s => LogError("BGDatabase Error: Can not reverse bind a value using BGDataBinderRowGo binder. " + s));
        }

        //has any error in config
        private bool HasError
        {
            get
            {
                if (Error == null) return false;
                LogError(Error);
                return true;
            }
        }

        //-------------------------- Listeners
        //add listeners if needed
        private void AddListeners()
        {
            if (!IsLiveUpdateOn || listenersWasAdded) return;
            listenersWasAdded = true;
            BGRepo.OnLoad += OnLoad;
            BGRepo.I.Events.OnBatchUpdate += OnBatch;
            var e = Entity;
            e?.Meta.AddEntityUpdatedListener(e.Id, EntityIsChanged);
        }

        //remove previously attached listeners if needed
        private void RemoveListeners()
        {
            if (!listenersWasAdded) return;
            BGRepo.OnLoad -= OnLoad;
            BGRepo.I.Events.OnBatchUpdate -= OnBatch;
            var e = Entity;
            e?.Meta.RemoveEntityUpdatedListener(e.Id, EntityIsChanged);
        }

        //target entity changed event listener
        private void EntityIsChanged(object sender, BGEventArgsEntityUpdated e) => Bind();

        //on database loaded listener handler
        private void OnLoad(bool loaded)
        {
            if (!loaded) return;
            Bind();
        }

        //on batch update  listener handler
        private void OnBatch(object sender, BGEventArgsBatch e)
        {
            if (!e.WasEntitiesUpdated(MetaId)) return;
            Bind();
        }

        /// <inheritdoc/>
        protected override void OnDestroy() => RemoveListeners();
    }
}