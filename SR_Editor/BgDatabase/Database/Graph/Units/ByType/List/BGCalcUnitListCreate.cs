/*
<copyright file="BGCalcUnitListCreate.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System.Collections;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// list literal unit
    /// </summary>
    [BGCalcUnitDefinition("By type/list/List create")]
    public class BGCalcUnitListCreate : BGCalcUnitWithInPortsCount
    {
        public const int Code = 60;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        protected override BGCalcTypeCode InPortType => BGCalcTypeCodeRegistry.Object;
        protected override BGCalcValueOutput CreateOutputPort() => ValueOutput(BGCalcTypeCodeRegistry.List, "list", "r", CreateList);

        private IList CreateList(BGCalcFlowI flow)
        {
            var list = new ArrayList();
            var count = Count;
            for (var i = 0; i < count; i++) list.Add(flow.GetValue(inputs[i]));

            return list;
        }
    }
}