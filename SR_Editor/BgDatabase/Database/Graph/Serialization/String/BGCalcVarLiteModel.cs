/*
<copyright file="BGCalcVarLiteModel.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// serializable lite var model
    /// </summary>
    [Serializable]
    public class BGCalcVarLiteModel
    {
        public byte Id;
        public byte TypeCode;
        public string Value;
        public string State;

        public BGCalcVarLiteModel(BGCalcVarLite variable)
        {
            Id = variable.Id;
            TypeCode = variable.TypeCode != null ? (byte)variable.TypeCode.TypeCode : (byte)0;
            if (variable.TypeCode != null)
            {
                if (variable.TypeCode is BGCalcTypeCodeStateful stateful) State = stateful.WriteState();
                if (variable.TypeCode.SupportDefaultValue) Value = variable.TypeCode.ValueToString(variable.Value);
            }
        }

        public BGCalcVarLite ToVar(BGCalcVarsLiteOwnerI owner)
        {
            var calcVar = BGCalcVarLite.Create(owner, Id, BGCalcTypeCodeRegistry.Get(TypeCode));
            if (calcVar.TypeCode != null)
            {
                if (calcVar.TypeCode is BGCalcTypeCodeStateful stateful) stateful.ReadState(State);
                if (calcVar.TypeCode.SupportDefaultValue) calcVar.Value = calcVar.TypeCode.ValueFromString(Value);
            }

            return calcVar;
        }
    }
}