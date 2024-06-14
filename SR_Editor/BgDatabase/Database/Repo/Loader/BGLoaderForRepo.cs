/*
<copyright file="BGLoaderForRepo.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Abstract loader for database
    /// </summary>
    public abstract class BGLoaderForRepo
    {
        /// <summary>
        /// Loader name
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Load database
        /// </summary>
        public abstract byte[] Load(LoadRequest request);
        // public abstract string GetPath(string basePath, params string[] paths);

        protected static string AppendPaths(string basePath, params string[] paths)
        {
            if (paths == null) return basePath;

            for (var i = 0; i < paths.Length; i++)
            {
                basePath += '_';
                basePath += paths[i];
            }

            return basePath;
        }

        protected bool IsEmpty(LoadRequest request) => request == null || string.IsNullOrEmpty(request.basePath);

        /// <summary>
        /// Convert load request to the path 
        /// </summary>
        protected abstract string ToPath(LoadRequest request);

        /// <summary>
        /// Data container for database load request
        /// </summary>
        public class LoadRequest
        {
            public readonly BGRepoCustomLoaderModel.DatabaseResource databaseResource;
            public readonly string basePath;
            public readonly string[] paths;

            public LoadRequest(BGRepoCustomLoaderModel.DatabaseResource databaseResource) => this.databaseResource = databaseResource;

            public LoadRequest(string basePath) => this.basePath = basePath;

            public LoadRequest(string basePath, params string[] paths) : this(basePath) => this.paths = paths;

            public string ToPath(BGLoaderForRepo loader) => loader.ToPath(this);
        }
    }
}