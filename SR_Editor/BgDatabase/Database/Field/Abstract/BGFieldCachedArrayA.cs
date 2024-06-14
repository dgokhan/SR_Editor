/*
<copyright file="BGFieldCachedArrayA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Abstract field for array value type
    /// T is the type of array element
    /// </summary>
    public abstract class BGFieldCachedArrayA<T> : BGFieldCachedClassA<T[]>, BGArrayI
    {
        protected BGFieldCachedArrayA(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        protected BGFieldCachedArrayA(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Value
        //================================================================================================
        /// <summary>
        /// How much array elements the entity with provided index have
        /// </summary>
        public int CountValues(int entityIndex) => this[entityIndex]?.Length ?? 0;

        /// <inheritdoc/>
        public override bool AreStoredValuesEqual(BGField field, int myEntityIndex, int otherEntityIndex)
        {
            if (!(field is BGFieldCachedArrayA<T> typed)) return false;

            var valueList = this[myEntityIndex];
            var valueList2 = typed[otherEntityIndex];

            var isEmpty = BGUtil.IsEmpty(valueList);
            var isEmpty2 = BGUtil.IsEmpty(valueList2);

            if (isEmpty && isEmpty2) return true;
            if (isEmpty || isEmpty2) return false;

            if (valueList.Length != valueList2.Length) return false;

            for (var i = 0; i < valueList.Length; i++)
            {
                var myValue = valueList[i];
                var myValue2 = valueList2[i];
                if (!AreEqual(myValue, myValue2)) return false;
            }

            return true;
        }

        /// <summary>
        /// Are values of type T equal?
        /// </summary>
        protected abstract bool AreEqual(T myValue, T myValue2);

        //================================================================================================
        //                                              Static
        //================================================================================================
        /*/// <summary>
        /// makes sure field has value for entity with id entityId
        /// </summary>
        public static List<T> EnsureValue(BGFieldCachedListA<T> field, int entityIndex)
        {
            var list = field[entityIndex];
            if (list != null) return list;

            list = new List<T>();
            field[entityIndex] = list;
            return list;
        }

        /// <summary>
        /// makes sure field has empty value for entity with id entityId
        /// </summary>
        public static List<T> EnsureValueCleared(BGFieldCachedListA<T> field, int entityIndex)
        {
            var list = EnsureValue(field, entityIndex);
            list.Clear();
            return list;
        }*/
    }
}