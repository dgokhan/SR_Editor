/*
<copyright file="BGCalcUnitLocationAttribute.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// unit location attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class BGCalcUnitDefinitionAttribute : Attribute
    {
        public BGCalcUnitDefinitionAttribute(string name) : this(name, false)
        {
        }

        public BGCalcUnitDefinitionAttribute(string name, bool hidden)
        {
            this.name = name;
            this.hidden = hidden;
        }

        /// <summary>
        /// node path in units browser
        /// </summary>
        public string name { get; }
        
        /// <summary>
        /// should node be hidden in units browser
        /// </summary>
        public bool hidden { get; }
    }
}