/*
<copyright file="BGFieldUnityAssetI.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// field storing unity assets
    /// </summary>
    public interface BGFieldUnityAssetI
    {
        /// <summary>
        /// get asset key/path by row index
        /// </summary>
        string GetAssetPath(int entityIndex);
        /// <summary>
        /// set asset key/path for row with provided index
        /// </summary>
        void SetAssetPath(int entityIndex, string path);
    }
}