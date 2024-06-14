/*
<copyright file="BGIndexOperator.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// abstract index operator
    /// </summary>
    public abstract class BGIndexOperator
    {
        //get result for provided storage
        internal abstract void GetResult<T>(List<T> result, BGIndexStorage storage) where T: BGEntity;
    }
}