using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Get incoming related entities 
    /// </summary>
    [BGCalcUnitDefinition("Database/Generic/relation/Get by relation incoming")]
    public class BGCalcUnitGetByRelationInbound: BGCalcUnit
    {
        private BGCalcValueInput fieldInput;
        private BGCalcValueInput entityInput;

        public const int Code = 142;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />        
        public override void Definition()
        {
            fieldInput = ValueInput(BGCalcTypeCodeRegistry.Field, "field", "a");
            entityInput = ValueInput(BGCalcTypeCodeRegistry.Entity, "entity", "b");

            ValueOutput(BGCalcTypeCodeRegistry.List, "result", "q", GetResult);
        }

        private List<BGEntity> GetResult(BGCalcFlowI flow)
        {
            var field = flow.GetValue<BGField>(fieldInput);
            if (field == null) throw new Exception("Can not get a field cause the field is not set!");
            var entity = flow.GetValue<BGEntity>(entityInput);
            if (entity == null) throw new Exception("Can not get an entity cause the entity is not set!");
            if (!field.Meta.Equals(entity.Meta)) throw new Exception($"Field {field.FullName} and entity {entity.FullName} have different metas (should be the same)!");
            if (!(field is BGAbstractRelationI relation)) throw new Exception($"Field {field.FullName} is not a relation (it should be)!");

            return relation.GetRelatedIn(entity.Id);
        }
    }
}