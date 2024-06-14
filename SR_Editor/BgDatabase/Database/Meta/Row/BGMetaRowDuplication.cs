/*
<copyright file="BGMetaRowDuplication.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Helper class for ROW table duplication support
    /// </summary>
    public class BGMetaRowDuplication
    {
        protected readonly BGMetaEntity meta;
        protected readonly bool copyRows;
        protected string metaName;

        public BGMetaEntity cloneMeta;

        protected readonly List<Tuple<BGField, BGField>> fields = new List<Tuple<BGField, BGField>>();
        protected readonly List<Tuple<BGEntity, BGEntity>> rows = new List<Tuple<BGEntity, BGEntity>>();
        protected readonly List<BGMetaRowDuplicationNested> nested = new List<BGMetaRowDuplicationNested>();

        public BGMetaRowDuplication(BGMetaRow meta, string metaName, bool copyRows)
        {
            if (meta.Repo.HasMeta(metaName)) throw new Exception("Meta with name " + metaName + " already exists");
            if (BGLocalizationUglyHacks.HasLocaleField(meta)) throw new Exception("This meta has localization field(s), which does not support meta duplication");
            this.meta = meta;
            this.metaName = metaName;
            this.copyRows = copyRows;
        }

        internal BGMetaRowDuplication(BGMetaNested meta, bool copyRows)
        {
            this.meta = meta;
            this.copyRows = copyRows;
        }

        /// <summary>
        /// Creates table duplicate
        /// </summary>
        public virtual void CreateCloneMeta()
        {
            cloneMeta = new BGMetaRow(meta.Repo, metaName);
            meta.CopyAttributesTo(cloneMeta);
        }

        /// <summary>
        /// Clone all fields
        /// </summary>
        public virtual void CreateCloneFields()
        {
            meta.ForEachField(field =>
            {
                if (field is BGFieldNested n) nested.Add(new BGMetaRowDuplicationNested(this, n, copyRows));
                else
                {
                    var cloneField = GetCloneField(field);
                    if (cloneField == null)
                    {
                        cloneField = cloneMeta.GetField(field.Name, false);
                        if (cloneField == null) cloneField = field.Clone(cloneMeta, cloneMeta.NewFieldId);
                    }

                    fields.Add(Tuple.Create(field, cloneField));
                }
            });

            //keys
            meta.ForEachKey(key =>
            {
                var keyFields = new List<BGField>();
                key.ForEachField(field =>
                {
                    var cloneKeyField = cloneMeta.GetField(GetToFieldId(field.Id));
                    keyFields.Add(cloneKeyField);
                });
                var clonedKey = new BGKey(key.Name, keyFields.ToArray()) { IsUnique = key.IsUnique, Comment = key.Comment, ControllerType = key.ControllerType };
            });

            //index
            meta.ForEachIndex(index =>
            {
                var cloneKeyField = cloneMeta.GetField(GetToFieldId(index.Field.Id));
                var clonedIndex = new BGIndex(index.Name, cloneKeyField);
            });

            //nested
            foreach (var duplicationNested in nested)
            {
                duplicationNested.CreateCloneMeta();
                duplicationNested.CreateCloneFields();
            }


            //need to sort cloned fields (because of name field)
            var cloneIndex = 0;
            for (var i = 0; i < meta.CountFields; i++)
            {
                var field = meta.GetField(i);
                var clonedField = cloneMeta.GetField(field.Name, false);
                if (clonedField == null) continue;
                if (clonedField.Index != cloneIndex) cloneMeta.SwapFields(clonedField.Index, cloneIndex);
                cloneIndex++;
            }
        }

        protected virtual BGField GetCloneField(BGField field) => null;

        /// <summary>
        /// Clone all rows
        /// </summary>
        public virtual void CreateCloneRows()
        {
            meta.ForEachEntity(entity =>
            {
                rows.Add(Tuple.Create(entity, cloneMeta.NewEntity()));
            });
            foreach (var duplicationNested in nested) duplicationNested.CreateCloneRows();
        }

        protected virtual void CopyValues()
        {
            foreach (var fieldTuple in fields)
            foreach (var row in rows)
                CopyValue(fieldTuple, row);

            foreach (var duplicationNested in nested) duplicationNested.CopyValues();
        }

        protected virtual void CopyValue(Tuple<BGField, BGField> fieldTuple, Tuple<BGEntity, BGEntity> rowTuple) =>
            fieldTuple.Item2.CopyValue(fieldTuple.Item1, rowTuple.Item1.Id, rowTuple.Item1.Index, rowTuple.Item2.Id);

        /// <summary>
        /// Clone the source table
        /// </summary>
        public BGMetaRow Execute()
        {
            CreateCloneMeta();
            CreateCloneFields();

            if (copyRows)
            {
                CreateCloneRows();
                CopyValues();
            }

            return (BGMetaRow)cloneMeta;
        }

        /// <summary>
        /// Get new row ID for provided fromRowId
        /// </summary>
        public BGId GetToRowId(BGId fromRowId) => GetToId(rows, fromRowId);

        /// <summary>
        /// Get new field ID for provided fromFieldId
        /// </summary>
        public BGId GetToFieldId(BGId fromFieldId) => GetToId(fields, fromFieldId);

        private static BGId GetToId<T>(List<Tuple<T, T>> tuples, BGId fromId) where T : BGObjectI
        {
            foreach (var tuple in tuples)
                if (tuple.Item1.Id == fromId)
                    return tuple.Item2.Id;

            return BGId.Empty;
        }

        /*public bool IsNameUsed(string fieldName)
        {
            foreach (var field in fields)
            {
                if (string.Equals(field.Item2.Name, fieldName)) return true;
            }
            foreach (var n in nested)
            {
                if(string.Equals(n.metaName, fieldName)) return true;
            }
            return false;
        }*/
    }

    /// <summary>
    /// Helper class for NESTED table duplication support
    /// </summary>
    public class BGMetaRowDuplicationNested : BGMetaRowDuplication
    {
        private readonly BGMetaRowDuplication parent;
        private readonly BGFieldNested fieldNested;

        private BGFieldNested cloneField;

        public BGMetaRowDuplicationNested(BGMetaRowDuplication parent, BGFieldNested fieldNested, bool copyRows) : base((BGMetaNested)fieldNested.RelatedMeta, copyRows)
        {
            this.parent = parent;
            this.fieldNested = fieldNested;
        }

        private static string GetNestedMetaName(BGMetaRowDuplication parent, BGFieldNested fieldNested)
        {
            return BGUtil.DuplicateMetaName(fieldNested.RelatedMeta, s => !fieldNested.Meta.HasField(s));
        }

        public override void CreateCloneMeta()
        {
            metaName = GetNestedMetaName(parent, fieldNested);
            cloneField = new BGFieldNested(parent.cloneMeta, metaName);
            cloneMeta = cloneField.NestedMeta;

            //change fields names back to original
            cloneField.Name = fieldNested.Name;
            cloneField.NestedMeta.OwnerRelation.Name = fieldNested.NestedMeta.OwnerRelation.Name;

            cloneMeta.System = fieldNested.Meta.System;
            cloneMeta.UniqueName = fieldNested.Meta.UniqueName;
            cloneMeta.Singleton = fieldNested.Meta.Singleton;
            cloneMeta.EmptyName = fieldNested.Meta.EmptyName;
            cloneMeta.Comment = fieldNested.Meta.Comment;
            cloneMeta.ControllerType = fieldNested.Meta.ControllerType;
            cloneMeta.UserDefinedReadonly = fieldNested.Meta.UserDefinedReadonly;
        }

        protected override BGField GetCloneField(BGField field)
        {
            if (fieldNested.NestedMeta.OwnerRelationId == field.Id) return cloneField.NestedMeta.OwnerRelation;
            return null;
        }

        protected override void CopyValue(Tuple<BGField, BGField> fieldTuple, Tuple<BGEntity, BGEntity> rowTuple)
        {
            if (fieldTuple.Item1.Id == fieldNested.NestedMeta.OwnerRelationId)
            {
                //nested relation field
                var fromRelation = (BGFieldRelationSingle)fieldTuple.Item1;
                var toRelation = (BGFieldRelationSingle)fieldTuple.Item2;
                var fromRelatedEntityId = fromRelation.GetStoredValue(rowTuple.Item1.Index);
                if (!fromRelatedEntityId.IsEmpty) toRelation.SetStoredValue(rowTuple.Item2.Index, parent.GetToRowId(fromRelatedEntityId));
            }
            else base.CopyValue(fieldTuple, rowTuple);
        }
    }
}