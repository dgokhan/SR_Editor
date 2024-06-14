/*
<copyright file="BGFieldCachedClassListA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Basic class for a field with list value type, holding class-based values
    /// </summary>
    public abstract partial class BGFieldCachedClassListA<T> : BGFieldCachedListA<T> where T : class
    {
        protected BGFieldCachedClassListA(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        protected BGFieldCachedClassListA(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Value
        //================================================================================================

        /// <inheritdoc />
        public override List<T> this[int entityIndex]
        {
            set
            {
                var oldValue = this[entityIndex];
                var entity = Meta[entityIndex];
                FireBeforeValueChanged(entity, oldValue, value);
                StoreSet(entityIndex, value);
                FireValueChanged(entity, oldValue, value);
            }
        }

        /// <inheritdoc />
        public override void CopyValue(BGField fromField, BGId fromEntityId, int fromEntityIndex, BGId toEntityId)
        {
            if (fromEntityIndex == -1 || fromField.IsDeleted) return;
            var index = Meta.FindEntityIndex(toEntityId);
            if (index == -1) return;

            var otherField = (BGFieldCachedClassListA<T>)fromField;
            var otherValue = otherField[fromEntityIndex];
            if (otherValue == null) ClearValueNoEvent(index);
            else
            {
                var result = new List<T>();
                var valueClonable = typeof(ICloneable).IsAssignableFrom(typeof(T));
                if (valueClonable)
                    foreach (var val in otherValue)
                        result.Add((T)((ICloneable)val).Clone());
                else
                    foreach (var val in otherValue)
                        result.Add(BGUtil.Clone(val));
                StoreSet(index, result);
            }
        }
    }
}