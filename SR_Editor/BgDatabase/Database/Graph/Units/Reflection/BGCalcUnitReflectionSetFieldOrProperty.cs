/*
<copyright file="BGCalcUnitReflectionSetFieldOrProperty.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Reflection;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// set field/property value using reflection
    /// </summary>
    [BGCalcUnitDefinition("Reflection/Set field(property)")]
    public class BGCalcUnitReflectionSetFieldOrProperty : BGCalcUnit2ControlsA
    {
        public const int Code = 131;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        private BGCalcValueInput objectInput;
        private BGCalcValueInput nameInput;
        private BGCalcValueInput isPropertyInput;
        private BGCalcValueInput valueInput;

        /// <inheritdoc />
        public override void Definition()
        {
            base.Definition();
            objectInput = ValueInput(BGCalcTypeCodeRegistry.Object, "object", "a");
            nameInput = ValueInput(BGCalcTypeCodeRegistry.String, "fieldName", "b");
            isPropertyInput = ValueInput(BGCalcTypeCodeRegistry.Bool, "isProperty", "c");
            valueInput = ValueInput(BGCalcTypeCodeRegistry.Object, "value", "d");
        }

        /// <inheritdoc />
        protected override void Run(BGCalcFlowI flow)
        {
            var obj = flow.GetValue<object>(objectInput);
            if (obj == null) throw new Exception("object is not set!");
            var fieldName = flow.GetValue<string>(nameInput);
            if (string.IsNullOrEmpty(fieldName)) throw new Exception("field/property name is not set!");
            var value = flow.GetValue<object>(valueInput);
            var isProperty = flow.GetValue<bool>(isPropertyInput);
            if (isProperty)
            {
                var property = obj.GetType().GetProperty(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (property == null) throw new Exception($"property {fieldName} can not be found!");
                property.SetValue(obj, value);
            }
            else
            {
                var field = obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (field == null) throw new Exception($"field {fieldName} can not be found!");
                field.SetValue(obj, value);
            }
        }
    }
}