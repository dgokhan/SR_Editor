/*
<copyright file="BGMetaEntityValidator.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// validator for meta
    /// </summary>
    public partial class BGMetaEntityValidator : BGValidator
    {
        //=================================================================================================================
        //                      Static
        //=================================================================================================================
        private static readonly Dictionary<Type, Type> MetaType2ValidatorType = new Dictionary<Type, Type>();

        /// <summary>
        /// Get validator for provided entity
        /// </summary>
        public static BGMetaEntityValidator GetValidator(BGMetaEntity meta)
        {
            if (MetaType2ValidatorType.Count == 0) FillInValidators<BGMetaEntityValidator>(MetaType2ValidatorType);
            var validator = BGUtil.Get(MetaType2ValidatorType, meta.GetType());
            var metaEntityValidator = validator != null ? BGUtil.Create<BGMetaEntityValidator>(validator, false) : new BGMetaEntityValidator();
            metaEntityValidator.meta = meta;
            return metaEntityValidator;
        }

        //=================================================================================================================
        //                      Not static
        //=================================================================================================================
        private readonly HashSet<string> uniqueNames = new HashSet<string>();
        private readonly HashSet<string> duplicateNames = new HashSet<string>();
        protected BGMetaEntity meta;
        private BGFieldEntityName nameField;

        //=================================================================================================================
        //                      Methods
        //=================================================================================================================
        /// <summary>
        /// check if  meta is valid
        /// </summary>
        public virtual void Start(params BGValidationLog[] logs)
        {
            nameField = meta.NameField;

            //singleton
            if (meta.Singleton && meta.CountEntities > 1) Add(logs, "Meta [$] is a singleton. There are [$] entities exist.", meta.Name, meta.CountEntities);
        }

        /// <summary>
        /// validate single row
        /// </summary>
        public virtual void Validate(BGEntity entity, params BGValidationLog[] logs)
        {
            if (!meta.UniqueName) return;

            var entityName = nameField[entity.Index];

            if (entityName == null || string.IsNullOrEmpty(entityName.Trim())) return;

            if (!uniqueNames.Add(entityName)) duplicateNames.Add(entityName);
            // Add(logs, "duplicate name: $", entityName);
        }

        /// <summary>
        /// Called when all rows are validated using Validate method
        /// </summary>
        public void Finish(params BGValidationLog[] logs)
        {
            if (duplicateNames.Count <= 0) return;

            var result = "";
            foreach (var name in duplicateNames)
            {
                if (result.Length > 0) result += ',';
                result += name;
            }

            Add(logs, "Entity name should be unique for meta [$], but there are following duplicate names: $", meta.Name, result);
        }
    }
}