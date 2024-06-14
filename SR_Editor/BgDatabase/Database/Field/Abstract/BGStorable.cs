/*
<copyright file="BGStorable.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Interface for field internal (stored) value of type T
    /// Stored value can be different from actual field value (for example Unity asset fields)
    /// </summary>
    public interface BGStorable<T>
    {
        /// <summary>
        /// Set stored value for entity with specified index.
        /// Stored value can be different from actual field value (for example Unity asset fields)
        /// </summary>
        void SetStoredValue(int entityIndex, T value);

        /// <summary>
        /// Get stored value  for entity with specified index.
        /// Stored value can be different from actual field value (for example Unity asset fields)
        /// </summary>
        T GetStoredValue(int entityIndex);
    }

    /// <summary>
    /// Interface for field internal (stored) value of type string
    /// Stored value can be different from actual field value (for example Unity asset fields)
    /// </summary>
    public interface BGStorableString : BGStorable<string>
    {
    }
}