/*
<copyright file="BGCalcUnitGetGameObject.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Get Unity GameObject
    /// </summary>
    [BGCalcUnitDefinition("Unity/Component GetGameObject")]
    public class BGCalcUnitGetGameObject : BGCalcUnit
    {
        public const int Code = 128;
        
        /// <inheritdoc />
        public override ushort TypeCode => Code;

        private BGCalcValueInput componentInput;

        /// <inheritdoc />
        public override void Definition()
        {
            componentInput = ValueInput(BGCalcTypeCodeRegistry.Component, "component", "a");
            ValueOutput(BGCalcTypeCodeRegistry.GameObject, "component", "c", GetGameObject);
        }

        private GameObject GetGameObject(BGCalcFlowI flow)
        {
            var component = flow.GetValue<Component>(componentInput);
            if (component == null) throw new Exception("Component is not set!");
            return component.gameObject;
        }
    }
}