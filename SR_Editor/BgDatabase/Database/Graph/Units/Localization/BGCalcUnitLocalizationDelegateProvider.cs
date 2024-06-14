/*
<copyright file="BGCalcUnitLocalizationDelegateProvider.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// delegate provider factory for localization-based units
    /// </summary>
    public static class BGCalcUnitLocalizationDelegateProvider
    {
        private const string DelegateClass = "BansheeGz.BGDatabase.BGCalcUnitLocalizationDelegate";

        private static BGCalcUnitLocalizationI delegateObject;

        /// <summary>
        /// Get localization delegate implementation if it's available
        /// </summary>
        public static BGCalcUnitLocalizationI Delegate
        {
            get
            {
                if (delegateObject != null) return delegateObject;
                var type = BGUtil.GetType(DelegateClass);
                if (type == null) throw new Exception("Can not find localization delegate class " + DelegateClass);
                delegateObject = Activator.CreateInstance(type) as BGCalcUnitLocalizationI;
                if (delegateObject == null) throw new Exception("Can not create localization delegate - object is null ");
                return delegateObject;
            }
        }
    }
}