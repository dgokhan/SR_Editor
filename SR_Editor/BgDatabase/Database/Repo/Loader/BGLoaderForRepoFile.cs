/*
<copyright file="BGLoaderForRepoFile.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System.IO;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// database loader, used for a file
    /// </summary>
    public class BGLoaderForRepoFile : BGLoaderForRepo
    {
        private readonly string filePath;

        /// <inheritdoc/>
        public override string Name => "File";

        /// <summary>
        /// Used file path
        /// </summary>
        public string FilePath => filePath;

        public BGLoaderForRepoFile(string filePath) => this.filePath = filePath;

        /// <inheritdoc/>
        public override byte[] Load(LoadRequest request)
        {
            byte[] content = null;
            if (IsEmpty(request)) content = Load(filePath);
            else
            {
                var path = ToPath(request);
                content = Load(path);
            }

            return content;
        }

        /// <inheritdoc/>
        protected override string ToPath(LoadRequest request)
        {
            var requestBasePath = filePath ?? request.basePath;
            var baseKeyNoExt = Path.ChangeExtension(requestBasePath, null);
            var fullKeyNoExt = AppendPaths(baseKeyNoExt, request.paths);
            var path = Path.ChangeExtension(fullKeyNoExt, "bytes");
            return path;
        }

        /// <summary>
        /// Load file content
        /// </summary>
        public byte[] Load(string path)
        {
            if (!File.Exists(path)) return null;
            return File.ReadAllBytes(path);
        }

        /// <summary>
        /// Load database from provided file 
        /// </summary>
        public BGRepo Load()
        {
            var content = Load((LoadRequest)null);
            if (content == null) return null;
            var repo = new BGRepo();
            repo.Load(content);
            repo.RepoLoader = this;
            return repo;
        }

        /*
        public override string GetPath(string basePath, params string[] paths)
        {
            var result = Path.ChangeExtension(basePath, null);
            result = AppendPaths(result, paths);
            return Path.ChangeExtension(result, "bytes");
        }
    */
    }
}