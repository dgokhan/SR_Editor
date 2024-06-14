/*
<copyright file="BGCalcUnitCast.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// cast object to specified type unit
    /// </summary>
    [BGCalcUnitDefinition("By type/object/Cast")]
    public class BGCalcUnitCast : BGCalcUnit
    {
        private BGCalcValueInput a;
        public const int Code = 63;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override void Definition()
        {
            a = ValueInput(BGCalcTypeCodeRegistry.Object, "A", "a");
            ValueOutput(BGCalcTypeCodeRegistry.Bool, "as bool", "b", flow => (bool)flow.GetValue<object>(a));
            ValueOutput(BGCalcTypeCodeRegistry.String, "as string", "c", flow => (string)flow.GetValue<object>(a));
            ValueOutput(BGCalcTypeCodeRegistry.Int, "as int", "d", flow => (int)flow.GetValue<object>(a));
            ValueOutput(BGCalcTypeCodeRegistry.Float, "as float", "e", flow => (float)flow.GetValue<object>(a));
            ValueOutput(BGCalcTypeCodeRegistry.BGId, "as ID", "f", flow => (BGId)flow.GetValue<object>(a));
            ValueOutput(BGCalcTypeCodeRegistry.Entity, "as entity", "g", flow => (BGEntity)flow.GetValue<object>(a));
            ValueOutput(BGCalcTypeCodeRegistry.Field, "as field", "h", flow => (BGField)flow.GetValue<object>(a));
            ValueOutput(BGCalcTypeCodeRegistry.Meta, "as meta", "i", flow => (BGMetaEntity)flow.GetValue<object>(a));
        }
    }
}