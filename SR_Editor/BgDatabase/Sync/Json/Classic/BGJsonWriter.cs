/*
<copyright file="BGJsonWriter.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Writer to JSON format
    /// </summary>
    public partial class BGJsonWriter
    {
        /// <summary>
        /// Write database data to JSON format 
        /// </summary>
        public string Write(BGRepo repo, bool skipData = false, Action<BGJsonRepoModel> beforeWriting = null)
        {
            var model = new BGJsonRepoModel();
            
            WriteRepo(repo, skipData, model);

            beforeWriting?.Invoke(model);
            
            return JsonUtility.ToJson(model, true);
        }

        public static void WriteRepo(BGRepo repo, bool skipData, BGJsonRepoModel model)
        {
            //            BGLocalizationEditorUglyHacks.WriteJson(model, repo);
            model.ProducedBy = "BGDatabase";
            model.DbVersion = BGRepo.Version;
            model.DbBuild = BGRepo.VersionBuild;
            model.Format = BGJsonFormatEnum.Classic;
            
            //addons
            repo.Addons.ForEachAddon(addon =>
            {
                model.Addons.Add(new BGJsonRepoModel.Addon
                {
                    Config = addon.ConfigToString(),
                    Type = addon.GetType().FullName
                });
            });


            //tables
            repo.ForEachMeta(meta =>
            {
                var jsonMeta = new BGJsonRepoModel.Meta();
                model.Metas.Add(jsonMeta);

                jsonMeta.Singleton = meta.Singleton;
                jsonMeta.UniqueName = meta.UniqueName;
                jsonMeta.EmptyName = meta.EmptyName;
                jsonMeta.UserDefinedReadonly = meta.UserDefinedReadonly;

                WriteObjMeta(meta, jsonMeta);

                meta.ForEachField(field =>
                {
                    var jsonField = new BGJsonRepoModel.Field
                    {
                        DefaultValue = field.DefaultValue,
                        Required = field.Required,
                        UserDefinedReadonly = field.UserDefinedReadonly,
                        CustomEditor = field.CustomEditorTypeAsString,
                        StringFormatter = field.CustomStringFormatterTypeAsString
                    };
                    jsonMeta.Fields.Add(jsonField);
                    WriteObjMeta(field, jsonField);
                });

                if (!skipData)
                    meta.ForEachEntity(entity =>
                    {
                        var jsonEntity = new BGJsonRepoModel.Entity();
                        jsonMeta.Entities.Add(jsonEntity);
                        jsonEntity.Id = entity.Id.ToString();

                        meta.ForEachField(field =>
                        {
                            jsonEntity.Values.Add(new BGJsonRepoModel.FieldValue
                            {
                                Name = field.Name,
                                Value = field.ToString(entity.Index)
                            });
                        });
                    });

                meta.ForEachKey(key =>
                {
                    var keyJson = new BGJsonRepoModel.Key
                    {
                        Id = key.Id.ToString(),
                        Unique = key.IsUnique,
                        Name = key.Name,
                        Comment = key.Comment,
                        ControllerType = key.ControllerType,
                    };
                    jsonMeta.Keys.Add(keyJson);
                    key.ForEachField(field =>
                    {
                        keyJson.FieldIds.Add(field.Id.ToString());
                    });
                });
                meta.ForEachIndex(index =>
                {
                    var indexJson = new BGJsonRepoModel.Index
                    {
                        Id = index.Id.ToString(),
                        Name = index.Name,
                        FieldId = index.Field.Id.ToString(),
                        Comment = index.Comment,
                        ControllerType = index.ControllerType,
                    };
                    jsonMeta.Indexes.Add(indexJson);
                });
            });
            
            //views
            repo.ForEachView(view =>
            {
                var jsonView = new BGJsonRepoModel.View()
                {
                    Id = view.Id.ToString(),
                    Name = view.Name,
                    Addon = view.Addon,
                    Comment = view.Comment,
                    ControllerType = view.ControllerType,
                    Config = view.ConfigToString(),
                };
                model.Views.Add(jsonView);

                //fields
                var viewRepo = new BGJsonRepoModel();
                WriteRepo(view.DelegateMeta.Repo, true, viewRepo);
                jsonView.Repo = viewRepo;
                
                //mappings
                view.Mappings.Trim();
                foreach (var metaId in view.Mappings.IncludedMetas) jsonView.MetaMappings.Add(new BGJsonRepoModel.MetaMapping { MetaId = metaId.ToString() });
            });
        }

        private static void WriteObjMeta(BGMetaObject repoObj, BGJsonRepoModel.ObjMeta jsonObj)
        {
            jsonObj.Id = repoObj.Id.ToString();
            jsonObj.Name = repoObj.Name;
            jsonObj.Addon = repoObj.Addon;
            jsonObj.IsSystem = repoObj.System;
            jsonObj.Type = repoObj.GetType().FullName;
            jsonObj.Config = repoObj.ConfigToString();
            jsonObj.Comment = repoObj.Comment;
            jsonObj.ControllerType = repoObj.ControllerType;
        }
    }
}