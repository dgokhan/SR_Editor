/*
<copyright file="BGDsJson.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

//DO NOT CHANGE NAMESPACE! we are stuck here cause class is stored inside settings

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase.Editor
{
    /// <summary>
    /// Data source for JSON format
    /// </summary>
    [Descriptor(Name = "Json", SupportSettings = false)]
    public partial class BGDsJson : BGDsFileBased
    {
        /// <inheritdoc/>
        public override bool RequireMergeSettings => false;

        public BGJsonFormatEnum Format;
        
        /// <inheritdoc/>
        public override string ConfigToString()
        {
            return JsonUtility.ToJson(new Settings
            {
                Path = Path,
                ActionsType = ActionsType,
                Format = Format
            });
        }

        /// <inheritdoc/>
        public override void ConfigFromString(string config)
        {
            var settings = JsonUtility.FromJson<Settings>(config);
            Path = settings.Path;
            ActionsType = settings.ActionsType;
            Format = settings.Format;
        }

        
        [Serializable]
        protected class Settings : BGDsFileBased.Settings
        {
            public BGJsonFormatEnum Format;
        }
    }
}