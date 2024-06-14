/*
<copyright file="BGCalcUnitGraphStart.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// unit for start node
    /// </summary>
    [BGCalcUnitDefinition("Hidden/Start", true)]
    public class BGCalcUnitGraphStart : BGCalcUnit
    {
        public const int Code = 1;

        private BGCalcControlOutput startPort;

        public BGCalcControlOutput StartPort => startPort;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override void Definition() => startPort = ControlOutput("start", "s");
    }
}