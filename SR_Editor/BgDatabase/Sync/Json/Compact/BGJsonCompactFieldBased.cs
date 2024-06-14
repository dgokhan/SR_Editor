using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    internal class BGJsonCompactFieldBased : BGJsonCompactReader.JsonEntitiesReader, BGJsonCompactWriter.JsonEntitiesWriter
    {
        private const string entityIds = "EntityIds";
        private const string entityValues = "EntityValues";
        public BGJsonFormatEnum Format => BGJsonFormatEnum.CompactFieldBased;

        public void OnRead(JSONObject metaObject, BGMetaEntity meta, Dictionary<string, JSONObject> name2Field)
        {
            BGJsonCompactReader.ReadArray(metaObject, entityIds, node => meta.NewEntity(new BGId(node.Value)));

            foreach (var pair in name2Field)
            {
                var fieldName = pair.Key;
                var jsonField = pair.Value;
                var field = meta.GetField(fieldName);
                if (field is BGFieldNested) continue;
                
                var counter = 0;
                var valuesCount = BGJsonCompactReader.ReadArray(jsonField, entityValues, node =>
                {
                    try
                    {
                        field.FromString(counter++, node.Value);
                    }
                    catch (Exception e)
                    {
                    }
                });
                if (valuesCount != meta.CountEntities)
                    throw new Exception("Values count mismatch: " +
                                        $"field {fieldName} has {valuesCount} values, but it should have {meta.CountEntities}");
            }

            BGJsonCompactReader.ReadArray(metaObject, nameof(BGJsonRepoModel.Meta.Entities), node =>
            {
                var entity = meta.NewEntity(new BGId(BGJsonCompactReader.Str(node, nameof(BGJsonRepoModel.Entity.Id))));
                foreach (var pair in node)
                {
                    var fieldName = pair.Key;
                    if (fieldName == nameof(BGJsonRepoModel.Entity.Id)) continue;
                    var fieldValue = pair.Value.Value;
                    try
                    {
                        meta.GetField(fieldName).FromString(entity.Index, fieldValue);
                    }
                    catch (Exception e)
                    {
                    }
                }
            });
        }

        public void OnWrite(JSONObject jsonMeta, BGMetaEntity meta, Dictionary<string, JSONObject> name2Field)
        {
            var ids = new JSONArray();
            jsonMeta.Add(entityIds, ids);
            meta.ForEachEntity(entity => ids.Add(entity.Id.ToString()));

            meta.ForEachField(field =>
            {
                var fieldJson = name2Field[field.Name];
                var valuesJson = new JSONArray();
                fieldJson.Add(entityValues, valuesJson);
                for (var i = 0; i < meta.CountEntities; i++) valuesJson.Add(field.ToString(i));
            }, field => !(field is BGFieldNested));
        }
    }
}