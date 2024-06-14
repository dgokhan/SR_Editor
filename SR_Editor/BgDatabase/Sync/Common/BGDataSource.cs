/*
<copyright file="BGDataSource.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// abstract data source
    /// </summary>
    public abstract partial class BGDataSource : BGConfigurableI
    {
        public enum ActionsTypeEnum : byte
        {
            All,
            ImportOnly,
            ExportOnly,
            NoActions
        }
    
        private static readonly List<Type> dataSources = new List<Type>();

        private ActionsTypeEnum actionsType;

        public ActionsTypeEnum ActionsType
        {
            get => actionsType;
            set => actionsType = value;
        }

        /// <summary>
        /// All types, derived from abstract data source
        /// </summary>
        public static List<Type> DataSources
        {
            get
            {
                if (dataSources.Count == 0) dataSources.AddRange(BGUtil.GetAllSubTypes(typeof(BGDataSource)));
                return dataSources;
            }
        }

        public virtual bool IsExportAllowed => actionsType == ActionsTypeEnum.All || actionsType == ActionsTypeEnum.ExportOnly; 
        public virtual bool IsImportAllowed => actionsType == ActionsTypeEnum.All || actionsType == ActionsTypeEnum.ImportOnly; 
        
        /// <summary>
        /// Does this data source have any configuration error?
        /// </summary>
        public abstract string Error { get; }

        /// <summary>
        /// Does merge settings are used for this data source type?
        /// </summary>
        public virtual bool RequireMergeSettings => true;

        /// <summary>
        /// Convert data source parameters to a string
        /// </summary>
        public abstract string ConfigToString();

        /// <summary>
        /// Restore data source parameters from a string
        /// </summary>
        public abstract void ConfigFromString(string config);

        /// <summary>
        /// attribute, indicating data source's merge settings support
        /// </summary>
        public class Descriptor : BGAttribute
        {
            public bool SupportSettings = true;
        }
    }
}