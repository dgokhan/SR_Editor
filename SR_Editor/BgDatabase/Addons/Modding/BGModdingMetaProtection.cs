/*
<copyright file="BGModdingMetaProtection.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Container for data protection settings for one single table
    /// </summary>
    public class BGModdingMetaProtection
    {
        /// <summary>
        /// Fired than any data protection settings are changed
        /// </summary>
        public event Action Changed;

        internal bool addDisabled;
        internal bool deleteDisabled;
        internal bool editDisabled;

        //fields
        public Dictionary<BGId, BGModdingRepoProtection.FieldSettingEnum> fields = new Dictionary<BGId, BGModdingRepoProtection.FieldSettingEnum>();

        //rows
        public Dictionary<BGId, bool> rowsEdit = new Dictionary<BGId, bool>();
        public Dictionary<BGId, bool> rowsDelete = new Dictionary<BGId, bool>();

        //cells
        public Dictionary<BGId, Dictionary<BGId, bool>> cells = new Dictionary<BGId, Dictionary<BGId, bool>>();


        /// <summary>
        /// Is adding new rows is disabled
        /// </summary>
        public bool AddDisabled
        {
            get => addDisabled;
            set
            {
                if (addDisabled == value) return;
                addDisabled = value;
                FireEvent();
            }
        }

        /// <summary>
        /// Is deleting new rows is disabled
        /// </summary>
        public bool DeleteDisabled
        {
            get => deleteDisabled;
            set
            {
                if (deleteDisabled == value) return;
                deleteDisabled = value;
                FireEvent();
            }
        }

        /// <summary>
        /// Is modifying existing rows data is disabled
        /// </summary>
        public bool EditDisabled
        {
            get => editDisabled;
            set
            {
                if (editDisabled == value) return;
                editDisabled = value;
                FireEvent();
            }
        }

        internal Dictionary<BGId, BGModdingRepoProtection.FieldSettingEnum> Fields => fields;

        internal Dictionary<BGId, bool> RowsEdit => rowsEdit;

        internal Dictionary<BGId, bool> RowsDelete => rowsDelete;

        internal Dictionary<BGId, Dictionary<BGId, bool>> Cells => cells;

        /// <summary>
        /// Is any global parameter is enabled 
        /// </summary>
        public bool ProtectedAny => AddDisabled || DeleteDisabled || EditDisabled;

        /// <summary>
        /// Creates the current object clone 
        /// </summary>
        public BGModdingMetaProtection Clone()
        {
            //do we really need to clone collections????? - this is super slow
            var clone = new BGModdingMetaProtection
            {
                addDisabled = addDisabled,
                deleteDisabled = deleteDisabled,
                editDisabled = editDisabled,


                fields = new Dictionary<BGId, BGModdingRepoProtection.FieldSettingEnum>(Fields),

                rowsEdit = new Dictionary<BGId, bool>(RowsEdit),
                rowsDelete = new Dictionary<BGId, bool>(RowsDelete),

                cells = new Dictionary<BGId, Dictionary<BGId, bool>>()
            };
            foreach (var pair in cells) clone.cells[pair.Key] = new Dictionary<BGId, bool>(pair.Value);
            return clone;
        }

        //on any change
        private void FireEvent()
        {
            Changed?.Invoke();
        }

        /// <summary>
        /// Is data editing is disabled for specified field 
        /// </summary>
        public bool HasFieldEdit(BGId fieldId)
        {
            return fields.ContainsKey(fieldId);
        }

        /// <summary>
        /// Set if data editing is disabled for specified field 
        /// </summary>
        public void SetFieldEdit(BGId fieldId, BGModdingRepoProtection.FieldSettingEnum setting)
        {
            if (setting == BGModdingRepoProtection.FieldSettingEnum.Inherited)
            {
                if (!fields.TryGetValue(fieldId, out var oldValue)) return;
                if (oldValue == setting)
                {
                    fields.Remove(fieldId);
                    return;
                }

                fields.Remove(fieldId);
            }
            else
            {
                if (fields.TryGetValue(fieldId, out var oldValue) && oldValue == setting) return;
                fields[fieldId] = setting;
            }

            FireEvent();
        }

        /// <summary>
        /// Get data protection level for specified field 
        /// </summary>
        public BGModdingRepoProtection.FieldSettingEnum GetFieldEdit(BGId fieldId)
        {
            if (!fields.TryGetValue(fieldId, out var value)) return BGModdingRepoProtection.FieldSettingEnum.Inherited;
            return value;
        }

        /// <summary>
        /// Removes existing data editing protection for specified row  
        /// </summary>
        public bool RemoveRowsEdit(BGId entityId)
        {
            var removed = rowsEdit.Remove(entityId);
            if (removed) FireEvent();
            return removed;
        }

        /// <summary>
        /// Removes deleting data protection for specified row  
        /// </summary>
        public bool RemoveRowsDelete(BGId entityId)
        {
            var removed = rowsDelete.Remove(entityId);
            if (removed) FireEvent();
            return removed;
        }

        /// <summary>
        /// Removes cell editing data protection for specified field  
        /// </summary>
        public bool RemoveCellField(BGId fieldId)
        {
            var removed = cells.Remove(fieldId);
            if (removed) FireEvent();
            return removed;
        }

        /// <summary>
        /// If specified cell has any protection
        /// </summary>
        public bool? Get(BGId fieldId, BGId entityId)
        {
            if (!cells.TryGetValue(fieldId, out var row2Disabled)) return null;

            if (!row2Disabled.TryGetValue(entityId, out var result)) return null;
            return result;
        }

        /// <summary>
        /// If specified row has remove protection
        /// </summary>
        public bool? GetRowDelete(BGId entityId)
        {
            if (!rowsDelete.TryGetValue(entityId, out var result)) return null;
            return result;
        }

        /// <summary>
        /// If specified row has edit protection
        /// </summary>
        public bool? GetRowEdit(BGId entityId)
        {
            if (!rowsEdit.TryGetValue(entityId, out var result)) return null;
            return result;
        }

        /// <summary>
        /// Add row edit protection setting for specified row
        /// </summary>
        public bool AddRowEditDisabled(BGId entityId, bool disabled)
        {
            if (rowsEdit.TryGetValue(entityId, out var result) && result == disabled) return false;
            rowsEdit[entityId] = disabled;
            FireEvent();
            return true;
        }

        /// <summary>
        /// Add row delete protection setting for specified row
        /// </summary>
        public bool AddRowDeleteDisabled(BGId entityId, bool disabled)
        {
            if (rowsDelete.TryGetValue(entityId, out var result) && result == disabled) return false;
            rowsDelete[entityId] = disabled;
            FireEvent();
            return true;
        }

        /// <summary>
        /// Remove row edit protection setting for specified row
        /// </summary>
        public bool RemoveRowEdit(BGId entityId)
        {
            if (!rowsEdit.Remove(entityId)) return false;
            FireEvent();
            return true;
        }

        /// <summary>
        /// Remove row delete protection setting for specified row
        /// </summary>
        public bool RemoveRowDelete(BGId entityId)
        {
            if (!rowsDelete.Remove(entityId)) return false;
            FireEvent();
            return true;
        }
    }
}