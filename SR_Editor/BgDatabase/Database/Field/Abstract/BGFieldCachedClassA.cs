/*
<copyright file="BGFieldCachedClassA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Field with class value type
    /// </summary>
    public abstract partial class BGFieldCachedClassA<T> : BGFieldCachedA<T> where T : class
    {
        protected BGFieldCachedClassA(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        protected BGFieldCachedClassA(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Value
        //================================================================================================
        /// <inheritdoc />
        public override void CopyValue(BGField fromField, BGId fromEntityId, int fromEntityIndex, BGId toEntityId)
        {
            if (fromEntityIndex == -1 || fromField.IsDeleted) return;
            var index = Meta.FindEntityIndex(toEntityId);
            if (index == -1) return;

            var otherField = (BGFieldCachedClassA<T>)fromField;
            var otherValue = otherField[fromEntityIndex];
            switch (otherValue)
            {
                case null:
                    ClearValueNoEvent(index);
                    break;
                case ICloneable cloneable:
                    StoreSet(index, (T)cloneable.Clone());
                    break;
                default:
                    StoreSet(index, BGUtil.Clone(otherValue));
                    break;
            }
        }

        /// <inheritdoc />
        public override T this[int entityIndex]
        {
            set
            {
                //there is NO reliable method to compare class based values
                var oldValue = this[entityIndex];
                var entity = Meta[entityIndex];
                FireBeforeValueChanged(entity, oldValue, value);
                StoreSet(entityIndex, value);
                FireValueChanged(entity, oldValue, value);
            }
        }
    }
}