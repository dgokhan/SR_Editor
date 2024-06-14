/*
<copyright file="BGCalcTypeCode.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// abstract type code implementation
    /// type code is mapped to some C# type
    /// </summary>
    public abstract class BGCalcTypeCode
    {
        protected BGCalcTypeCode()
        {
        }

        /// <summary>
        /// Type code is used by internal code for faster vars deserialization, DO NOT OVERRIDE IT
        /// </summary>
        public abstract byte TypeCode { get; }

        /// <summary>
        /// type code name
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// type code type
        /// </summary>
        public abstract Type Type { get; }

        /// <summary>
        /// does type support default value?
        /// </summary>
        public abstract bool SupportDefaultValue { get; }

        /// <summary>
        ///  type default value as object
        /// </summary>
        public abstract object DefaultValue { get; }

        /// <summary>
        /// type title
        /// </summary>
        public virtual string TypeTitle => Name;

        /// <summary>
        /// are two value equal? both values should have the same type, corresponding to type code 
        /// </summary>
        public virtual bool AreEqual(object o1, object o2) => Equals(o1, o2);

        //=======================================================================================
        //                              Serialization
        //=======================================================================================
        /// <summary>
        /// convert value to binary array
        /// </summary>
        public abstract void ValueToBytes(BGBinaryWriter writer, object value);

        /// <summary>
        /// restore value from binary array
        /// </summary>
        public abstract object ValueFromBytes(BGBinaryReader reader);

        /// <summary>
        /// convert value to string
        /// </summary>
        public abstract string ValueToString(object value);

        /// <summary>
        /// restore value from string
        /// </summary>
        public abstract object ValueFromString(string value);

        //=======================================================================================
        //                              Conversion
        //=======================================================================================
        /// <summary>
        /// can the value of this type be converted from the value of provided type?
        /// </summary>
        public virtual bool CanBeConvertedFrom(BGCalcTypeCode otherCode) => false;

        /// <summary>
        /// can the value of this type be converted from the value of provided type?
        /// </summary>
        public virtual object ConvertFrom(BGCalcTypeCode otherCode, object value) => value;

        //=======================================================================================
        //                              Equals
        //=======================================================================================
        protected bool Equals(BGCalcTypeCode other) => TypeCode == other.TypeCode;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((BGCalcTypeCode)obj);
        }

        public override int GetHashCode() => TypeCode;
    }

    /// <summary>
    /// abstract type code implementation
    /// type code is mapped to T type
    /// </summary>
    public abstract class BGCalcTypeCode<T> : BGCalcTypeCode
    {
        public override Type Type => typeof(T);
    }
}