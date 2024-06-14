/*
<copyright file="BGFieldReferenceSingleA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// abstract Field for referencing single object from unity scene 
    /// </summary>
    public abstract partial class BGFieldReferenceSingleA<T> : BGFieldReferenceA<T> where T : MonoBehaviour
    {
        protected BGFieldReferenceSingleA(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        protected BGFieldReferenceSingleA(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        /// <inheritdoc />
        public override T this[int entityIndex]
        {
            get
            {
                //micro-optimization code copied from base class
                if (entityIndex >= StoreCount) ThrowIndexOutOfBoundOnRead(entityIndex);
                var value = StoreItems[entityIndex];
                if (value.IsEmpty) return null;
                return GetById(value);
            }
            set
            {
                var oldValue = StoreGet(entityIndex);
                var hasValue = !oldValue.IsEmpty;
                if (value == null)
                {
                    StoreSet(entityIndex, BGId.Empty);
                    if (events.On && hasValue) FireStoredValueChanged(Meta[entityIndex], oldValue, BGId.Empty);
                }
                else
                {
                    var referenceId = IdProvider(value);
                    if (referenceId.IsEmpty)
                    {
                        if (!hasValue) return;
                        StoreSet(entityIndex, BGId.Empty);
                        FireStoredValueChanged(Meta[entityIndex], referenceId, BGId.Empty);
                    }
                    else
                    {
                        if (oldValue == referenceId) return;
                        SetStoredValue(entityIndex, referenceId);
                    }
                }
            }
        }

        //================================================================================================
        //                                              Overrides
        //================================================================================================
        /// <summary>
        /// Get ID value for provided component 
        /// </summary>
        protected abstract BGId IdProvider(T component);
        /// <summary>
        /// Get component by ID 
        /// </summary>
        protected abstract T GetById(BGId id);
    }
}