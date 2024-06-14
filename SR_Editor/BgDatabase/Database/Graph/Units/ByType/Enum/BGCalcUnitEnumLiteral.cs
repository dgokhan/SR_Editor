/*
<copyright file="BGCalcUnitEnumLiteral.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// enum literal unit
    /// </summary>
    [BGCalcUnitDefinition("By type/enum/Enum literal")]
    public class BGCalcUnitEnumLiteral : BGCalcUnit
    {
        public static readonly byte TypeVarId = 1;
        public static readonly byte ValueVarId = 2;

        public event Action OnEnumTypeChange;

        private BGCalcVarLite typeVar;
        private BGCalcVarLite valueVar;
        private BGCalcValueOutputI resultPort;

        public const int Code = 93;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        public BGCalcValueOutputI ResultPort => resultPort;

        private Type EnumType
        {
            get
            {
                if (typeVar == null) return null;
                var typeString = typeVar.Value as string;
                if (string.IsNullOrEmpty(typeString)) return null;
                var type = BGUtil.GetType(typeString);
                if (type == null || !type.IsEnum) return null;
                return type;
            }
        }

        /// <inheritdoc />
        public override void Definition()
        {
            typeVar = GetOrAddVar(TypeVarId, BGCalcTypeCodeRegistry.String);
            CheckOutputPort();

            typeVar.OnValueChange += CheckOutputPort;
        }

        /// <inheritdoc />
        public override string GetPublicVarLabel(byte varId)
        {
            if (TypeVarId == varId) return "type";
            if (ValueVarId == varId) return "value";
            return null;
        }

        private void CheckOutputPort()
        {
            var enumType = EnumType;
            if (enumType != null)
            {
                var typeCode = new BGCalcTypeCodeEnum(enumType);
                valueVar = GetVar(ValueVarId);
                if (valueVar == null || !Equals(valueVar.TypeCode, typeCode))
                {
                    RemoveValueVar();
                    valueVar = BGCalcVarLite.Create(this, ValueVarId, typeCode);
                }

                RemoveResultPort();
                resultPort = ValueOutput(typeCode, "result", "v", flow => (Enum)valueVar.Value);
            }
            else
            {
                RemoveValueVar();
                RemoveResultPort();
            }

            OnEnumTypeChange?.Invoke();
        }

        private void RemoveResultPort()
        {
            if (resultPort == null) return;
            resultPort.DisconnectAll();
            RemovePort(resultPort);
            resultPort = null;
        }

        private void RemoveValueVar()
        {
            if (valueVar == null) return;
            GetVars(true).RemoveVar(valueVar.Id);
            valueVar = null;
        }
    }
}