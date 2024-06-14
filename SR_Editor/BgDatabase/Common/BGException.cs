/*
<copyright file="BGException.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Basic exception class
    /// </summary>
    public partial class BGException : Exception
    {
        public BGException(string message, params object[] args) : base(BGUtil.Format(message, args))
        {
        }

        /// <summary>
        /// Throw an exception if condition is true
        /// </summary>
        public static void ThrowIf(bool condition, string message, params object[] path)
        {
            if (!condition) return;
            throw new BGException(message, path);
        }
    }
}