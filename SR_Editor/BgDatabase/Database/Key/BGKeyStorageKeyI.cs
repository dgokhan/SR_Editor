/*
<copyright file="BGKeyStorageKeyI.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Data container for all key's values
    /// </summary>
    public interface BGKeyStorageKeyI : IEquatable<BGKeyStorageKeyI>
    {
        /// <summary>
        /// Does the key's value at index=index equal to provided value?
        /// </summary>
        bool IsValueEquals(object value, int index);
        
        /// <summary>
        /// Clone this object
        /// </summary>
        BGKeyStorageKeyI Clone();
    }
}