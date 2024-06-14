/*
<copyright file="BGMetaPartitionModelDefault.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Default table partitioning model
    /// </summary>
    public class BGMetaPartitionModelDefault : BGMetaPartitionModelA, BGMetaPartitionModelA.FieldOwner
    {
        private readonly BGPartitionFieldTypeEnum fieldType;
        private readonly BGField field;

        public BGPartitionFieldTypeEnum FieldType => fieldType;

        public BGField Field => field;

        public BGMetaPartitionModelDefault(BGField field) : base(field.Meta)
        {
            this.field = field;
            switch (field)
            {
                case BGFieldByte _:
                    fieldType = BGPartitionFieldTypeEnum.Byte;
                    break;
                case BGFieldShort _:
                    fieldType = BGPartitionFieldTypeEnum.Short;
                    break;
                case BGFieldInt _:
                    fieldType = BGPartitionFieldTypeEnum.Int;
                    break;
                case BGFieldByteNullable _:
                    fieldType = BGPartitionFieldTypeEnum.NullableByte;
                    break;
                case BGFieldShortNullable _:
                    fieldType = BGPartitionFieldTypeEnum.NullableShort;
                    break;
                case BGFieldIntNullable _:
                    fieldType = BGPartitionFieldTypeEnum.NullableInt;
                    break;
                case BGFieldRelationSingle _:
                    fieldType = BGPartitionFieldTypeEnum.Relation;
                    break;
            }
        }

        /// <inheritdoc />
        public override int? GetPartitionIndex(BGEntity entity)
        {
            switch (fieldType)
            {
                case BGPartitionFieldTypeEnum.Relation:
                {
                    var value = ((BGFieldRelationSingle)field)[entity.Index];
                    return value?.Index;
                }
                case BGPartitionFieldTypeEnum.Byte:
                {
                    var value = ((BGFieldByte)field)[entity.Index];
                    if (value == 0) return null;
                    return (int)value - 1;
                }
                case BGPartitionFieldTypeEnum.Short:
                {
                    var value = ((BGFieldShort)field)[entity.Index];
                    if (value == 0) return null;
                    return (int)value - 1;
                }
                case BGPartitionFieldTypeEnum.Int:
                {
                    var value = ((BGFieldInt)field)[entity.Index];
                    if (value == 0) return null;
                    return value - 1;
                }
                case BGPartitionFieldTypeEnum.NullableByte:
                {
                    var value = ((BGFieldByteNullable)field)[entity.Index];
                    if (value == null) return null;
                    return (int)value;
                }
                case BGPartitionFieldTypeEnum.NullableShort:
                {
                    var value = ((BGFieldShortNullable)field)[entity.Index];
                    if (value == null) return null;
                    return (int)value;
                }
                case BGPartitionFieldTypeEnum.NullableInt:
                {
                    var value = ((BGFieldIntNullable)field)[entity.Index];
                    if (value == null) return null;
                    return value;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(fieldType));
            }
        }
    }
}