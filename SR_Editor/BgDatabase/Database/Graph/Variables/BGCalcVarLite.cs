/*
<copyright file="BGCalcVarLite.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// graph lite variable.
    /// Lite vars are used in ports
    /// </summary>
    public class BGCalcVarLite : BGCalcVarA<BGCalcVarsLiteOwnerI>
    {
        private readonly byte id;

        public byte Id => id;

        protected BGCalcVarLite(BGCalcVarsLiteOwnerI owner, BGCalcTypeCode typeCode, byte id) : base(owner, typeCode)
        {
            this.id = id;
            owner.GetVars(true).AddVar(this);
        }


        //====================================== equals

        protected bool Equals(BGCalcVarLite other)
        {
            return base.Equals(other) && id == other.id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((BGCalcVarLite)obj);
        }

        public override int GetHashCode()
        {
            unchecked { return (base.GetHashCode() * 397) ^ id.GetHashCode(); }
        }


        //====================================== Misc
        /// <summary>
        /// CLone lite variable to provided owner
        /// </summary>
        public BGCalcVarLite CloneTo(BGCalcVarsLiteOwnerI owner, bool cloneValue = false)
        {
            var clone = Create(owner, id, typeCode);
            if (cloneValue) clone.value = value;
            return clone;
        }

        /// <inheritdoc />
        public override void FireOnChange()
        {
            base.FireOnChange();
            owner.OnVarsChange();
        }

        //====================================== Static
        public static BGCalcVarLite Create(BGCalcVarsLiteOwnerI owner, byte id, BGCalcTypeCode code) => new BGCalcVarLite(owner, code, id);
    }
}