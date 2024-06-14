/*
<copyright file="BGFieldEnumA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// abstract class for enum
    /// T is enum type
    /// </summary>
    public abstract class BGFieldEnumA<T> : BGFieldCachedA<Enum, T>, BGFieldEnumI, BGFieldWithCustomConfigI where T : struct, IComparable, IConvertible, IFormattable
    {
        //================================================================================================
        //                                              Fields
        //================================================================================================

        /// <inheritdoc />
        public override int ConstantSize => ValueSize;

        /// <summary>
        /// enum value size in bytes
        /// </summary>
        protected abstract int ValueSize { get; }

        /// <inheritdoc/>
        public override bool CanBeUsedAsKey => true;


        protected Enum DefaultEnumValue => Activator.CreateInstance(EnumType) as Enum;

        /// <inheritdoc/>
        public Type UnderlyingType => typeof(T);

        private Type enumType;

        /// <inheritdoc/>
        public Type EnumType
        {
            get => enumType;
            set
            {
                var error = GetErrorForEnumType(value, UnderlyingType);
                if (!string.IsNullOrEmpty(error)) throw new BGException("Can not change enum Type:" + error);

                if (enumType == value) return;
                //try to resolve values with new enum
                var meta = Meta;
                for (var i = 0; i < meta.CountEntities; i++)
                {
                    var storedValue = GetStoredValue(i);
                    if (!Enum.IsDefined(value, storedValue))
                    {
                        var enumValue = "[enum name is not resolvable]";
                        try
                        {
                            enumValue = Enum.GetName(enumType, storedValue);
                        }
                        catch
                        {
                            //ignore
                        }

                        throw new BGException("Can not resolve value with provided enum type $: entity index=$, entity name=$, not resolvable enum value=$(enum index=$)",
                            value.FullName, i, meta[i].Name, enumValue, storedValue);
                    }
                }

                enumType = value;
                events.MetaWasChanged(Meta);
            }
        }

        //================================================================================================
        //                                              Constructors
        //================================================================================================

        protected BGFieldEnumA(BGMetaEntity meta, string name, Type enumType) : base(meta, name)
        {
            var error = GetErrorForEnumType(enumType, UnderlyingType);
            if (!string.IsNullOrEmpty(error))
            {
                Meta.Unregister(this);
                throw new BGException(error);
            }

            this.enumType = enumType;
        }

        protected BGFieldEnumA(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Value
        //================================================================================================
        /// <inheritdoc />
        public override Enum this[int entityIndex]
        {
            get
            {
                //micro-optimization code copied from base class
                if (entityIndex >= StoreCount) ThrowIndexOutOfBoundOnRead(entityIndex);
                var storeValue= StoreItems[entityIndex];
                return StoredValueToEnum(storeValue);
            }
            set
            {
                var newStoredValue = EnumToStoredValue(value);
                if (events.On)
                {
                    var oldValue = this[entityIndex];
                    // if (EqualityComparer<T>.Default.Equals(Store[entityIndex], newStoredValue)) return;
                    if (Equals(oldValue, value)) return;
                    var entity = Meta[entityIndex];
                    FireBeforeValueChanged(entity, oldValue, value);
                    StoreSet(entityIndex, newStoredValue);
                    FireValueChanged(entity, oldValue, value);
                }
                else StoreSet(entityIndex, newStoredValue);
            }
        }

        /// <summary>
        /// convert type value to enum 
        /// </summary>
        protected abstract Enum StoredValueToEnum(T value);

        /// <summary>
        /// convert enum to type value  
        /// </summary>
        protected abstract T EnumToStoredValue(Enum value);

        //================================================================================================
        //                                              Configuration
        //================================================================================================
        /// <inheritdoc />
        public override string ConfigToString()
        {
            return JsonUtility.ToJson(new JsonConfig { EnumType = enumType.FullName });
        }

        /// <inheritdoc />
        public override void ConfigFromString(string config)
        {
            var typeName = JsonUtility.FromJson<JsonConfig>(config).EnumType;
            enumType = GetEnumType(this, UnderlyingType, typeName);
        }

        [Serializable]
        private struct JsonConfig
        {
            public string EnumType;
        }

        /// <inheritdoc />
        public override byte[] ConfigToBytes()
        {
            var writer = new BGBinaryWriter(20);
            //version
            writer.AddInt(1);
            //toId
            writer.AddString(enumType.FullName);

            return writer.ToArray();
        }

        /// <inheritdoc />
        public override void ConfigFromBytes(ArraySegment<byte> config)
        {
            var reader = new BGBinaryReader(config);
            var version = reader.ReadInt();
            switch (version)
            {
                case 1:
                {
                    var typeName = reader.ReadString();
                    enumType = GetEnumType(this, UnderlyingType, typeName);
                    break;
                }
                default:
                {
                    throw new BGException("Unknown version: $", version);
                }
            }
        }

        //================================================================================================
        //                                              Serialization
        //================================================================================================
        /// <inheritdoc />
        public override string ToString(int entityIndex) => Enum.GetName(EnumType, this[entityIndex]);

        /// <inheritdoc />
        public override void FromString(int entityIndex, string value)
        {
            if (string.IsNullOrEmpty(value)) this[entityIndex] = DefaultEnumValue;
            else
            {
                if (!Enum.IsDefined(EnumType, value)) throw new BGException("Invalid enum value $ for enum $, entity index=$", value, EnumType.FullName, entityIndex);
                this[entityIndex] = (Enum)Enum.Parse(EnumType, value);
            }
        }

        //================================================================================================
        //                                              enum Type
        //================================================================================================
        private static BGEnumTypeNameMapper Mapper
        {
            get
            {
                var type = BGUtil.GetType("BansheeGz.BGDatabase.Editor.BGEnumTypeNameMapperDefault");
                if (type == null) return null;
                return Activator.CreateInstance(type) as BGEnumTypeNameMapper;
            }
        }

        /// <summary>
        /// Method tries to load C# enum type from provided type name 
        /// </summary>
        public static Type GetEnumType(BGField field, Type underlyingType, string typeName)
        {
            var finalType = typeName;
            if (string.IsNullOrEmpty(finalType)) throw new BGException("Can not deserialize field $: enum type is not set!", field.FullName);
            var enumType = BGUtil.GetType(finalType);
            if (enumType == null)
            {
                var mapper = Mapper;
                var mappedType = mapper?.Map(finalType);
                if (mappedType != null)
                {
                    finalType = mappedType;
                    enumType = BGUtil.GetType(mappedType);
                    if (enumType == null) throw new BGException("Can not deserialize field $: both enum type $ and mapped enum type $ are not found!", field.FullName, typeName, mappedType);
                }
            }

            if (enumType == null) throw new BGException("Can not deserialize field $: enum type $ is not found!", field.FullName, finalType);
            if (!enumType.IsEnum) throw new BGException("Can not deserialize field $: enum type $ is not enum!", field.FullName, finalType);
            if (enumType.GetEnumUnderlyingType() != underlyingType)
                throw new BGException("Can not deserialize field $: enum type $ has wrong underlying type, expected $ found $ !",
                    field.FullName, finalType, underlyingType.FullName, enumType.GetEnumUnderlyingType().FullName);
            return enumType;
        }

        //================================================================================================
        //                                              Utilities
        //================================================================================================
        /// <summary>
        /// Check enum type and underlying type for possible errors
        /// </summary>
        public static string GetErrorForEnumType(Type enumType, Type targetUnderlyingType)
        {
            if (enumType == null) return "enumType can not be null";
            if (!enumType.IsEnum) return BGUtil.Format("enumType $ is not enum", enumType.FullName);

            var underlyingType = Enum.GetUnderlyingType(enumType);
            if (underlyingType != targetUnderlyingType)
                return BGUtil.Format("underlying type mismatch for enum $. Required type is $, but actual type is $",
                    enumType.FullName, targetUnderlyingType.FullName, underlyingType.FullName);

            return null;
        }
    }

    /// <summary>
    /// interface for enum field 
    /// </summary>
    public interface BGFieldEnumI
    {
        /// <summary>
        /// underlying enum type
        /// </summary>
        Type UnderlyingType { get; }

        /// <summary>
        /// enum type
        /// </summary>
        Type EnumType { get; set; }
    }
}