/*
<copyright file="BGCalcUnitLocalizationI.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// delegate provider for localization-based units
    /// </summary>
    public interface BGCalcUnitLocalizationI
    {
        /// <summary>
        /// get current locale
        /// </summary>
        string CurrentLocale { get; set; }
        
        /// <summary>
        /// get value type for localization meta
        /// </summary>
        Type GetValueType(BGMetaEntity meta);
        
        /// <summary>
        /// get localized value for provided entity
        /// </summary>
        object GetValue(BGMetaEntity meta, BGEntity entity);
    }
}