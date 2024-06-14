/*
<copyright file="BGJson.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.IO;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// class for dealing with JSON import/export 
    /// </summary>
    public static partial class BGJson
    {
        /// <summary>
        /// Export database to the JSON file 
        /// </summary>
        public static void Export(BGRepo repo, string path, bool skipData = false, bool removeSensitive = false, BGJsonFormatEnum format = BGJsonFormatEnum.Classic) => 
            File.WriteAllText(path, ExportToString(repo, skipData, removeSensitive, format));

        /// <summary>
        /// Export database to the JSON string 
        /// </summary>
        public static string ExportToString(BGRepo repo, bool skipData = false, bool removeSensitive = false, BGJsonFormatEnum format = BGJsonFormatEnum.Classic) 
        {
            switch (format)
            {
                case BGJsonFormatEnum.Classic:
                    return new BGJsonWriter().Write(repo, skipData, removeSensitive ? RemoveSensitive : (Action<BGJsonRepoModel>)null);
                case BGJsonFormatEnum.CompactRowBased:
                    return new BGJsonCompactWriter().Write(repo, new BGJsonCompactRowBased(), skipData, removeSensitive);
                case BGJsonFormatEnum.CompactFieldBased:
                    return new BGJsonCompactWriter().Write(repo, new BGJsonCompactFieldBased(), skipData, removeSensitive );
                default:
                    throw new ArgumentOutOfRangeException(nameof(format), format, null);
            }
        }

        /// <summary>
        /// Import database from JSON file 
        /// </summary>
        public static void Import(BGLogger logger, BGRepo targetRepo, string path, bool skipData = false, BGJsonFormatEnum format = BGJsonFormatEnum.Classic)
        {
            BGSyncUtil.ReadFile(logger, path, content => 
                ImportFromString(logger, targetRepo, System.Text.Encoding.UTF8.GetString(content), skipData, format));
        }

        /// <summary>
        /// Import database from JSON string 
        /// </summary>
        public static void ImportFromString(BGLogger logger, BGRepo targetRepo, string jsonContent, bool skipData = false, BGJsonFormatEnum format = BGJsonFormatEnum.Classic)
        {
            targetRepo.Transaction(() =>
            {
                targetRepo.Addons.Clear();

                BGRepo repo;
                switch (format)
                {
                    case BGJsonFormatEnum.Classic:
                        repo = new BGJsonReader(jsonContent, skipData).Repo;
                        break;
                    case BGJsonFormatEnum.CompactRowBased:
                        repo = new BGJsonCompactReader(jsonContent,new BGJsonCompactRowBased(), skipData ).Repo;
                        break;
                    case BGJsonFormatEnum.CompactFieldBased:
                        repo = new BGJsonCompactReader(jsonContent, new BGJsonCompactFieldBased(), skipData).Repo;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(format), format, null);
                }

                targetRepo.Addons.AddFrom(repo.Addons);
                targetRepo.Merge(repo);
            });
        }

        //remove sensitive information
        private static void RemoveSensitive(BGJsonRepoModel model)
        {
            if (model?.Addons == null) return;
            if (model.Addons.Count == 0) return;

            model.Addons.ForEach(addon =>
            {
                return;
                addon.Config = "{\"content\" : \"[sensitive]\"}";
            });
        }
    }
}