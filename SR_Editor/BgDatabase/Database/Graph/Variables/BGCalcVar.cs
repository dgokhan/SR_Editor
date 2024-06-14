/*
<copyright file="BGCalcVar.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// graph variable
    /// </summary>
    public class BGCalcVar : BGCalcVarA<BGCalcVarsOwnerI>, BGObjectI
    {
        // public event Action<BGCalcVar, object, object> OnValueChange2;

        private readonly BGId id;
        private string name;
        private bool isPublic;

        /// <inheritdoc />
        public BGId Id => id;


        /// <summary>
        /// variable unique name
        /// </summary>
        public string Name
        {
            get => name;
            set
            {
                if (name == value) return;
                var checkName = BGMetaObject.CheckName(value);
                if (checkName != null) throw new Exception(checkName);

                if (owner.GetVars()?.Variables.Count > 0 && owner.GetVars().GetVar(value) != null) throw new Exception("Variable with such name already exists");

                name = value;
                FireOnChange();
            }
        }

        /// <summary>
        /// is variable public
        /// </summary>
        public bool IsPublic
        {
            get => isPublic;
            set
            {
                if (isPublic == value) return;
                isPublic = value;
                FireOnChange();
            }
        }


        public override void FireOnChange()
        {
            base.FireOnChange();
            owner.OnVarsChange();
        }

        //====================================== constructors
        protected BGCalcVar(BGCalcVarsOwnerI owner, BGId id, string name, BGCalcTypeCode typeCode) : base(owner, typeCode)
        {
            this.id = id;

            var checkName = BGMetaObject.CheckName(name);
            if (checkName != null) throw new Exception(checkName);
            // if (!typeCode.CanBeUsedAsVar) throw new Exception("this type code can not be used as a var");
            this.name = name;
            owner.GetVars(true).AddVar(this);
        }

        //====================================== methods

        public override string ToString()
        {
            return name + " [" + (value == null ? "null" : value.ToString()) + "]";
        }

        //====================================== Equal
        protected bool Equals(BGCalcVar other) => base.Equals(other) && id.Equals(other.id); 

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((BGCalcVar)obj);
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }

        //====================================== Static
        public BGCalcVar CloneTo(BGCalcVarsOwnerI owner, bool cloneId = false, bool cloneValue = false)
        {
            var clone = Create(owner, cloneId ? id : BGId.NewId, name, typeCode);
            if (cloneValue)
            {
                clone.value = value;
                clone.isPublic = isPublic;
            }

            return clone;
        }

        //====================================== Static
        public static BGCalcVar Create(BGCalcVarsOwnerI owner, string name, BGCalcTypeCode code)
        {
            return Create(owner, BGId.NewId, name, code);
        }

        public static BGCalcVar Create(BGCalcVarsOwnerI owner, BGId id, string name, BGCalcTypeCode code)
        {
            return new BGCalcVar(owner, id, name, code);
        }
    }
}