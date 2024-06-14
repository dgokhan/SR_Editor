/*
<copyright file="BGCalcTypeCodeEnum.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Globalization;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// type code for enum type
    /// </summary>
    public class BGCalcTypeCodeEnum : BGCalcTypeCode<Enum>, BGCalcTypeCodeStateful
    {
        private string enumTypeAsString;
        private Type enumType;

        public const byte Code = 7;
        
        /// <inheritdoc />
        public override bool SupportDefaultValue => true;
        
        /// <inheritdoc />
        public override byte TypeCode => Code;

        /// <inheritdoc />
        public override object DefaultValue
        {
            get
            {
                var type = EnumType;
                if (type == null) return null;
                var values = Enum.GetValues(type);
                if (values.Length == 0) return null;
                return values.GetValue(0);
            }
        }

        public string EnumTypeAsString => enumTypeAsString;

        /// <summary>
        /// enum type
        /// </summary>
        public Type EnumType
        {
            get
            {
                if (enumType != null) return enumType;
                var type = BGUtil.GetType(enumTypeAsString);
                if (type != null && type.IsEnum) enumType = type;
                return enumType;
            }
        }

        /// <inheritdoc />
        public override string TypeTitle => Name + " [" + enumTypeAsString + "]";
        
        /// <inheritdoc />
        public override string Name => "enum";


        //===========================================================================
        //                                      Constructor
        //===========================================================================
        internal BGCalcTypeCodeEnum()
        {
        }

        public BGCalcTypeCodeEnum(Type enumType)
        {
            if (enumType == null) throw new Exception($"type can not be null!");
            if (!enumType.IsEnum) throw new Exception($"{enumType.FullName} type is not enum!");
            enumTypeAsString = enumType.FullName;
            this.enumType = enumType;
        }

        //===========================================================================
        //                                      Serialization
        //===========================================================================
        /// <inheritdoc />
        public override void ValueToBytes(BGBinaryWriter writer, object value)
        {
            var enumType = EnumType;
            if (enumType == null) throw new Exception("Can not serialize enum field, cause enum type with name " + enumTypeAsString + " can not be found!");
            var code = Type.GetTypeCode(enumType.GetEnumUnderlyingType());
            writer.AddByte((byte)code);
            switch (code)
            {
                case System.TypeCode.Byte:
                    writer.AddByte((byte)value);
                    break;
                case System.TypeCode.Int16:
                    writer.AddShort((short)value);
                    break;
                case System.TypeCode.Int32:
                    writer.AddInt((int)value);
                    break;
                case System.TypeCode.Int64:
                    writer.AddLong((long)value);
                    break;
                case System.TypeCode.SByte:
                    writer.AddSByte((sbyte)value);
                    break;
                case System.TypeCode.UInt16:
                    writer.AddUShort((ushort)value);
                    break;
                case System.TypeCode.UInt32:
                    writer.AddUInt((uint)value);
                    break;
                case System.TypeCode.UInt64:
                    writer.AddULong((ulong)value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(code), "Unsupported enum underlying enum type code=" + code);
            }
        }

        /// <inheritdoc />
        public override object ValueFromBytes(BGBinaryReader reader)
        {
            var enumType = EnumType;
            if (enumType == null) throw new Exception("Can not deserialize enum field, cause enum type with name " + enumTypeAsString + " can not be found!");
            var code = reader.ReadByte();
            object value;
            switch ((TypeCode)code)
            {
                case System.TypeCode.Byte:
                    value = reader.ReadByte();
                    break;
                case System.TypeCode.Int16:
                    value = reader.ReadShort();
                    break;
                case System.TypeCode.Int32:
                    value = reader.ReadInt();
                    break;
                case System.TypeCode.Int64:
                    value = reader.ReadLong();
                    break;
                case System.TypeCode.SByte:
                    value = reader.ReadSByte();
                    break;
                case System.TypeCode.UInt16:
                    value = reader.ReadUShort();
                    break;
                case System.TypeCode.UInt32:
                    value = reader.ReadUInt();
                    break;
                case System.TypeCode.UInt64:
                    value = reader.ReadULong();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(code), "Unsupported enum underlying enum type code=" + code);
            }

            return Enum.ToObject(enumType, value);
        }

        /// <inheritdoc />
        public override string ValueToString(object value)
        {
            var enumType = EnumType;
            if (enumType == null) throw new Exception("Can not serialize enum field, cause enum type with name " + enumTypeAsString + " can not be found!");
            var json = new JsonValue
            {
                code = (byte)Type.GetTypeCode(enumType.GetEnumUnderlyingType())
            };
            switch ((TypeCode)json.code)
            {
                case System.TypeCode.Byte:
                    json.value = ((byte)value).ToString(CultureInfo.InvariantCulture);
                    break;
                case System.TypeCode.Int16:
                    json.value = ((short)value).ToString(CultureInfo.InvariantCulture);
                    break;
                case System.TypeCode.Int32:
                    json.value = ((int)value).ToString(CultureInfo.InvariantCulture);
                    break;
                case System.TypeCode.Int64:
                    json.value = ((long)value).ToString(CultureInfo.InvariantCulture);
                    break;
                case System.TypeCode.SByte:
                    json.value = ((sbyte)value).ToString(CultureInfo.InvariantCulture);
                    break;
                case System.TypeCode.UInt16:
                    json.value = ((ushort)value).ToString(CultureInfo.InvariantCulture);
                    break;
                case System.TypeCode.UInt32:
                    json.value = ((uint)value).ToString(CultureInfo.InvariantCulture);
                    break;
                case System.TypeCode.UInt64:
                    json.value = ((ulong)value).ToString(CultureInfo.InvariantCulture);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(json.code), "Unsupported enum underlying enum type code=" + json.code);
            }

            return JsonUtility.ToJson(json);
        }

        /// <inheritdoc />
        public override object ValueFromString(string valueString)
        {
            if (string.IsNullOrEmpty(valueString)) return null;
            var json = JsonUtility.FromJson<JsonValue>(valueString);

            object value;
            switch ((TypeCode)json.code)
            {
                case System.TypeCode.Byte:
                    value = byte.Parse(json.value);
                    break;
                case System.TypeCode.Int16:
                    value = short.Parse(json.value);
                    break;
                case System.TypeCode.Int32:
                    value = int.Parse(json.value);
                    break;
                case System.TypeCode.Int64:
                    value = long.Parse(json.value);
                    break;
                case System.TypeCode.SByte:
                    value = sbyte.Parse(json.value);
                    break;
                case System.TypeCode.UInt16:
                    value = ushort.Parse(json.value);
                    break;
                case System.TypeCode.UInt32:
                    value = uint.Parse(json.value);
                    break;
                case System.TypeCode.UInt64:
                    value = ulong.Parse(json.value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(json.code), "Unsupported enum underlying enum type code=" + json.code);
            }

            return Enum.ToObject(enumType, value);
        }


        [Serializable]
        private class JsonValue
        {
            public byte code;
            public string value;
        }

        //===========================================================================
        //                                      Equals
        //===========================================================================
        protected bool Equals(BGCalcTypeCodeEnum other)
        {
            return base.Equals(other) && enumTypeAsString == other.enumTypeAsString;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((BGCalcTypeCodeEnum)obj);
        }

        public override int GetHashCode()
        {
            unchecked { return (base.GetHashCode() * 397) ^ (enumTypeAsString != null ? enumTypeAsString.GetHashCode() : 0); }
        }

        //===========================================================================
        //                                      State
        //===========================================================================
        /// <inheritdoc />
        public void ReadState(BGBinaryReader reader)
        {
            enumTypeAsString = reader.ReadString();
            enumType = null;
        }

        /// <inheritdoc />
        public void WriteState(BGBinaryWriter writer) => writer.AddString(enumTypeAsString ?? "");

        /// <inheritdoc />
        public void ReadState(string state)
        {
            enumTypeAsString = state;
            enumType = null;
        }

        /// <inheritdoc />
        public string WriteState() => enumTypeAsString;

        //===========================================================================
        //                                      enum underlying types
        //===========================================================================
        /*//byte, sbyte, short, ushort, int, uint, long or ulong : https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/enums
        //values are copied from System.TypeCode
        public enum BGEnumTypeCode
        {
            SByte = 5,
            Byte = 6,
            Int16 = 7,
            UInt16 = 8,
            Int32 = 9,
            UInt32 = 10, 
            Int64 = 11, 
            UInt64 = 12, 
        }*/
    }
}