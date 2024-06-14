/*
<copyright file="BGLoaderForRepoCustom.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.IO;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// database loader, used for custom location database
    /// </summary>
    public class BGLoaderForRepoCustom : BGLoaderForRepo
    {
        public const string LoaderName = "Custom";

        /// <summary>
        /// Unity meta ID for custom database 
        /// </summary>
        public const string CustomDatabaseGuid = "3637ea689da0cff4b8d5c0fb5d609c15";

        /// <inheritdoc/>
        public override string Name => LoaderName;

        /// <inheritdoc/>
        public override byte[] Load(LoadRequest request)
        {
            if (BGRepo.DefaultRepoCustomLoaderModel == null) return null;

            byte[] content = null;
            if (IsEmpty(request))
            {
                var databaseResource = request.databaseResource;
                if (databaseResource != null)
                {
                    BGRepo.DefaultRepoAssetId = databaseResource.AssetId > 0 ? databaseResource.AssetId : 0;
                    BGRepo.DefaultRepoAssetPath = (!string.IsNullOrEmpty(databaseResource.AssetPath) ? databaseResource.AssetPath : null) ?? BGRepoCustomLoaderModel.DatabaseKey;
                }
                else
                {
                    BGRepo.DefaultRepoAssetId = 0;
                    BGRepo.DefaultRepoAssetPath = null;
                }

                content = BGRepo.DefaultRepoCustomLoaderModel.MainDatabaseResource.Content;
            }
            else
            {
                var key = ToPath(request);
                var databaseResource = BGRepo.DefaultRepoCustomLoaderModel.Get(key);
                content = databaseResource?.Content;
            }

            return content;
        }

        /// <inheritdoc/>
        protected override string ToPath(LoadRequest request)
        {
            var filePathNoFolder = Path.GetFileName(request.basePath);
            var baseKeyNoExt = Path.ChangeExtension(filePathNoFolder, null);
            return AppendPaths(baseKeyNoExt, request.paths);
        }
    }
}