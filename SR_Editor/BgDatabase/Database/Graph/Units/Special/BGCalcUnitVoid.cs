/*
<copyright file="BGCalcUnitVoid.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// void unit
    /// </summary>
    [BGCalcUnitDefinition("Special/void", true)]
    public class BGCalcUnitVoid : BGCalcUnit
    {
        public const int Code = 123;

        public override ushort TypeCode => Code;

        public override string Title => "VOID (I do not exist)";

        public override void Definition()
        {
        }
    }
}