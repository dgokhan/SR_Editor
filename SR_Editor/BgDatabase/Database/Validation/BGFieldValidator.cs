/*
<copyright file="BGFieldValidator.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// validator for field
    /// </summary>
    public partial class BGFieldValidator : BGValidator
    {
        //=================================================================================================================
        //                      Static
        //=================================================================================================================
        private static readonly Dictionary<Type, Type> Type2Validator = new Dictionary<Type, Type>();

        /// <summary>
        /// construct validator for provided field 
        /// </summary>
        public static BGFieldValidator GetValidator(BGField field)
        {
            if (Type2Validator.Count == 0) FillInValidators<BGFieldValidator>(Type2Validator);
            var customValidator = BGUtil.Get(Type2Validator, field.GetType());
            BGFieldValidator fieldValidator = null;
            if (customValidator != null) fieldValidator = BGUtil.Create<BGFieldValidator>(customValidator, false);
            else if (field is BGFieldUnityAssetI)
            {
                var assetValidatorType = BGUtil.GetType("BansheeGz.BGDatabase.Editor.BGFieldValidatorAsset");
                if (assetValidatorType != null) fieldValidator = BGUtil.Create<BGFieldValidator>(assetValidatorType, false);
            }

            if (fieldValidator == null) fieldValidator = new BGFieldValidator();
            fieldValidator.field = field;
            return fieldValidator;
        }

        //=================================================================================================================
        //                      Not static
        //=================================================================================================================
        protected virtual BGField field { get; set; }

        //=================================================================================================================
        //                      Methods
        //=================================================================================================================

        /// <summary>
        ///if field data for entity is valid 
        /// </summary>
        public virtual void Validate(BGEntity entity, Func<BGValidationLog[]> logsProvider)
        {
            if (!field.Required) return;
            if (field.GetValue(entity.Id) != null) return;

            Add(logsProvider(), "Field [$] is required, but has no value at entity #$ [$]", field.FullName, entity.Index, entity.FullName);
        }
    }
}