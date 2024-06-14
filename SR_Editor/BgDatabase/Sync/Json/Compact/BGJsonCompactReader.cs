using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    public class BGJsonCompactReader
    {
        private readonly BGRepo repo = new BGRepo();

        public BGRepo Repo => repo;

        internal BGJsonCompactReader(string content, JsonEntitiesReader entitiesReader, bool skipData = false)
        {
            var jsonRoot = (JSONObject)JSONNode.Parse(content);

            var formatStr = Str(jsonRoot, nameof(BGJsonRepoModel.Format));
            if (string.IsNullOrEmpty(formatStr))
                throw new Exception($"Can not find required [{nameof(BGJsonRepoModel.Format)}] attribute in json content, probably json has the wrong format");
            var format = (BGJsonFormatEnum)byte.Parse(formatStr);
            if (format != entitiesReader.Format)
                throw new Exception($"Can not parse json: formats mismatch! Expected format is [{entitiesReader.Format}], actual format is [{format}] ");

            ReadArray(jsonRoot, nameof(BGJsonRepoModel.Addons), node =>
            {
                repo.Addons.Add(BGAddon.Create(
                    node[nameof(BGJsonRepoModel.Addon.Type)],
                    node[nameof(BGJsonRepoModel.Addon.Config)]
                ));
            });

            Read(jsonRoot, repo, entitiesReader, skipData);
        }

        /// <summary>
        /// read data from JSON model and fill in provided database
        /// </summary>
        private static void Read(JSONObject root, BGRepo repo, JsonEntitiesReader entitiesReader, bool skipData = false)
        {
            //metas
            ReadArray(root, nameof(BGJsonRepoModel.Metas), metaNode =>
            {
                var meta = BGMetaEntity.Create(repo, Str(metaNode, nameof(BGJsonRepoModel.Meta.Type)),
                    new BGId(Str(metaNode, nameof(BGJsonRepoModel.Meta.Id))),
                    Str(metaNode, nameof(BGJsonRepoModel.Meta.Name)),
                    Str(metaNode, nameof(BGJsonRepoModel.Meta.Config)),
                    Bool(metaNode, nameof(BGJsonRepoModel.Meta.IsSystem)),
                    Str(metaNode, nameof(BGJsonRepoModel.Meta.Addon)),
                    Bool(metaNode, nameof(BGJsonRepoModel.Meta.UniqueName)),
                    Bool(metaNode, nameof(BGJsonRepoModel.Meta.Singleton)),
                    Bool(metaNode, nameof(BGJsonRepoModel.Meta.EmptyName)));
                meta.Comment = Str(metaNode, nameof(BGJsonRepoModel.Meta.Comment));
                meta.ControllerType = Str(metaNode, nameof(BGJsonRepoModel.Meta.ControllerType));
                meta.UserDefinedReadonly = Bool(metaNode, nameof(BGJsonRepoModel.Meta.UserDefinedReadonly));

                //fields
                var name2Field = new Dictionary<string, JSONObject>(meta.CountFields);
                ReadArray((JSONObject)metaNode, nameof(BGJsonRepoModel.Meta.Fields), fieldNode =>
                {
                    var field = BGField.Create(meta,
                        Str(fieldNode, nameof(BGJsonRepoModel.Field.Type)),
                        new BGId(Str(fieldNode, nameof(BGJsonRepoModel.Field.Id))),
                        Str(fieldNode, nameof(BGJsonRepoModel.Field.Name)),
                        Str(fieldNode, nameof(BGJsonRepoModel.Field.Config)),
                        Bool(fieldNode, nameof(BGJsonRepoModel.Field.IsSystem)),
                        Str(fieldNode, nameof(BGJsonRepoModel.Field.Addon)),
                        Str(fieldNode, nameof(BGJsonRepoModel.Field.DefaultValue)),
                        Bool(fieldNode, nameof(BGJsonRepoModel.Field.Required)));
                    field.CustomStringFormatterTypeAsString = Str(fieldNode, nameof(BGJsonRepoModel.Field.StringFormatter));
                    field.CustomEditorTypeAsString = Str(fieldNode, nameof(BGJsonRepoModel.Field.CustomEditor));
                    field.Comment = Str(fieldNode, nameof(BGJsonRepoModel.Field.Comment));
                    field.ControllerType = Str(fieldNode, nameof(BGJsonRepoModel.Field.ControllerType));
                    field.UserDefinedReadonly = Bool(fieldNode, nameof(BGJsonRepoModel.Field.UserDefinedReadonly));
                    name2Field[field.Name] = (JSONObject)fieldNode;
                });

                //keys
                ReadArray((JSONObject)metaNode, nameof(BGJsonRepoModel.Meta.Keys), keyNode =>
                {
                    var keyName = Str(keyNode, nameof(BGJsonRepoModel.Key.Name));
                    var fields = new List<BGField>();
                    ReadArray((JSONObject)keyNode, nameof(BGJsonRepoModel.Key.FieldIds), keyFieldNode =>
                    {
                        var fieldId = new BGId(keyFieldNode.Value);
                        var field = meta.GetField(fieldId, false);
                        if (field == null)
                        {
                            //no field- no key
                            return;
                        }

                        fields.Add(field);
                    });
                    if (fields.Count == 0) return;

                    var key = BGKey.Create(new BGId(Str(keyNode, nameof(BGJsonRepoModel.Key.Id))), keyName,
                        Bool(keyNode, nameof(BGJsonRepoModel.Key.Unique)), fields.ToArray());
                    key.Comment = Str(keyNode, nameof(BGJsonRepoModel.Key.Comment));
                    key.ControllerType = Str(keyNode, nameof(BGJsonRepoModel.Key.ControllerType));
                });

                //indexes
                ReadArray((JSONObject)metaNode, nameof(BGJsonRepoModel.Meta.Indexes), indexNode =>
                {
                    var indexId = new BGId(Str(indexNode, nameof(BGJsonRepoModel.Index.Id)));
                    var fieldId = new BGId(Str(indexNode, nameof(BGJsonRepoModel.Index.FieldId)));
                    var fieldName = Str(indexNode, nameof(BGJsonRepoModel.Index.Name));
                    var field = meta.GetField(fieldId, false);
                    if (field == null)
                    {
                        //no field- no key
                        return;
                    }

                    var index = BGIndex.Create(indexId, fieldName, field);
                    index.Comment = Str(indexNode, nameof(BGJsonRepoModel.Index.Comment));
                    index.ControllerType = Str(indexNode, nameof(BGJsonRepoModel.Index.ControllerType));
                });

                //entities
                if (!skipData) entitiesReader.OnRead((JSONObject)metaNode, meta, name2Field);
            });


            //views
            ReadArray(root, nameof(BGJsonRepoModel.Views), viewNode =>
            {
                //views
                var view = BGMetaView.Create(repo, new BGId(Str(viewNode, nameof(BGJsonRepoModel.View.Id))), Str(viewNode, nameof(BGJsonRepoModel.View.Name)));
                view.System = Bool(viewNode, nameof(BGJsonRepoModel.View.IsSystem));
                view.Addon = Str(viewNode, nameof(BGJsonRepoModel.View.Addon));
                view.Comment = Str(viewNode, nameof(BGJsonRepoModel.View.Comment));
                view.ControllerType = Str(viewNode, nameof(BGJsonRepoModel.View.ControllerType));
                view.ConfigFromString(Str(viewNode, nameof(BGJsonRepoModel.View.Config)));

                //fields
                var viewRepo = new BGRepo();
                Read((JSONObject)viewNode[nameof(BGJsonRepoModel.View.Repo)], viewRepo, entitiesReader, true);
                view.DelegateMeta = (BGMetaRow)viewRepo.GetMeta(view.Id);

                //mappings
                ReadArray((JSONObject)viewNode, nameof(BGJsonRepoModel.View.MetaMappings),
                    node => { view.Mappings.Add(new BGId(Str(node, nameof(BGJsonRepoModel.MetaMapping.MetaId)))); });
            });
        }

        internal static string Str(JSONNode node, string name)
        {
            var subNode = node[name];
            return subNode == null ? null : subNode.Value;
        }

        internal static bool Bool(JSONNode node, string name)
        {
            var subNode = node[name];
            return subNode != null && subNode.AsBool;
        }

        internal static int ReadArray(JSONObject node, string name, Action<JSONNode> action)
        {
            var array = node[name];
            if (!(array is JSONArray nodeArray)) return 0;
            var values = nodeArray.Values;
            var count = 0;
            foreach (var arrayNode in values)
            {
                count++;
                action(arrayNode);
            }
            return count;
        }

        internal interface JsonEntitiesReader
        {
            BGJsonFormatEnum Format { get; }
            void OnRead(JSONObject metaObject, BGMetaEntity meta, Dictionary<string, JSONObject> jsonObjects);
        }
    }
}