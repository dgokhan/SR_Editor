/*
<copyright file="BGCalcUnitGetComponents.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Get Unity components
    /// </summary>
    [BGCalcUnitDefinition("Unity/GameObject GetComponents")]
    public class BGCalcUnitGetComponents : BGCalcUnit
    {
        public const int Code = 127;
        
        /// <inheritdoc />
        public override ushort TypeCode => Code;

        private BGCalcValueInput gameObjectInput;
        private BGCalcValueInput typeInput;
        private BGCalcValueOutput componentsOutput;

        /// <inheritdoc />
        public override void Definition()
        {
            gameObjectInput = ValueInput(BGCalcTypeCodeRegistry.GameObject, "gameObject", "a");
            typeInput = ValueInput(BGCalcTypeCodeRegistry.String, "type", "b");
            componentsOutput = ValueOutput(BGCalcTypeCodeRegistry.List, "components", "c", GetComponents);
        }

        private Array GetComponents(BGCalcFlowI flow)
        {
            var gameObject = flow.GetValue<GameObject>(gameObjectInput);
            if (gameObject == null) throw new Exception("Game Object is not set!");
            var typeName = flow.GetValue<string>(typeInput);
            if (string.IsNullOrEmpty(typeName)) throw new Exception("Component type is not set!");
            var type = BGUtil.GetType(typeName);
            if (type == null) throw new Exception($"Type {typeName} is not found");
            // if (!typeof(Component).IsAssignableFrom(type)) throw new Exception($"Type {typeName} is not a Unity Component!");
            var components = gameObject.GetComponents(type);
            // if (component == null) throw new Exception($"Component {typeName} is not found!");
            return components;
        }
    }
}