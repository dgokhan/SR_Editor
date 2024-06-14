using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    public class BGJsonCompactWriter
    {
        /// <summary>
        /// Write database data to JSON format 
        /// </summary>
        internal string Write(BGRepo repo, JsonEntitiesWriter entitiesWriter, bool skipData = false, bool removeSensitive = false)
        {
            var root = new JSONObject();
            WriteRepo(repo, skipData, root, entitiesWriter, removeSensitive);
            return root.ToString(4);
        }

        private static void WriteRepo(BGRepo repo, bool skipData, JSONObject jsonRoot, JsonEntitiesWriter entitiesWriter, bool removeSensitive)
        {
            jsonRoot.Add(nameof(BGJsonRepoModel.ProducedBy), "BGDatabase");
            jsonRoot.Add(nameof(BGJsonRepoModel.DbVersion), BGRepo.Version);
            jsonRoot.Add(nameof(BGJsonRepoModel.DbBuild), BGRepo.VersionBuild);
            jsonRoot.Add(nameof(BGJsonRepoModel.Format), (byte)entitiesWriter.Format);

            //addons
            var jsonAddons = new JSONArray();
            jsonRoot.Add(nameof(BGJsonRepoModel.Addons) ,jsonAddons );
            repo.Addons.ForEachAddon(addon =>
            {
                var jsonAddon = new JSONObject();
                jsonAddons.Add(jsonAddon);
                jsonAddon.Add(nameof(BGJsonRepoModel.Addon.Type), addon.GetType().FullName);

                //if (removeSensitive && typeof(BGAddonLiveUpdate) == addon.GetType()) jsonAddon.Add(nameof(BGJsonRepoModel.Addon.Config), "{\"content\" : \"[sensitive]\"}");
                //else 
                    jsonAddon.Add(nameof(BGJsonRepoModel.Addon.Config), addon.ConfigToString());
            });


            //tables
            var jsonMetas = new JSONArray();
            jsonRoot.Add(nameof(BGJsonRepoModel.Metas) ,jsonMetas );
            repo.ForEachMeta(meta =>
            {
                var jsonMeta = new JSONObject();
                jsonMetas.Add(jsonMeta);

                jsonMeta.Add(nameof(BGJsonRepoModel.Meta.Singleton), meta.Singleton);
                jsonMeta.Add(nameof(BGJsonRepoModel.Meta.UniqueName), meta.UniqueName);
                jsonMeta.Add(nameof(BGJsonRepoModel.Meta.EmptyName), meta.EmptyName);
                jsonMeta.Add(nameof(BGJsonRepoModel.Meta.UserDefinedReadonly), meta.UserDefinedReadonly);

                WriteObjMeta(meta, jsonMeta);

                //fields
                var name2Field = new Dictionary<string, JSONObject>(meta.CountFields);
                var jsonFields = new JSONArray();
                jsonMeta.Add(nameof(BGJsonRepoModel.Meta.Fields),jsonFields);
                meta.ForEachField(field =>
                {
                    var jsonField = new JSONObject();

                    name2Field[field.Name] = jsonField;
                    jsonFields.Add(jsonField);
                    
                    jsonField.Add(nameof(BGJsonRepoModel.Field.DefaultValue), field.DefaultValue);
                    jsonField.Add(nameof(BGJsonRepoModel.Field.Required), field.Required);
                    jsonField.Add(nameof(BGJsonRepoModel.Field.UserDefinedReadonly), field.UserDefinedReadonly);
                    jsonField.Add(nameof(BGJsonRepoModel.Field.CustomEditor), field.CustomEditorTypeAsString);
                    jsonField.Add(nameof(BGJsonRepoModel.Field.StringFormatter), field.CustomStringFormatterTypeAsString);
                    WriteObjMeta(field, jsonField);
                });
                
                //keys
                var jsonKeys = new JSONArray();
                jsonMeta.Add(nameof(BGJsonRepoModel.Meta.Keys),jsonKeys);
                meta.ForEachKey(key =>
                {
                    var jsonKey = new JSONObject();
                    jsonKeys.Add(jsonKey);
                    
                    jsonKey.Add(nameof(BGJsonRepoModel.Key.Id), key.Id.ToString());
                    jsonKey.Add(nameof(BGJsonRepoModel.Key.Unique), key.IsUnique);
                    jsonKey.Add(nameof(BGJsonRepoModel.Key.Name), key.Name);
                    jsonKey.Add(nameof(BGJsonRepoModel.Key.Comment), key.Comment);
                    jsonKey.Add(nameof(BGJsonRepoModel.Key.ControllerType), key.ControllerType);

                    var fieldIds = new JSONArray();
                    jsonKey.Add(nameof(BGJsonRepoModel.Key.FieldIds), fieldIds);
                    key.ForEachField(field =>
                    {
                        fieldIds.Add(field.Id.ToString());
                    });
                });
                
                //indexes
                var jsonIndexes = new JSONArray();
                jsonMeta.Add(nameof(BGJsonRepoModel.Meta.Indexes),jsonIndexes);
                meta.ForEachIndex(index =>
                {
                    var jsonIndex = new JSONObject();
                    jsonIndexes.Add(jsonIndex);
                    jsonIndex.Add(nameof(BGJsonRepoModel.Index.Id), index.Id.ToString());
                    jsonIndex.Add(nameof(BGJsonRepoModel.Index.Name), index.Name);
                    jsonIndex.Add(nameof(BGJsonRepoModel.Index.FieldId), index.Field.Id.ToString());
                    jsonIndex.Add(nameof(BGJsonRepoModel.Index.Comment), index.Comment);
                    jsonIndex.Add(nameof(BGJsonRepoModel.Index.ControllerType), index.ControllerType);
                });

                //entities
                if (!skipData) entitiesWriter.OnWrite(jsonMeta, meta, name2Field);
            });

            //views
            var jsonViews = new JSONArray();
            jsonRoot.Add(nameof(BGJsonRepoModel.Views) ,jsonViews );
            repo.ForEachView(view =>
            {
                var jsonView = new JSONObject();
                jsonViews.Add(jsonView);
                jsonView.Add(nameof(BGJsonRepoModel.View.Id), view.Id.ToString());
                jsonView.Add(nameof(BGJsonRepoModel.View.Name), view.Name);
                jsonView.Add(nameof(BGJsonRepoModel.View.Addon), view.Addon);
                jsonView.Add(nameof(BGJsonRepoModel.View.Comment), view.Comment);
                jsonView.Add(nameof(BGJsonRepoModel.View.ControllerType), view.ControllerType);
                jsonView.Add(nameof(BGJsonRepoModel.View.Config), view.ConfigToString());

                //fields
                var jsonViewRepo = new JSONObject();
                WriteRepo(view.DelegateMeta.Repo, true, jsonViewRepo, entitiesWriter, removeSensitive);
                jsonView.Add(nameof(BGJsonRepoModel.View.Repo), jsonViewRepo);

                //mappings
                view.Mappings.Trim();
                var jsonMappings = new JSONArray();
                jsonView.Add(nameof(BGJsonRepoModel.View.MetaMappings), jsonMappings);
                foreach (var metaId in view.Mappings.IncludedMetas)
                {
                    var jsonMapping = new JSONObject();
                    jsonMappings.Add(jsonMapping);
                    jsonMapping.Add(nameof(BGJsonRepoModel.MetaMapping.MetaId), metaId.ToString());
                }
            });
        }

        private static void WriteObjMeta(BGMetaObject repoObj, JSONObject jsonObj)
        {
            jsonObj.Add(nameof(BGJsonRepoModel.ObjMeta.Id), repoObj.Id.ToString());
            jsonObj.Add(nameof(BGJsonRepoModel.ObjMeta.Name), repoObj.Name);
            jsonObj.Add(nameof(BGJsonRepoModel.ObjMeta.Addon), repoObj.Addon);
            jsonObj.Add(nameof(BGJsonRepoModel.ObjMeta.IsSystem), repoObj.System);
            jsonObj.Add(nameof(BGJsonRepoModel.ObjMeta.Type), repoObj.GetType().FullName);
            jsonObj.Add(nameof(BGJsonRepoModel.ObjMeta.Config), repoObj.ConfigToString());
            jsonObj.Add(nameof(BGJsonRepoModel.ObjMeta.Comment), repoObj.Comment);
            jsonObj.Add(nameof(BGJsonRepoModel.ObjMeta.ControllerType), repoObj.ControllerType);
        }
        
        internal interface JsonEntitiesWriter
        {
            BGJsonFormatEnum Format { get; }
            void OnWrite(JSONObject metaObject, BGMetaEntity meta, Dictionary<string, JSONObject> jsonObjects);
        }

    }
}