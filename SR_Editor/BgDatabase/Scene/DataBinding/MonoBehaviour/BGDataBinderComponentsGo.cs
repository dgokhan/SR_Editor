/*
<copyright file="BGDataBinderComponentsGo.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Inject one single value from database to multiple components
    /// </summary>
    [AddComponentMenu("BansheeGz/BGDataBinderComponentsGo")]
    public class BGDataBinderComponentsGo : BGDataBinderGoA
    {
        public enum DataBinderComponentsSourceType : byte
        {
            Field,
            Template,
            Graph,
        }

        //serializable
        [SerializeField] [HideInInspector] private DataBinderComponentsSourceType sourceType;

        //field
        [SerializeField] [HideInInspector] private string metaIdString;
        [SerializeField] [HideInInspector] private string entityIdString;
        [SerializeField] [HideInInspector] private string fieldIdString;

        //template
        [SerializeField] [HideInInspector] private string template;

        //graph
        [SerializeField] [HideInInspector] private byte typeCode = BGCalcTypeCodeString.Code;
        [SerializeField] private byte[] graphContent;

        //live update
        [SerializeField] [HideInInspector] private bool liveUpdate;

        //target
        [SerializeField] [HideInInspector] private string targetComponentClassName;
        [SerializeField] [HideInInspector] private string targetFieldName;
        [SerializeField] [HideInInspector] private string includeTag;
        [SerializeField] [HideInInspector] private string excludeTag;

        //non serializable
        private bool listenersWasAdded;
        [NonSerialized] private BGDBA BindDelegate;
        private Type targetComponentType;

        //props
        public byte[] GraphContent
        {
            get => graphContent;
            set => graphContent = value;
        }

        public string Template
        {
            get => template;
            set => template = value;
        }

        public string IncludeTag
        {
            get => includeTag;
            set => includeTag = value;
        }

        public string ExcludeTag
        {
            get => excludeTag;
            set => excludeTag = value;
        }

        public override bool SupportReverseBinding => false;
        public bool LiveUpdate
        {
            get => liveUpdate;
            set => liveUpdate = value;
        }

        public DataBinderComponentsSourceType SourceType
        {
            get => sourceType;
            set
            {
                if (sourceType == value) return;
                sourceType = value;
                BindDelegate = null;
            }
        }

        public string TargetComponentClassName
        {
            get => targetComponentClassName;
            set
            {
                if (targetComponentClassName == value) return;
                targetComponentClassName = value;
                targetComponentType = null;
            }
        }

        public string TargetFieldName
        {
            get => targetFieldName;
            set
            {
                if (targetFieldName == value) return;
                targetFieldName = value;
            }
        }

        public MemberInfo TargetField
        {
            get
            {
                var targetType = TargetComponentType;
                if (targetType == null) return null;
                if (string.IsNullOrEmpty(targetFieldName)) return null;
                MemberInfo result = targetType.GetProperty(targetFieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (result == null) result = targetType.GetField(targetFieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                return result;
            }
        }

        public Type TargetComponentType
        {
            get
            {
                if (string.IsNullOrEmpty(targetComponentClassName)) return null;
                if (targetComponentType != null && string.Equals(targetComponentType.FullName, targetComponentClassName)) return targetComponentType;
                targetComponentType = BGUtil.GetType(targetComponentClassName);
                return targetComponentType;
            }
        }

        protected bool IsLiveUpdateOn => liveUpdate && (Application.isPlaying || BGUtil.TestIsRunning) && Error == null;

        public override string Error
        {
            get
            {
                if (string.IsNullOrEmpty(targetComponentClassName)) return "Target component is not set";
                var targetType = TargetComponentType;
                if (targetType == null) return $"Can not load target type  {targetComponentClassName}";
                if (!typeof(Component).IsAssignableFrom(targetType)) return $"Target type  {targetComponentClassName} is not Unity component!";

                var targetField = TargetField;
                if (targetField == null) return $"Can not load target field/property {targetFieldName} at  {targetComponentClassName} class";
                return null;
            }
        }

        public Type GraphTargetType
        {
            get
            {
                switch (typeCode)
                {
                    case BGCalcTypeCodeBool.Code:
                        return typeof(bool);
                    case BGCalcTypeCodeString.Code:
                        return typeof(string);
                    case BGCalcTypeCodeInt.Code:
                        return typeof(int);
                    case BGCalcTypeCodeFloat.Code:
                        return typeof(float);
                    default:
                        return typeof(object);
                }
            }
            set
            {
                if (value == typeof(bool)) typeCode = BGCalcTypeCodeBool.Code;
                else if (value == typeof(string)) typeCode = BGCalcTypeCodeString.Code;
                else if (value == typeof(int)) typeCode = BGCalcTypeCodeInt.Code;
                else if (value == typeof(float)) typeCode = BGCalcTypeCodeFloat.Code;
                else typeCode = BGCalcTypeCodeObject.Code;
            }
        }

        public byte GraphTypeCode
        {
            get => typeCode;
            set => typeCode = value;
        }

        public BGCalcGraph Graph
        {
            get
            {
                InjectToDelegate();
                if (!(BindDelegate is BGDBGraph graphBinder)) return null;
                return graphBinder.Graph;
            }
        }

        /// <summary>
        /// Source table ID as string
        /// </summary>
        public string MetaIdString => metaIdString;

        /// <summary>
        /// Source row ID as string
        /// </summary>
        public string EntityIdString => entityIdString;

        /// <summary>
        /// Source field ID as string
        /// </summary>
        public string FieldIdString
        {
            get => fieldIdString;
            set { fieldIdString = value; }
        }

        /// <summary>
        /// Target field's path
        /// </summary>
        public BGDBField.FieldPath FieldPath
        {
            get
            {
                InjectToDelegate();
                if (!(BindDelegate is BGDBField fieldDelegate)) return null;
                return fieldDelegate.BindSourceProvider.FieldPath;
            }
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
                InjectToDelegate();
                if (!(BindDelegate is BGDBField fieldDelegate)) return null;
                return fieldDelegate.Meta;
            }
            set
            {
                if (value == null)
                {
                    metaIdString = null;
                    entityIdString = null;
                    fieldIdString = null;
                }
                else metaIdString = value.Id.ToString();

                InjectToDelegate();
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
            set => Entity = BGRepo.I.GetEntity(value);
        }

        /// <summary>
        /// Source row
        /// </summary>
        public BGEntity Entity
        {
            get
            {
                InjectToDelegate();
                if (!(BindDelegate is BGDBField fieldDelegate)) return null;
                return fieldDelegate.Entity;
            }
            set
            {
                var entity = Entity;
                if (value == null && entity == null) return;
                if (value != null && entity != null && entity.Id == value.Id) return;

                if (value == null)
                {
                    metaIdString = null;
                    entityIdString = null;
                }
                else
                {
                    metaIdString = value.Meta.Id.ToString();
                    entityIdString = value.Id.ToString();
                }

                InjectToDelegate();
            }
        }

        /// <summary>
        /// Source field ID
        /// </summary>
        private BGId FieldId
        {
            get
            {
                var field = Field;
                return field?.Id ?? BGId.Empty;
            }
        }

        /// <summary>
        /// Source field
        /// </summary>

        public BGField Field
        {
            get
            {
                InjectToDelegate();
                if (!(BindDelegate is BGDBField fieldDelegate)) return null;
                return fieldDelegate.Field;
            }
            set
            {
                if (value == null) fieldIdString = null;
                else
                {
                    fieldIdString = value.Id.ToString();
                    metaIdString = value.MetaId.ToString();
                }

                InjectToDelegate();
            }
        }


        protected override void OnDestroy() => RemoveListeners();

        protected override void FirstBind()
        {
            Bind();
            AddListeners();
        }

        public override void Bind()
        {
            if (!bindedOnce)
            {
                bindedOnce = true;
                FirstBind();
            }
            else BindInternal();

            FireOnBind();
        }

        public override void ReverseBind()
        {
            //not supported
        }

        //========================   Bind
        private void BindInternal()
        {
            try
            {
                InjectToDelegate();

                var value = BindDelegate.GetValue();

                var targetType = TargetComponentType;
                if (targetType == null) throw new Exception($"Can not load target type component class {targetComponentClassName}");
                if (!typeof(Component).IsAssignableFrom(targetType)) throw new Exception($"Target type  {targetComponentClassName} is not Unity component!");
                var targetField = TargetField;
                if (targetField == null) throw new Exception($"Can not find target field/property with name {targetFieldName} at class {targetComponentClassName}");
                FieldInfo fieldInfo = null;
                PropertyInfo propertyInfo = null;
                if (targetField is PropertyInfo) propertyInfo = (PropertyInfo)targetField;
                else fieldInfo = (FieldInfo)targetField;

                var includeTagOn = !string.IsNullOrEmpty(includeTag);
                var excludeTagOn = !string.IsNullOrEmpty(excludeTag);

                //replace with FindObjectsOfType(type, includeActive)
                // var components = FindObjectsOfType(targetType);
                var components = Resources.FindObjectsOfTypeAll(targetType);
                foreach (var component in components)
                {
                    var typed = (Component)component;
                    //skip prefabs
                    var go = typed.gameObject;
                    if (go.scene.name == null) continue;
                    
                    //skip by included tag
                    if (includeTagOn && !go.CompareTag(includeTag)) continue;
                    //skip by excluded tag
                    if (excludeTagOn && go.CompareTag(excludeTag)) continue;
                    
                    if (propertyInfo != null) propertyInfo.SetValue(component, value);
                    else fieldInfo.SetValue(component, value);
                }
            }
            catch (Exception e)
            {
                Debug.Log("BGDatabase.BGDataBinderComponentsGo: Exception while binding! See the exception log below for more details");
                Debug.LogException(e);
            }
        }

        private HashSet<string> StringToHashSet(string tag)
        {
            return null;
        }

        internal void InjectToDelegate()
        {
            switch (sourceType)
            {
                case DataBinderComponentsSourceType.Field:
                {
                    BGDBField binder;
                    if (!(BindDelegate is BGDBField field)) BindDelegate = binder = new BGDBField();
                    else binder = field;
                    binder.MetaIdString = metaIdString;
                    binder.FieldIdString = fieldIdString;
                    binder.EntityIdString = entityIdString;
                    break;
                }
                case DataBinderComponentsSourceType.Template:
                {
                    BGDBTemplate binder;
                    if (!(BindDelegate is BGDBTemplate field)) BindDelegate = binder = new BGDBTemplate();
                    else binder = field;
                    binder.Template = template;
                    break;
                }
                case DataBinderComponentsSourceType.Graph:
                {
                    BGDBGraph binder;
                    if (!(BindDelegate is BGDBGraph field)) BindDelegate = binder = new BGDBGraph();
                    else binder = field;
                    var graph = binder.Graph;
                    //this will prevent from changing graph on the fly, hopefully nobody will use it this way 
                    if (graph == null)
                    {
                        if (graphContent != null && graphContent.Length > 0)
                        {
                            graph = BGCalcGraph.ExistingGraph();
                            graph.FromBytes(new ArraySegment<byte>(graphContent));
                        }
                        else graph = BGCalcGraph.NewGraph(typeCode == 0 ? BGCalcTypeCodeRegistry.String : BGCalcTypeCodeRegistry.Get(typeCode));
                    }

                    binder.Graph = graph;
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(sourceType));
            }

            BindDelegate.Error = null;
        }


        //========================   Listeners
        protected void AddListeners()
        {
            if (!IsLiveUpdateOn || listenersWasAdded) return;
            listenersWasAdded = true;
            BGRepo.OnLoad += OnLoad;
            BGRepo.I.Events.OnBatchUpdate += OnBatch;
            BindDelegate?.AddFieldsListeners(Bind);
        }

        private void RemoveListeners()
        {
            if (!listenersWasAdded) return;
            BGRepo.OnLoad -= OnLoad;
            BGRepo.I.Events.OnBatchUpdate -= OnBatch;
            BindDelegate?.RemoveFieldsListeners();
        }

        //on database loaded listener handler
        private void OnLoad(bool loaded)
        {
            if (!loaded) return;
            Bind();
        }

        //on batch update listener handler
        private void OnBatch(object sender, BGEventArgsBatch e)
        {
            Bind();
        }
    }
}