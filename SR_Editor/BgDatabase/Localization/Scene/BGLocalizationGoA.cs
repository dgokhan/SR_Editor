/*
<copyright file="BGLocalizationGoA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    public abstract partial class BGLocalizationGoA : BGEntityGo, BGAddonLocalization.LocaleChangeReceiver
    {
        public enum LocalizationModeEnum
        {
            Awake,
            Start,
            None
        }

        [SerializeField] [HideInInspector] private Component targetComponent;
        [SerializeField] [HideInInspector] private string targetField;
        [SerializeField] [HideInInspector] private bool isTargetProperty;
        [SerializeField] [HideInInspector] private List<LocalizationPath> path;
        [SerializeField] [HideInInspector] private bool isArray;
        [SerializeField] [HideInInspector] private int arrayIndex;
        [SerializeField] [HideInInspector] private bool includePrivate;

        [SerializeField] private LocalizationModeEnum localizationMode;

        public Component TargetComponent => targetComponent;

        public string TargetField => targetField;

        public bool IsTargetProperty => isTargetProperty;

        public List<LocalizationPath> Path
        {
            get => path;
            set => path = value;
        }

        public bool IsArray => isArray;

        public int ArrayIndex => arrayIndex;

        public LocalizationModeEnum LocalizationMode => localizationMode;

        public bool IncludePrivate => includePrivate;

        public override void Awake()
        {
            base.Awake();
            if (localizationMode != LocalizationModeEnum.Awake) return;
            Localize();
        }

        public override void Start()
        {
            base.Start();
            if (localizationMode != LocalizationModeEnum.Start) return;
            Localize();
        }

        public override void EntityChanged()
        {
            base.EntityChanged();
            Localize();
        }

        public void LocaleChanged()
        {
            Localize();
        }

        public void Localize()
        {
            if (targetComponent == null || string.IsNullOrEmpty(targetField)) return;
            if (!Application.isPlaying) return;

            var entity = Entity;
            if (!(entity?.Meta is BGMetaLocalizationA)) return;

            var addon = entity.Repo.Addons.Get<BGAddonLocalization>();
            if (addon == null) return;

            var currentLocale = addon.CurrentLocale;
            if (string.IsNullOrEmpty(currentLocale)) return;

            var field = entity.Meta.GetField(currentLocale);
            if (field == null) return;

            var value = field.GetValue(entity.Id);
            var target = targetComponent as object;

            if (path != null && path.Count > 0)
                for (var i = 0; i < path.Count; i++)
                {
                    var localizationPath = path[i];
                    if (localizationPath.IsProperty)
                    {
                        var typeProperty = target.GetType().GetProperty(localizationPath.Field, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                        if (typeProperty == null) return;
                        target = typeProperty.GetValue(target, null);
                        if (target == null) return;
                    }
                    else
                    {
                        var typeField = target.GetType().GetField(localizationPath.Field, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                        if (typeField == null) return;
                        target = typeField.GetValue(target);
                        if (target == null) return;
                    }
                }

            if (isTargetProperty)
            {
                var typeProperty = target.GetType().GetProperty(targetField, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (typeProperty == null) return;
                if (isArray) SetArrayElement(typeProperty.GetValue(target, null), value, array => typeProperty.SetValue(target, array, null));
                else typeProperty.SetValue(target, value, null);
            }
            else
            {
                var typeField = target.GetType().GetField(targetField, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (typeField == null) return;
                if (isArray) SetArrayElement(typeField.GetValue(target), value, array => typeField.SetValue(target, array));
                else typeField.SetValue(target, value);
            }
        }

        private void SetArrayElement(object arrayObj, object value, Action<Array> setter)
        {
            if (!(arrayObj is Array array)) return;
            if (array.Length - 1 < arrayIndex) return;
            array.SetValue(value, arrayIndex);
            setter(array);
        }

        public void SetTarget(Component targetComponent, string targetField, bool isTargetProperty, List<LocalizationPath> path, bool isArray, int arrayIndex)
        {
            this.targetComponent = targetComponent;
            this.targetField = targetField;
            this.isTargetProperty = isTargetProperty;
            this.path = path;
            this.isArray = isArray;
            this.arrayIndex = arrayIndex;
            Localize();
        }

        public bool IsTarget(Component targetComponent, string targetField, List<LocalizationPath> path)
        {
            if (!Equals(TargetComponent, targetComponent) || !string.Equals(TargetField, targetField)) return false;

            var myPathEmpty = Path == null || Path.Count == 0;
            var otherPathEmpty = path == null || path.Count == 0;

            if (myPathEmpty && otherPathEmpty) return true;
            if (myPathEmpty || otherPathEmpty) return false;
            if (Path.Count != path.Count) return false;

            for (var i = 0; i < Path.Count; i++)
            {
                var myPath = Path[i];
                var otherPath = path[i];
                if (!Equals(myPath, otherPath)) return true;
            }

            return true;
        }

        [Serializable]
        public class LocalizationPath
        {
            public string Field;
            public bool IsProperty;

            protected bool Equals(LocalizationPath other)
            {
                return string.Equals(Field, other.Field) && IsProperty == other.IsProperty;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;
                return Equals((LocalizationPath)obj);
            }

            public override int GetHashCode()
            {
                unchecked { return ((Field != null ? Field.GetHashCode() : 0) * 397) ^ IsProperty.GetHashCode(); }
            }

            public override string ToString()
            {
                return Field;
            }
        }
    }
}