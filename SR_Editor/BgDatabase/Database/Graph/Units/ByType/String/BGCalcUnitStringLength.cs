/*
<copyright file="BGCalcUnitStringLength.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// string length unit
    /// </summary>
    [BGCalcUnitDefinition("By type/string/String length")]
    public class BGCalcUnitStringLength : BGCalcUnitStringA<int>
    {
        public const int Code = 95;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        protected override BGCalcTypeCode<int> OutputCode => BGCalcTypeCodeRegistry.Int;

        /// <inheritdoc />
        protected override string OutputLabel => "A.Length";

        /// <inheritdoc />
        protected override int Operation(string a) => string.IsNullOrEmpty(a) ? 0 : a.Length;
    }
}