/*
<copyright file="BGCalcUnitStringToUpper.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System.Globalization;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// string value to uppercase unit
    /// </summary>
    [BGCalcUnitDefinition("By type/string/ToUpper")]
    public class BGCalcUnitStringToUpper : BGCalcUnitStringAString
    {
        public const int Code = 53;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        protected override string OutputLabel => "toUpper(A)";

        /// <inheritdoc />
        protected override string Operation(string a) => a.ToUpper(CultureInfo.InvariantCulture);
    }
}