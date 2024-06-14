/*
<copyright file="BGStorageI.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// field values storage, which can convert values to T[]
    /// </summary>
    public interface BGStorageI<T> : BGStorable<T>
    {
        /// <summary>
        /// convert internal T values to T[]
        /// </summary>
        T[] CopyRawValues();
        /*
        T GetStoredValue(int entityIndex);
        void SetStoredValue(int entityIndex, T value);
    */
    }
}