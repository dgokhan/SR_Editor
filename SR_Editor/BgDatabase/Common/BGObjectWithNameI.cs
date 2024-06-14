/*
<copyright file="BGObjectWithNameI.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// basic interface for object with unique id and name
    /// </summary>
    public partial interface BGObjectWithNameI : BGObjectI
    {
        /// <summary>
        /// Object's name
        /// </summary>
        string Name { get; }
    }
}