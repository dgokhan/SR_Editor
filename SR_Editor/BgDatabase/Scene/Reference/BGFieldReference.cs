/*
<copyright file="BGFieldReference.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Reference to the database field with custom Editor GUI
    /// </summary>
    [Serializable]
    public class BGFieldReference : BGMetaReference
    {
        //referenced field ID
        [SerializeField] protected string fieldId;

        private BGField field;

        public string FieldId => fieldId;

        /// <summary>
        /// referenced field
        /// </summary>
        public BGField Field
        {
            get => GetField();
            set => SetField(value);
        }
        
        public BGFieldReference()
        {
        }

        public BGFieldReference(BGField field) => SetField(field);


        /// <summary>
        /// Retrieve the field using table and field IDs
        /// </summary>
        public BGField GetField()
        {
            if (field?.Meta != null && !field.Meta.IsDeleted && field.Id == BGId.Parse(fieldId)) return field;

            var meta = Meta;
            if (meta == null) return null;
            field = meta.GetField(BGId.Parse(fieldId), false);
            return field;
        }

        /// <summary>
        /// Reference a new field 
        /// </summary>
        public void SetField(BGField field)
        {
            if (field == null) Reset();
            else
            {
                var metaConstraintId = MetaIdConstraint;
                if (!metaConstraintId.IsEmpty && field.MetaId != metaConstraintId)
                    throw new Exception("Can not assign a field, cause meta is wrong. IDs mismatch "
                                        + field.MetaId + "!=" + metaConstraintId);
                metaId = field.MetaId.ToString();
                fieldId = field.Id.ToString();
                this.field = field;
            }
        }

        /// <summary>
        /// Reset internal state
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            fieldId = null;
            field = null;
        }

        protected bool Equals(BGFieldReference other) => base.Equals(other) && fieldId == other.fieldId;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BGFieldReference)obj);
        }

        public override int GetHashCode()
        {
            unchecked { return (base.GetHashCode() * 397) ^ (fieldId != null ? fieldId.GetHashCode() : 0); }
        }
    }
}