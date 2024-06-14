/*
<copyright file="BGCalcUnitDebug.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// debug message to Unity console
    /// </summary>
    [BGCalcUnitDefinition("Special/Debug")]
    public class BGCalcUnitDebug : BGCalcUnit2ControlsA
    {
        public const int Code = 101;

        private BGCalcValueInput messagePort;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override void Definition()
        {
            base.Definition();
            messagePort = ValueInput<object>("message", "m");
        }

        /// <inheritdoc />
        protected override void Run(BGCalcFlowI flow) => Debug.Log(flow.GetValue(messagePort));
    }
}