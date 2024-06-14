/*
<copyright file="BGCalcUnitAndGroup.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// AND bool unit with multiple inputs
    /// </summary>
    [BGCalcUnitDefinition("By type/bool/And Group")]
    public class BGCalcUnitAndGroup : BGCalcUnitWithInPortsCount
    {
        public const int Code = 133;
        
        /// <inheritdoc />
        public override ushort TypeCode => Code;

        protected override BGCalcTypeCode InPortType => BGCalcTypeCodeRegistry.Bool;
        protected override int Min => 2;
        protected override BGCalcValueOutput CreateOutputPort() => ValueOutput(BGCalcTypeCodeRegistry.Bool, "1 & n", "r", Result);

        private bool Result(BGCalcFlowI flow)
        {
            var count = Count;
            for (var i = 0; i < count; i++)
            {
                var value = flow.GetValue<bool>(inputs[i]);
                if (!value) return false;
            }
            return true;
        }
    }
}