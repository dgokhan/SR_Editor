/*
<copyright file="BGMTFieldEnum.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Multi-threaded field for enums
    /// </summary>
    public class BGMTFieldEnum : BGMTFieldEnumA<int>
    {
        internal BGMTFieldEnum(BGField field) : base(field)
        {
        }

        internal BGMTFieldEnum(BGMTMeta meta, BGMTFieldEnum otherField) : base(meta, otherField)
        {
        }

        internal override BGMTField DeepClone(BGMTMeta meta)
        {
            return new BGMTFieldEnum(meta, this);
        }

        protected internal override Enum this[int entityIndex]
        {
            get => (Enum)Enum.ToObject(enumType, GetStoredValue(entityIndex));
            set => SetStoredValue(entityIndex, Convert.ToInt32(value));
        }
    }
}