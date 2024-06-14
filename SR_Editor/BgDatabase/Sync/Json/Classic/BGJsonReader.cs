/*
<copyright file="BGJsonReader.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// JSON content reader
    /// </summary>
    public partial class BGJsonReader
    {
        private readonly BGRepo repo = new BGRepo();

        public BGRepo Repo => repo;

        public BGJsonReader(string content, bool skipData = false)
        {
            var model = JsonUtility.FromJson<BGJsonRepoModel>(content);
            BGUtil.ForEach(model.Addons, addon => repo.Addons.Add(BGAddon.Create(addon.Type, addon.Config)));

            Read(model, repo, skipData);
        }

        /// <summary>
        /// read data from JSON model and fill in provided database
        /// </summary>
        public static void Read(BGJsonRepoModel model, BGRepo repo, bool skipData = false)
        {
            if (model.Format != BGJsonFormatEnum.Classic) throw new Exception($"Can not import JSON: json format mismatch, expected format is {BGJsonFormatEnum.Classic}," +
                                                                              $" the actual format is {model.Format}"); 
            
            //metas
            BGUtil.ForEach(model.Metas, jsonMeta =>
            {
                var meta = BGMetaEntity.Create(repo, jsonMeta.Type, new BGId(jsonMeta.Id), jsonMeta.Name, jsonMeta.Config, jsonMeta.IsSystem, jsonMeta.Addon, jsonMeta.UniqueName, jsonMeta.Singleton,
                    jsonMeta.EmptyName);
                meta.Comment = jsonMeta.Comment;
                meta.ControllerType = jsonMeta.ControllerType;
                meta.UserDefinedReadonly = jsonMeta.UserDefinedReadonly;

                //fields
                BGUtil.ForEach(jsonMeta.Fields, jsonField =>
                {
                    var field = BGField.Create(meta, jsonField.Type, new BGId(jsonField.Id), jsonField.Name, jsonField.Config, jsonField.IsSystem, jsonField.Addon, jsonField.DefaultValue,
                        jsonField.Required);
                    field.CustomStringFormatterTypeAsString = string.IsNullOrEmpty(jsonField.StringFormatter) ? null : jsonField.StringFormatter;
                    field.CustomEditorTypeAsString = jsonField.CustomEditor;
                    field.Comment = jsonField.Comment;
                    field.ControllerType = jsonField.ControllerType;
                    field.UserDefinedReadonly = jsonField.UserDefinedReadonly;
                });

                //entities
                if (!skipData)
                    BGUtil.ForEach(jsonMeta.Entities, jsonEntity =>
                    {
                        var entity = meta.NewEntity(new BGId(jsonEntity.Id));
                        BGUtil.ForEach(jsonEntity.Values, value =>
                        {
                            try
                            {
                                meta.GetField(value.Name).FromString(entity.Index, value.Value);
                            }
                            catch (Exception e)
                            {
                            }
                        });
                    });
                BGUtil.ForEach(jsonMeta.Keys, jsonKey =>
                {
                    if (jsonKey.FieldIds == null || jsonKey.FieldIds.Count == 0) return;
                    var fields = new List<BGField>();
                    foreach (var fieldIdStr in jsonKey.FieldIds)
                    {
                        var fieldId = new BGId(fieldIdStr);
                        var field = meta.GetField(fieldId, false);
                        if (field == null)
                        {
                            //no field- no key
                            return;
                        }

                        fields.Add(field);
                    }

                    var key = BGKey.Create(new BGId(jsonKey.Id), jsonKey.Name, jsonKey.Unique, fields.ToArray());
                    key.Comment = jsonKey.Comment;
                    key.ControllerType = jsonKey.ControllerType;
                });
                
                BGUtil.ForEach(jsonMeta.Indexes, jsonIndex =>
                {
                    var fieldId = new BGId(jsonIndex.FieldId);
                    var field = meta.GetField(fieldId, false);
                    if (field == null)
                    {
                        //no field- no key
                        return;
                    }

                    var index = BGIndex.Create(new BGId(jsonIndex.Id), jsonIndex.Name, field);
                    index.Comment = jsonIndex.Comment;
                    index.ControllerType = jsonIndex.ControllerType;

                });
            });

            //views
            BGUtil.ForEach(model.Views, jsonView =>
            {
                //views
                var view = BGMetaView.Create(repo, new BGId(jsonView.Id), jsonView.Name);
                view.System = jsonView.IsSystem;
                view.Addon = jsonView.Addon;
                view.Comment = jsonView.Comment;
                view.ControllerType = jsonView.ControllerType;
                view.ConfigFromString(jsonView.Config);

                //fields
                var viewRepo = new BGRepo();
                Read(jsonView.Repo, viewRepo, true);
                view.DelegateMeta = (BGMetaRow) viewRepo.GetMeta(view.Id);
                
                //mappings
                BGUtil.ForEach(jsonView.MetaMappings, jsonMetaMapping =>
                {
                    view.Mappings.Add(new BGId(jsonMetaMapping.MetaId));
                });
            });

        }
    }
}