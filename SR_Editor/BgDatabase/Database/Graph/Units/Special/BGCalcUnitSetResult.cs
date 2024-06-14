/*
<copyright file="BGCalcUnitSetResult.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Set result unit
    /// </summary>
    public class BGCalcUnitSetResult : BGCalcUnit2ControlsA
    {
        public static readonly byte TypeCodeVarId = 1;
        private BGCalcValueInput valueInput;

        public const int Code = 110;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override string Title => "Set result";
        
        /// <summary>
        /// result code type code object
        /// </summary>
        public BGCalcTypeCode ResultTypeCode => BGCalcTypeCodeRegistry.Get((byte)GetVar(TypeCodeVarId).Value);

        /// <inheritdoc />
        public override void Definition()
        {
            base.Definition();
            var resultTypeCode = ResultTypeCode;
            if (resultTypeCode == null) throw new Exception("Result type code var is not found!");

            valueInput = ValueInput(resultTypeCode, "result", "r");
        }

        /// <inheritdoc />
        protected override void Run(BGCalcFlowI flow)
        {
            // var resultTypeCode = ResultTypeCode;
            var value = flow.GetValue(valueInput);
            flow.Result = value;
        }

        /// <summary>
        /// initialize with type code object
        /// </summary>
        /// <param name="resultCode"></param>
        public void Init(BGCalcTypeCode resultCode)
        {
            GetVars()?.Variables.Clear();
            var typeCodeVar = BGCalcVarLite.Create(this, TypeCodeVarId, BGCalcTypeCodeRegistry.Byte);
            typeCodeVar.Value = resultCode.TypeCode;
        }
    }
}