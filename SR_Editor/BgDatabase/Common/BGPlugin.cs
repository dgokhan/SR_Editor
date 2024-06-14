/*
<copyright file="BGPlugin.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Basic interface for a plugin
    /// </summary>
    public interface BGPlugin
    {
        /// <summary>
        /// plugin current version
        /// </summary>
        string Version { get; }
    }

    /// <summary>
    /// Attribute to be used with plugin implementation class
    /// </summary>
    public class BGPluginAttribute : Attribute
    {
        /// <summary>
        /// plugin current version
        /// </summary>
        public string Version { get; set; }
    }
}