/*
<copyright file="BGSyncDuplicateEntitiesMonitor.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Detects entities with duplicate ID field value
    /// </summary>
    public class BGSyncDuplicateEntitiesMonitor
    {
        private readonly BGField field;
        private readonly HashSet<object> processedValues = new HashSet<object>();
        private readonly bool isString;

        private BGSyncDuplicateEntitiesMonitor(BGField field)
        {
            this.field = field;
            isString = field is BGFieldString;
        }

        public static BGSyncDuplicateEntitiesMonitor Get(BGSyncIdConfig idConfig, BGMetaEntity meta)
        {
            var metaConfig = idConfig?.GetMetaConfig(meta.Id);
            if (metaConfig == null) return null;
            if (metaConfig.configType != BGSyncIdConfig.IdConfigEnum.Field) return null;
            var field = meta.GetField(metaConfig.FieldId, false);
            if (field == null) return null;
            return new BGSyncDuplicateEntitiesMonitor(field);
        }

        public void Add(BGEntity entity)
        {
            var value = field.GetValue(entity.Index);
            if (value != null) processedValues.Add(value);
        }
        
        /// <summary>
        /// check if id value is set and unique 
        /// </summary>
        public bool Process(BGEntity entity, BGLogger logger, bool printWarnings)
        {
            if (entity == null)
            {
                BGSyncUtil.AppendWarning(logger, printWarnings, "Duplicate monitor: Entity is $!", "null");
                return false;
            }
            var value = field.GetValue(entity.Index);
            if (value == null || (isString && (string)value == string.Empty))
            {
                BGSyncUtil.AppendWarning(logger, printWarnings, "Row # $, of meta=$ is skipped while exporting, cause ID value is not set!", entity.Index, field.Meta.Name);
                return false;
            }

            if (processedValues.Add(value)) return true;

            BGSyncUtil.AppendWarning(logger, printWarnings, "Row # $, of meta=$ is skipped while exporting, cause ID value=$ is a duplicate!", entity.Index, field.Meta.Name, value);
            return false;
        }
    }
}