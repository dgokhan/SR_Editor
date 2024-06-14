/*
<copyright file="BGCalcCell.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// data container for cell pointer
    /// </summary>
    public class BGCalcCell : BGObjectI
    {
        private BGField field;
        private BGEntity entity;

        public BGId Id => entity?.Id ?? BGId.Empty;

        public BGField Field
        {
            get => field;
            set => field = value;
        }

        public BGEntity Entity
        {
            get => entity;
            set => entity = value;
        }

        public bool Ok => field != null && entity != null;

        public BGCalcCell(BGField field, BGEntity entity)
        {
            this.field = field ?? throw new Exception("Can not construct cell object, cause field is null");
            this.entity = entity ?? throw new Exception("Can not construct cell object, cause entity is null");
            if (entity.MetaId != field.MetaId)
                throw new Exception($"Can not construct cell object, cause provided entity and field are from different tables! MetaId mismatch {entity.MetaId} != {field.MetaId}");
        }

        public void Set(object value)
        {
            CheckFields();
            if (field.ReadOnly) throw new Exception($"Can not set cell value, cause field {field.FullName} is readonly!");
            field.SetValue(entity.Index, value);
        }

        public object Get()
        {
            CheckFields();
            if (field is BGFieldCalcI) throw new Exception("Can not get a value cause field is calculated field. To get calculated field value from graph, use 'Call calculated cell' unit!");
            return field.GetValue(entity.Index);
        }

        private void CheckFields()
        {
            if (field == null) throw new Exception("Can not get a value cause field is null");
            if (entity == null) throw new Exception("Can not get a value cause entity is null");
        }


        public override string ToString()
        {
            return $"[" + (field == null ? "" : field.FullName) + "].[" + (entity == null ? "" : entity.Name) + "]";
        }
    }
}