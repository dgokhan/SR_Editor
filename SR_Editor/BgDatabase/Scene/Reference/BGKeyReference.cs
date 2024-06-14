/*
<copyright file="BGKeyReference.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Reference to the database key 
    /// </summary>
    [Serializable]
    public class BGKeyReference : BGMetaReference
    {
        //referenced key ID
        [SerializeField] private string keyId;

        private BGKey key;

        
        /// <summary>
        /// referenced key
        /// </summary>
        public BGKey Key
        {
            get => GetKey();
            set => SetKey(value);
        }

        public string KeyId => keyId;

        public BGKeyReference()
        {
        }

        public BGKeyReference(BGKey key) => SetKey(key);


        /// <summary>
        /// Retrieve the key using table and key IDs
        /// </summary>
        public BGKey GetKey()
        {
            if (key?.Meta != null && !key.Meta.IsDeleted && key.Id == BGId.Parse(keyId)) return key;

            var meta = Meta;
            if (meta == null) return null;
            key = meta.GetKey(BGId.Parse(keyId), false);
            return key;
        }

        /// <summary>
        /// Reference a new key 
        /// </summary>
        public void SetKey(BGKey key)
        {
            if (key == null) Reset();
            else
            {
                var metaConstraintId = MetaIdConstraint;
                if (!metaConstraintId.IsEmpty && key.MetaId != metaConstraintId)
                    throw new Exception("Can not assign a key, cause meta is wrong. IDs mismatch "
                                        + key.MetaId + "!=" + metaConstraintId);
                metaId = key.MetaId.ToString();
                keyId = key.Id.ToString();
                this.key = key;
            }
        }

        /// <summary>
        /// Reset internal state
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            keyId = null;
            key = null;
        }

        protected bool Equals(BGKeyReference other) => base.Equals(other) && keyId == other.keyId;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BGKeyReference)obj);
        }

        public override int GetHashCode()
        {
            unchecked { return (base.GetHashCode() * 397) ^ (keyId != null ? keyId.GetHashCode() : 0); }
        }
    }
}