/*
<copyright file="BGSaveLoadEventsManager.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    //save-load events container and manager
    internal class BGSaveLoadEventsManager
    {
        private BGAddonSaveLoad addon;

        //entities handlers
        private readonly Dictionary<BGEntityPointer, EventHandler<BGSaveLoadEventArgsEntityChanged>> metaPointer2EntityHandlers =
            new Dictionary<BGEntityPointer, EventHandler<BGSaveLoadEventArgsEntityChanged>>();

        //cell handlers
        private readonly Dictionary<BGCellPointer, EventHandler<BGSaveLoadEventArgsCellChanged>> metaPointer2CellHandlers =
            new Dictionary<BGCellPointer, EventHandler<BGSaveLoadEventArgsCellChanged>>();

        private BGRepo oldRepo;

        internal BGAddonSaveLoad Addon
        {
            set => addon = value;
        }

        internal BGSaveLoadEventsManager(BGAddonSaveLoad addon)
        {
            this.addon = addon;
        }

        internal void Add(BGEntityPointer pointer, EventHandler<BGSaveLoadEventArgsEntityChanged> handler)
        {
            Add(metaPointer2EntityHandlers, pointer, handler);
        }

        internal void Remove(BGEntityPointer pointer, EventHandler<BGSaveLoadEventArgsEntityChanged> handler)
        {
            Remove(metaPointer2EntityHandlers, pointer, handler);
        }

        internal void Add(BGCellPointer pointer, EventHandler<BGSaveLoadEventArgsCellChanged> handler)
        {
            Add(metaPointer2CellHandlers, pointer, handler);
        }

        internal void Remove(BGCellPointer pointer, EventHandler<BGSaveLoadEventArgsCellChanged> handler)
        {
            Remove(metaPointer2CellHandlers, pointer, handler);
        }


        //method is called before loading
        internal void BeforeLoad()
        {
            if (metaPointer2EntityHandlers.Count == 0) return;

            //make sure to save old data which has event listeners
            oldRepo = new BGRepo();
            ForEachMatchingEntity(
                (pointer, iteration) => iteration.oldMeta = iteration.meta.CloneTo(oldRepo, null, null, false),
                (pointer, iteration) => iteration.oldEntity = iteration.oldMeta.NewEntity(pointer.EntityId),
                iteration =>
                {
                    iteration.oldMeta.ForEachField(oldField =>
                    {
                        var field = iteration.meta.GetField(oldField.Id, false);
                        if (field == null) return;
                        var entity = iteration.entity;
                        oldField.CopyValue(field, entity.Id, entity.Index, entity.Id);
                    });
                });
            ForEachMatchingCell(
                (pointer, iteration) => iteration.oldMeta = iteration.meta.CloneTo(oldRepo, null, field => field.Id == iteration.field.Id, false),
                (pointer, iteration) => iteration.oldEntity = iteration.oldMeta.NewEntity(pointer.EntityId),
                (pointer, iteration) => iteration.oldField = iteration.field.CloneTo(iteration.oldMeta, false),
                iteration =>
                {
                    var entity = iteration.entity;
                    iteration.oldField.CopyValue(iteration.field, entity.Id, entity.Index, entity.Id);
                });
        }

        // method is called after loading 
        internal void AfterLoad()
        {
            //after load the database is fully reloaded- and the addon is new as well
            var fieldsData = new List<BGSaveLoadEventArgsEntityChanged.FieldChangedData>();
            ForEachMatchingEntity(null, null,
                iteration =>
                {
                    fieldsData.Clear();
                    iteration.oldMeta.ForEachField(oldField =>
                    {
                        var field = iteration.meta.GetField(oldField.Id, false);
                        if (field == null) return;
                        var entity = iteration.entity;
                        var oldEntity = iteration.oldEntity;

                        if (field.AreStoredValuesEqual(oldField, entity.Index, oldEntity.Index)) return;
                        fieldsData.Add(new BGSaveLoadEventArgsEntityChanged.FieldChangedData(field, oldField.GetValue(oldEntity.Index), field.GetValue(entity.Index)));
                    });
                    if (fieldsData.Count == 0) return;

                    // fire event
                    using (var args = BGSaveLoadEventArgsEntityChanged.Get(iteration.meta, iteration.entity, fieldsData))
                    {
                        try
                        {
                            iteration.handler(addon, args);
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }
                    }
                });

            ForEachMatchingCell(null, null, null,
                iteration =>
                {
                    if (iteration.field.AreStoredValuesEqual(iteration.oldField, iteration.entity.Index, iteration.oldEntity.Index)) return;

                    // fire event
                    using (var args = BGSaveLoadEventArgsCellChanged.Get(iteration.meta, iteration.field, iteration.entity,
                        iteration.oldField.GetValue(iteration.oldEntity.Index), iteration.field.GetValue(iteration.entity.Index)))
                    {
                        try
                        {
                            iteration.handler(addon, args);
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }
                    }
                });


            oldRepo = null;
        }

        //iterate matching entities
        private void ForEachMatchingEntity(
            Action<BGEntityPointer, EntityMatchingIteration<BGSaveLoadEventArgsEntityChanged>> metaProvider,
            Action<BGEntityPointer, EntityMatchingIteration<BGSaveLoadEventArgsEntityChanged>> entityProvider,
            Action<EntityMatchingIteration<BGSaveLoadEventArgsEntityChanged>> action
        )
        {
            if (metaPointer2EntityHandlers.Count == 0) return;
            var iteration = new EntityMatchingIteration<BGSaveLoadEventArgsEntityChanged>();
            var repo = addon.Repo;
            foreach (var pair in metaPointer2EntityHandlers)
            {
                var handler = pair.Value;
                if (handler == null) continue;
                iteration.Clear();
                iteration.handler = handler;
                var entityPointer = pair.Key;
                var entityId = entityPointer.EntityId;

                //repo meta
                iteration.meta = entityPointer.GetMeta(repo);
                if (iteration.meta == null) continue;
                //backup repo meta
                iteration.oldMeta = entityPointer.GetMeta(oldRepo);
                if (iteration.oldMeta == null)
                {
                    if (metaProvider == null) continue;
                    metaProvider(entityPointer, iteration);
                }

                if (iteration.oldMeta == null) continue;

                //repo entity
                iteration.entity = iteration.meta.GetEntity(entityId);
                if (iteration.entity == null) continue;
                //backup repo entity
                iteration.oldEntity = iteration.oldMeta.GetEntity(entityId);
                if (iteration.oldEntity == null)
                {
                    if (entityProvider == null) continue;
                    entityProvider(entityPointer, iteration);
                }

                if (iteration.oldEntity == null) continue;

                //action!
                action(iteration);
            }
        }

        //iterate matching cells
        private void ForEachMatchingCell(
            Action<BGCellPointer, CellMatchingIteration> metaProvider,
            Action<BGCellPointer, CellMatchingIteration> entityProvider,
            Action<BGCellPointer, CellMatchingIteration> fieldProvider,
            Action<CellMatchingIteration> action
        )
        {
            if (metaPointer2CellHandlers.Count == 0) return;
            var iteration = new CellMatchingIteration();
            var repo = addon.Repo;
            foreach (var pair in metaPointer2CellHandlers)
            {
                var handler = pair.Value;
                if (handler == null) continue;
                iteration.Clear();
                iteration.handler = handler;
                var cellPointer = pair.Key;
                var entityId = cellPointer.EntityId;
                var fieldId = cellPointer.FieldId;

                //repo meta
                iteration.meta = cellPointer.GetMeta(repo);
                if (iteration.meta == null) continue;
                //backup repo meta
                iteration.oldMeta = cellPointer.GetMeta(oldRepo);
                if (iteration.oldMeta == null)
                {
                    if (metaProvider == null) continue;
                    metaProvider(cellPointer, iteration);
                }

                if (iteration.oldMeta == null) continue;

                //repo field
                iteration.field = iteration.meta.GetField(fieldId, false);
                if (iteration.field == null) continue;
                //backup repo field
                iteration.oldField = iteration.oldMeta.GetField(fieldId, false);
                if (iteration.oldField == null)
                {
                    if (fieldProvider == null) continue;
                    fieldProvider(cellPointer, iteration);
                }

                if (iteration.oldField == null) continue;

                //repo entity
                iteration.entity = iteration.meta.GetEntity(entityId);
                if (iteration.entity == null) continue;
                //backup repo entity
                iteration.oldEntity = iteration.oldMeta.GetEntity(entityId);
                if (iteration.oldEntity == null)
                {
                    if (entityProvider == null) continue;
                    entityProvider(cellPointer, iteration);
                }

                if (iteration.oldEntity == null) continue;

                //action!
                action(iteration);
            }
        }

        //add a handler to a dictionary
        private static void Add<T, TK>(Dictionary<TK, EventHandler<T>> dict, TK pointer, EventHandler<T> handler) where T : BGEventArgsA where TK : BGMetaPointer
        {
            if (!dict.TryGetValue(pointer, out var myHandler))
            {
                myHandler = handler;
                dict.Add(pointer, myHandler);
            }
            else
            {
                myHandler += handler;
                dict[pointer] = myHandler;
            }
        }

        //remove a handler from a dictionary
        private static void Remove<T, TK>(Dictionary<TK, EventHandler<T>> dict, TK pointer, EventHandler<T> handler) where T : BGEventArgsA where TK : BGMetaPointer
        {
            if (!dict.TryGetValue(pointer, out var myHandler)) return;
            myHandler -= handler;
            if (myHandler == null) dict.Remove(pointer);
            else dict[pointer] = myHandler;
        }

        //data container for matching entities
        private class EntityMatchingIteration<T> where T : EventArgs
        {
            internal BGMetaEntity meta;
            internal BGMetaEntity oldMeta;
            internal BGEntity entity;
            internal BGEntity oldEntity;
            internal EventHandler<T> handler;

            internal virtual void Clear()
            {
                meta = null;
                oldMeta = null;
                entity = null;
                oldEntity = null;
            }
        }

        //data container for matching cells
        private class CellMatchingIteration : EntityMatchingIteration<BGSaveLoadEventArgsCellChanged>
        {
            internal BGField field;
            internal BGField oldField;

            internal override void Clear()
            {
                base.Clear();
                field = null;
                oldField = null;
            }
        }
    }
}