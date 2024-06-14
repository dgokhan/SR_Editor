/*
<copyright file="BGCalcTypeCodeRegistry.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// type code registry 
    /// </summary>
    public static class BGCalcTypeCodeRegistry
    {
        public static readonly BGCalcTypeCodeControl Control = new BGCalcTypeCodeControl();
        public static readonly BGCalcTypeCodeBool Bool = new BGCalcTypeCodeBool();
        public static readonly BGCalcTypeCodeString String = new BGCalcTypeCodeString();
        public static readonly BGCalcTypeCodeInt Int = new BGCalcTypeCodeInt();
        public static readonly BGCalcTypeCodeFloat Float = new BGCalcTypeCodeFloat();
        public static readonly BGCalcTypeCodeBGId BGId = new BGCalcTypeCodeBGId();

        public static readonly BGCalcTypeCodeSource EntitySource = new BGCalcTypeCodeSource();

        // public static readonly BGCalcVarTypeCode Enum = new BGCalcVarTypeCodeEnum();// <-- enum is stateful
        public static readonly BGCalcTypeCodeObject Object = new BGCalcTypeCodeObject();
        public static readonly BGCalcTypeCodeList List = new BGCalcTypeCodeList();
        public static readonly BGCalcTypeCodeMeta Meta = new BGCalcTypeCodeMeta();
        public static readonly BGCalcTypeCodeField Field = new BGCalcTypeCodeField();
        public static readonly BGCalcTypeCodeEntity Entity = new BGCalcTypeCodeEntity();
        public static readonly BGCalcTypeCodeCell Cell = new BGCalcTypeCodeCell();
        public static readonly BGCalcTypeCodeByte Byte = new BGCalcTypeCodeByte();
        public static readonly BGCalcTypeCodeShort Short = new BGCalcTypeCodeShort();
        public static readonly BGCalcTypeCodeSByte SByte = new BGCalcTypeCodeSByte();
        public static readonly BGCalcTypeCodeUShort UShort = new BGCalcTypeCodeUShort();
        public static readonly BGCalcTypeCodeVector2 Vector2 = new BGCalcTypeCodeVector2();
        public static readonly BGCalcTypeCodeVector3 Vector3 = new BGCalcTypeCodeVector3();
        public static readonly BGCalcTypeCodeVector4 Vector4 = new BGCalcTypeCodeVector4();
        public static readonly BGCalcTypeCodeGameObject GameObject = new BGCalcTypeCodeGameObject();
        public static readonly BGCalcTypeCodeComponent Component = new BGCalcTypeCodeComponent();
        public static readonly BGCalcTypeCodeCalcAction CalcAction = new BGCalcTypeCodeCalcAction();


        /// <summary>
        /// all supported type codes
        /// </summary>
        public static BGCalcTypeCode[] TypeCodes
        {
            get
            {
                return new BGCalcTypeCode[]
                {
                    Control,
                    Bool,
                    String,
                    Int,
                    Float,
                    BGId,
                    new BGCalcTypeCodeEnum(),
                    new BGCalcTypeCodeEntityRuntime(),
                    EntitySource,
                    Object,
                    List,
                    Meta,
                    Field,
                    Entity,
                    Cell,
                    Byte,
                    Short,
                    SByte,
                    UShort,
                    Vector2,
                    Vector3,
                    Vector4,
                    GameObject,
                    Component,
                    CalcAction
                };
            }
        }

        /// <summary>
        /// convert byte value to type code object
        /// </summary>
        public static BGCalcTypeCode Get(byte code)
        {
            switch (code)
            {
                //1
                case BGCalcTypeCodeControl.Code:
                {
                    return Control;
                }
                //2
                case BGCalcTypeCodeBool.Code:
                {
                    return Bool;
                }
                //3
                case BGCalcTypeCodeString.Code:
                {
                    return String;
                }
                //4
                case BGCalcTypeCodeInt.Code:
                {
                    return Int;
                }
                //5
                case BGCalcTypeCodeFloat.Code:
                {
                    return Float;
                }
                //6
                case BGCalcTypeCodeBGId.Code:
                {
                    return BGId;
                }
                //7
                case BGCalcTypeCodeEnum.Code:
                {
                    return new BGCalcTypeCodeEnum();
                }
                //8
                case BGCalcTypeCodeEntityRuntime.Code:
                {
                    return new BGCalcTypeCodeEntityRuntime();
                }
                //9
                case BGCalcTypeCodeSource.Code:
                {
                    return new BGCalcTypeCodeSource();
                }
                //10
                case BGCalcTypeCodeObject.Code:
                {
                    return Object;
                }
                //11
                case BGCalcTypeCodeList.Code:
                {
                    return List;
                }
                //12
                case BGCalcTypeCodeMeta.Code:
                {
                    return Meta;
                }
                //14
                case BGCalcTypeCodeField.Code:
                {
                    return Field;
                }
                //15
                case BGCalcTypeCodeEntity.Code:
                {
                    return Entity;
                }
                //16
                case BGCalcTypeCodeCell.Code:
                {
                    return Cell;
                }
                //17
                case BGCalcTypeCodeByte.Code:
                {
                    return Byte;
                }
                //18
                case BGCalcTypeCodeShort.Code:
                {
                    return Byte;
                }
                //19
                case BGCalcTypeCodeSByte.Code:
                {
                    return SByte;
                }
                //20
                case BGCalcTypeCodeUShort.Code:
                {
                    return UShort;
                }
                //21
                case BGCalcTypeCodeVector2.Code:
                {
                    return Vector2;
                }
                //22
                case BGCalcTypeCodeVector3.Code:
                {
                    return Vector3;
                }
                //23
                case BGCalcTypeCodeVector4.Code:
                {
                    return Vector4;
                }
                //24
                case BGCalcTypeCodeGameObject.Code:
                {
                    return GameObject;
                }
                //25
                case BGCalcTypeCodeComponent.Code:
                {
                    return Component;
                }
                //26
                case BGCalcTypeCodeCalcAction.Code:
                {
                    return CalcAction;
                }

                default:
                    throw new Exception($"unknown type code {code}");
            }
        }

        /// <summary>
        /// convert type to type code object
        /// </summary>
        public static BGCalcTypeCode Get(Type type)
        {
            BGCalcTypeCode result = null;
            switch (type.FullName)
            {
                case "BansheeGz.BGDatabase.BGCalcControl":
                {
                    result = Control;
                    break;
                }
                case "System.Boolean":
                {
                    result = Bool;
                    break;
                }
                case "System.String":
                {
                    result = String;
                    break;
                }
                case "System.Int32":
                {
                    result = Int;
                    break;
                }
                case "System.Single":
                {
                    result = Float;
                    break;
                }
                case "BansheeGz.Database.BGId":
                {
                    result = BGId;
                    break;
                }
                //do not uncomment it!
                /*
                case "System.Enum":
                {
                    result = new BGCalcVarTypeCodeEnum();
                    break;
                }
                case "BansheeGz.Database.BGEntity":
                {
                    result = new BGCalcVarTypeCodeEntity();
                    break;
                }
            */
                case "BansheeGz.Database.BGCalcVarTypeCodeEnum":
                {
                    result = EntitySource;
                    break;
                }
                case "System.Object":
                {
                    result = Object;
                    break;
                }
                case "System.Collections.IList":
                {
                    result = List;
                    break;
                }
                case "BansheeGz.Database.BGMetaEntity":
                {
                    result = Meta;
                    break;
                }
                case "BansheeGz.Database.BGField":
                {
                    result = Field;
                    break;
                }
                case "BansheeGz.Database.BGEntity":
                {
                    result = Entity;
                    break;
                }
                case "BansheeGz.Database.BGCalcCell":
                {
                    result = Cell;
                    break;
                }
                case "System.Byte":
                {
                    result = Byte;
                    break;
                }
                case "System.Int16":
                {
                    result = Short;
                    break;
                }
                case "System.SByte":
                {
                    result = SByte;
                    break;
                }
                case "System.UInt16":
                {
                    result = UShort;
                    break;
                }
                case "UnityEngine.Vector2":
                {
                    result = Vector2;
                    break;
                }
                case "UnityEngine.Vector3":
                {
                    result = Vector3;
                    break;
                }
                case "UnityEngine.Vector4":
                {
                    result = Vector4;
                    break;
                }
                case "UnityEngine.GameObject":
                {
                    result = GameObject;
                    break;
                }
                case "UnityEngine.Component":
                {
                    result = Component;
                    break;
                }
                case "BansheeGz.BGDatabase.BGFieldCalcActionValue":
                {
                    result = CalcAction;
                    break;
                }
            }

            return result;
        }

        public static List<BGCalcTypeCode> Find(Predicate<BGCalcTypeCode> filter)
        {
            var result = new List<BGCalcTypeCode>();
            foreach (var typeCode in TypeCodes)
            {
                if (filter != null && !filter(typeCode)) continue;
                result.Add(typeCode);
            }

            return result;
        }
    }
}