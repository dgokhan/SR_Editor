/*
<copyright file="BGPartitionFieldTypeEnum.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// All supported partitioning field types
    /// </summary>
    public enum BGPartitionFieldTypeEnum
    {
        Relation,
        Byte,
        Short,
        Int,
        NullableByte,
        NullableShort,
        NullableInt
    }
}