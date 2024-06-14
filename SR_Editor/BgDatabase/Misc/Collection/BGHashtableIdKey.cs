/*
<copyright file="BGHashtableIdKey.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// serializable dictionary with BGId key
    /// </summary>
    [Serializable]
    public partial class BGHashtableIdKey<T> : BGHashtableForSerialization<BGId, T>
    {
        public List<string> myKeys = new List<string>();

        protected override void FromKeys()
        {
            myKeys.Clear();
            foreach (var key in keys) myKeys.Add(key.ToString());
        }

        protected override void ToKeys()
        {
            foreach (var key in myKeys) keys.Add(new BGId(key));
            myKeys.Clear();
        }
    }
}