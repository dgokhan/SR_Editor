/*
<copyright file="BGAttribute.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Basic attribute with name 
    /// </summary>
    public partial class BGAttribute : Attribute
    {
        public string Name { get; set; }

        /// <summary>
        /// Get name by type 
        /// </summary>
        public static string GetName(Type fieldType)
        {
            var descriptor = BGUtil.GetAttribute<BGAttribute>(fieldType, true);
            return descriptor != null ? descriptor.Name : fieldType.FullName;
        }
    }
}