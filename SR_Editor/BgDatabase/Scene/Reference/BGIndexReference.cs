/*
<copyright file="BGIndexReference.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Reference to the database index 
    /// </summary>
    [Serializable]
    public class BGIndexReference : BGMetaReference
    {
        //referenced index ID
        [SerializeField] private string indexId;

        private BGIndex index;

        public string IndexId => indexId;

        /// <summary>
        /// referenced index
        /// </summary>
        public BGIndex Index
        {
            get => GetIndex();
            set => SetIndex(value);
        }
        
        public BGIndexReference()
        {
        }

        public BGIndexReference(BGIndex index) => SetIndex(index);


        /// <summary>
        /// Retrieve the index using table and index ID
        /// </summary>
        public BGIndex GetIndex()
        {
            if (index?.Meta != null && !index.Meta.IsDeleted && index.Id == BGId.Parse(indexId)) return index;

            var meta = Meta;
            if (meta == null) return null;
            index = meta.GetIndex(BGId.Parse(indexId), false);
            return index;
        }

        /// <summary>
        /// Reference a new index 
        /// </summary>
        public void SetIndex(BGIndex index)
        {
            if (index == null) Reset();
            else
            {
                var metaConstraintId = MetaIdConstraint;
                if (!metaConstraintId.IsEmpty && index.MetaId != metaConstraintId)
                    throw new Exception("Can not assign an index, cause meta is wrong. IDs mismatch "
                                        + index.MetaId + "!=" + metaConstraintId);
                metaId = index.MetaId.ToString();
                indexId = index.Id.ToString();
                this.index = index;
            }
        }

        /// <summary>
        /// Reset internal state
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            indexId = null;
            index = null;
        }

        protected bool Equals(BGIndexReference other) => base.Equals(other) && indexId == other.indexId;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BGIndexReference)obj);
        }

        public override int GetHashCode()
        {
            unchecked { return (base.GetHashCode() * 397) ^ (indexId != null ? indexId.GetHashCode() : 0); }
        }
    }
}