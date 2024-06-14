/*
<copyright file="BGMTFieldEnumA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// abstract Multi-threaded field for enums
    /// </summary>
    public abstract class BGMTFieldEnumA<T> : BGMTFieldCached<Enum, T> where T : struct, IComparable, IConvertible, IFormattable
    {
        protected Type enumType;

        protected BGMTFieldEnumA(BGField field) : base(field)
        {
            enumType = ((BGFieldEnumI)field).EnumType;
        }

        protected BGMTFieldEnumA(BGMTMeta meta, BGMTFieldEnumA<T> otherField) : base(meta, otherField)
        {
            enumType = otherField.enumType;
        }

        public override void CopyTo(BGField field, BGEntity entity, BGMTEntity fromEntity)
        {
            var relation = (BGFieldEnumA<T>)field;
            relation.SetStoredValue(entity.Index, GetStoredValue(fromEntity.Index));
        }
    }
}