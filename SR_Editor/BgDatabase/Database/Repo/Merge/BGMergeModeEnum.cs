/*
<copyright file="BGMergeModeEnum.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Merge modes.
    /// 1) Merge- merge 2 repos together
    /// 2) Transfer - moves data from one repo to another
    /// </summary>
    public enum BGMergeModeEnum : byte
    {
        Merge,
        Transfer
    }
}