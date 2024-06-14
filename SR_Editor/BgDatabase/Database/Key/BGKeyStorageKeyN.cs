/*
<copyright file="KeyStorageKeyN.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    //this is used inside dictionary as a N values key, where N>4
    internal class BGKeyStorageKeyN : BGKeyStorageKeyI
    {
        public static readonly BGObjectPool<BGKeyStorageKeyN> Pool = new BGObjectPool<BGKeyStorageKeyN>(
            () => new BGKeyStorageKeyN(Array.Empty<object>()),
            k => k.Values = Array.Empty<object>()
        );

        internal object[] Values;

        public BGKeyStorageKeyN(object[] values) => Values = values;

        /// <inheritdoc/>
        public bool IsValueEquals(object value, int index) => Equals(Values[index], value);

        /// <inheritdoc/>
        public BGKeyStorageKeyI Clone()
        {
            var clonedValues = new object[Values.Length];
            for (var i = 0; i < Values.Length; i++) clonedValues[i] = Values[i];
            return new BGKeyStorageKeyN(clonedValues);
        }

        /// <inheritdoc/>
        public bool Equals(BGKeyStorageKeyI key)
        {
            if (key == null) return false;
            for (var i = 0; i < Values.Length; i++)
            {
                var value = Values[i];
                if (!key.IsValueEquals(value, i)) return false;
            }
            return true;
        }


        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                var value = Values[0];
                var hashCode = value != null ? value.GetHashCode() : 0;
                for (var i = 1; i < Values.Length; i++)
                {
                    value = Values[i];
                    hashCode = (hashCode * 397) ^ (value != null ? value.GetHashCode() : 0);
                }

                return hashCode;
            }
        }
    }
}