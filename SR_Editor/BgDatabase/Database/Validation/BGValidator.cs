/*
<copyright file="BGValidator.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// abstract validator
    /// </summary>
    public partial class BGValidator
    {
        //=================================================================================================================
        //                      Static
        //=================================================================================================================
        //fill in all validator types
        protected static void FillInValidators<T>(Dictionary<Type, Type> type2Validator) where T : BGValidator
        {
            var allSubTypes = BGUtil.GetAllSubTypes(typeof(T));
            foreach (var type in allSubTypes)
            {
                var attribute = BGUtil.GetAttribute<ValidatorAttribute>(type);
                if (attribute == null || attribute.Target == null) continue;
                type2Validator.Add(attribute.Target, type);
            }
        }

        //add single message to the log
        protected static void Add(BGValidationLog[] logs, string message, params object[] parameters)
        {
            if (logs == null || logs.Length == 0 || string.IsNullOrEmpty(message)) return;
            var formattedString = BGUtil.Format(message, parameters);
            for (var i = 0; i < logs.Length; i++) logs[i].Add(formattedString);
        }

        //=================================================================================================================
        //                      Attribute
        //=================================================================================================================
        /// <summary>
        /// attribute to mark validator for specific type
        /// </summary>
        public class ValidatorAttribute : Attribute
        {
            public Type Target;
        }
    }
}