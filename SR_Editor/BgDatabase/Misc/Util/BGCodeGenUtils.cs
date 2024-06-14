/*
<copyright file="BGCodeGenUtils.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// This class is used by generated code
    /// </summary>
    public static class BGCodeGenUtils
    {
        //multi-threaded environment in read-only database mode 
        public static bool MultiThreadedEnvironment;
        private static readonly List<BGEntity> reusableList = new List<BGEntity>();

        //=========================================================================
        //                          Public utilities
        //=========================================================================
        /// <summary>
        /// Get meta from default repo
        /// </summary>
        public static T GetMeta<T>(BGId metaId, Action onUnload) where T : BGMetaEntity
        {
            var meta = BGRepo.I.GetMeta<T>(metaId);
            if (meta == null)
            {
                return null;
            }

            // meta.OnUnload += m => onUnload();
            new UnloadEventHelper(meta, onUnload);
            return meta;
        }

        /// <summary>
        /// Get the field from provided meta
        /// </summary>
        public static T GetField<T>(BGMetaEntity meta, BGId fieldId, Action onUnload) where T : BGField
        {
            var field = (T)meta.GetField(fieldId);
            if (field == null)
            {
                return null;
            }

            // field.OnUnload += m => onUnload();
            new UnloadEventHelper(field, onUnload);
            return field;
        }

        /// <summary>
        /// Get the key from provided meta
        /// </summary>
        public static BGKey GetKey(BGMetaEntity meta, BGId keyId, Action onUnload)
        {
            var key = meta.GetKey(keyId);
            if (key == null)
            {
                return null;
            }

            // key.OnUnload += m => onUnload();
            new UnloadEventHelper(key, onUnload);
            return key;
        }

        /// <summary>
        /// Get the index from provided meta
        /// </summary>
        public static BGIndex GetIndex(BGMetaEntity meta, BGId indexId, Action onUnload)
        {
            var index = meta.GetIndex(indexId);
            if (index == null)
            {
                return null;
            }

            // index.OnUnload += m => onUnload();
            new UnloadEventHelper(index, onUnload);
            return index;
        }

        /// <summary>
        /// Get the nested field value from provided meta
        /// </summary>
        public static List<T> GetNested<T>(BGFieldNested fieldNested, int entityIndex) where T : BGEntity
        {
            var val = fieldNested[entityIndex];
            if (val == null || val.Count == 0) return null;
            var result = new List<T>(val.Count);
            for (var i = 0; i < val.Count; i++) result.Add((T)val[i]);
            return result;
        }

        /// <summary>
        /// Get inbound relation  value from provided relation field
        /// </summary>
        public static List<T> GetRelatedInbound<T>(BGAbstractRelationI relation, BGId id) where T : BGEntity
        {
            var value = relation.GetRelatedIn(id, MultiThreadedEnvironment ? null : reusableList);
            if (value.Count == 0) return null;
            var result = new List<T>(value.Count);
            for (var i = 0; i < value.Count; i++) result.Add((T)value[i]);
            ClearReusableEntityList();
            return result;
        }

        /// <summary>
        /// Find entities for provided table
        /// </summary>
        public static void ForEachEntity<T>(BGMetaEntity meta, Action<T> action, Predicate<T> filter = null, Comparison<T> sort = null) where T : BGEntity
        {
            meta.ForEachEntity(
                entity => action((T)entity),
                filter == null ? null : (Predicate<BGEntity>)(entity => filter((T)entity)),
                sort == null ? null : (Comparison<BGEntity>)((e1, e2) => sort((T)e1, (T)e2)));
        }

        /// <summary>
        /// Find entity for provided table
        /// </summary>
        public static T FindEntity<T>(BGMetaEntity meta, Predicate<T> filter = null) where T : BGEntity
        {
            if (filter == null) return meta.CountEntities == 0 ? null : (T) meta.GetEntity(0);
            return (T)meta.FindEntity(entity => filter((T)entity));
        }

        /// <summary>
        /// Find entities for provided table
        /// </summary>
        public static List<T> FindEntities<T>(BGMetaEntity meta, Predicate<T> filter, List<T> result, Comparison<T> sort) where T : BGEntity
        {
            ClearReusableEntityList();
            var value = meta.FindEntities(
                filter == null ? (Predicate<BGEntity>)null : e => filter((T)e),
                MultiThreadedEnvironment ? null : reusableList,
                sort == null ? (Comparison<BGEntity>)null : (e1, e2) => sort((T)e1, (T)e2));
            InitList(ref result, value.Count);

            if (value.Count == 0) return result;

            for (var i = 0; i < value.Count; i++) result.Add((T)value[i]);
            ClearReusableEntityList();

            return result;
        }

        /// <summary>
        /// Add an entity to multi-relational field 
        /// </summary>
        public static void MultipleRelationAdd<T>(BGFieldRelationMultiple relation, int entityIndex, T related) where T : BGEntity
        {
            if (related == null) throw new Exception("Can not add a related entity, cause value is null");
            var val = relation[entityIndex];
            if (val == null) val = new List<BGEntity> { related };
            else val.Add(related);
            //this is required for the event to be fired
            relation[entityIndex] = val;
        }

        /// <summary>
        /// Removes an entity from multi-relational field 
        /// </summary>
        public static void MultipleRelationRemove<T>(BGFieldRelationMultiple relation, int entityIndex, T related) where T : BGEntity
        {
            if (related == null) throw new Exception("Can not remove a related entity, cause value is null");
            var val = relation[entityIndex];
            if (val == null) return;

            val.RemoveAll(e => Equals(e, related));
            //this is required for the event to be fired
            relation[entityIndex] = val.Count == 0 ? null : val;
        }

        /// <summary>
        /// Get a value from multi-relational field 
        /// </summary>
        public static List<T> MultipleRelationGet<T>(BGField<List<BGEntity>> relation, int entityIndex) where T : BGEntity
        {
            var val = relation[entityIndex];
            if (val == null || val.Count == 0) return null;
            var result = new List<T>(val.Count);
            for (var i = 0; i < val.Count; i++) result.Add((T)val[i]);
            return result;
        }

        /// <summary>
        /// Set a value to multi-relational field 
        /// </summary>
        public static void MultipleRelationSet<T>(BGField<List<BGEntity>> relation, int entityIndex, List<T> value) where T : BGEntity
        {
            var val = relation[entityIndex];
            if (value != null && value.Count > 0)
            {
                InitList(ref val, value.Count);
                for (var i = 0; i < value.Count; i++) val.Add(value[i]);
            }
            else val = null;

            relation[entityIndex] = val;
        }

        /// <summary>
        /// Get a value from multi-relational view relation field 
        /// </summary>
        public static List<T> MultipleViewRelationGet<T>(BGFieldViewRelationMultiple relation, int entityIndex) where T : BGAbstractEntityI
        {
            var val = relation[entityIndex];
            if (val == null || val.Count == 0) return null;
            var result = new List<T>(val.Count);
            for (var i = 0; i < val.Count; i++) result.Add((T)(object)val[i]);
            return result;
        }

        /// <summary>
        /// Set a value to multi-relational view relation  field 
        /// </summary>
        public static void MultipleViewRelationSet<T>(BGFieldViewRelationMultiple relation, int entityIndex, List<T> value) where T : BGAbstractEntityI
        {
            var val = relation[entityIndex];
            if (value != null && value.Count > 0)
            {
                InitList(ref val, value.Count);
                for (var i = 0; i < value.Count; i++) val.Add((BGEntity)(object)value[i]);
            }
            else val = null;

            relation[entityIndex] = val;
        }


        /// <summary>
        /// Get a value from enumList field 
        /// </summary>
        public static List<T> EnumListGet<T>(BGFieldEnumList enumListField, int entityIndex) where T : Enum
        {
            var value = enumListField[entityIndex];
            if (value == null || value.Count == 0) return null;
            var result = new List<T>(value.Count);
            for (var i = 0; i < value.Count; i++) result.Add((T)value[i]);
            return result;
        }

        /// <summary>
        /// Set a value to enumList field 
        /// </summary>
        public static void EnumListSet<T>(BGFieldEnumList fieldEnumList, int entityIndex, List<T> value) where T : Enum
        {
            List<Enum> valueToSet = null;
            if (value != null && value.Count > 0)
            {
                valueToSet = new List<Enum>(value.Count);
                for (var i = 0; i < value.Count; i++) valueToSet.Add(value[i]);
            }

            fieldEnumList[entityIndex] = valueToSet;
        }

        //=========================================================================
        //                          Misc private utilities
        //=========================================================================
        //make sure the list is created
        private static void InitList<T>(ref List<T> result, int capacity = 0) where T : BGEntity
        {
            if (result != null) result.Clear();
            else result = new List<T>(capacity);
        }

        //clears reusable list is created
        private static void ClearReusableEntityList()
        {
            if (MultiThreadedEnvironment) return;
            reusableList.Clear();
        }

        private class UnloadEventHelper
        {
            private Action unloadAction;

            public UnloadEventHelper(BGObject target, Action unloadAction)
            {
                this.unloadAction = unloadAction;
                target.OnUnload += Unload;
            }

            private void Unload(BGObject obj)
            {
                obj.OnUnload -= Unload;
                unloadAction?.Invoke();
                unloadAction = null;
            }
        }
    }
}