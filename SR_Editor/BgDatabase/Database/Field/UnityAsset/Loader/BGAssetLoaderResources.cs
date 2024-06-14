/*
<copyright file="BGAssetLoaderResources.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;
// using Object = UnityEngine.Object;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// loader for loading assets from Resources folder
    /// </summary>
    [AssetLoaderDescriptor(Name = "Resources", ManagerType = "BansheeGz.BGDatabase.Editor.BGAssetLoaderManagerResources")]
    public partial class BGAssetLoaderResources : BGAssetLoaderA
    {
        /// <inheritdoc />
        public override string Name => "Resources";

        /// <inheritdoc />
        public override T Load<T>(string path)
        {
            if (string.IsNullOrEmpty(path)) return null;
            return Resources.Load<T>(path);
        }

        /// <inheritdoc />
        public override T[] LoadAll<T>(string path)
        {
            if (string.IsNullOrEmpty(path)) return null;
            var loadAll = Resources.LoadAll<T>(path);
            return loadAll;
        }
    }
}