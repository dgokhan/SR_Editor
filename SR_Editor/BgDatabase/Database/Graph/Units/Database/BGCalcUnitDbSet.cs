/*
<copyright file="BGCalcUnitDbSet.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Set rows values
    /// </summary>
    public class BGCalcUnitDbSet : BGCalcUnitDbRowBasedA
    {
        private BGCalcControlInput enterPort;
        private BGCalcControlOutput exitPort;
        public const int Code = 109;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        protected override string Operation => "set";

        /// <inheritdoc />
        public override void Definition()
        {
            var meta = Meta;
            if (meta == null) throw new Exception("Meta is not found! id=" + MetaId);

            enterPort = ControlInput("enter", "y", RunMe);
            exitPort = ControlOutput("exit", "z");

            base.Definition();

            var portCount = PortsCount;
            var fields = meta.FindFields(null, f => !f.ReadOnly);
            if (portCount + fields.Count > byte.MaxValue) fields.RemoveRange(byte.MaxValue - portCount, portCount + fields.Count - byte.MaxValue);
            foreach (var field in fields)
                switch (field)
                {
                    case BGFieldEnumI fieldEnumI:
                        ValueInput(new BGCalcTypeCodeEnum(fieldEnumI.EnumType), field.Name, field.Id.ToString());
                        break;
                    case BGListI _:
                    case BGFieldRelationMultiple _:
                    case BGFieldManyRelationsMultiple _:
                    case BGFieldNested _:
                        ValueInput(BGCalcTypeCodeRegistry.List, field.Name, field.Id.ToString());
                        break;
                    case BGFieldRelationSingle relation:
                        ValueInput(new BGCalcTypeCodeEntityRuntime(relation.RelatedMeta), field.Name, field.Id.ToString());
                        break;
                    case BGFieldManyRelationsSingle _:
                        ValueInput(BGCalcTypeCodeRegistry.Entity, field.Name, field.Id.ToString());
                        break;
                    default:
                        ValueInput(field.ValueType, field.Name, field.Id.ToString());
                        break;
                }
        }

        private BGCalcControlOutputI RunMe(BGCalcFlowI flow)
        {
            var meta = MetaCached;
            //all connected field ports
            var fieldPorts = FindPorts(port => port is BGCalcValueInputI && port.Id.Length > 1 && port.IsConnected);
            var entity = GetEntity(flow);
            foreach (var port in fieldPorts)
            {
                var field = meta.GetField(BGId.Parse(port.Id));
                var value = flow.GetValue(port as BGCalcValueInputI);
                field.SetValue(entity.Index, value);
            }

            return exitPort;
        }
    }
}