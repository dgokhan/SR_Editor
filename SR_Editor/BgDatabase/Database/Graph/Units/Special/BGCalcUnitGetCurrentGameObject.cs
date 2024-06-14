/*
<copyright file="BGCalcUnitGetCurrentGameObject.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Get current GameObject
    /// </summary>
    public class BGCalcUnitGetCurrentGameObject : BGCalcUnit
    {
        public const int Code = 125;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override string Title => "Get current GameObject";

        /// <inheritdoc />
        public override void Definition() => ValueOutput(BGCalcTypeCodeRegistry.GameObject, "gameObject", "e", GetGameObject);

        private GameObject GetGameObject(BGCalcFlowI flow) => flow.Context.CurrentGameObject;
    }
}