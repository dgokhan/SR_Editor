/*
<copyright file="BGDataBinderDatabaseGo.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
#if !BG_SA
using Object = UnityEngine.Object;
#endif

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// This binder is used for binder-less setup.
    /// One single binder is used to inject all values.
    /// It scans all components, which have BGDatabaseEntityId string field with assigned id and uses naming convention
    /// to inject the values (fields/properties must be named exactly as database fields and have the same type)  
    /// </summary>
    [AddComponentMenu("BansheeGz/BGDataBinderDatabaseGo")]
    public class BGDataBinderDatabaseGo : BGDataBinderGoA
    {
        /// <summary>
        /// The name of the field, which is used for holding entity ID
        /// </summary>
        public const string IdFieldName = "BGDatabaseEntityId";

        private static BGDataBinderDatabaseGo last;

        private static BinderConfigurationMB binderConfigMB;


        //-serializable
        [SerializeField] private bool debugMode;
        [SerializeField] private bool liveUpdate;

        //-not serializable
        private List<BinderData> bindersMB;
        private bool listenersWasAdded;

        public static BinderConfigurationMB BinderConfigMB => binderConfigMB ?? (binderConfigMB = new BinderConfigurationMB());

        public override string Error => null;

        public bool DebugMode => debugMode;

        public bool LiveUpdate => liveUpdate;

        protected bool IsLiveUpdateOn => liveUpdate && (Application.isPlaying || BGUtil.TestIsRunning);

        //========================================= Unity


        //========================================= Binds
        protected override void FirstBind()
        {
            if (last != null) Debug.Log("WARNING! BGDataBinderDatabaseGo: you have more than 1 instance of BGDataBinderDatabaseGo in your scene. This is not optimal, cause 1 is enough");
            last = this;

            if (!liveUpdate) Bind();
            else
            {
                bindersMB = new List<BinderData>();
                BinderConfigMB.Bind(bindersMB, debugMode);
                AddListeners();
            }
        }

        public override void Bind()
        {
            if (!bindedOnce)
            {
                bindedOnce = true;
                FirstBind();
            }
            else BinderConfigMB.Bind(null, debugMode);
            FireOnBind();
        }

        public override void ReverseBind()
        {
            BinderConfigMB.ReverseBind(null, debugMode);
        }

        protected override void OnDestroy()
        {
            RemoveListeners();
        }

        //========================================= Listeners
        private void AddListeners()
        {
            if (!IsLiveUpdateOn || listenersWasAdded) return;
            listenersWasAdded = true;
            BGRepo.OnLoad += OnLoad;
            BGRepo.I.Events.OnBatchUpdate += OnBatch;
            AddListeners(bindersMB);
        }

        private void AddListeners(List<BinderData> binders)
        {
            foreach (var binderData in binders)
            {
                var entity = BGRepo.I.GetEntity(binderData.EntityId);
                entity?.Meta.AddEntityUpdatedListener(binderData.EntityId, EntityIsChanged);
            }
        }

        private void RemoveListeners()
        {
            if (!listenersWasAdded) return;
            BGRepo.OnLoad -= OnLoad;
            BGRepo.I.Events.OnBatchUpdate -= OnBatch;
            RemoveListeners(bindersMB);
        }

        private void RemoveListeners(List<BinderData> binders)
        {
            if (binders == null) return;
            foreach (var binderData in binders)
            {
                var entity = BGRepo.I.GetEntity(binderData.EntityId);
                entity?.Meta.RemoveEntityUpdatedListener(binderData.EntityId, EntityIsChanged);
            }
        }

        private void EntityIsChanged(object sender, BGEventArgsEntityUpdated e)
        {
            EntityIsChanged(bindersMB, e.Entity);
        }

        private void EntityIsChanged(List<BinderData> binders, BGEntity entity)
        {
            if (entity == null) return;
            foreach (var binder in binders)
            {
                if (binder.EntityId != entity.Id || binder.Target == null) continue;
                BGDBAutoMapRegistry.Instance.GetAutoMappedConfig(entity.Meta, binder.Target.GetType(), true).Bind(entity, binder.Target, false, null);
            }
        }

        private void OnLoad(bool loaded)
        {
            if (!loaded) return;
            Bind();
        }

        private void OnBatch(object sender, BGEventArgsBatch e)
        {
            Bind();
        }

        //========================================= Nested classes
        public class BinderData
        {
            public readonly BGId EntityId;
            public readonly Object Target;

            public BinderData(BGId entityId, Object target)
            {
                EntityId = entityId;
                Target = target;
            }
        }
        
        /// <summary>
        /// Abstract data container for binder configuration
        /// </summary>
        public abstract class BinderConfigurationA
        {
            protected readonly Dictionary<FieldInfo, Type> field2Type = new Dictionary<FieldInfo, Type>();

            /// <summary>
            /// iterate over all found types with BGDatabaseEntityId field
            /// </summary>
            public void ForEach(Action<FieldInfo, Type> action)
            {
                foreach (var pair in field2Type) action(pair.Key, pair.Value);
            }

            /// <summary>
            /// resolve the list of objects for provided type 
            /// </summary>
            public abstract Object[] Find(Type type);

            //log debug message
            protected static void Log(string message) => Debug.Log("BGDataBinderDatabaseGo [debugMode=on]: " + message);
        }


        /// <summary>
        /// Abstract generic data container for binder configuration
        /// </summary>
        public abstract class BinderConfigurationA<T> : BinderConfigurationA where T : Object
        {
            protected BinderConfigurationA()
            {
                var candidatesTypes = BGUtil.GetAllSubTypes(typeof(T));
                var typeOfString = typeof(string);
                for (var i = 0; i < candidatesTypes.Count; i++)
                {
                    var monoBehavior = candidatesTypes[i];
                    var field = BGPrivate.GetField(monoBehavior, IdFieldName, false);
                    if (field == null || field.FieldType != typeOfString) continue;
                    field2Type.Add(field, monoBehavior);
                }
            }

            /// <summary>
            /// Bind all objects
            /// </summary>
            public void Bind(List<BinderData> binders, bool debugMode) => BindInternal(binders, debugMode, (config, entity, o, logger) => config.Bind(entity, o, false, logger));

            /// <summary>
            /// Reverse bind all objects
            /// </summary>
            public void ReverseBind(List<BinderData> binders, bool debugMode) => BindInternal(binders, debugMode, (config, entity, o, logger) => config.ReverseBind(entity, o, logger));


            //bind internal
            private void BindInternal(List<BinderData> binders, bool debugMode, Action<BGDBAutoMapRegistry.AutoMappedConfig, BGEntity, T, Action<string>> action)
            {
                if (!BGRepo.Ok)
                {
                    Debug.LogError("BGDataBinderDatabaseGo: Can not bind, cause database is not loaded. Error: " + BGRepo.DefaultRepoErrorOnLoad);
                    return;
                }

                var logger = debugMode ? Log : (Action<string>)null;
                var count = 0;
                foreach (var pair in field2Type)
                {
                    var fieldInfo = pair.Key;
                    var type = pair.Value;

                    var objects = Find(type);
                    if (objects != null)
                        for (var i = 0; i < objects.Length; i++)
                        {
                            var o = (T)objects[i];

                            var entityIdString = (string)fieldInfo.GetValue(o);
                            var entityId = BGId.Parse(entityIdString);
                            if (entityId.IsEmpty)
                            {
                                if (debugMode)
                                {
                                    if (string.IsNullOrEmpty(entityIdString)) Log("Empty BGDatabaseEntityId value at " + ToString(o));
                                    else Log("Invalid BGDatabaseEntityId value [" + entityIdString + "] at " + ToString(o));
                                }

                                continue;
                            }

                            var entity = BGRepo.I.GetEntity(entityId);
                            if (entity == null)
                            {
                                if (debugMode) Log("Can not find entity with id [" + entityIdString + "], defined at " + ToString(o));
                                continue;
                            }

                            binders?.Add(new BinderData(entityId, o));
                            var autoMappedConfig = BGDBAutoMapRegistry.Instance.GetAutoMappedConfig(entity.Meta, type, true);
                            action(autoMappedConfig, entity, o, logger);
                            count++;
                        }
                }

                logger?.Invoke(count + " objects processed.");
            }

            //convert obj to string representation
            protected abstract string ToString(T obj);
        }

        /// <summary>
        /// data container for binder configuration for MonoBehaviour extended classes 
        /// </summary>
        public class BinderConfigurationMB : BinderConfigurationA<MonoBehaviour>
        {
            /// <inheritdoc/>
            public override Object[] Find(Type type) => FindObjectsOfType(type);

            /// <inheritdoc/>
            protected override string ToString(MonoBehaviour obj)
            {
                var transform1 = obj.transform;
                var path = transform1.name;
                while (transform1.parent != null)
                {
                    transform1 = transform1.parent;
                    path = transform1.name + "/" + path;
                }

                return "GameObject [" + path + "]";
            }
        }

/*
        public class BinderConfigurationSO : BinderConfigurationA<ScriptableObject>
        {
            public override Object[] Find(Type type)
            {
                return Resources.FindObjectsOfTypeAll(type);
            }

            protected override string ToString(ScriptableObject obj)
            {
                return "ScriptableObject [" + obj.GetType() + "]";
            }
        }
*/

        public static BGId GetId(Object asset, ref FieldInfo fieldInfo)
        {
            try
            {
                if (fieldInfo == null) fieldInfo = BGPrivate.GetField(asset.GetType(), IdFieldName);
                return BGId.Parse((string)fieldInfo.GetValue(asset));
            }
            catch
            {
                return BGId.Empty;
            }
        }
    }
}