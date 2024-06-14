/*
<copyright file="BGCalcUnitLiteral.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// unit for introducing a literal
    /// </summary>
    public class BGCalcUnitLiteral : BGCalcUnit
    {
        public const int Code = 103;

        public static readonly byte ValueVarId = 1;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        public BGCalcVarLite ValueVar => GetVar(ValueVarId);

        public BGCalcTypeCode ConstantTypeCode => ValueVar.TypeCode;

        /// <inheritdoc />
        public override string Title
        {
            get
            {
                var code = ConstantTypeCode;
                if (code == null) return "Introduce [ERROR]";
                return code.Name + " literal";
            }
        }

        /// <inheritdoc />
        public override void Definition() => ValueOutput(ConstantTypeCode, "result", "r", GetValue);

        /// <inheritdoc />
        public override string GetPublicVarLabel(byte varId) => varId == ValueVarId ? "value" : null;

        private object GetValue(BGCalcFlowI flow) => ValueVar.Value;


        /// <summary>
        /// initialize internal state with variable type
        /// </summary>
        public void Init(BGCalcTypeCode code)
        {
            GetVars()?.Variables.Clear();
            // var idVar = BGCalcVar.Create(this, TypeCodeVarId, "t", BGCalcVarTypeCodeRegistry.Byte);
            // idVar.Value = code.TypeCode;

            var valueVar = BGCalcVarLite.Create(this, ValueVarId, code);
        }
    }
}