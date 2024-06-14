/*
<copyright file="BGStringFormatter.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Implement this interface to customize string representations of field's values.
    /// String values are used in Excel/GoogleSheets export/import
    /// You need to assign your own BGStringFormatter implementation type to field's StringFormatter attribute
    /// </summary>
    /// <typeparam name="T">The field's value type. It must be exact type</typeparam>
    public interface BGStringFormatter<T>
    {
        /// <summary>
        /// restore value from string.
        /// </summary>
        /// <exception cref="BGStringFormatterUseDefaultException">Throw BGStringFormatterUseDefaultException exception to notify default FromString should be used</exception>
        T FromString(string value);

        /// <summary>
        /// get value as string, 
        /// </summary>
        /// <exception cref="BGStringFormatterUseDefaultException">Throw BGStringFormatterUseDefaultException exception to notify default ToString should be used</exception>
        string ToString(T value);
    }

    /// <summary>
    /// Throw this exception from  BGStringFormatter to notify default toString/fromString should be used
    /// </summary>
    public class BGStringFormatterUseDefaultException : Exception
    {
    }
}