/*
<copyright file="BGCalcUnitStringTrim.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// trim string value unit
    /// </summary>
    [BGCalcUnitDefinition("By type/string/Trim")]
    public class BGCalcUnitStringTrim : BGCalcUnitStringAString
    {
        public const int Code = 58;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        protected override string OutputLabel => "toUpper(A)";

        /// <inheritdoc />
        protected override string Operation(string a) => a.Trim();
    }
}