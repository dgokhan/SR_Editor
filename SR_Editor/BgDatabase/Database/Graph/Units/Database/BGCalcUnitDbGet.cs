/*
<copyright file="BGCalcUnitDbGet.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// get row node
    /// </summary>
    public class BGCalcUnitDbGet : BGCalcUnitDbRowBasedA
    {
        public const int Code = 104;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        protected override string Operation => "get";

        /// <inheritdoc />
        public override void Definition()
        {
            var meta = Meta;
            if (meta == null) throw new Exception("Meta is not found! id=" + MetaId);
            base.Definition();

            var portCount = PortsCount;
            var fields = meta.FindFields(null, f => !(f is BGFieldCalcAction));
            if (portCount + fields.Count > byte.MaxValue) fields.RemoveRange(byte.MaxValue - portCount, portCount + fields.Count - byte.MaxValue);
            foreach (var field in fields)
                switch (field)
                {
                    case BGFieldEnumI fieldEnumI:
                        ValueOutput(new BGCalcTypeCodeEnum(fieldEnumI.EnumType), field.Name, field.Id.ToString(), flow => GetValue(flow, field.MetaId, field.Id));
                        break;
                    case BGListI _:
                    case BGFieldRelationMultiple _:
                    case BGFieldManyRelationsMultiple _:
                    case BGFieldNested _:
                        ValueOutput(BGCalcTypeCodeRegistry.List, field.Name, field.Id.ToString(), flow => GetValue(flow, field.MetaId, field.Id));
                        break;
                    case BGFieldRelationSingle relation:
                        ValueOutput(new BGCalcTypeCodeEntityRuntime(relation.RelatedMeta), field.Name, field.Id.ToString(), flow => GetValue(flow, field.MetaId, field.Id));
                        break;
                    case BGFieldManyRelationsSingle _:
                        ValueOutput(BGCalcTypeCodeRegistry.Entity, field.Name, field.Id.ToString(), flow => GetValue(flow, field.MetaId, field.Id));
                        break;
                    default:
                        ValueOutput(field.ValueType, field.Name, field.Id.ToString(), flow => GetValue(flow, field.MetaId, field.Id));
                        break;
                }
        }

        private object GetValue(BGCalcFlowI flow, BGId metaId, BGId fieldId)
        {
            var field = MetaCached.GetField(fieldId, false);
            if (field == null) throw new Exception($"Can not get a field with Id={fieldId}!");
            var entity = GetEntity(flow);
            if (entity.MetaId != field.MetaId) throw new Exception($"Can not get a value, cause entity is from different table! Expected=" + field.MetaName + " actual=" + entity.MetaName);

            if (field is BGFieldCalcI) return CallCalculated(flow, field, entity);

            BGCalcUnitCellGetValue.AddListeners(flow, field, entity);
            return field.GetValue(entity.Index);
        }

        public static object CallCalculated(BGCalcFlowI flow, BGField field, BGEntity entity)
        {
            var storable = (BGStorable<BGFieldCalcValue>)field;
            var calcField = (BGFieldCalcI)field;
            var value = storable.GetStoredValue(entity.Index);
            var graph = value?.Graph ?? calcField.Graph;
            if (graph == null) return null;
            
            var context = BGCalcFlowContext.Get();
            try
            {
                context.CopyCellsFrom(flow.Context);
                return BGFieldCalcA<object>.Run(context, graph, field, entity);
            }
            finally
            {
                BGCalcFlowContext.Return(context);
            }
            
        }
    }
}