/*
<copyright file="BGCalcUnitVarA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// abstract variable-based unit
    /// </summary>
    public abstract class BGCalcUnitVarA : BGCalcUnit
    {
        public static readonly byte VarId = 1;
        public static readonly byte VarType = 2;

        public BGCalcVar GraphVar => Graph.GetVars(true).GetVar(VariableId);

        /// <summary>
        /// variable ID
        /// </summary>
        public BGId VariableId
        {
            get
            {
                var idVar = GetVar(VarId);
                var varId = (BGId)idVar.Value;
                return varId;
            }
        }

        /// <inheritdoc />
        public override string Title
        {
            get
            {
                var action = this is BGCalcUnitGetVar ? "Get" : "Set";
                var variable = GraphVar;
                if (variable == null) return $"{action} variable[ERROR]";
                return $"{action} variable [" + variable.Name + "]";
            }
        }

        /// <summary>
        /// variable type code
        /// </summary>
        public BGCalcTypeCode VariableTypeCode
        {
            get
            {
                var typeVar = GetVar(VarType);
                var code = BGCalcTypeCodeRegistry.Get((byte)typeVar.Value);
                return code;
            }
        }

        /// <summary>
        /// initialize variable with ID and value type code 
        /// </summary>
        public void Init(BGId varId, BGCalcTypeCode code)
        {
            GetVars()?.Variables.Clear();
            var idVar = BGCalcVarLite.Create(this, VarId, BGCalcTypeCodeRegistry.BGId);
            idVar.Value = varId;

            var typeVar = BGCalcVarLite.Create(this, VarType, BGCalcTypeCodeRegistry.Byte);
            typeVar.Value = code.TypeCode;
        }
    }
}