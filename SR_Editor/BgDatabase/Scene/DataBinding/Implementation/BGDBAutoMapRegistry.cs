/*
<copyright file="BGDBAutoMapRegistry.cs" company="BansheeGz">
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
    /// Helper class for mapping database fields to the C# classes fields
    /// </summary>
    public class BGDBAutoMapRegistry
    {
        public const string EntityName = "entityName";
        private readonly Dictionary<AutoMappedConfigKey, AutoMappedConfig> configs = new Dictionary<AutoMappedConfigKey, AutoMappedConfig>();

        private static BGDBAutoMapRegistry instance;

        /// <summary>
        /// singleton object
        /// </summary>
        public static BGDBAutoMapRegistry Instance => instance ?? (instance = new BGDBAutoMapRegistry());

        private BGDBAutoMapRegistry()
        {
        }

        public void Invalidate() => configs.Clear();
        
        /// <summary>
        /// Provide binding config for given table and type 
        /// </summary>
        public AutoMappedConfig GetAutoMappedConfig(BGMetaEntity meta, Type targetComponentType, bool includeBaseTypes)
        {
            var key = new AutoMappedConfigKey(meta.Id, targetComponentType, includeBaseTypes);
            if (configs.TryGetValue(key, out var config)) return config;

            config = new AutoMappedConfig(meta, targetComponentType, includeBaseTypes);
            configs.Add(key, config);
            return config;
        }

        /// <summary>
        /// data container for table+type 
        /// </summary>
        private struct AutoMappedConfigKey
        {
            private readonly BGId metaId;
            private readonly Type targetType;
            private readonly bool includeBaseTypes;

            public AutoMappedConfigKey(BGId metaId, Type targetType, bool includeBaseTypes)
            {
                this.metaId = metaId;
                this.targetType = targetType;
                this.includeBaseTypes = includeBaseTypes;
            }

            public bool Equals(AutoMappedConfigKey other)
            {
                return metaId.Equals(other.metaId) && Equals(targetType, other.targetType) && includeBaseTypes == other.includeBaseTypes;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                return obj is AutoMappedConfigKey configKey && Equals(configKey);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = metaId.GetHashCode();
                    hashCode = (hashCode * 397) ^ (targetType != null ? targetType.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ includeBaseTypes.GetHashCode();
                    return hashCode;
                }
            }
        }

        /// <summary>
        /// data container for table+type binder config 
        /// </summary>
        public class AutoMappedConfig
        {
            private readonly BGMetaEntity meta;
            private readonly Type targetType;

            private Dictionary<BGId, FieldInfo> fieldId2field;
            private Dictionary<BGId, PropertyInfo> propertyId2field;

            public AutoMappedConfig(BGMetaEntity meta, Type targetType, bool includeBaseTypes)
            {
                this.meta = meta;
                this.targetType = targetType;

                meta.ForEachField(field =>
                {
                    var fieldName = field.Name;
                    if ("name".Equals(fieldName)) fieldName = EntityName;

                    var fieldInfo = BGPrivate.GetField(targetType, fieldName, false, includeBaseTypes);
                    if (fieldInfo != null)
                    {
                        fieldId2field = fieldId2field ?? new Dictionary<BGId, FieldInfo>();
                        fieldId2field.Add(field.Id, fieldInfo);
                    }
                    else
                    {
                        var propertyInfo = BGPrivate.GetProperty(targetType, fieldName, false, includeBaseTypes);
                        if (propertyInfo != null)
                        {
                            propertyId2field = propertyId2field ?? new Dictionary<BGId, PropertyInfo>();
                            propertyId2field.Add(field.Id, propertyInfo);
                        }
                    }
                });
            }

            /// <summary>
            /// The number of mapped fields and properties
            /// </summary>
            public int Count => (fieldId2field?.Count ?? 0) + (propertyId2field?.Count ?? 0);

            /// <summary>
            /// inject the value from database into target component 
            /// </summary>
            public void Bind(BGEntity entity, Object target, bool detailedLog, Action<string> logger)
            {
                string logMessage = null;
                if (detailedLog) logMessage = $"Row [{entity.FullName}] to [{target.name}] object: ";
                BindInternal(entity, logger,
                    (field, info, index) =>
                    {
                        var value = field.GetValue(entity.Index);
                        info.SetValue(target, value);
                        if (detailedLog) logMessage += $"[{field.Name}={value}], ";
                    },
                    (field, info, index) =>
                    {
                        var value = field.GetValue(entity.Index);
                        info.SetValue(target, value, null);
                        if (detailedLog) logMessage += $"[{field.Name}={value}], ";
                    });
                if (detailedLog) logger?.Invoke(logMessage);
            }

            /// <summary>
            /// inject the value from target component into database 
            /// </summary>
            public void ReverseBind(BGEntity entity, Object target, Action<string> logger)
            {
                BindInternal(entity, logger,
                    (field, info, index) => field.SetValue(entity.Index, info.GetValue(target)),
                    (field, info, index) => field.SetValue(entity.Index, info.GetValue(target, null))
                );
            }

            //bind internal
            private void BindInternal(BGEntity entity, Action<string> logger, Action<BGField, FieldInfo, int> action, Action<BGField, PropertyInfo, int> action2)
            {
                if (fieldId2field != null)
                    foreach (var pair in fieldId2field)
                    {
                        var field = entity.Meta.GetField(pair.Key, false);
                        if (field == null) continue;
                        try
                        {
                            action(field, pair.Value, entity.Index);
                        }
                        catch (Exception e)
                        {
                            logger?.Invoke("DataBinder error: details are " +
                                           "[From Meta=" + entity.Meta.Name + ", field=" + field.Name + ", entity=" + entity.Name + "] " +
                                           "[To Class=" + targetType.Name + "] " +
                                           " Original error is: " + e.Message);
                        }
                    }

                if (propertyId2field != null)
                    foreach (var pair in propertyId2field)
                    {
                        var field = entity.Meta.GetField(pair.Key, false);
                        if (field == null) continue;
                        try
                        {
                            action2(field, pair.Value, entity.Index);
                        }
                        catch (Exception e)
                        {
                            logger?.Invoke("DataBinder error: details are " +
                                           "[From Meta=" + entity.Meta.Name + ", field=" + field.Name + ", entity=" + entity.Name + "] " +
                                           "[To Class=" + targetType.Name + "] " +
                                           " Original error is: " + e.Message);
                        }
                    }
            }

            /// <summary>
            /// call the action for each mapped field 
            /// </summary>
            public void ForEachField(Action<BGId, FieldInfo> action)
            {
                if (fieldId2field == null) return;
                foreach (var pair in fieldId2field) action(pair.Key, pair.Value);
            }

            /// <summary>
            /// call the action for each mapped property 
            /// </summary>
            public void ForEachProperty(Action<BGId, PropertyInfo> action)
            {
                if (propertyId2field == null) return;
                foreach (var pair in propertyId2field) action(pair.Key, pair.Value);
            }
        }
    }
}