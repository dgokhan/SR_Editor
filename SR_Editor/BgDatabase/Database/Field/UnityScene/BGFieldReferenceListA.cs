/*
<copyright file="BGFieldReferenceListA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// abstract Field for referencing list of objects from unity scene 
    /// </summary>
    public abstract partial class BGFieldReferenceListA<T> : BGFieldReferenceA<List<T>> where T : MonoBehaviour
    {
        /// <inheritdoc />
        public override bool ReadOnly => true;

        protected BGFieldReferenceListA(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        protected BGFieldReferenceListA(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        /// <inheritdoc />
        public override List<T> this[int entityIndex]
        {
            get
            {
                var value = GetStoredValue(entityIndex);
                if (value == BGId.Empty) return null;

                List<T> result = null;
                var candidates = Object.FindObjectsOfType<T>();
                for (var i = 0; i < candidates.Length; i++)
                {
                    var candidate = candidates[i];
                    if (IdProvider(candidate) != value) continue;

                    result = result ?? new List<T>();
                    result.Add(candidate);
                }

                return result;
            }
            set { }
        }

        //================================================================================================
        //                                              Overrides
        //================================================================================================
        /// <summary>
        /// get ID value from component
        /// </summary>
        protected abstract BGId IdProvider(T component);
    }
}