/*
<copyright file="KeyStorageKey2.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    //this is used inside dictionary as a 2 values key 
    internal class BGKeyStorageKey2 : BGKeyStorageKeyI
    {
        internal object Value0;
        internal object Value1;

        public BGKeyStorageKey2(object value0, object value1)
        {
            Value0 = value0;
            Value1 = value1;
        }

        public override int GetHashCode()
        {
            unchecked { return ((Value0 != null ? Value0.GetHashCode() : 0) * 397) ^ (Value1 != null ? Value1.GetHashCode() : 0); }
        }

        /// <inheritdoc/>
        public bool IsValueEquals(object value, int index)
        {
            switch (index)
            {
                case 0:
                    return Equals(Value0, value);
                case 1:
                    return Equals(Value1, value);
                default:
                    return false;
            }
        }

        /// <inheritdoc/>
        public BGKeyStorageKeyI Clone() => new BGKeyStorageKey2(Value0, Value1);

        public bool Equals(BGKeyStorageKeyI key) => key.IsValueEquals(Value0, 0) && key.IsValueEquals(Value1, 1);
    }

    internal class BGKeyStorageKey2<T0, T1> : BGKeyStorageKeyI
    {
        public static readonly BGObjectPool<BGKeyStorageKey2<T0, T1>> Pool = new BGObjectPool<BGKeyStorageKey2<T0, T1>>(
            () => new BGKeyStorageKey2<T0, T1>(default, default),
            k =>
            {
                k.Value0 = default;
                k.Value1 = default;
            }
        );

        internal T0 Value0;
        internal T1 Value1;

        public BGKeyStorageKey2(T0 value0, T1 value1)
        {
            Value0 = value0;
            Value1 = value1;
        }

        /// <inheritdoc/>
        public bool IsValueEquals(object otherValue, int index)
        {
            switch (index)
            {
                case 0:
                    return Value0 == null ? otherValue == null : Value0.Equals(otherValue);
                case 1:
                    return Value1 == null ? otherValue == null : Value1.Equals(otherValue);
                default:
                    return false;
            }
        }

        /// <inheritdoc/>
        public BGKeyStorageKeyI Clone() => new BGKeyStorageKey2<T0, T1>(Value0, Value1);

        public bool Equals(BGKeyStorageKeyI key)
        {
            if (key == null) return false;
            if (key is BGKeyStorageKey2 keyA)
            {
                //this does not produce garbage
                var otherValue0 = keyA.Value0;
                if (!(Value0 == null ? otherValue0 == null : Value0.Equals(otherValue0))) return false;
                var otherValue1 = keyA.Value1;
                return Value1 == null ? otherValue1 == null : Value1.Equals(otherValue1);
            }
            else
            {
                //probably never used
                var keyTyped = (BGKeyStorageKey2<T0, T1>)key;
                var otherValue0 = keyTyped.Value0;
                if (!(Value0 == null ? otherValue0 == null : Value0.Equals(otherValue0))) return false;
                var otherValue1 = keyTyped.Value1;
                return Value1 == null ? otherValue1 == null : Value1.Equals(otherValue1);
            }
        }

        public override int GetHashCode()
        {
            unchecked { return ((Value0 != null ? Value0.GetHashCode() : 0) * 397) ^ (Value1 != null ? Value1.GetHashCode() : 0); }
        }
    }
}