/*
<copyright file="BGCalcVarModel.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// serializable var model
    /// </summary>
    [Serializable]
    public class BGCalcVarModel
    {
        public string Id;
        public string Name;
        public bool IsPublic;
        public byte TypeCode;
        public string State;
        public string Value;

        public BGCalcVarModel(BGCalcVar variable)
        {
            Id = variable.Id.ToString();
            Name = variable.Name;
            IsPublic = variable.IsPublic;
            TypeCode = variable.TypeCode != null ? (byte)variable.TypeCode.TypeCode : (byte)0;
            if (variable.TypeCode != null)
            {
                if (variable.TypeCode is BGCalcTypeCodeStateful stateful) State = stateful.WriteState();
                if (variable.TypeCode.SupportDefaultValue) Value = variable.TypeCode.ValueToString(variable.Value);
            }
        }

        public BGCalcVar ToVar(BGCalcVarsOwnerI owner)
        {
            var calcVar = BGCalcVar.Create(owner, BGId.Parse(Id), Name, BGCalcTypeCodeRegistry.Get(TypeCode));
            calcVar.IsPublic = IsPublic;
            if (calcVar.TypeCode != null)
            {
                if (calcVar.TypeCode is BGCalcTypeCodeStateful) ((BGCalcTypeCodeStateful)calcVar).ReadState(State);
                if (calcVar.TypeCode.SupportDefaultValue) calcVar.Value = calcVar.TypeCode.ValueFromString(Value);
            }

            return calcVar;
        }
    }
}