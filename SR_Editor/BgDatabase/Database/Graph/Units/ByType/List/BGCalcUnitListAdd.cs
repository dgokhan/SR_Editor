/*
<copyright file="BGCalcUnitListAdd.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// add list item  unit
    /// </summary>
    [BGCalcUnitDefinition("By type/list/List add")]
    public class BGCalcUnitListAdd : BGCalcUnit2ControlsA
    {
        private BGCalcValueInput listIn;
        private BGCalcValueInput obj;
        private BGCalcValueOutput listOut;

        public const int Code = 65;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override void Definition()
        {
            base.Definition();
            listIn = ValueInput(BGCalcTypeCodeRegistry.List, "list", "a");
            obj = ValueInput(BGCalcTypeCodeRegistry.Object, "object", "b");
            listOut = ValueOutput(BGCalcTypeCodeRegistry.List, "result", "c", null);
        }

        /// <inheritdoc />
        protected override void Run(BGCalcFlowI flow)
        {
            var list = flow.GetValue<IList>(listIn);
            var toAdd = flow.GetValue(obj);
            if (list is Array array) flow.SetValue(listOut, new ArrayList(array) { obj }.ToArray(array.GetType().GetElementType()));
            else
            {
                list.Add(toAdd);
                flow.SetValue(listOut, list);
            }
        }
    }
}