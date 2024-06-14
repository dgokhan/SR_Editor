/*
<copyright file="BGRepoDeltaUtils.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Utilities for working with delta files
    /// </summary>
    public static class BGRepoDeltaUtils
    {
        /// <summary>
        /// Iterate over all matching tables
        /// Matching meaning the tables with same IDs 
        /// </summary>
        public static void ForEachMatchingMeta(BGRepo repo1, BGRepo repo2, Action<BGMetaEntity, BGMetaEntity> action)
        {
            repo1.ForEachMeta(meta1 =>
            {
                var meta2 = repo2.GetMeta(meta1.Id);
                if (meta2 == null) return;
                action(meta1, meta2);
            });
        }

        /// <summary>
        /// Iterate over all matching fields
        /// Matching meaning the fields with same IDs 
        /// </summary>
        public static void ForEachMatchingField(BGMetaEntity meta1, BGMetaEntity meta2, Action<BGField, BGField> action)
        {
            meta1.ForEachField(field =>
            {
                var targetField = meta2.GetField(field.Id, false);
                if (targetField == null) return;
                if (targetField.GetType() != field.GetType()) return;
                action(field, targetField);
            });
        }

        /// <summary>
        /// Iterate over all matching rows
        /// Matching meaning the rows with same IDs 
        /// </summary>
        public static void ForEachMatchingEntity(BGMetaEntity meta1, BGMetaEntity meta2, Action<BGEntity, BGEntity> action)
        {
            var count = meta1.CountEntities;
            for (var i = 0; i < count; i++)
            {
                var entity1 = meta1.GetEntity(i);
                var entity2 = meta2.GetEntity(entity1.Id);
                if (entity2 == null) continue;
                action(entity1, entity2);
            }
        }

        /// <summary>
        /// all rows IDs except matching ones (e.g. for all new rows inside meta1)
        /// </summary>
        public static List<BGId> Except(BGMetaEntity meta1, BGMetaEntity meta2)
        {
            var entityIds = ToEntityIds(meta1);
            var targetEntityIds = ToEntityIds(meta2);
            var result = entityIds.Except<BGId>(targetEntityIds).ToList();
            return result;
        }

        //get all ros IDs
        private static List<BGId> ToEntityIds(BGMetaEntity meta)
        {
            var count = meta.CountEntities;
            var result = new List<BGId>(count);
            for (var i = 0; i < count; i++) result.Add(meta.GetEntity(i).Id);
            return result;
        }

        /// <summary>
        /// Create matching meta in repo (meta with the same name and ID)
        /// </summary>
        public static BGMetaEntity CreateMeta(BGRepo repo, BGMetaEntity meta)
        {
            var myMeta = BGMetaEntity.Create(repo, typeof(BGMetaRow).AssemblyQualifiedName, meta.Id, meta.Name,
                (string)null, false, null, false, false, false);
            return myMeta;
        }

        /// <summary>
        /// Create matching field in myMeta (field with the same name and ID and type)
        /// </summary>
        public static BGField CreateField(BGMetaEntity myMeta, BGField field)
        {
            var myField = BGField.Create(myMeta, field.GetType().AssemblyQualifiedName, field.Id, field.Name, field.ConfigToString(), false, null, null, false);
            return myField;
        }

        /// <summary>
        /// Convert ArraySegment to byte[]
        /// </summary>
        public static byte[] ToArray(ArraySegment<byte> arraySegment)
        {
            var array = new byte[arraySegment.Count];
            Array.Copy(arraySegment.Array, arraySegment.Offset, array, 0, arraySegment.Count);
            return array;
        }

        /// <summary>
        /// Was the entity with provided ID added? 
        /// </summary>
        public static bool IsAdded(BGMetaEntity storedMeta, BGId entityId) => !storedMeta.HasEntity(entityId);

        /// <summary>
        /// Do provided entities has any different field value?
        /// </summary>
        public static bool IsChanged(BGEntity e, BGEntity e2)
        {
            if (e == null || e2 == null) return false;
            var m = e.Meta;
            var m2 = e2.Meta;
            var isChanged = false;
            ForEachMatchingField(m, m2, (f, f2) =>
            {
                if (isChanged) return;
                isChanged = !f.AreStoredValuesEqual(f2, e.Index, e2.Index);
            });
            return isChanged;
        }

        /// <summary>
        /// Was the entity with provided ID deleted? 
        /// </summary>
        public static bool IsDeleted(BGMetaEntity meta, BGId entityId) => !meta.HasEntity(entityId);
    }
}