/*
<copyright file="BGObjectI.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// basic interface for object with unique id
    /// </summary>
    public partial interface BGObjectI
    {
        /// <summary>
        /// object's ID
        /// </summary>
        BGId Id { get; }
    }
}