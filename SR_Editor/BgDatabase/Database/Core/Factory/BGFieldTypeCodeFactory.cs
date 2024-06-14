/*
<copyright file="BGFieldTypeCodeFactory.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Registry for mapping field code to field type
    /// PROPOSED CHANGE: This class should be auto-generated with code generator somehow
    /// </summary>
    public class BGFieldTypeCodeFactory : BGFieldTypeCodeFactory.BGFieldTypeCodeFactoryI
    {
        //singleton object
        public static readonly BGFieldTypeCodeFactory Instance = new BGFieldTypeCodeFactory();

        private BGFieldTypeCodeFactory()
        {
        }

        /// <summary>
        /// Create database field using provided type code and parameters
        /// </summary>
        public BGField Create(BGMetaEntity meta, ushort typeCode, BGId id, string name, ArraySegment<byte> config, bool system, string addon, string defaultValue, bool required)
        {
            var field = Create(meta, typeCode, id, name);
            if (field == null) throw new Exception($"Can not create a field: unsupported field type code={typeCode}!");

            field.DefaultValue = defaultValue;
            field.System = system;
            field.Addon = addon;
            field.Required = required;
            field.ConfigFromBytes(config);
            return field;
        }

        /// <inheritdoc/>
        public BGField Create(BGMetaEntity meta, ushort typeCode, BGId id, string name)
        {
            BGField field;
            switch (typeCode)
            {
                case BGFieldArrayByte.CodeType:
                {
                    field = new BGFieldArrayByte(meta, id, name);
                    break;
                }
                case BGFieldHashtable.CodeType:
                {
                    field = new BGFieldHashtable(meta, id, name);
                    break;
                }
                case BGFieldEnum.CodeType:
                {
                    field = new BGFieldEnum(meta, id, name);
                    break;
                }
                case BGFieldEnumByte.CodeType:
                {
                    field = new BGFieldEnumByte(meta, id, name);
                    break;
                }
                case BGFieldEnumList.CodeType:
                {
                    field = new BGFieldEnumList(meta, id, name);
                    break;
                }
                case BGFieldEnumShort.CodeType:
                {
                    field = new BGFieldEnumShort(meta, id, name);
                    break;
                }
                case BGFieldListBool.CodeType:
                {
                    field = new BGFieldListBool(meta, id, name);
                    break;
                }
                case BGFieldListDouble.CodeType:
                {
                    field = new BGFieldListDouble(meta, id, name);
                    break;
                }
                case BGFieldListFloat.CodeType:
                {
                    field = new BGFieldListFloat(meta, id, name);
                    break;
                }
                case BGFieldListGuid.CodeType:
                {
                    field = new BGFieldListGuid(meta, id, name);
                    break;
                }
                case BGFieldListInt.CodeType:
                {
                    field = new BGFieldListInt(meta, id, name);
                    break;
                }
                case BGFieldListLong.CodeType:
                {
                    field = new BGFieldListLong(meta, id, name);
                    break;
                }
                case BGFieldListString.CodeType:
                {
                    field = new BGFieldListString(meta, id, name);
                    break;
                }
                case BGFieldBool.CodeType:
                {
                    field = new BGFieldBool(meta, id, name);
                    break;
                }
                case BGFieldByte.CodeType:
                {
                    field = new BGFieldByte(meta, id, name);
                    break;
                }
                case BGFieldDecimal.CodeType:
                {
                    field = new BGFieldDecimal(meta, id, name);
                    break;
                }
                case BGFieldDouble.CodeType:
                {
                    field = new BGFieldDouble(meta, id, name);
                    break;
                }
                case BGFieldFloat.CodeType:
                {
                    field = new BGFieldFloat(meta, id, name);
                    break;
                }
                case BGFieldGuid.CodeType:
                {
                    field = new BGFieldGuid(meta, id, name);
                    break;
                }
                case BGFieldInt.CodeType:
                {
                    field = new BGFieldInt(meta, id, name);
                    break;
                }
                case BGFieldLong.CodeType:
                {
                    field = new BGFieldLong(meta, id, name);
                    break;
                }
                case BGFieldShort.CodeType:
                {
                    field = new BGFieldShort(meta, id, name);
                    break;
                }
                case BGFieldString.CodeType:
                {
                    field = new BGFieldString(meta, id, name);
                    break;
                }
                case BGFieldText.CodeType:
                {
                    field = new BGFieldText(meta, id, name);
                    break;
                }
                case BGFieldBoolNullable.CodeType:
                {
                    field = new BGFieldBoolNullable(meta, id, name);
                    break;
                }
                case BGFieldDoubleNullable.CodeType:
                {
                    field = new BGFieldDoubleNullable(meta, id, name);
                    break;
                }
                case BGFieldFloatNullable.CodeType:
                {
                    field = new BGFieldFloatNullable(meta, id, name);
                    break;
                }
                case BGFieldGuidNullable.CodeType:
                {
                    field = new BGFieldGuidNullable(meta, id, name);
                    break;
                }
                case BGFieldIntNullable.CodeType:
                {
                    field = new BGFieldIntNullable(meta, id, name);
                    break;
                }
                case BGFieldLongNullable.CodeType:
                {
                    field = new BGFieldLongNullable(meta, id, name);
                    break;
                }
                case BGFieldManyRelationsMultiple.CodeType:
                {
                    field = new BGFieldManyRelationsMultiple(meta, id, name);
                    break;
                }
                case BGFieldManyRelationsSingle.CodeType:
                {
                    field = new BGFieldManyRelationsSingle(meta, id, name);
                    break;
                }
                case BGFieldNested.CodeType:
                {
                    field = new BGFieldNested(meta, id, name);
                    break;
                }
                case BGFieldRelationMultiple.CodeType:
                {
                    field = new BGFieldRelationMultiple(meta, id, name);
                    break;
                }
                case BGFieldRelationSingle.CodeType:
                {
                    field = new BGFieldRelationSingle(meta, id, name);
                    break;
                }
                case BGFieldEntityName.CodeType:
                {
                    field = new BGFieldEntityName(meta, id, name);
                    break;
                }
                case BGFieldId.CodeType:
                {
                    field = new BGFieldId(meta, id, name);
                    break;
                }
                case BGFieldViewRelationSingle.CodeType:
                {
                    field = new BGFieldViewRelationSingle(meta, id, name);
                    break;
                }
                case BGFieldViewRelationMultiple.CodeType:
                {
                    field = new BGFieldViewRelationMultiple(meta, id, name);
                    break;
                }
                case BGFieldByteNullable.CodeType:
                {
                    field = new BGFieldByteNullable(meta, id, name);
                    break;
                }
                case BGFieldShortNullable.CodeType:
                {
                    field = new BGFieldShortNullable(meta, id, name);
                    break;
                }

                case BGFieldMetaReference.CodeType:
                {
                    field = new BGFieldMetaReference(meta, id, name);
                    break;
                }

                //BEFORE ADDING NEW CODES- MAKE SURE THEY DO NOT COLLIDE WITH LOCALIZATION CODES!!!!! (80-95)
                default:
                {
                    field = BGLocalizationUglyHacks.LocalizationFieldFactory?.Create(meta, typeCode, id, name);
                    break;
                }
            }

            return field;
        }

        /// <summary>
        /// Custom interface, providing additional field type code to field type mapping
        /// </summary>
        public interface BGFieldTypeCodeFactoryI
        {
            /// <summary>
            /// Create database field, using provided parameters
            /// </summary>
            BGField Create(BGMetaEntity meta, ushort typeCode, BGId id, string name);
        }
    }
}