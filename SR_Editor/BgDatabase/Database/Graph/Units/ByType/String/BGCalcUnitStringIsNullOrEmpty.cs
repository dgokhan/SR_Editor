/*
<copyright file="BGCalcUnitStringIsNullOrEmpty.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// is string value null or empty unit
    /// </summary>
    [BGCalcUnitDefinition("By type/string/IsNullOrEmpty")]
    public class BGCalcUnitStringIsNullOrEmpty : BGCalcUnitStringA<bool>
    {
        public const int Code = 84;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        protected override BGCalcTypeCode<bool> OutputCode => BGCalcTypeCodeRegistry.Bool;

        /// <inheritdoc />
        protected override string OutputLabel => "IsEmpty(A)";

        /// <inheritdoc />
        protected override bool Operation(string a) => string.IsNullOrEmpty(a);
    }
}