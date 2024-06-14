/*
<copyright file="BGDsFileBased.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.IO;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// File-based data source
    /// </summary>
    public abstract partial class BGDsFileBased : BGDataSource
    {
        /// <summary>
        /// file path
        /// </summary>
        public string Path;

        /// <inheritdoc/>
        public override string Error
        {
            get
            {
                if (string.IsNullOrEmpty(Path)) return "No path defined";
                if (!Directory.Exists(System.IO.Path.GetDirectoryName(RealPath))) return "Folder for the file does not exist: " + RealPath;

                return null;
            }
        }

        /// <summary>
        /// final absolute file path
        /// </summary>
        public string RealPath
        {
            get
            {
                var path = Path;
                if (string.IsNullOrEmpty(path)) return path;
                var isPathAbsolute = System.IO.Path.IsPathRooted(path);
                //if (!isPathAbsolute) path = System.IO.Path.Combine(Application.dataPath, path);
                return path;
            }
        }

        /// <inheritdoc/>
        public override string ConfigToString()
        {
            return JsonUtility.ToJson(new Settings
            {
                Path = Path,
                ActionsType = ActionsType,
            });
        }

        /// <inheritdoc/>
        public override void ConfigFromString(string config)
        {
            var settings = JsonUtility.FromJson<Settings>(config);
            Path = settings.Path;
            ActionsType = settings.ActionsType;
        }

        [Serializable]
        protected class Settings
        {
            public ActionsTypeEnum ActionsType;
            public string Path;
        }
    }
}