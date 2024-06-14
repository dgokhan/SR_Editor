/*
<copyright file="BGCalcUnitStringJoin2.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System.Text;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// join list of strings values unit
    /// </summary>
    [BGCalcUnitDefinition("By type/string/String join2")]
    public class BGCalcUnitStringJoin2 : BGCalcUnitWithInPortsCount
    {
        public const int Code = 139;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        protected override BGCalcTypeCode InPortType => BGCalcTypeCodeRegistry.String;
        protected override BGCalcValueOutput CreateOutputPort() => ValueOutput(BGCalcTypeCodeRegistry.String, "result", "r", CreateString);

        private string CreateString(BGCalcFlowI flow)
        {
            var result = new StringBuilder();
            var count = Count;
            for (var i = 0; i < count; i++)
            {
                var value = flow.GetValue(inputs[i]);
                if (value == null) continue;
                result.Append(value);
            }

            return result.ToString();
        }
    }
}