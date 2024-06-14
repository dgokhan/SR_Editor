/*
<copyright file="BGMTFieldEnumByte.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Multi-threaded field for byte enums
    /// </summary>
    public class BGMTFieldEnumByte : BGMTFieldEnumA<byte>
    {
        internal BGMTFieldEnumByte(BGField field) : base(field)
        {
        }

        internal BGMTFieldEnumByte(BGMTMeta meta, BGMTFieldEnumByte otherField) : base(meta, otherField)
        {
        }

        internal override BGMTField DeepClone(BGMTMeta meta)
        {
            return new BGMTFieldEnumByte(meta, this);
        }

        protected internal override Enum this[int entityIndex]
        {
            get => (Enum)Enum.ToObject(enumType, GetStoredValue(entityIndex));
            set => SetStoredValue(entityIndex, Convert.ToByte(value));
        }
    }
}