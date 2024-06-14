/*
<copyright file="BGLiveUpdateDataProcessor.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Remote data processor. 
    /// </summary>
    public class BGLiveUpdateDataProcessor
    {
        private readonly BGAddonLiveUpdate addon;
        private readonly BGRepo defaultRepo;

        public BGLiveUpdateDataProcessor(BGAddonLiveUpdate addon, BGRepo defaultRepo)
        {
            this.addon = addon;
            this.defaultRepo = defaultRepo;
        }


        // transfer data from abstract data container to underlying table
        internal void Process(BGLiveUpdateData data)
        {
            if (data == null) return;
            addon.Log.AddDetail("$ entity rows found for '$' table", data.RowsCount, data.Meta.Name);
            var valueResolver = addon.ValueResolver;
            data.ForEachRow((entityId, values) =>
            {
                //resolve entity
                BGEntity entity;
                var meta = data.Meta;
                if (entityId.IsEmpty) entity = meta.NewEntity();
                else
                {
                    if (meta.HasEntity(entityId))
                    {
                        addon.Log.AddDetail("Duplicate entity with id $, skipping", entityId);
                        return;
                    }

                    entity = meta.NewEntity(entityId);
                }

                //fill in all fields values
                for (var i = 0; i < values.Length; i++)
                {
                    var fieldValue = values[i];
                    var field = data.Fields[i];

                    try
                    {
                        if (valueResolver != null)
                            try
                            {
                                fieldValue = valueResolver.Resolve(field, fieldValue);
                            }
                            catch (Exception e)
                            {
                                Debug.Log("Value resolver thrown exception while resolving value. Field=" + field.FullName + ", value=" + fieldValue);
                                Debug.LogException(e);
                            }

                        BGUtil.FromString(field, entity.Index, fieldValue);
                        addon.Log.AddCellSuccess(meta.Id, "Index $. Field $. Value $", i, field.Name, fieldValue);
                    }
                    catch (Exception e)
                    {
//                        Debug.LogException(e);
                        try
                        {
                            if (!TryToFix(field, entity.Index, fieldValue)) AssignDefault(field, meta.Id, entity.Id, i, fieldValue);
                            else addon.Log.AddCellSuccess(meta.Id, "Index $. Field $. Value (fixed)=$", i, field.Name, fieldValue);
                        }
                        catch
                        {
                            //in case of the error- try to assign the value from default database
                            AssignDefault(field, meta.Id, entity.Id, i, fieldValue);
                        }
                    }
                }
            });
        }

        //try to assign default value from default database
        private void AssignDefault(BGField field, BGId metaId, BGId entityId, int i, string fieldValue)
        {
            var defaultAssigned = AssignDefault(field, metaId, entityId);
            addon.Log.AddCellFailed(metaId, entityId, field.Id, "Index $. Field $. Invalid value $. Fallback value was" + (defaultAssigned ? "" : " NOT") + " assigned", i, field.Name, fieldValue);
        }

        //try to assign default value from default database
        private bool AssignDefault(BGField field, BGId metaId, BGId entityId)
        {
            try
            {
                var existingMeta = defaultRepo[metaId];
                var existingField = existingMeta?.GetField(field.Id, false);
                if (existingField == null) return false;
                var existingEntity = existingMeta.GetEntity(entityId);
                if (existingEntity == null) return false;
                field.CopyValue(existingField, entityId, existingEntity.Index, entityId);
            }
            catch
            {
                return false;
            }

            return true;
        }

        //try to fix invalid data format
        private bool TryToFix(BGField field, int entityIndex, string fieldValue)
        {
            if (!string.IsNullOrEmpty(fieldValue))
                if (
                    field is BGFieldLong ||
                    field is BGFieldInt ||
                    field is BGFieldFloat ||
                    field is BGFieldDouble ||
                    field is BGFieldDecimal ||
                    field is BGFieldLongNullable ||
                    field is BGFieldIntNullable ||
                    field is BGFieldFloatNullable ||
                    field is BGFieldDoubleNullable ||
                    field is BGFieldListFloat ||
                    field is BGFieldListDouble
                )
                    if (fieldValue.IndexOf(',') != -1)
                    {
                        fieldValue = fieldValue.Replace(",", "");

                        BGUtil.FromString(field, entityIndex, fieldValue);

                        return true;
                    }

            return false;
        }


         /// <summary>
         /// Remote data for one single table
         /// </summary>
        public class BGLiveUpdateData
        {
            private readonly BGMetaEntity meta;

            private readonly BGField[] fields;

            private readonly List<string[]> data = new List<string[]>();
            private readonly List<BGId> entityIds = new List<BGId>();

            public int RowsCount => data.Count;

            public BGMetaEntity Meta => meta;

            public BGField[] Fields => fields;

            public BGLiveUpdateData(BGMetaEntity meta, BGField[] fields)
            {
                this.meta = meta;
                this.fields = fields;
            }

            //add fields values for specified entity
            public void Add(BGId entityId, string[] values, BGLiveUpdateLog log, int rowIndex)
            {
                if (values == null) return;
                //this should never happen
                // if (fields.Length != values.Length) throw new BGException("Field values counts mismatch $!=$", fields.Length, values.Length);
                if (fields.Length != values.Length) return;

                if (entityId.IsEmpty)
                {
                    //new row with no ID - check if any field has any value- we need to filter out empty rows 
                    var anyValue = false;
                    foreach (var value in values)
                    {
                        if (string.IsNullOrEmpty(value) || value.Trim().Equals(string.Empty)) continue;
                        anyValue = true;
                        break;
                    }

                    if (!anyValue)
                    {
                        log?.AddDetail("WARNING! No values found for row # $! skipping the row", rowIndex);
                        return;
                    }

                    entityId = BGId.NewId;
                }

                data.Add(values);
                entityIds.Add(entityId);
            }

            //iterate over all rows
            public void ForEachRow(Action<BGId, string[]> action)
            {
                for (var i = 0; i < data.Count; i++)
                {
                    var row = data[i];
                    var entityId = entityIds[i];
                    action(entityId, row);
                }
            }
        }
    }
}