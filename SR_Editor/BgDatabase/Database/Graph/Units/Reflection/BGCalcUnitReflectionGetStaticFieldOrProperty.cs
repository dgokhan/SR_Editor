/*
<copyright file="BGCalcUnitReflectionGetStaticFieldOrProperty.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Reflection;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// get static field/property value using reflection
    /// </summary>
    [BGCalcUnitDefinition("Reflection/Get static field(property)")]
    public class BGCalcUnitReflectionGetStaticFieldOrProperty : BGCalcUnit
    {
        public const int Code = 130;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        private BGCalcValueInput typeNameInput;
        private BGCalcValueInput nameInput;
        private BGCalcValueInput isPropertyInput;

        /// <inheritdoc />
        public override void Definition()
        {
            typeNameInput = ValueInput(BGCalcTypeCodeRegistry.String, "typeName", "a");
            nameInput = ValueInput(BGCalcTypeCodeRegistry.String, "fieldName", "b");
            isPropertyInput = ValueInput(BGCalcTypeCodeRegistry.Bool, "isProperty", "c");
            ValueOutput(BGCalcTypeCodeRegistry.Object, "value", "d", GetValue);
        }

        private object GetValue(BGCalcFlowI flow)
        {
            var typeName = flow.GetValue<string>(typeNameInput);
            if (typeName == null) throw new Exception("typeName is not set!");
            var type = BGUtil.GetType(typeName);
            if (type == null) throw new Exception($"type with name {typeName} can not be found!");
            var fieldName = flow.GetValue<string>(nameInput);
            if (string.IsNullOrEmpty(fieldName)) throw new Exception("field/property name is not set!");

            var isProperty = flow.GetValue<bool>(isPropertyInput);
            if (isProperty)
            {
                var property = type.GetProperty(fieldName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
                if (property == null) throw new Exception($"property {fieldName} can not be found!");
                return property.GetValue(null);
            }

            var field = type.GetField(fieldName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
            if (field == null) throw new Exception($"field {fieldName} can not be found!");
            return field.GetValue(null);
        }
    }
}