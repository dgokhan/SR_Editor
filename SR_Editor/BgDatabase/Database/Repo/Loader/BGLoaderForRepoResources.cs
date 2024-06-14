/*
<copyright file="BGLoaderForRepoResources.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Loader for loading database as Unity Text resource from Resources folder
    /// </summary>
    public class BGLoaderForRepoResources : BGLoaderForRepo
    {
        public const string LoaderName = "Resources";

        //all possible database locations
        private static string[] pathes =
        {
            "bansheegz_database",
            "bansheegz_database_default5",
            "bansheegz_database_default4",
            "bansheegz_database_default3",
            "bansheegz_database_default2",
            "bansheegz_database_default1",
            "bansheegz_database_default"
        };

        public static string[] Pathes => pathes;

        /// <inheritdoc/>
        public override string Name => LoaderName;

        /// <inheritdoc/>
        public override byte[] Load(LoadRequest request)
        {
            TextAsset data = null;
            if (IsEmpty(request))
                //main database loading
                for (var i = 0; i < pathes.Length; i++)
                {
                    var path = pathes[i];
                    data = Load(path);
                    if (data == null) continue;
                    BGRepo.DefaultRepoAssetId = data.GetInstanceID();
                    BGRepo.DefaultRepoAssetPath = path;
                    break;
                }
            else
            {
                var path = ToPath(request);
                var textAssets = Resources.LoadAll<TextAsset>(path);
                byte[] content = null;
                if (textAssets != null && textAssets.Length > 0)
                {
                    if (textAssets.Length > 1)
                        throw new BGException("You have more than one file with name $ under Resources folder! " +
                                              "Please, backup all these files and leave the only right one under Resources folder!", path);
                    data = textAssets[0];
                }
            }

            return data == null ? null : data.bytes;
        }

        /// <inheritdoc/>
        protected override string ToPath(LoadRequest request)
        {
            if (IsEmpty(request)) return pathes[0];
            return AppendPaths(request.basePath, request.paths);
        }

        /*
        public override string GetPath(string basePath, params string[] paths)
        {
            if (paths == null) return basePath;

            var result = basePath;
            result = AppendPaths(result, paths);
            return result;
        }
        */

        /// <summary>
        /// load text asset from resources folder 
        /// </summary>
        public TextAsset Load(string path) => Resources.Load<TextAsset>(path);
    }
}