/*
<copyright file="KeyStorageKey4.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    //this is used inside dictionary as a 4 values key
    internal class BGKeyStorageKey4 : BGKeyStorageKeyI
    {
        internal object Value0;
        internal object Value1;
        internal object Value2;
        internal object Value3;

        public BGKeyStorageKey4(object value0, object value1, object value2, object value3)
        {
            Value0 = value0;
            Value1 = value1;
            Value2 = value2;
            Value3 = value3;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Value0 != null ? Value0.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Value1 != null ? Value1.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Value2 != null ? Value2.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Value3 != null ? Value3.GetHashCode() : 0);
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
                case 3:
                    return Equals(Value3, value);
                default:
                    return false;
            }
        }

        /// <inheritdoc/>
        public BGKeyStorageKeyI Clone() => new BGKeyStorageKey4(Value0, Value1, Value2, Value3);

        public bool Equals(BGKeyStorageKeyI key) => key.IsValueEquals(Value0, 0) && key.IsValueEquals(Value1, 1) && key.IsValueEquals(Value2, 2) && key.IsValueEquals(Value3, 3);
    }

    internal class BGKeyStorageKey4<T0, T1, T2, T3> : BGKeyStorageKeyI
    {
        public static readonly BGObjectPool<BGKeyStorageKey4<T0, T1, T2, T3>> Pool =
            new BGObjectPool<BGKeyStorageKey4<T0, T1, T2, T3>>(
                () => new BGKeyStorageKey4<T0, T1, T2, T3>(default, default, default, default),
                k =>
                {
                    k.Value0 = default;
                    k.Value1 = default;
                    k.Value2 = default;
                    k.Value3 = default;
                }
            );

        internal T0 Value0;
        internal T1 Value1;
        internal T2 Value2;
        internal T3 Value3;

        public BGKeyStorageKey4(T0 value0, T1 value1, T2 value2, T3 value3)
        {
            Value0 = value0;
            Value1 = value1;
            Value2 = value2;
            Value3 = value3;
        }

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
                case 3:
                    return Value3 == null ? otherValue == null : Value3.Equals(otherValue);
                default:
                    return false;
            }
        }

        /// <inheritdoc/>
        public BGKeyStorageKeyI Clone() => new BGKeyStorageKey4<T0, T1, T2, T3>(Value0, Value1, Value2, Value3);

        /// <inheritdoc/>
        public bool Equals(BGKeyStorageKeyI key)
        {
            if (key == null) return false;
            if (key is BGKeyStorageKey4 keyA)
            {
                //this does not produce garbage
                var otherValue0 = keyA.Value0;
                if (!(Value0 == null ? otherValue0 == null : Value0.Equals(otherValue0))) return false;
                var otherValue1 = keyA.Value1;
                if (!(Value1 == null ? otherValue1 == null : Value1.Equals(otherValue1))) return false;
                var otherValue2 = keyA.Value2;
                if (!(Value2 == null ? otherValue2 == null : Value2.Equals(otherValue2))) return false;
                var otherValue3 = keyA.Value3;
                return Value3 == null ? otherValue3 == null : Value3.Equals(otherValue3);
            }
            else
            {
                //probably never used
                var keyTyped = (BGKeyStorageKey4<T0, T1, T2, T3>)key;
                var otherValue0 = keyTyped.Value0;
                if (!(Value0 == null ? otherValue0 == null : Value0.Equals(otherValue0))) return false;
                var otherValue1 = keyTyped.Value1;
                if (!(Value1 == null ? otherValue1 == null : Value1.Equals(otherValue1))) return false;
                var otherValue2 = keyTyped.Value2;
                if (!(Value2 == null ? otherValue2 == null : Value2.Equals(otherValue2))) return false;
                var otherValue3 = keyTyped.Value3;
                return Value3 == null ? otherValue3 == null : Value3.Equals(otherValue3);
            }
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Value0 != null ? Value0.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Value1 != null ? Value1.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Value2 != null ? Value2.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Value3 != null ? Value3.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}