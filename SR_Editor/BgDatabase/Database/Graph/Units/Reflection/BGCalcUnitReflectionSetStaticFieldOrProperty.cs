/*
<copyright file="BGCalcUnitReflectionSetStaticFieldOrProperty.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Reflection;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// set static field/property value using reflection
    /// </summary>
    [BGCalcUnitDefinition("Reflection/Set static field(property)")]
    public class BGCalcUnitReflectionSetStaticFieldOrProperty : BGCalcUnit2ControlsA
    {
        public const int Code = 132;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        private BGCalcValueInput typeNameInput;
        private BGCalcValueInput nameInput;
        private BGCalcValueInput isPropertyInput;
        private BGCalcValueInput valueInput;

        /// <inheritdoc />
        public override void Definition()
        {
            base.Definition();
            typeNameInput = ValueInput(BGCalcTypeCodeRegistry.String, "typeName", "a");
            nameInput = ValueInput(BGCalcTypeCodeRegistry.String, "fieldName", "b");
            isPropertyInput = ValueInput(BGCalcTypeCodeRegistry.Bool, "isProperty", "c");
            valueInput = ValueInput(BGCalcTypeCodeRegistry.Object, "value", "d");
        }

        /// <inheritdoc />
        protected override void Run(BGCalcFlowI flow)
        {
            var typeName = flow.GetValue<string>(typeNameInput);
            if (string.IsNullOrEmpty(typeName)) throw new Exception("typeName is not set!");
            var type = BGUtil.GetType(typeName);
            if (type == null) throw new Exception($"type with name {typeName} can not be found!");

            var fieldName = flow.GetValue<string>(nameInput);
            if (string.IsNullOrEmpty(fieldName)) throw new Exception("field/property name is not set!");
            var value = flow.GetValue<object>(valueInput);
            var isProperty = flow.GetValue<bool>(isPropertyInput);
            if (isProperty)
            {
                var property = type.GetProperty(fieldName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
                if (property == null) throw new Exception($"property {fieldName} can not be found!");
                property.SetValue(null, value);
            }
            else
            {
                var field = type.GetField(fieldName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
                if (field == null) throw new Exception($"field {fieldName} can not be found!");
                field.SetValue(null, value);
            }
        }
    }
}