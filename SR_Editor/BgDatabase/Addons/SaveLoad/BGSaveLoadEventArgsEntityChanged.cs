/*
<copyright file="BGSaveLoadEventArgsEntityChanged.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Entity changed event arguments
    /// </summary>
    public class BGSaveLoadEventArgsEntityChanged : BGEventArgsA
    {
        private static readonly BGObjectPool<BGSaveLoadEventArgsEntityChanged> pool = new BGObjectPool<BGSaveLoadEventArgsEntityChanged>(() => new BGSaveLoadEventArgsEntityChanged());
        protected override BGObjectPool Pool => pool;


        private BGMetaEntity meta;
        private BGEntity entity;

        private readonly List<FieldChangedData> fieldsData = new List<FieldChangedData>();

        public BGMetaEntity Meta => meta;

        public BGEntity Entity => entity;

        public List<FieldChangedData> FieldsData => fieldsData;

        private BGSaveLoadEventArgsEntityChanged()
        {
        }

        public override void Clear()
        {
            meta = null;
            entity = null;
            fieldsData.Clear();
        }

        public FieldChangedData GetFieldData(string fieldName)
        {
            for (var i = 0; i < FieldsData.Count; i++)
            {
                var fieldData = FieldsData[i];
                if (fieldData.field == null) continue;
                if (string.Equals(fieldData.field.Name, fieldName, StringComparison.Ordinal)) return fieldData;
            }

            return null;
        }

        public override string ToString()
        {
            var fields = new StringBuilder();
            for (var i = 0; i < FieldsData.Count; i++)
            {
                var fieldData = FieldsData[i];
                if (fieldData?.field == null) continue;
                if (i != 0) fields.Append(", ");
                fields.Append("[" + fieldData.field.Name + ": " + fieldData.oldValue + "->" + fieldData.newValue + "]");
            }

            return "BGSaveLoadEventArgsEntityChanged: " + (entity == null ? "[no entity]" : entity.FullName) + ", fields: " + fields;
        }

        public static BGSaveLoadEventArgsEntityChanged Get(BGMetaEntity meta, BGEntity entity, List<FieldChangedData> fieldData)
        {
            var instance = pool.Get();
            instance.Clear();
            instance.meta = meta;
            instance.entity = entity;
            if (fieldData != null)
            {
                foreach (var data in fieldData) instance.fieldsData.Add(data);
            }

            return instance;
        }

        public class FieldChangedData
        {
            internal BGField field;
            internal object oldValue;
            internal object newValue;

            public FieldChangedData(BGField field, object oldValue, object newValue)
            {
                this.field = field;
                this.oldValue = oldValue;
                this.newValue = newValue;
            }
        }
    }
}