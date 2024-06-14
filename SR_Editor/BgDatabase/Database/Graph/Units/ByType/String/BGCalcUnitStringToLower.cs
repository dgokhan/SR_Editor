/*
<copyright file="BGCalcUnitStringToLower.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System.Globalization;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// string value to lower case unit
    /// </summary>
    [BGCalcUnitDefinition("By type/string/ToLower")]
    public class BGCalcUnitStringToLower : BGCalcUnitStringAString
    {
        public const int Code = 52;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        protected override string OutputLabel => "toLower(A)";

        /// <inheritdoc />
        protected override string Operation(string a) => a.ToLower(CultureInfo.InvariantCulture);
    }
}