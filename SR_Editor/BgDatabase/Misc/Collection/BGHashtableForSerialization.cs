/*
<copyright file="BGHashtableForSerialization.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// serializable dictionary 
    /// </summary>
    [Serializable]
    public partial class BGHashtableForSerialization<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        public List<TKey> keys = new List<TKey>();
        public List<TValue> values = new List<TValue>();

        /// <inheritdoc />
        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();
            foreach (var pair in this)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }

            FromKeys();
        }


        /// <inheritdoc />
        public void OnAfterDeserialize()
        {
            Clear();
            ToKeys();

            if (keys.Count != values.Count)
                throw new BGException(string.Format("there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable.",
                    keys.Count, values.Count));

            for (var i = 0; i < keys.Count; i++) Add(keys[i], values[i]);
        }

        protected virtual void FromKeys()
        {
        }

        protected virtual void ToKeys()
        {
        }

        protected virtual void FromValues()
        {
        }

        protected virtual void ToValues()
        {
        }
    }
}