/*
<copyright file="BGCalcUnitChangeType.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// change value C# type unit
    /// </summary>
    [BGCalcUnitDefinition("By type/object/Change type")]
    public class BGCalcUnitChangeType : BGCalcUnit
    {
        private BGCalcValueInput a;
        public const int Code = 96;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override void Definition()
        {
            a = ValueInput(BGCalcTypeCodeRegistry.Object, "A", "a");
            ValueOutput(BGCalcTypeCodeRegistry.Bool, "as bool", "b", flow => ChangeType(flow, System.TypeCode.Boolean));
            ValueOutput(BGCalcTypeCodeRegistry.Byte, "as byte", "c", flow => ChangeType(flow, System.TypeCode.Byte));
            ValueOutput(BGCalcTypeCodeRegistry.Float, "as float", "d", flow => ChangeType(flow, System.TypeCode.Single));
            ValueOutput(BGCalcTypeCodeRegistry.Int, "as int", "e", flow => ChangeType(flow, System.TypeCode.Int32));
            ValueOutput(BGCalcTypeCodeRegistry.Short, "as short", "f", flow => ChangeType(flow, System.TypeCode.Int16));
            ValueOutput(BGCalcTypeCodeRegistry.SByte, "as sbyte", "g", flow => ChangeType(flow, System.TypeCode.SByte));
            ValueOutput(BGCalcTypeCodeRegistry.UShort, "as ushort", "h", flow => ChangeType(flow, System.TypeCode.UInt16));
        }

        private object ChangeType(BGCalcFlowI flow, TypeCode typeCode) => Convert.ChangeType(flow.GetValue<object>(a), typeCode);
    }
}