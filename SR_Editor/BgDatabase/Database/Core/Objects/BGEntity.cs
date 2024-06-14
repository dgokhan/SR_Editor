/*
<copyright file="BGEntity.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Generic Database row.
    /// </summary>
    public partial class BGEntity : BGObject, BGAbstractEntityI, BGIndexableI, IComparable<BGEntity>, IEquatable<BGEntity>
    {
        /// <inheritdoc cref="BGAbstractEntityI.Name" />
        public virtual string Name
        {
            get
            {
                var field = Meta.NameField;
                return field?[Index];
            }
            set
            {
                var field = Meta.NameField;
                if (field == null) return;
                field[Index] = value;
            }
        }

        /// <inheritdoc cref="BGAbstractEntityI.Index" />
        public int Index { internal set; get; }

        //========================================================================================
        //              Fields
        //========================================================================================

        /// <inheritdoc/>
        public BGMetaEntity Meta { get; private set; }

        /// <inheritdoc/>
        public BGId MetaId => Meta.Id;

        /// <inheritdoc/>
        public string MetaName => Meta.Name;

        /// <inheritdoc/>
        public string FullName => MetaName + "." + Name;

        /// <inheritdoc/>
        public BGRepo Repo => Meta.Repo;

        //========================================================================================
        //              Constructors
        //========================================================================================
        protected internal BGEntity(BGMetaEntity meta) : this(meta, meta.NewEntityId)
        {
            Meta.OnEntityCreate(this);
        }

        protected internal BGEntity(BGMetaEntity meta, BGId id) : base(id)
        {
            Meta = meta ?? throw new BGException("Meta can not be null");
            Meta.Register(this);
        }


        //========================================================================================
        //              Registration
        //========================================================================================

        //========================================================================================
        //              Methods
        //========================================================================================

        /// <inheritdoc cref="BGAbstractEntityI.Delete" />
        public override void Delete()
        {
            if (IsDeleted) return;

            Exception ex = null;
            BGUtil.Catch(ref ex, () =>
            {
                Meta.FireEntityBeforeDelete(this);
            });

            base.Delete();

            BGUtil.Catch(ref ex, () =>
            {
                Meta.Unregister(this);
            });

            BGUtil.Catch(ref ex, () =>
            {
                Meta.FireEntityDeleted(this);
            });

            BGUtil.Catch(ref ex, Unload, () =>
            {
                // Index = -1;
                Meta = null;
            });

            if (ex != null) throw ex;
        }

        protected internal override void Unload()
        {
            Index = -1;
            base.Unload();
        }

        /// <summary>
        /// Creates a rows clone
        /// </summary>
        public BGEntity Duplicate()
        {
            if (Meta.IsManagingItsOwnEntities) throw new Exception("This meta does not support entity duplication.");
            var entity = Meta.NewEntity(new BGMetaEntity.NewEntityContext(e => e.CopyFieldsValuesFromNoEvents(this)));
            return entity;
        }

        /// <summary>
        /// Copy fields values from the source entity to this entity
        /// </summary>
        public void CopyFieldsValuesFrom(BGEntity source, Predicate<BGField> fieldFilter = null)
        {
            if (source == null) throw new Exception("Can not copy fields values: the source entity is null");
            if (source.Id == Id) throw new Exception("Can not copy fields values: the source entity is the same as target entity");
            if (!Meta.Equals(source.Meta))
                throw new Exception($"Can not copy fields values: the source entity belongs to a different meta! Required meta={MetaName}, source entity meta={source.MetaName}");

            List<BGField> changedFields = null;
            if (Meta.Repo.Events.On)
            {
                changedFields = new List<BGField>();
                Meta.ForEachField(field =>
                {
                    if (!field.AreStoredValuesEqual(field, Index, source.Index)) changedFields.Add(field);
                }, fieldFilter);
            }

            CopyFieldsValuesFromNoEvents(source);

            if (changedFields != null && changedFields.Count > 0)
            {
                foreach (var field in changedFields) field.FireValueChanged(this);
            }
        }

        private void CopyFieldsValuesFromNoEvents(BGEntity source, Predicate<BGField> fieldFilter = null)
        {
            Meta.ForEachField(field =>
            {
                if (field is BGFieldNested nested)
                {
                    var existingEntities = nested[Index];
                    if (existingEntities != null && existingEntities.Count > 0) nested.NestedMeta.DeleteEntities(existingEntities);
                }

                field.DuplicateValue(source.Id, source.Index, Id);
            }, fieldFilter);
        }

        /// <inheritdoc/>
        public int CompareTo(BGEntity other) => other == null ? 0 : Index.CompareTo(other.Index);

        /// <inheritdoc/>
        public bool Equals(BGEntity other) => other != null && Id == other.Id;

        //=================================================================================================================
        //                      FIELDS
        //=================================================================================================================
        /// <summary>
        /// remove field value 
        /// </summary>
        public void ClearFieldValue(BGId fieldId) => Meta.GetField(fieldId).ClearValue(Index);

        /// <inheritdoc/>
        public T Get<T>(BGField field)
        {
            if (field == null) throw new BGException("Provided field is null");
            if (field.MetaId != MetaId) throw new BGException("Field does not belong to entity's Meta. Entity's meta ($), field's meta ($)", Meta.Name, field.Meta.Name);
            if (field.Meta.Repo != Meta.Repo) throw new BGException("Field does not belong to entity's Repo.");

            var fieldWithType = field as BGField<T>;
            if (fieldWithType == null) throw new BGException("Can not get a value! Field ($) has type ($), but provided generic parameter has ($) type", field.FullName, field.ValueType.FullName, typeof(T));

            return fieldWithType[Index];
        }

        /// <inheritdoc/>
        public T Get<T>(BGId fieldId) => Get<T>(Meta.GetField(fieldId));

        /// <inheritdoc/>
        public T Get<T>(string fieldName) => Get<T>(Meta.GetField(fieldName));

        /// <inheritdoc/>
        public void Set<T>(BGField field, T value)
        {
            if (field == null) throw new BGException("Provided field is null");
            if (field.MetaId != MetaId) throw new BGException("Field does not belong to entity's Meta. Entity's meta ($), field's meta ($)", Meta.Name, field.Meta.Name);
            if (field.Meta.Repo != Meta.Repo) throw new BGException("Field does not belong to entity's Repo.");

            var fieldWithType = field as BGField<T>;
            if (fieldWithType == null)
                throw new BGException("Can not set a value: value type mismatch! Field ($) has type ($), but provided value has ($) type", field.FullName, field.ValueType.FullName, typeof(T));
            fieldWithType[Index] = value;
        }

        /// <inheritdoc/>
        public void Set<T>(string fieldName, T value) => Set(Meta.GetField(fieldName), value);

        /// <inheritdoc/>
        public void Set<T>(BGId fieldId, T value) => Set(Meta.GetField(fieldId), value);

        /// <summary>
        /// Get meta as some subclass of generic BGMetaEntity    
        /// </summary>
        public T MetaAs<T>() where T : BGMetaEntity => (T)Meta;

        //========================================================================================
        //              Misc
        //========================================================================================
        public override string ToString() => "Entity [id:" + Id + (IsDeleted ? ", [deleted]" : ", name:" + Name) + (Meta == null ? "" : ", meta=" + Meta.Name) + "]";

        //========================================================================================
        //              Interfaces
        //========================================================================================
        /// <summary>
        /// Interface for entity factory (used by generated classes)    
        /// </summary>
        public interface EntityFactory
        {
            /// <summary>
            /// Row's constructor for new row
            /// </summary>
            BGEntity NewEntity(BGMetaEntity meta);

            /// <summary>
            /// Row's constructor for existing row
            /// </summary>
            BGEntity NewEntity(BGMetaEntity meta, BGId id);
        }
    }
}