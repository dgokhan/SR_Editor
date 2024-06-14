/*
<copyright file="KeyStorageKey1.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    //this is used inside dictionary as a single value key
    internal class BGKeyStorageKey1 : BGKeyStorageKeyI
    {
        internal object Value0;

        public BGKeyStorageKey1(object value0) => Value0 = value0;

        /// <inheritdoc/>
        public bool IsValueEquals(object value, int index) => Equals(Value0, value);

        /// <inheritdoc/>
        public BGKeyStorageKeyI Clone() => new BGKeyStorageKey1(Value0);

        public override int GetHashCode() => (Value0 != null ? Value0.GetHashCode() : 0);
        public bool Equals(BGKeyStorageKeyI key) => key.IsValueEquals(Value0, 0);
    }

    //this is used as a lookup key to prevent GC
    internal class BGKeyStorageKey1<T0> : BGKeyStorageKeyI
    {
        public static readonly BGObjectPool<BGKeyStorageKey1<T0>> Pool = new BGObjectPool<BGKeyStorageKey1<T0>>(
            () => new BGKeyStorageKey1<T0>(default),
            k => k.Value0 = default
        );

        internal T0 Value0;

        private BGKeyStorageKey1(T0 value0) => Value0 = value0;

        /// <inheritdoc/>
        public bool IsValueEquals(object otherValue, int index) => Value0 == null ? otherValue == null : Value0.Equals(otherValue);

        /// <inheritdoc/>
        public BGKeyStorageKeyI Clone() => new BGKeyStorageKey1<T0>(Value0);

        public bool Equals(BGKeyStorageKeyI key)
        {
            if (key == null) return false;
            //this should not produce garbage
            if (key is BGKeyStorageKey1 keyA) return IsValueEquals(keyA.Value0, 0);
            return IsValueEquals(((BGKeyStorageKey1<T0>)key).Value0, 0);
        }

        public override int GetHashCode() => (Value0 != null ? Value0.GetHashCode() : 0);
    }
}