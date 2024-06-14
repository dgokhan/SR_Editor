/*
<copyright file="BGIdDictionary.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// dictionary with BGId key
    /// </summary>
    public partial class BGIdDictionary<T> : Dictionary<BGId, T>
    {
        public BGIdDictionary()
        {
        }

        public BGIdDictionary(int capacity) : base(capacity)
        {
        }

        public BGIdDictionary(BGIdDictionary<T> source) : base(source)
        {
        }
    }
}