/*
<copyright file="BGCalcVarA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// abstract parent for var & varlite 
    /// </summary>
    public abstract class BGCalcVarA
    {
        public event Action OnValueChange;
        protected BGCalcTypeCode typeCode;

        protected object value;

        /// <summary>
        /// variable type
        /// </summary>
        public Type Type => typeCode.Type;

        /// <summary>
        /// variable type code
        /// </summary>
        public BGCalcTypeCode TypeCode => typeCode;

        /// <summary>
        /// variable value
        /// </summary>
        // set => typeCode = value;
        public object Value
        {
            get
            {
                switch (value)
                {
                    case null:
                        return value;
                    case BGObjectI dbObject when RefreshDbValue(ref dbObject):
                        value = dbObject;
                        break;
                }

                return value;
            }
            set
            {
                if (Equals(this.value, value)) return;
                var oldValue = this.value;
                this.value = value;
                FireOnChange();
            }
        }

        protected BGCalcVarA(BGCalcTypeCode typeCode)
        {
            this.typeCode = typeCode ?? throw new Exception("code can not be null");
            if (typeCode.SupportDefaultValue) value = typeCode.DefaultValue;
        }

        /// <summary>
        /// on variable value changed event
        /// </summary>
        public virtual void FireOnChange() => OnValueChange?.Invoke();

        /// <summary>
        /// clear listeners
        /// </summary>
        public virtual void ClearListeners() => OnValueChange = null;

        //================================================== Equals
        protected bool Equals(BGCalcVarA other) => Equals(typeCode, other.typeCode) && Equals(value, other.value);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((BGCalcVarA)obj);
        }

        public override int GetHashCode()
        {
            unchecked { return ((typeCode != null ? typeCode.GetHashCode() : 0) * 397) ^ (value != null ? value.GetHashCode() : 0); }
        }

        //================================================== Refresh db object
        /// <summary>
        /// this method is used to update database object reference to the actual object in case database was reloaded 
        /// </summary>
        public static bool RefreshDbValue(ref BGObjectI dbObject)
        {
            //auto-renew for db objects
            var refreshed = false;
            switch (dbObject)
            {
                case BGMetaEntity meta:
                {
                    if (meta.IsDeleted)
                    {
                        dbObject = BGRepo.I.GetMeta(meta.Id);
                        refreshed = true;
                    }

                    break;
                }
                case BGField field:
                {
                    if (field.IsDeleted || field.Meta.IsDeleted)
                    {
                        refreshed = true;
                        dbObject = null;
                        var meta = BGRepo.I.GetMeta(field.MetaId);
                        if (meta != null) dbObject = meta.GetField(field.Id, false);
                    }

                    break;
                }
                case BGEntity entity:
                {
                    if (entity.Meta.IsDeleted)
                    {
                        refreshed = true;
                        dbObject = null;
                        var meta = BGRepo.I.GetMeta(entity.MetaId);
                        if (meta != null) dbObject = meta.GetEntity(entity.Id);
                    }

                    break;
                }
                case BGCalcCell cell:
                {
                    var cellField = cell.Field;
                    var cellEntity = cell.Entity;
                    if (cellField != null)
                    {
                        if (cellField.IsDeleted || cellField.Meta.IsDeleted)
                        {
                            refreshed = true;
                            cell.Field = null;
                            var meta = BGRepo.I.GetMeta(cellField.MetaId);
                            if (meta != null)
                            {
                                cell.Field = meta.GetField(cellField.Id, false);
                                if (cellEntity != null && cellEntity.Meta.IsDeleted) cell.Entity = meta.GetEntity(cellEntity.Id);
                            }
                        }
                    }
                    else if (cellEntity != null)
                        if (cellEntity.IsDeleted)
                        {
                            refreshed = true;
                            cell.Entity = null;
                            var meta = BGRepo.I.GetMeta(cellEntity.MetaId);
                            if (meta != null) cell.Entity = meta.GetEntity(cellEntity.Id);
                        }

                    break;
                }
            }

            return refreshed;
        }
    }

    /// <summary>
    /// abstract parent for var & varlite  with variable owner
    /// </summary>
    public abstract class BGCalcVarA<T> : BGCalcVarA where T : BGCalcVarsOwnerBaseI
    {
        protected readonly T owner;

        protected BGCalcVarA(T owner, BGCalcTypeCode typeCode) : base(typeCode)
        {
            if (owner == null) throw new Exception("var owner can not be null");
            this.owner = owner;
        }
    }
}