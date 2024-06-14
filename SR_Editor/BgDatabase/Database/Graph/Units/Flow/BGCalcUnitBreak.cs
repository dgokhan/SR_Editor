/*
<copyright file="BGCalcUnitBreak.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// break from loop unit
    /// </summary>
    [BGCalcUnitDefinition("Flow/Break")]
    public class BGCalcUnitBreak : BGCalcUnit
    {
        private BGCalcControlInput input;
        public const int Code = 118;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override void Definition() => input = ControlInput("enter", "a", Run);

        private BGCalcControlOutputI Run(BGCalcFlowI flow)
        {
            flow.BreakIsRequested = true;
            return null;
        }
    }
}