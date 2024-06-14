/*
<copyright file="BGLiveUpdateValueResolver.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Interface for converting remote data value to the database field value
    /// </summary>
    public interface BGLiveUpdateValueResolver
    {
        /// <summary>
        /// Convert remote cell value to the database field value 
        /// </summary>
        string Resolve(BGField field, string value);
    }
}