/*
<copyright file="BGMTFieldEnumShort.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Multi-threaded field for short enums
    /// </summary>
    public class BGMTFieldEnumShort : BGMTFieldEnumA<short>
    {
        internal BGMTFieldEnumShort(BGField field) : base(field)
        {
        }

        internal BGMTFieldEnumShort(BGMTMeta meta, BGMTFieldEnumShort otherField) : base(meta, otherField)
        {
        }

        internal override BGMTField DeepClone(BGMTMeta meta)
        {
            return new BGMTFieldEnumShort(meta, this);
        }

        protected internal override Enum this[int entityIndex]
        {
            get => (Enum)Enum.ToObject(enumType, GetStoredValue(entityIndex));
            set => SetStoredValue(entityIndex, Convert.ToInt16(value));
        }
    }
}