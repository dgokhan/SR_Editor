/*
<copyright file="BGCalcUnitSetVar.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// set variable value unit
    /// </summary>
    public class BGCalcUnitSetVar : BGCalcUnitVarA
    {
        private BGCalcValueInput value;
        private BGCalcControlOutput exit;

        public const int Code = 117;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override void Definition()
        {
            ControlInput("enter", "e", Run);
            exit = ControlOutput("exit", "x");

            value = ValueInput(VariableTypeCode.Type, "value", "v");
        }

        private BGCalcControlOutput Run(BGCalcFlowI flow)
        {
            var variable = flow.GetVars(true).GetVar(VariableId);
            if (variable == null) throw new Exception("Can not get target graph variable!");
            variable.Value = flow.GetValue(value);
            return exit;
        }
    }
}