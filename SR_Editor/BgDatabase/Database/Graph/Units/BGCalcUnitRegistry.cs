/*
<copyright file="BGCalcUnitRegistry.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    //this class should be generated with code generator
    /// <summary>
    /// Units registry for faster unit creation
    /// </summary>
    public static class BGCalcUnitRegistry
    {
        /// <summary>
        /// create a unit by its type code 
        /// </summary>
        public static BGCalcUnitI Create(ushort code)
        {
            switch (code)
            {
                // 1
                case BGCalcUnitGraphStart.Code:
                    return new BGCalcUnitGraphStart();
                //===============  by type/bool
                // 2
                case BGCalcUnitAnd.Code:
                    return new BGCalcUnitAnd();
                // 3
                case BGCalcUnitOr.Code:
                    return new BGCalcUnitOr();
                // 4
                case BGCalcUnitOrExclusive.Code:
                    return new BGCalcUnitOrExclusive();
                // 5
                case BGCalcUnitOrNegate.Code:
                    return new BGCalcUnitOrNegate();
                // 6
                case BGCalcUnitEqual.Code:
                    return new BGCalcUnitEqual();
                // 7
                case BGCalcUnitNotEqual.Code:
                    return new BGCalcUnitNotEqual();
                // 50
                case BGCalcUnitIsNull.Code:
                    return new BGCalcUnitIsNull();
                // 51
                case BGCalcUnitNull.Code:
                    return new BGCalcUnitNull();
                // 63
                case BGCalcUnitCast.Code:
                    return new BGCalcUnitCast();
                // 88
                case BGCalcUnitBoolParse.Code:
                    return new BGCalcUnitBoolParse();
                // 88
                case BGCalcUnitChangeType.Code:
                    return new BGCalcUnitChangeType();


                //===============  by type/int
                // 8
                case BGCalcUnitIntLess.Code:
                    return new BGCalcUnitIntLess();
                // 9
                case BGCalcUnitIntLessOrEqual.Code:
                    return new BGCalcUnitIntLessOrEqual();
                // 10
                case BGCalcUnitIntGreater.Code:
                    return new BGCalcUnitIntGreater();
                // 11
                case BGCalcUnitIntGreaterOrEqual.Code:
                    return new BGCalcUnitIntGreaterOrEqual();
                // 12
                case BGCalcUnitIntAdd.Code:
                    return new BGCalcUnitIntAdd();
                // 14
                case BGCalcUnitIntSubtract.Code:
                    return new BGCalcUnitIntSubtract();
                // 15
                case BGCalcUnitIntAbs.Code:
                    return new BGCalcUnitIntAbs();
                // 16
                case BGCalcUnitIntMultiply.Code:
                    return new BGCalcUnitIntMultiply();
                // 17
                case BGCalcUnitIntDivide.Code:
                    return new BGCalcUnitIntDivide();
                // 18
                case BGCalcUnitIntModulo.Code:
                    return new BGCalcUnitIntModulo();
                // 85
                case BGCalcUnitIntParse.Code:
                    return new BGCalcUnitIntParse();
                // 89
                case BGCalcUnitIntEqual.Code:
                    return new BGCalcUnitIntEqual();

                //===============  by type/float
                // 19
                case BGCalcUnitFloatLess.Code:
                    return new BGCalcUnitFloatLess();
                // 20
                case BGCalcUnitFloatLessOrEqual.Code:
                    return new BGCalcUnitFloatLessOrEqual();
                // 21
                case BGCalcUnitFloatGreater.Code:
                    return new BGCalcUnitFloatGreater();
                // 22
                case BGCalcUnitFloatGreaterOrEqual.Code:
                    return new BGCalcUnitFloatGreaterOrEqual();
                // 23
                case BGCalcUnitFloatAdd.Code:
                    return new BGCalcUnitFloatAdd();
                // 24
                case BGCalcUnitFloatSubtract.Code:
                    return new BGCalcUnitFloatSubtract();
                // 25
                case BGCalcUnitFloatAbs.Code:
                    return new BGCalcUnitFloatAbs();
                // 26
                case BGCalcUnitFloatMultiply.Code:
                    return new BGCalcUnitFloatMultiply();
                // 27
                case BGCalcUnitFloatDivide.Code:
                    return new BGCalcUnitFloatDivide();
                // 28
                case BGCalcUnitFloatModulo.Code:
                    return new BGCalcUnitFloatModulo();
                // 29
                case BGCalcUnitFloatCeil.Code:
                    return new BGCalcUnitFloatCeil();
                // 30
                case BGCalcUnitFloatCeilToInt.Code:
                    return new BGCalcUnitFloatCeilToInt();
                // 31
                case BGCalcUnitFloatFloor.Code:
                    return new BGCalcUnitFloatFloor();
                // 32
                case BGCalcUnitFloatFloorToInt.Code:
                    return new BGCalcUnitFloatFloorToInt();
                // 33
                case BGCalcUnitFloatApproximate.Code:
                    return new BGCalcUnitFloatApproximate();
                // 34
                case BGCalcUnitFloatSin.Code:
                    return new BGCalcUnitFloatSin();
                // 35
                case BGCalcUnitFloatCos.Code:
                    return new BGCalcUnitFloatCos();
                // 36
                case BGCalcUnitFloatTan.Code:
                    return new BGCalcUnitFloatTan();
                // 37
                case BGCalcUnitFloatAsin.Code:
                    return new BGCalcUnitFloatAsin();
                // 38
                case BGCalcUnitFloatAcos.Code:
                    return new BGCalcUnitFloatAcos();
                // 39
                case BGCalcUnitFloatAtan.Code:
                    return new BGCalcUnitFloatAtan();
                // 40
                case BGCalcUnitFloatAtan2.Code:
                    return new BGCalcUnitFloatAtan2();
                // 41
                case BGCalcUnitFloatDeg2Rad.Code:
                    return new BGCalcUnitFloatDeg2Rad();
                // 42
                case BGCalcUnitFloatRad2Deg.Code:
                    return new BGCalcUnitFloatRad2Deg();
                // 43
                case BGCalcUnitFloatConstantPi.Code:
                    return new BGCalcUnitFloatConstantPi();
                // 44
                case BGCalcUnitFloatConstantInfinity.Code:
                    return new BGCalcUnitFloatConstantInfinity();
                // 45
                case BGCalcUnitFloatConstantInfinityNegative.Code:
                    return new BGCalcUnitFloatConstantInfinityNegative();
                // 46
                case BGCalcUnitFloatConstantEpsilon.Code:
                    return new BGCalcUnitFloatConstantEpsilon();
                // 47
                case BGCalcUnitFloatSqrt.Code:
                    return new BGCalcUnitFloatSqrt();
                // 48
                case BGCalcUnitFloatPow.Code:
                    return new BGCalcUnitFloatPow();
                // 49
                case BGCalcUnitFloatRoundToInt.Code:
                    return new BGCalcUnitFloatRoundToInt();
                // 86
                case BGCalcUnitFloatParse.Code:
                    return new BGCalcUnitFloatParse();
                // 87
                case BGCalcUnitFloatConstantNan.Code:
                    return new BGCalcUnitFloatConstantNan();
                // 90
                case BGCalcUnitFloatEqual.Code:
                    return new BGCalcUnitFloatEqual();

                //===============  by type/string
                // 52
                case BGCalcUnitStringToLower.Code:
                    return new BGCalcUnitStringToLower();
                // 53
                case BGCalcUnitStringToUpper.Code:
                    return new BGCalcUnitStringToUpper();
                // 54
                case BGCalcUnitStringSubString.Code:
                    return new BGCalcUnitStringSubString();
                // 55
                case BGCalcUnitStringSubString2.Code:
                    return new BGCalcUnitStringSubString2();
                // 56
                case BGCalcUnitStringIndexOf.Code:
                    return new BGCalcUnitStringIndexOf();
                // 57
                case BGCalcUnitStringAdd.Code:
                    return new BGCalcUnitStringAdd();
                // 58
                case BGCalcUnitStringTrim.Code:
                    return new BGCalcUnitStringTrim();
                // 59
                case BGCalcUnitStringSplit.Code:
                    return new BGCalcUnitStringSplit();
                // 69
                case BGCalcUnitStringJoin.Code:
                    return new BGCalcUnitStringJoin();
                // 81
                case BGCalcUnitObjectToString.Code:
                    return new BGCalcUnitObjectToString();
                // 84
                case BGCalcUnitStringIsNullOrEmpty.Code:
                    return new BGCalcUnitStringIsNullOrEmpty();
                // 95
                case BGCalcUnitStringLength.Code:
                    return new BGCalcUnitStringLength();


                //===============  by type/list
                // 60
                case BGCalcUnitListCreate.Code:
                    return new BGCalcUnitListCreate();
                // 61
                case BGCalcUnitListCount.Code:
                    return new BGCalcUnitListCount();
                // 62
                case BGCalcUnitListGet.Code:
                    return new BGCalcUnitListGet();
                // 64
                case BGCalcUnitListRemoveAt.Code:
                    return new BGCalcUnitListRemoveAt();
                // 65
                case BGCalcUnitListAdd.Code:
                    return new BGCalcUnitListAdd();
                // 66
                case BGCalcUnitListClear.Code:
                    return new BGCalcUnitListClear();
                // 67
                case BGCalcUnitListInsert.Code:
                    return new BGCalcUnitListInsert();
                // 68
                case BGCalcUnitListIndexOf.Code:
                    return new BGCalcUnitListIndexOf();
                // 82
                case BGCalcUnitListAddRange.Code:
                    return new BGCalcUnitListAddRange();
                // 83
                case BGCalcUnitListContains.Code:
                    return new BGCalcUnitListContains();
                // 91
                case BGCalcUnitListSet.Code:
                    return new BGCalcUnitListSet();
                // 92
                case BGCalcUnitListRemove.Code:
                    return new BGCalcUnitListRemove();

                //===============  by type/meta
                // 70
                case BGCalcUnitGetMeta.Code:
                    return new BGCalcUnitGetMeta();

                //===============  by type/field
                // 71
                case BGCalcUnitGetField.Code:
                    return new BGCalcUnitGetField();

                //===============  by type/entity
                // 72
                case BGCalcUnitGetEntity.Code:
                    return new BGCalcUnitGetEntity();
                // 79
                case BGCalcUnitCreateEntity.Code:
                    return new BGCalcUnitCreateEntity();
                // 80
                case BGCalcUnitDeleteEntity.Code:
                    return new BGCalcUnitDeleteEntity();

                //===============  by type/cell
                // 73
                case BGCalcUnitCellGetValue.Code:
                    return new BGCalcUnitCellGetValue();
                // 74
                case BGCalcUnitCellSetValue.Code:
                    return new BGCalcUnitCellSetValue();
                // 75
                case BGCalcUnitCellGetValue2.Code:
                    return new BGCalcUnitCellGetValue2();
                // 76
                case BGCalcUnitCellSetValue2.Code:
                    return new BGCalcUnitCellSetValue2();
                // 77
                case BGCalcUnitConstructCell.Code:
                    return new BGCalcUnitConstructCell();
                // 78
                case BGCalcUnitDeconstructCell.Code:
                    return new BGCalcUnitDeconstructCell();

                //===============  enum
                // 93
                case BGCalcUnitEnumLiteral.Code:
                    return new BGCalcUnitEnumLiteral();
                // 94
                case BGCalcUnitEnumToInt.Code:
                    return new BGCalcUnitEnumToInt();


                //===============  misc
                //100
                // case BGCalcUnitTest.Code:
                // return new BGCalcUnitTest();
                //101
                case BGCalcUnitDebug.Code:
                    return new BGCalcUnitDebug();
                //102
                case BGCalcUnitFor.Code:
                    return new BGCalcUnitFor();
                //103
                case BGCalcUnitLiteral.Code:
                    return new BGCalcUnitLiteral();
                //106
                case BGCalcUnitIf.Code:
                    return new BGCalcUnitIf();
                //107
                case BGCalcUnitWhile.Code:
                    return new BGCalcUnitWhile();
                //108
                case BGCalcUnitForEach.Code:
                    return new BGCalcUnitForEach();
                //116
                case BGCalcUnitGetVar.Code:
                    return new BGCalcUnitGetVar();
                //117
                case BGCalcUnitSetVar.Code:
                    return new BGCalcUnitSetVar();
                //118
                case BGCalcUnitBreak.Code:
                    return new BGCalcUnitBreak();

                //===============  DB
                //104
                case BGCalcUnitDbGet.Code:
                    return new BGCalcUnitDbGet();
                //105
                case BGCalcUnitDbCount.Code:
                    return new BGCalcUnitDbCount();
                //109
                case BGCalcUnitDbSet.Code:
                    return new BGCalcUnitDbSet();
                //119
                case BGCalcUnitGetFieldMeta.Code:
                    return new BGCalcUnitGetFieldMeta();
                //120
                case BGCalcUnitGetEntityMeta.Code:
                    return new BGCalcUnitGetEntityMeta();
                //121
                case BGCalcUnitGetMetaFields.Code:
                    return new BGCalcUnitGetMetaFields();
                //122
                case BGCalcUnitGetMetaEntities.Code:
                    return new BGCalcUnitGetMetaEntities();
                //124
                case BGCalcUnitCountEntities.Code:
                    return new BGCalcUnitCountEntities();

                //special
                //110
                case BGCalcUnitSetResult.Code:
                    return new BGCalcUnitSetResult();
                //111
                case BGCalcUnitGetCurrentEntity.Code:
                    return new BGCalcUnitGetCurrentEntity();
                //112
                case BGCalcUnitGetCurrentLocale.Code:
                    return new BGCalcUnitGetCurrentLocale();
                //113
                case BGCalcUnitSetCurrentLocale.Code:
                    return new BGCalcUnitSetCurrentLocale();
                //114
                case BGCalcUnitGetLocalizedValue.Code:
                    return new BGCalcUnitGetLocalizedValue();
                //115
                case BGCalcUnitCallCalculated.Code:
                    return new BGCalcUnitCallCalculated();
                //123
                case BGCalcUnitVoid.Code:
                    return new BGCalcUnitVoid();
                //125
                case BGCalcUnitGetCurrentGameObject.Code:
                    return new BGCalcUnitGetCurrentGameObject();

                //unity
                //126
                case BGCalcUnitGetComponent.Code:
                    return new BGCalcUnitGetComponent();
                //127
                case BGCalcUnitGetComponents.Code:
                    return new BGCalcUnitGetComponents();
                //128
                case BGCalcUnitGetGameObject.Code:
                    return new BGCalcUnitGetGameObject();

                //reflection
                //129
                case BGCalcUnitReflectionGetFieldOrProperty.Code:
                    return new BGCalcUnitReflectionGetFieldOrProperty();
                //130
                case BGCalcUnitReflectionGetStaticFieldOrProperty.Code:
                    return new BGCalcUnitReflectionGetStaticFieldOrProperty();
                //131
                case BGCalcUnitReflectionSetFieldOrProperty.Code:
                    return new BGCalcUnitReflectionSetFieldOrProperty();
                //132
                case BGCalcUnitReflectionSetStaticFieldOrProperty.Code:
                    return new BGCalcUnitReflectionSetStaticFieldOrProperty();

                //and or groups
                //133
                case BGCalcUnitAndGroup.Code:
                    return new BGCalcUnitAndGroup();
                //134
                case BGCalcUnitOrGroup.Code:
                    return new BGCalcUnitOrGroup();

                //135
                case BGCalcUnitIntMin.Code:
                    return new BGCalcUnitIntMin();
                //136
                case BGCalcUnitIntMax.Code:
                    return new BGCalcUnitIntMax();
                //137
                case BGCalcUnitFloatMin.Code:
                    return new BGCalcUnitFloatMin();
                //138
                case BGCalcUnitFloatMax.Code:
                    return new BGCalcUnitFloatMax();
                //139
                case BGCalcUnitStringJoin2.Code:
                    return new BGCalcUnitStringJoin2();

                //140
                case BGCalcUnitGetBySingleRelation.Code:
                    return new BGCalcUnitGetBySingleRelation();
                //141
                case BGCalcUnitGetByMultipleRelation.Code:
                    return new BGCalcUnitGetByMultipleRelation();
                //142
                case BGCalcUnitGetByRelationInbound.Code:
                    return new BGCalcUnitGetByRelationInbound();

                default:
                    throw new Exception($"Can not create a unit: Unknown type code {code}");
            }
        }
    }
}