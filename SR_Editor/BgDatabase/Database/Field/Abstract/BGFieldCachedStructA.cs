/*
<copyright file="BGFieldCachedStructA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Basic class for a field with struct value type
    /// T is a type of the struct
    /// </summary>
    public abstract partial class BGFieldCachedStructA<T> : BGFieldCachedA<T>, BGStructI where T : struct
    {
        /// <inheritdoc />
        public override int ConstantSize => ValueSize;

        /// <summary>
        /// how much bytes T value takes 
        /// </summary>
        protected abstract int ValueSize { get; }


        protected BGFieldCachedStructA(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        protected BGFieldCachedStructA(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        /// <inheritdoc />
        public override T this[int entityIndex]
        {
            set
            {
                if (events.On)
                {
                    var oldValue = this[entityIndex];
                    if (EqualityComparer<T>.Default.Equals(oldValue, value)) return;
                    var entity = Meta[entityIndex];
                    FireBeforeValueChanged(entity, oldValue, value);
                    StoreSet(entityIndex, value);
                    FireValueChanged(entity, oldValue, value);
                }
                else StoreSet(entityIndex, value);
            }
        }

/*
        /// <inheritdoc />
        public override void CopyValue(BGField fieldFrom, BGId entityId)
        {
            var cached = (BGFieldCachedStructA<T>) fieldFrom;
            Store[entityId] = cached[entityId];
        }
*/
    }
}