/*
<copyright file="BGFieldCachedStructArrayA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Basic class for a field with struct array value type
    /// </summary>
    public abstract class BGFieldCachedStructArrayA<T> : BGFieldCachedArrayA<T> where T : struct
    {
        public BGFieldCachedStructArrayA(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        protected BGFieldCachedStructArrayA(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Serialization
        //================================================================================================


        //================================================================================================
        //                                              Value
        //================================================================================================
        /// <inheritdoc />
        public override T[] this[int entityIndex]
        {
            set
            {
                if (events.On)
                {
                    var oldValue = this[entityIndex];
                    if (BGUtil.ArraysValuesEqual(value, oldValue)) return;
                    var entity = Meta[entityIndex];
                    FireBeforeValueChanged(entity, oldValue, value);
                    StoreSet(entityIndex, value);
                    FireValueChanged(entity, oldValue, value);
                }
                else StoreSet(entityIndex, value);
            }
        }

        /// <inheritdoc />
        public override void CopyValue(BGField fromField, BGId fromEntityId, int fromEntityIndex, BGId toEntityId)
        {
            if (fromEntityIndex == -1 || fromField.IsDeleted) return;
            var index = Meta.FindEntityIndex(toEntityId);
            if (index == -1) return;

            var otherField = (BGFieldCachedStructArrayA<T>)fromField;
            var otherValue = otherField[fromEntityIndex];
            if (otherValue == null || otherValue.Length == 0) ClearValueNoEvent(index);
            else
            {
                var result = new T[otherValue.Length];
                Array.Copy(otherValue, result, otherValue.Length);
                StoreSet(index, result);
            }
        }
    }
}