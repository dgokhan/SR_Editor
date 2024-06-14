/*
<copyright file="BGRepoCustomLoaderModel.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Data container for all database content
    /// </summary>
    public class BGRepoCustomLoaderModel
    {
        public const string DatabaseKey = "bansheegz_database";

        private Dictionary<string, DatabaseResource> key2Resource;

        private DatabaseResource mainDatabaseResource;

        public DatabaseResource MainDatabaseResource => mainDatabaseResource;

        public BGRepoCustomLoaderModel(DatabaseResource mainDatabaseResource)
        {
            this.mainDatabaseResource = mainDatabaseResource ?? throw new BGException("mainDatabaseResource can not be null");
        }

        /// <summary>
        /// get database resource by it's key (name) 
        /// </summary>
        public DatabaseResource Get(string key)
        {
            if (key2Resource == null) return null;
            if (!key2Resource.TryGetValue(key, out var resource)) return null;
            return resource;
        }

        /// <summary>
        /// add database resource by it's key (name) 
        /// </summary>
        public void Add(string key, DatabaseResource resource)
        {
            if (key2Resource == null) key2Resource = new Dictionary<string, DatabaseResource>();
            key2Resource[key] = resource;
        }

        /// <summary>
        /// remove database resource using it's key (name) 
        /// </summary>
        public bool Remove(string key)
        {
            if (key2Resource == null) return false;
            return key2Resource.Remove(key);
        }

        /// <summary>
        /// iterate all database resources 
        /// </summary>
        public void ForEachKey(Action<string, DatabaseResource> action)
        {
            if (key2Resource == null || action == null) return;
            foreach (var pair in key2Resource) action(pair.Key, pair.Value);
        }

        /*
        public static string GetLocaleKey(string locale)
        {
            return DatabaseKey + "_locale_" + locale;
        }
        */

        /// <summary>
        /// is provided key  the main database key? 
        /// </summary>
        public static bool IsDatabaseKey(string key) => string.Equals(key, DatabaseKey);

        /// <summary>
        /// Data container for single database resource
        /// </summary>
        public class DatabaseResource
        {
            private string assetPath;
            private int assetId;
            private byte[] content;

            public virtual string AssetPath
            {
                get => assetPath;
                set => assetPath = value;
            }

            public virtual int AssetId
            {
                get => assetId;
                set => assetId = value;
            }

            public virtual byte[] Content
            {
                get => content;
                set => content = value;
            }

            public DatabaseResource()
            {
            }

            public DatabaseResource(byte[] content) => this.content = content;

            public DatabaseResource(string assetPath, int assetId, byte[] content)
            {
                this.assetPath = assetPath;
                this.assetId = assetId;
                this.content = content;
            }
        }
    }
}