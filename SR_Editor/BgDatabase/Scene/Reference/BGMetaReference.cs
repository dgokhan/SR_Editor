/*
<copyright file="BGMetaReference.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Reference to the database table
    /// </summary>
    [Serializable]
    public class BGMetaReference
    {
        [SerializeField] protected string metaId;

        /// <summary>
        /// meta constraint is used to limit table selection to the single table 
        /// </summary>
        public virtual BGId MetaIdConstraint => BGId.Empty;

        /// <summary>
        /// Referenced table
        /// </summary>
        public BGMetaEntity Meta
        {
            get
            {
                var idConstraint = MetaIdConstraint;
                var meta = BGRepo.I.GetMeta(idConstraint.IsEmpty ? BGId.Parse(metaId) : idConstraint);
                return meta;
            }
            set => metaId = value?.Id.ToString();
        }

        /// <summary>
        /// Referenced table ID as string
        /// </summary>
        public string MetaId => metaId;

        /// <summary>
        /// Reset internal state
        /// </summary>
        public virtual void Reset() => metaId = null;

        protected bool Equals(BGMetaReference other) => metaId == other.metaId;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BGMetaReference)obj);
        }

        public override int GetHashCode() => (metaId != null ? metaId.GetHashCode() : 0);

    }
}