/*
<copyright file="KeyStorageKey3.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    //this is used inside dictionary as a 3 values key
    internal class BGKeyStorageKey3 : BGKeyStorageKeyI
    {
        internal object Value0;
        internal object Value1;
        internal object Value2;

        public BGKeyStorageKey3(object value0, object value1, object value2)
        {
            Value0 = value0;
            Value1 = value1;
            Value2 = value2;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Value0 != null ? Value0.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Value1 != null ? Value1.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Value2 != null ? Value2.GetHashCode() : 0);
                return hashCode;
            }
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
                case 2:
                    return Equals(Value2, value);
                default:
                    return false;
            }
        }

        /// <inheritdoc/>
        public BGKeyStorageKeyI Clone() => new BGKeyStorageKey3(Value0, Value1, Value2);

        public bool Equals(BGKeyStorageKeyI key) => key.IsValueEquals(Value0, 0) && key.IsValueEquals(Value1, 1) && key.IsValueEquals(Value2, 2);
    }

    internal class BGKeyStorageKey3<T0, T1, T2> : BGKeyStorageKeyI
    {
        public static readonly BGObjectPool<BGKeyStorageKey3<T0, T1, T2>> Pool =
            new BGObjectPool<BGKeyStorageKey3<T0, T1, T2>>(
                () => new BGKeyStorageKey3<T0, T1, T2>(default, default, default),
                k =>
                {
                    k.Value0 = default;
                    k.Value1 = default;
                    k.Value2 = default;
                }
            );

        internal T0 Value0;
        internal T1 Value1;
        internal T2 Value2;

        public BGKeyStorageKey3(T0 value0, T1 value1, T2 value2)
        {
            Value0 = value0;
            Value1 = value1;
            Value2 = value2;
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
                case 2:
                    return Value2 == null ? otherValue == null : Value2.Equals(otherValue);
                default:
                    return false;
            }
        }

        /// <inheritdoc/>
        public BGKeyStorageKeyI Clone() => new BGKeyStorageKey3<T0, T1, T2>(Value0, Value1, Value2);

        public bool Equals(BGKeyStorageKeyI key)
        {
            if (key == null) return false;
            if (key is BGKeyStorageKey3 keyA)
            {
                //this does not produce garbage
                var otherValue0 = keyA.Value0;
                if (!(Value0 == null ? otherValue0 == null : Value0.Equals(otherValue0))) return false;
                var otherValue1 = keyA.Value1;
                if (!(Value1 == null ? otherValue1 == null : Value1.Equals(otherValue1))) return false;
                var otherValue2 = keyA.Value2;
                return Value2 == null ? otherValue2 == null : Value2.Equals(otherValue2);
            }
            else
            {
                //probably never used
                var keyTyped = (BGKeyStorageKey3<T0, T1, T2>)key;
                var otherValue0 = keyTyped.Value0;
                if (!(Value0 == null ? otherValue0 == null : Value0.Equals(otherValue0))) return false;
                var otherValue1 = keyTyped.Value1;
                if (!(Value1 == null ? otherValue1 == null : Value1.Equals(otherValue1))) return false;
                var otherValue2 = keyTyped.Value2;
                return Value2 == null ? otherValue2 == null : Value2.Equals(otherValue2);
            }
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Value0 != null ? Value0.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Value1 != null ? Value1.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Value2 != null ? Value2.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}