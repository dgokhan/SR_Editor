/*
<copyright file="BGDBA.cs" company="BansheeGz">
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
    /// Abstract binder implementation class
    /// </summary>
    [Serializable]
    public abstract class BGDBA
    {
        //-------------------- Serializable
        //target
        [SerializeField] private GameObject targetGameObject;
        [SerializeField] private Component targetComponent;
        [SerializeField] private string targetFieldName;
        [SerializeField] private bool isTargetProperty;
        [SerializeField] private List<BGDataBinderGoA.PathItem> path = new List<BGDataBinderGoA.PathItem>();
        [SerializeField] private bool includePrivate;
        [SerializeField] private bool liveUpdate;

        /// <summary>
        /// Target GameObject
        /// </summary>
        public GameObject TargetGameObject
        {
            get => targetGameObject;
            set => targetGameObject = value;
        }

        /// <summary>
        /// Target GameObject's component
        /// </summary>
        public Component TargetComponent
        {
            get => targetComponent;
            set => targetComponent = value;
        }

        /// <summary>
        /// Target field/property name
        /// </summary>
        public string TargetFieldName
        {
            get => targetFieldName;
            set => targetFieldName = value;
        }

        /// <summary>
        /// Is target member a field or a property?
        /// </summary>
        public bool IsTargetProperty
        {
            get => isTargetProperty;
            set => isTargetProperty = value;
        }

        /// <summary>
        /// Target member as a string value
        /// </summary>
        public string TargetAsString
        {
            get
            {
                if (targetComponent == null || string.IsNullOrEmpty(targetFieldName)) return null;
                var result = targetComponent.GetType().Name;
                if (path != null && path.Count > 0)
                    for (var i = 0; i < path.Count; i++)
                        result += '.' + path[i].Field;

                result += '.' + targetFieldName;
                return result;
            }
        }

        /// <summary>
        /// Target C# member to inject the value to
        /// </summary>
        public MemberInfo TargetAsMember
        {
            get
            {
                if (targetComponent == null) return null;
                object obj = targetComponent;
                if (path != null && path.Count > 0)
                {
                    for (var i = 0; i < path.Count; i++)
                    {
                        var pathItem = path[i];
                        var bindingFlags = includePrivate ? BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic : BindingFlags.Instance | BindingFlags.Public;
                        if (pathItem.IsProperty)
                        {
                            var typeProperty = obj.GetType().GetProperty(pathItem.Field, bindingFlags);
                            if (typeProperty == null) return null;
                            obj = typeProperty.GetValue(obj, null);
                            if (obj == null) return null;
                        }
                        else
                        {
                            var typeField = obj.GetType().GetField(pathItem.Field, bindingFlags);
                            if (typeField == null) return null;
                            obj = typeField.GetValue(obj);
                            if (obj == null) return null;
                        }
                    }
                }

                if (isTargetProperty) return obj.GetType().GetProperty(targetFieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                return obj.GetType().GetField(targetFieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            }
        }

        /// <summary>
        /// Target field/property path
        /// </summary>
        public List<BGDataBinderGoA.PathItem> Path
        {
            get => path;
            set => path = value;
        }

        /// <summary>
        /// Should private fields/properties be included as possible targets
        /// </summary>
        public bool IncludePrivate
        {
            get => includePrivate;
            set => includePrivate = value;
        }

        /// <summary>
        /// Is live update on?
        /// </summary>
        public bool LiveUpdate
        {
            get => liveUpdate;
            set => liveUpdate = value;
        }

        /// <summary>
        /// Does binder configuration have any error? 
        /// </summary>
        public string Error
        {
            get
            {
                var v = ValueToBind;
                return error;
            }
            set => error = value;
        }

        /// <summary>
        /// Does this binder support reverse binding?
        /// </summary>
        public virtual bool SupportReverseBinding => true;

        //-------------------- Non serializable
        [NonSerialized] protected string error;
        [NonSerialized] private Type targetType;
        [NonSerialized] protected object target;
        [NonSerialized] protected PropertyInfo targetProperty;
        [NonSerialized] protected FieldInfo targetField;
        [NonSerialized] private int pathHashCode;
        [NonSerialized] protected readonly List<FieldEventHandler> eventHandlers = new List<FieldEventHandler>();

        /// <summary>
        /// Target member type
        /// </summary>
        public virtual Type TargetType
        {
            get
            {
                EnsureTarget();
                return targetType;
            }
        }

        //make sure the target is initialized 
        protected void EnsureTarget()
        {
            if (targetType != null)
            {
                var passedOk = false;
                if (isTargetProperty)
                {
                    if (targetProperty != null && string.Equals(targetProperty.Name, targetFieldName)) passedOk = true;
                }
                else
                {
                    if (targetField != null && string.Equals(targetField.Name, targetFieldName)) passedOk = true;
                }

                if (passedOk)
                    if (pathHashCode == PathHashCode())
                        //we assume everything fine: no need to initialize
                        return;
            }

            pathHashCode = -1;
            if (targetComponent == null)
            {
                error = "No target component";
                return;
            }

            //Target
            target = targetComponent;
            if (!InitPath()) return;

            if (IsError(string.IsNullOrEmpty(targetFieldName), "Target field/property name is not defined")) return;
            if (isTargetProperty)
            {
                targetProperty = target.GetType().GetProperty(targetFieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (IsError(targetProperty == null, "Can not find property: " + targetFieldName)) return;
                targetType = targetProperty.PropertyType;
            }
            else
            {
                targetField = target.GetType().GetField(targetFieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (IsError(targetField == null, "Can not find field: " + targetFieldName)) return;
                targetType = targetField.FieldType;
            }

            pathHashCode = PathHashCode();
        }

        //helper method to calculate target member hashcode
        private int PathHashCode()
        {
            if (path == null || path.Count == 0) return 0;

            const int seed = 487;
            const int modifier = 31;

            unchecked
            {
                var result = seed;
                for (var i = 0; i < path.Count; i++) result = result * modifier + path[i].GetHashCode();
                return result;
            }
        }

        /// <summary>
        /// final value to inject
        /// </summary>
        public abstract object ValueToBind { get; }

        public abstract object GetValue();

        //=================================================================================================================
        //                      Bind
        //=================================================================================================================

        /// <summary>
        /// inject value from database to target Unity component 
        /// </summary>
        public string Bind()
        {
            try
            {
                var valueToAssign = ValueToBind;
                if (error != null) return error;
                if (isTargetProperty) targetProperty.SetValue(target, valueToAssign, null);
                else targetField.SetValue(target, valueToAssign);
            }
            catch (Exception e)
            {
                error = e.Message;
            }

            return error;
        }

        /// <summary>
        /// inject value from  target Unity component to the database 
        /// </summary>
        public virtual string ReverseBind() => null;

        //initialize  target member path
        private bool InitPath()
        {
            if (path == null || path.Count <= 0) return true;

            for (var i = 0; i < path.Count; i++)
            {
                var pathItem = path[i];
                var bindingFlags = includePrivate ? BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic : BindingFlags.Instance | BindingFlags.Public;
                if (pathItem.IsProperty)
                {
                    var typeProperty = target.GetType().GetProperty(pathItem.Field, bindingFlags);
                    if (typeProperty == null)
                    {
                        error = "Can not find property: " + pathItem.Field;
                        return false;
                    }

                    target = typeProperty.GetValue(target, null);
                    if (target == null)
                    {
                        error = "Target object is null: " + pathItem.Field;
                        return false;
                    }
                }
                else
                {
                    var typeField = target.GetType().GetField(pathItem.Field, bindingFlags);
                    if (typeField == null)
                    {
                        error = "Can not find field: " + pathItem.Field;
                        return false;
                    }

                    target = typeField.GetValue(target);
                    if (target == null)
                    {
                        error = "Target object is null: " + pathItem.Field;
                        return false;
                    }
                }
            }

            return true;
        }

        // return true if condition is true and assign the error message
        private bool IsError(bool condition, string error)
        {
            if (!condition) return false;
            this.error = error;
            return true;
        }

        //=================================================================================================================
        //                      Listeners
        //=================================================================================================================
        /// <summary>
        /// add cell listeners for detecting database changes 
        /// </summary>
        public abstract int AddFieldsListeners(Action action);

        /// <summary>
        /// remove previously added cell listeners for detecting database changes 
        /// </summary>
        public virtual void RemoveFieldsListeners()
        {
            if (eventHandlers.Count <= 0) return;
            foreach (var eventHandler in eventHandlers) eventHandler.Release();
            eventHandlers.Clear();
        }

        //=================================================================================================================
        //                      Nested classes
        //=================================================================================================================

        /// <summary>
        /// Cell value listener
        /// </summary>
        public class FieldEventHandler
        {
            public readonly BGId metaId;
            public readonly BGId fieldId;
            public readonly BGId entityId;
            public readonly Action action;

            public FieldEventHandler(BGId metaId, BGId fieldId, BGId entityId, Action action)
            {
                this.metaId = metaId;
                this.fieldId = fieldId;
                this.entityId = entityId;
                this.action = action;
                var meta = BGRepo.I.GetMeta(metaId);
                var field = meta?.GetField(fieldId, false);
                if (field != null) field.ValueChanged += FieldListener;
            }

            public void Release()
            {
                var meta = BGRepo.I.GetMeta(metaId);
                var field = meta?.GetField(fieldId, false);
                if (field != null) field.ValueChanged -= FieldListener;
            }

            private void FieldListener(object sender, BGEventArgsField e)
            {
                if (e.Entity != null && e.Entity.Id != entityId) return;
                action();
            }
        }
    }
}