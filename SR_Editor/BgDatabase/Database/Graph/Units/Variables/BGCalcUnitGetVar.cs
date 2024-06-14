/*
<copyright file="BGCalcUnitGetVar.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// get variable value unit
    /// </summary>
    public class BGCalcUnitGetVar : BGCalcUnitVarA
    {
        public const int Code = 116;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override void Definition() => ValueOutput(GraphVar.TypeCode, "value", "v", GetValue);

        private object GetValue(BGCalcFlowI flow)
        {
            var variable = flow.GetVars(true).GetVar(VariableId);
            if (variable == null) throw new Exception("Can not get target graph variable! id=" + VariableId);
            return variable.Value;
        }
    }
}