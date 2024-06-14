/*
<copyright file="BGIdList.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// serializable list with BGId
    /// </summary>
    [Serializable]
    public partial class BGIdList : List<BGId>, ISerializationCallbackReceiver
    {
        public List<string> values = new List<string>();

        public BGIdList()
        {
        }

        public BGIdList(IEnumerable<BGId> collection) : base(collection)
        {
        }

        public void OnBeforeSerialize()
        {
            values.Clear();
            foreach (var val in this) values.Add(val.ToString());
        }

        public void OnAfterDeserialize()
        {
            foreach (var val in values) Add(new BGId(val));
            values.Clear();
        }
    }
}