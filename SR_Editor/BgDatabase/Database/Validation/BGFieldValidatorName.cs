/*
<copyright file="BGFieldValidatorName.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// validator for name field
    /// </summary>
    [ValidatorAttribute(Target = typeof(BGFieldEntityName))]
    public class BGFieldValidatorName : BGFieldValidator
    {
        public override void Validate(BGEntity entity, Func<BGValidationLog[]> logsProvider)
        {
            base.Validate(entity, logsProvider);

            if (!entity.Meta.EmptyName) return;
            
            var val = ((BGFieldEntityName)field)[entity.Index];
            if (!string.IsNullOrEmpty(val)) Add(logsProvider(), "Meta [$] is marked to have empty entity name, however entity #$ [$] has name value", entity.Meta.Name, entity.Index, entity.Name);
        }
    }
}