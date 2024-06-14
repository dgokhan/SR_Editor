using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    internal class BGJsonCompactRowBased : BGJsonCompactReader.JsonEntitiesReader, BGJsonCompactWriter.JsonEntitiesWriter
    {
        private const string EntityId = "_id";
        public BGJsonFormatEnum Format => BGJsonFormatEnum.CompactRowBased;

        public void OnRead(JSONObject metaObject, BGMetaEntity meta, Dictionary<string, JSONObject> jsonObjects)
        {
            BGJsonCompactReader.ReadArray(metaObject, nameof(BGJsonRepoModel.Meta.Entities), node =>
            {
                var entity = meta.NewEntity(new BGId(BGJsonCompactReader.Str(node, EntityId)));
                foreach (var pair in node)
                {
                    var fieldName = pair.Key;
                    if (fieldName == EntityId) continue;
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

        public void OnWrite(JSONObject jsonMeta, BGMetaEntity meta, Dictionary<string, JSONObject> jsonObjects)
        {
            var jsonEntities = new JSONArray();
            jsonMeta.Add(nameof(BGJsonRepoModel.Meta.Entities), jsonEntities);
            meta.ForEachEntity(entity =>
            {
                var jsonEntity = new JSONObject();
                jsonEntities.Add(jsonEntity);

                jsonEntity.Add(EntityId, entity.Id.ToString());
                meta.ForEachField(field => jsonEntity.Add(field.Name, field.ToString(entity.Index)));
            });
        }
    }
}