/*
<copyright file="BGCalcFlowEvents.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Data container for graph events and listeners 
    /// </summary>
    public class BGCalcFlowEvents
    {
        private const int MAXRowsCount = 5;

        private readonly Func<string> onChange;
        private readonly List<EventsFieldData> fields = new List<EventsFieldData>();
        private readonly List<EventsRowsData> editRow = new List<EventsRowsData>();
        private readonly List<EventsMetaData> createRow = new List<EventsMetaData>();
        private readonly List<EventsMetaData> deleteRow = new List<EventsMetaData>();

        private bool listenersAdded;

        public BGCalcFlowEvents(Func<string> onChange) => this.onChange = onChange;

        public bool AddBatchListeners { get; set; }

        /// <summary>
        /// provided meta should have onRowCreate listener  
        /// </summary>
        public void AddOnCreate(BGMetaEntity meta)
        {
            for (var i = 0; i < createRow.Count; i++)
            {
                var metaData = createRow[i];
                if (metaData.MetaId == meta.Id) return;
            }

            createRow.Add(new EventsMetaData(meta.Id));
        }

        /// <summary>
        /// provided meta should have onRowDelete listener  
        /// </summary>
        public void AddOnDelete(BGMetaEntity meta)
        {
            for (var i = 0; i < deleteRow.Count; i++)
            {
                var metaData = deleteRow[i];
                if (metaData.MetaId == meta.Id) return;
            }

            deleteRow.Add(new EventsMetaData(meta.Id));
        }

        /// <summary>
        /// provided meta + entity should have onRowUpdate listener  
        /// </summary>
        public void AddOnEdit(BGField field, BGEntity entity)
        {
            var found = false;
            for (var i = 0; i < fields.Count; i++)
            {
                var fieldData = fields[i];
                if (field.Id != fieldData.FieldId) continue;
                found = true;
            }

            if (found) return;
            for (var i = 0; i < editRow.Count; i++)
            {
                var rows = editRow[i];
                if (field.Id != rows.FieldId) continue;
                found = rows.EntityIds.Contains(entity.Id);
                if (found) break;
                if (rows.EntityIds.Count > MAXRowsCount)
                {
                    editRow.RemoveAt(i);
                    fields.Add(new EventsFieldData(field.MetaId, field.Id));
                }
                else rows.EntityIds.Add(entity.Id);

                found = true;
                break;
            }

            if (!found)
            {
                var rowsData = new EventsRowsData(field.MetaId, field.Id);
                rowsData.EntityIds.Add(entity.Id);
                editRow.Add(rowsData);
            }
        }

        /// <summary>
        /// Add database listeners using previously added listeners meta data
        /// </summary>
        public void AddListeners()
        {
            if (!Application.isPlaying && !BGUtil.TestIsRunning) return;
            if (listenersAdded) return;
            listenersAdded = true;
            //should we add add/remove entity listeners as well?

            foreach (var meta in createRow)
            {
                var table = BGRepo.I.GetMeta(meta.MetaId);
                if (table == null) continue;
                table.AnyEntityAdded += EntityAdded;
            }

            foreach (var meta in deleteRow)
            {
                var table = BGRepo.I.GetMeta(meta.MetaId);
                if (table == null) continue;
                table.AnyEntityDeleted += EntityDeleted;
            }

            foreach (var field in fields)
            {
                var table = BGRepo.I.GetMeta(field.MetaId);
                var dbField = table?.GetField(field.FieldId, false);
                if (dbField == null) continue;
                dbField.ValueChanged += FieldHandler;
            }

            foreach (var row in editRow)
            {
                var table = BGRepo.I.GetMeta(row.MetaId);
                var dbField = table?.GetField(row.FieldId, false);
                if (dbField == null) continue;
                dbField.ValueChanged += FieldHandlerByEntities;
            }

            if (AddBatchListeners)
            {
                BGRepo.I.Events.OnBatchUpdate += OnBatch;
                BGRepo.OnLoad += OnLoad;
            }
        }

        /// <summary>
        /// Clear all internal state
        /// </summary>
        public void Clear()
        {
            DisposeListeners();
            fields.Clear();
            editRow.Clear();
            createRow.Clear();
            deleteRow.Clear();
            listenersAdded = false;
        }


        //remove previously added listeners
        private void DisposeListeners()
        {
            foreach (var field in fields)
            {
                var table = BGRepo.I.GetMeta(field.MetaId);
                var dbField = table?.GetField(field.FieldId, false);
                if (dbField == null) continue;
                dbField.ValueChanged -= FieldHandler;
            }

            foreach (var row in editRow)
            {
                var table = BGRepo.I.GetMeta(row.MetaId);
                var dbField = table?.GetField(row.FieldId, false);
                if (dbField == null) continue;
                dbField.ValueChanged -= FieldHandlerByEntities;
            }

            foreach (var meta in createRow)
            {
                var table = BGRepo.I.GetMeta(meta.MetaId);
                if (table == null) continue;
                table.AnyEntityAdded -= EntityAdded;
            }

            foreach (var meta in deleteRow)
            {
                var table = BGRepo.I.GetMeta(meta.MetaId);
                if (table == null) continue;
                table.AnyEntityDeleted -= EntityDeleted;
            }

            if (AddBatchListeners)
            {
                BGRepo.I.Events.OnBatchUpdate -= OnBatch;
                BGRepo.OnLoad -= OnLoad;
            }
        }

        //on repo loaded event
        private void OnLoad(bool success)
        {
            if (!success) return;
            onChange();
        }

        //on batch update 
        private void OnBatch(object sender, BGEventArgsBatch e)
        {
            var found = false;
            if (e.EverythingChanged) found = true;
            else
            {
                for (var i = 0; i < editRow.Count; i++)
                {
                    var rowsData = editRow[i];
                    if (!e.WasEntitiesUpdated(rowsData.MetaId)) continue;
                    found = true;
                    break;
                }

                if (!found)
                    for (var i = 0; i < createRow.Count; i++)
                    {
                        var metaData = createRow[i];
                        if (!e.WasEntitiesAdded(metaData.MetaId)) continue;
                        found = true;
                        break;
                    }

                if (!found)
                    for (var i = 0; i < deleteRow.Count; i++)
                    {
                        var metaData = deleteRow[i];
                        if (!e.WasEntitiesDeleted(metaData.MetaId)) continue;
                        found = true;
                        break;
                    }
            }

            if (found) onChange();
        }

        //on field value changed
        private void FieldHandler(object sender, BGEventArgsField e) => onChange();

        //on field value changed
        private void FieldHandlerByEntities(object sender, BGEventArgsField e)
        {
            var found = false;
            for (var i = 0; i < editRow.Count; i++)
            {
                var row = editRow[i];

                if (!row.EntityIds.Contains(e.Entity.Id)) continue;
                found = true;
                break;
            }

            if (found) onChange();
        }

        //on row deleted
        private void EntityDeleted(object sender, BGEventArgsAnyEntity e) => onChange();

        //on row added
        private void EntityAdded(object sender, BGEventArgsAnyEntity e) => onChange();

        //data container for table listeners meta data
        private class EventsMetaData
        {
            public readonly BGId MetaId;

            public EventsMetaData(BGId metaId)
            {
                MetaId = metaId;
            }

            protected bool Equals(EventsMetaData other)
            {
                return MetaId.Equals(other.MetaId);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;
                return Equals((EventsMetaData)obj);
            }

            public override int GetHashCode()
            {
                return MetaId.GetHashCode();
            }
        }

        //data container for field listeners meta data
        private class EventsFieldData : EventsMetaData
        {
            public readonly BGId FieldId;

            public EventsFieldData(BGId metaId, BGId fieldId) : base(metaId)
            {
                FieldId = fieldId;
            }

            protected bool Equals(EventsFieldData other)
            {
                return base.Equals(other) && FieldId.Equals(other.FieldId);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;
                return Equals((EventsFieldData)obj);
            }

            public override int GetHashCode()
            {
                unchecked { return (base.GetHashCode() * 397) ^ FieldId.GetHashCode(); }
            }
        }

        //data container for entity listeners meta data
        private class EventsRowsData : EventsFieldData
        {
            public readonly List<BGId> EntityIds = new List<BGId>();

            public EventsRowsData(BGId metaId, BGId fieldId) : base(metaId, fieldId)
            {
            }
        }
    }
}