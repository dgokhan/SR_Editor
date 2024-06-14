/*
<copyright file="BGConfigForRepo.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// IS IT USED ??
    /// </summary>
    public class BGConfigForRepo
    {
        private readonly string assetPath;
        private readonly int assetId;

        public string AssetPath => assetPath;

        public int AssetId => assetId;

        public BGConfigForRepo(string assetPath, int assetId)
        {
            this.assetPath = assetPath;
            this.assetId = assetId;
        }
    }
}