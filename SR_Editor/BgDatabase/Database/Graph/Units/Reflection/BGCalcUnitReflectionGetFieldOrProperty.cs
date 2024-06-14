/*
<copyright file="BGCalcUnitReflectionGetFieldOrProperty.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Reflection;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// get field/property value using reflection
    /// </summary>
    [BGCalcUnitDefinition("Reflection/Get field(property)")]
    public class BGCalcUnitReflectionGetFieldOrProperty : BGCalcUnit
    {
        public const int Code = 129;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        private BGCalcValueInput objectInput;
        private BGCalcValueInput nameInput;
        private BGCalcValueInput isPropertyInput;

        /// <inheritdoc />
        public override void Definition()
        {
            objectInput = ValueInput(BGCalcTypeCodeRegistry.Object, "object", "a");
            nameInput = ValueInput(BGCalcTypeCodeRegistry.String, "fieldName", "b");
            isPropertyInput = ValueInput(BGCalcTypeCodeRegistry.Bool, "isProperty", "c");
            ValueOutput(BGCalcTypeCodeRegistry.Object, "value", "d", GetValue);
        }

        private object GetValue(BGCalcFlowI flow)
        {
            var obj = flow.GetValue<object>(objectInput);
            if (obj == null) throw new Exception("object is not set!");
            var fieldName = flow.GetValue<string>(nameInput);
            if (string.IsNullOrEmpty(fieldName)) throw new Exception("field/property name is not set!");
            var isProperty = flow.GetValue<bool>(isPropertyInput);
            if (isProperty)
            {
                var property = obj.GetType().GetProperty(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (property == null) throw new Exception($"property {fieldName} can not be found!");
                return property.GetValue(obj);
            }

            var field = obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (field == null) throw new Exception($"field {fieldName} can not be found!");
            return field.GetValue(obj);
        }
    }
}