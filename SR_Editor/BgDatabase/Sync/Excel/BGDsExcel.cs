/*
<copyright file="BGDsExcel.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

//DO NOT CHANGE NAMESPACE! we are stuck here cause class is stored inside settings

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase.Editor
{
    /// <summary>
    /// Data source for Excel file
    /// </summary>
    [Descriptor(Name = "Excel")]
    public partial class BGDsExcel : BGDsFileBased, BGSyncNameMapConfig.BGNameConfigOwner
    {
        public const string ImplementationType = "BansheeGz.BGDatabase.Editor.BGExcelService";
        public const string PluginPage = "https://www.bansheegz.com/BGDatabase/Downloads/EditorExcel";

        public BGSyncNameMapConfig NameMapConfig { get; set; }
        public bool NameMapConfigEnabled { get; set; }
        public BGSyncIdConfig IdConfig { get; set; }
        public bool IdConfigEnabled { get; set; }
        public BGSyncRelationsConfig RelationsConfig { get; set; }
        public bool RelationsConfigEnabled { get; set; }
        public BGSyncDisabledConfig DisabledConfig { get; set; }

        /// <summary>
        /// Initialize Excel delegate implementation if it's available
        /// </summary>
        public static BGExcelServiceI Service
        {
            get
            {
                var type = BGUtil.GetType(ImplementationType);
                if (type == null) return null;
                try
                {
                    var service = Activator.CreateInstance(type) as BGExcelServiceI;
                    return service;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        /// <inheritdoc/>
        public override string ConfigToString()
        {
            if (NameMapConfigEnabled && NameMapConfig != null) NameMapConfig.Trim();
            return JsonUtility.ToJson(new Settings
            {
                Path = Path,
                NameMapConfig = NameMapConfig,
                NameMapConfigEnabled = NameMapConfigEnabled,
                IdConfig = IdConfigEnabled ? IdConfig : null,
                IdConfigEnabled = IdConfigEnabled,
                RelationsConfig = RelationsConfigEnabled ? RelationsConfig : null,
                RelationsConfigEnabled = RelationsConfigEnabled,
                DisabledConfig = DisabledConfig,
                ActionsType = ActionsType
            });
        }

        /// <inheritdoc/>
        public override void ConfigFromString(string config)
        {
            var settings = JsonUtility.FromJson<Settings>(config);
            Path = settings.Path;
            NameMapConfig = settings.NameMapConfig;
            NameMapConfigEnabled = settings.NameMapConfigEnabled;
            IdConfig = settings.IdConfig;
            IdConfigEnabled = settings.IdConfigEnabled;
            RelationsConfig = settings.RelationsConfig;
            RelationsConfigEnabled = settings.RelationsConfigEnabled;
            DisabledConfig = settings.DisabledConfig;
            ActionsType = settings.ActionsType;
        }

        /// <summary>
        /// if the Excel file is xml based 
        /// </summary>
        public static bool UseXml(string path)
        {
            if (string.IsNullOrEmpty(path)) return false;
            var extension = System.IO.Path.GetExtension(path);
            return ".xlsx".Equals(extension) || "xlsx".Equals(extension);
        }

        [Serializable]
        protected class Settings : BGDsFileBased.Settings
        {
            public ActionsTypeEnum ActionsType;
            public BGSyncNameMapConfig NameMapConfig;
            public bool NameMapConfigEnabled;
            public bool IdConfigEnabled;
            public bool RelationsConfigEnabled;
            public BGSyncIdConfig IdConfig;
            public BGSyncDisabledConfig DisabledConfig;
            public BGSyncRelationsConfig RelationsConfig;
        }
    }
}