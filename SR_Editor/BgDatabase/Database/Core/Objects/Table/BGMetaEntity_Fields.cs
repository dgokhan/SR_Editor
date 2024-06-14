using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    public partial class BGMetaEntity
    {
        private readonly BGIdDictionary<BGField> id2Field = new BGIdDictionary<BGField>();
        private readonly Dictionary<string, BGField> name2Field = new Dictionary<string, BGField>();
        private readonly List<BGField> fields = new List<BGField>();

        internal BGId NewFieldId
        {
            get
            {
                var id = BGId.NewId;
                //probably it's not possible- but just in case
                while (HasField(id)) id = BGId.NewId;
                return id;
            }
        }
        
        /// <summary>
        /// number of fields
        /// </summary>
        public int CountFields
        {
            get
            {
                if (LazyLoader != null) LazyLoad();
                return fields.Count;
            }
        }

        //do not remove!!
        private void RebuildIndexes()
        {
            if (LazyLoader != null) LazyLoad();
            id2Field.Clear();
            name2Field.Clear();
            foreach (var field in fields)
            {
                id2Field[field.Id] = field;
                name2Field[field.Name] = field;
            }
        }

        /// <summary>
        /// Get entity's name field   
        /// </summary>
        public BGFieldEntityName NameField => GetField(BGFieldEntityName.NameFieldName, false) as BGFieldEntityName;
        
        /// <summary>
        /// Get all relations which point to this meta   
        /// </summary>
        public List<BGAbstractRelationI> RelationsInbound
        {
            get
            {
                if (LazyLoader != null) LazyLoad();

                var relationsInbound = new List<BGAbstractRelationI>();

                Repo.ForEachMeta(meta =>
                {
                    foreach (var field in meta.fields)
                    {
                        if (!(field is BGAbstractRelationI relation)) continue;
                        switch (relation)
                        {
                            case BGRelationI relationI:
                            {
                                var relatedMeta = relationI.RelatedMeta;
                                if (relatedMeta == null || !Equals(relatedMeta, this)) continue;
                                relationsInbound.Add(relation);
                                break;
                            }
                            case BGManyTablesRelationI manyTablesRelationI:
                            {
                                var relatedMetas = manyTablesRelationI.RelatedMetas;
                                if (relatedMetas == null || !relatedMetas.Contains(this)) continue;
                                relationsInbound.Add(relation);
                                break;
                            }
                        }
                    }
                });
                return relationsInbound;
            }
        }
        
        //-------iterators
        /// <summary>
        /// Iterate all fields and apply the action to each of them   
        /// </summary>
        public void ForEachField(Action<BGField> action)
        {
            if (LazyLoader != null) LazyLoad();
            //do not convert to foreach 
            var fieldsCount = fields.Count;
            for (var i = 0; i < fieldsCount; i++) action(fields[i]);
        }

        /// <summary>
        /// Iterate all fields, which comply to the filter and apply the action to each of them   
        /// </summary>
        public void ForEachField(Action<BGField> action, Predicate<BGField> filter)
        {
            if (LazyLoader != null) LazyLoad();
            if (filter == null) ForEachField(action);
            else
            {
                //do not convert to foreach 
                var fieldsCount = fields.Count;
                for (var i = 0; i < fieldsCount; i++)
                {
                    var field = fields[i];
                    if (!filter(field)) continue;
                    action(field);
                }
            }
        }

        /// <summary>
        /// Find first field, which comply to the filter   
        /// </summary>
        public BGField FindField(Predicate<BGField> filter)
        {
            if (LazyLoader != null) LazyLoad();
            //do not convert to foreach
            var fieldsCount = fields.Count;
            for (var i = 0; i < fieldsCount; i++)
            {
                var field = fields[i];
                if (filter(field)) return field;
            }

            return null;
        }

        /// <summary>
        /// Fill list of fields, which comply to the filter     
        /// </summary>
        public List<BGField> FindFields(List<BGField> result = null, Predicate<BGField> filter = null)
        {
            if (LazyLoader != null) LazyLoad();
            if (result == null) result = new List<BGField>();
            else result.Clear();

            var count = CountFields;
            if (count == 0) return result;
            if (filter == null) result.AddRange(fields);
            else ForEachField(field => result.Add(field), filter);
            return result;
        }

        /// <summary>
        /// Fill list of fields, which comply to the filter   
        /// </summary>
        [Obsolete("FieldsToList is deprecated, use FindFields instead.")]
        public List<BGField> FieldsToList(List<BGField> result = null, Predicate<BGField> filter = null) => FindFields(result, filter);

        //-------getters
        /// <summary>
        /// Get the field by its id. Second argument shows if exception is thrown in case of field is not found   
        /// </summary>
        public BGField GetField(BGId fieldId, bool errorIfNotFound = true)
        {
            if (LazyLoader != null) LazyLoad();
            if (id2Field.TryGetValue(fieldId, out var field)) return field;
            if (errorIfNotFound) throw new BGException("No field with id ($) at meta ($)", fieldId, Name);
            return null;
        }

        /// <summary>
        /// Get the field by its id. Second argument shows if exception is thrown in case of field is not found   
        /// </summary>
        public BGField<T> GetField<T>(BGId fieldId, bool errorIfNotFound = true)
        {
            var field = (BGField<T>)GetField(fieldId, errorIfNotFound);
            if (field == null && errorIfNotFound) throw new BGException("There is no field with id ($) and value type ($)", fieldId, typeof(T));
            return field;
        }

        /// <summary>
        /// Get the field by its name. Second argument shows if exception is thrown in case of field is not found   
        /// </summary>
        public BGField GetField(string name, bool errorIfNotFound = true)
        {
            if (name == null)
            {
                if (errorIfNotFound) throw new BGException("Field name can not be null");
                return null;
            }
            if (LazyLoader != null) LazyLoad();
            if (name2Field.TryGetValue(name, out var field)) return field;

            if (name.Length == 0) throw new BGException("Field name can not be empty");
            if (errorIfNotFound) throw new BGException("No field with name ($) at meta ($)", name, Name);
            return null;
        }

        /// <summary>
        /// Get the field by its name. Second argument shows if exception is thrown in case of field is not found   
        /// </summary>
        public BGField<T> GetField<T>(string name, bool errorIfNotFound = true)
        {
            var field = (BGField<T>)GetField(name, errorIfNotFound);
            if (field == null && errorIfNotFound) throw new BGException("There is no field with name ($) and value type ($)", name, typeof(T));
            return field;
        }

        /// <summary>
        /// Get the field by its index. Second argument shows if exception is thrown in case of field is not found   
        /// </summary>
        public BGField GetField(int index)
        {
            if (LazyLoader != null) LazyLoad();
            return fields[index];
        }

        /// <summary>
        /// Get the field by its name and cast to specified type. Second argument shows if exception is thrown in case of field is not found   
        /// </summary>
        public T GetFieldAs<T>(string name, bool errorIfNotFound = true) where T : BGField
        {
            var field = (T)GetField(name, errorIfNotFound);
            if (field == null && errorIfNotFound) throw new BGException("There is no field with name ($) and type ($)", name, typeof(T));
            return field;
        }

        /// <summary>
        /// Get the field by its id and cast to specified type. Second argument shows if exception is thrown in case of field is not found   
        /// </summary>
        public T GetFieldAs<T>(BGId id, bool errorIfNotFound = true) where T : BGField
        {
            var field = (T)GetField(id);
            if (field == null && errorIfNotFound) throw new BGException("There is no field with id ($) and type ($)", id, typeof(T));
            return field;
        }

        /// <summary>
        /// Get the field id by its name   
        /// </summary>
        public BGId GetFieldId(string name) => GetField(name).Id;

        /// <summary>
        /// Find field index by its id   
        /// </summary>
        public int GetFieldIndex(BGId id)
        {
            var fieldsCount = CountFields;
            for (var i = 0; i < fieldsCount; i++)
            {
                var field = fields[i];
                if (field.Id != id) continue;
                return i;
            }

            return -1;
        }

        /// <summary>
        /// Swap physical order for 2 fields   
        /// </summary>
        public void SwapFields(int fieldIndex1, int fieldIndex2)
        {
            var count = CountFields;
            if (fieldIndex1 < 0 || fieldIndex2 < 0 || fieldIndex1 >= count || fieldIndex2 >= count) throw new BGException("Invalid fields indexes for swap: $ and $ ", fieldIndex1, fieldIndex2);

            (fields[fieldIndex1], fields[fieldIndex2]) = (fields[fieldIndex2], fields[fieldIndex1]);

            if (events.On) events.FireAnyChange();
        }


        //-------has field
        /// <summary>
        /// Does this meta have a field with specified name?   
        /// </summary>
        public bool HasField(string name)
        {
            if (LazyLoader != null) LazyLoad();
            return name2Field.ContainsKey(name);
        }

        /// <summary>
        /// Does this meta have a field with specified name and type?   
        /// </summary>
        public bool HasField(string name, Type fieldTypeType)
        {
            if (!HasField(name)) return false;
            return GetField(name).GetType() == fieldTypeType;
        }

        /// <summary>
        /// Does this meta have a field with specified id?   
        /// </summary>
        public bool HasField(BGId id)
        {
            if (LazyLoader != null) LazyLoad();
            return id2Field.ContainsKey(id);
        }

        //on field name changed
        internal void FieldNameWasChanged(BGField field, string oldName)
        {
            name2Field.Remove(oldName);
            name2Field.Add(field.Name, field);
            Repo.Events.MetaWasChanged(this);
        }

        //register (add) the field
        internal void Register(BGField field)
        {
            if (LazyLoader != null) LazyLoad();
            //Add
            CheckFieldName(field.Name);

            id2Field.Add(field.Id, field);
            name2Field.Add(field.Name, field);
            fields.Add(field);
            field.OnCreate();
            Repo.Events.MetaWasChanged(this);
        }

        //unregister (remove) field
        internal void Unregister(BGField field)
        {
            if (LazyLoader != null) LazyLoad();
            field.OnDelete();
            id2Field.Remove(field.Id);
            name2Field.Remove(field.Name);
            fields.Remove(field);

            if (CountKeys > 0)
            {
                var keysToRemove = new List<BGKey>();
                ForEachKey(key => keysToRemove.Add(key), key => key.HasField(field));
                foreach (var key in keysToRemove) key.Delete();
            }

            if (CountIndexes > 0)
            {
                var indexesToRemove = new List<BGIndex>();
                ForEachIndex(index => indexesToRemove.Add(index), index => Equals(index.Field, field));
                foreach (var index in indexesToRemove) index.Delete();
            }

            Repo.Events.MetaWasChanged(this);
        }

        /// <summary>
        /// Check if the specified string can be used as new field/key name   
        /// </summary>
        public void CheckFieldName(string name)
        {
            var hasField = HasField(name);
            if (hasField) throw new BGException("Name is not unique: field with name ($) already exists!", name);
            var hasKey = HasKey(name);
            if (hasKey) throw new BGException("Name is not unique: key with name ($) already exists!", name);
            var hasIndex = HasIndex(name);
            if (hasIndex) throw new BGException("Name is not unique: index with name ($) already exists!", name);
        }
    }
}