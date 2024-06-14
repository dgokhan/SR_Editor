/*
<copyright file="BGFieldEnumListA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// abstract field with list of enums value (underlying type for enum is int)
    /// </summary>
    public abstract class BGFieldEnumListA<T> : BGFieldCachedClassListA<Enum>, BGFieldWithCustomConfigI where T : struct, IComparable, IConvertible, IFormattable
    {
        protected virtual char[] StringValueSeparator => AA;

        private Type enumType;
        private HashSet<Enum> constants;
        private BGFieldEnumListModeEnum mode;
        private bool allowDuplicates;
        public Type UnderlyingType => typeof(T);

        public BGFieldEnumListModeEnum Mode
        {
            get => mode;
            set
            {
                if (mode == value) return;
                mode = value;
                events.MetaWasChanged(Meta);
            }
        }
   
        /// <summary>
        /// enum type 
        /// </summary>
        public Type EnumType
        {
            get => enumType;
            set
            {
                var error = BGFieldEnum.GetErrorForEnumType(value, UnderlyingType);
                if (!string.IsNullOrEmpty(error)) throw new BGException("Can not change enum Type:" + error);

                if (enumType == value) return;
                //try to resolve values with new enum
                var meta = Meta;
                for (var i = 0; i < meta.CountEntities; i++)
                {
                    var storedValue = GetStoredValue(i);
                    if (storedValue == null || storedValue.Count == 0) continue;
                    foreach (var @enum in storedValue)
                    {
                        if (Enum.IsDefined(value, @enum)) continue;

                        var enumValue = "[enum name is not resolvable]";
                        try
                        {
                            enumValue = Enum.GetName(enumType, @enum);
                        }
                        catch
                        {
                            //ignore
                        }

                        throw new BGException("Can not resolve value with provided enum type $: entity index=$, entity name=$, not resolvable enum value=$(enum index=$)",
                            value.FullName, i, meta[i].Name, enumValue, @enum);
                    }
                }

                enumType = value;
                FillConstants();
                events.MetaWasChanged(Meta);
            }
        }
        //================================================================================================
        //                                              Constructors
        //================================================================================================

        protected BGFieldEnumListA(BGMetaEntity meta, string name, Type enumType) : base(meta, name)
        {
            var error = BGFieldEnum.GetErrorForEnumType(enumType, UnderlyingType);
            if (!string.IsNullOrEmpty(error))
            {
                Meta.Unregister(this);
                throw new BGException(error);
            }

            this.enumType = enumType;
            FillConstants();
        }

        protected BGFieldEnumListA(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Values
        //================================================================================================
        /// <inheritdoc />
        public override List<Enum> this[int entityIndex]
        {
            set
            {
                //check for non valid constants
                if (value != null && value.Count > 0 && constants != null && constants.Count > 0)
                {
                    for (var i = value.Count - 1; i >= 0; i--)
                    {
                        var @enum = value[i];
                        if (!constants.Contains(@enum)) value.RemoveAt(i);
                    }
                }

                base[entityIndex] = value;
            }
        }

        //================================================================================================
        //                                              Configuration
        //================================================================================================
        /// <inheritdoc />
        public override string ConfigToString() => JsonUtility.ToJson(new JsonConfig
        {
            EnumType = enumType.FullName,
            Mode = mode,
        });

        /// <inheritdoc />
        public override void ConfigFromString(string config)
        {
            var jsonConfig = JsonUtility.FromJson<JsonConfig>(config);
            var typeName = jsonConfig.EnumType;
            mode = jsonConfig.Mode;
            enumType = BGFieldEnumA<int>.GetEnumType(this, UnderlyingType, typeName);
            FillConstants();
        }

        [Serializable]
        private struct JsonConfig
        {
            public string EnumType;
            public BGFieldEnumListModeEnum Mode;
        }

        /// <inheritdoc />
        public override byte[] ConfigToBytes()
        {
            var writer = new BGBinaryWriter(20);
            //version
            writer.AddInt(2);
            
            //enum type name
            writer.AddString(enumType.FullName);

            //version 2
            writer.AddByte((byte)mode);

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
                case 2:
                {
                    var typeName = reader.ReadString();
                    enumType = BGFieldEnumA<int>.GetEnumType(this, UnderlyingType, typeName);
                    if (version == 2) mode = (BGFieldEnumListModeEnum) reader.ReadByte();
                    FillConstants();
                    break;
                }
                default:
                {
                    throw new BGException("Unknown version: $", version);
                }
            }
        }

        private void FillConstants()
        {
            if (enumType == null) return;
            var array = Enum.GetValues(enumType);
            if (array.Length > 0)
            {
                constants = new HashSet<Enum>();
                foreach (var obj in array) constants.Add((Enum)obj);
            }
        }

        /*
        private void EnsureEnumTypeNotNull(string typeName)
        {
            if (enumType != null) return;

            if (string.IsNullOrEmpty(typeName)) throw new BGException("Can not deserialize field $: enum type is not set!", FullName);
            throw new BGException("Can not deserialize field $: enum type $ is not found!", FullName, typeName);
        }
    */
    }

    public enum BGFieldEnumListModeEnum : byte
    {
        Flags = 0,
        List = 1,
        ListWithDuplicates = 2,
    }
}