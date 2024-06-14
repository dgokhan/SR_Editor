/*
<copyright file="BGKey.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Database key has 1-n fields for accessing row(s) quickly with Log(1) efficiency.
    /// Use GetEntityByKey/GetEntitiesByKey method for searching the rows
    /// </summary>
    public class BGKey : BGMetaObject
    {
        public const int MaxFieldsCount = 10;
        private readonly List<BGField> fields = new List<BGField>();

        /// <summary>
        /// the table, this key belongs to
        /// </summary>
        public BGMetaEntity Meta => fields[0].Meta;

        /// <summary>
        /// Number of fields in the key
        /// </summary>
        public int CountFields => fields.Count;

        /*
        public List<BGField> Fields
        {
            get { return fields; }
        }
        */


        /// <inheritdoc/>
        public override int Index => Meta.GetKeyIndex(Id);

        private bool isUnique;

        /// <summary>
        /// Is key unique (1 row maximum)
        /// </summary>
        public bool IsUnique
        {
            get => isUnique;
            set => isUnique = value;
        }

        /// <summary>
        /// Key name. Must be unique within meta
        /// </summary>
        public override string Name
        {
            set
            {
                if (string.Equals(Name, value)) return;
                Meta.CheckFieldName(value);

                var oldName = Name;
                //value will be checked in base method
                base.Name = value;

                Meta.KeyNameWasChanged(this, oldName);
            }
        }

        /// <summary>
        /// Full name (with meta name)
        /// </summary>
        public string FullName => Meta.Name + "." + Name;

        /// <summary>
        /// Get meta ID
        /// </summary>
        public BGId MetaId => Meta.Id;

        public BGRepo Repo => Meta.Repo;

        //=================================================================================================================
        //                      Constructors
        //=================================================================================================================
        //for new
        public BGKey(string name, BGField[] fields) : this(BGId.NewId, name, fields)
        {
        }

        //for existing
        private BGKey(BGId id, string name, BGField[] fields) : base(id, name)
        {
            if (fields == null || fields.Length == 0) throw new BGException("Fields can not be empty");
            if (fields.Length > MaxFieldsCount) throw new BGException("Fields count for a key can not exceed max=$", MaxFieldsCount);
            var meta = fields[0].Meta;
            var processedFields = new HashSet<BGId>();
            for (var i = 0; i < fields.Length; i++)
            {
                var field = fields[i];
                if (field == null) throw new BGException("Field can not be null! index=$", i);
                if (!field.CanBeUsedAsKey) throw new BGException("Field $ can not be used as a key!", field.Name);
                if (meta.Id != field.MetaId) throw new BGException("Fields with different metas was submitted, expected $, found $", meta.Name, field.MetaName);
                if (processedFields.Contains(field.Id)) throw new BGException("Duplicate field was submitted: $", field.MetaName);
                processedFields.Add(field.Id);
            }

            this.fields.AddRange(fields);
            meta.Register(this);
        }

        //=================================================================================================================
        //                      Config
        //=================================================================================================================
        /// <inheritdoc/>
        public override string ConfigToString()
        {
            //WTF why this is added  to config? it's already saved in fields!
            return JsonUtility.ToJson(new JsonConfig
            {
                IsUnique = isUnique,
            });
        }

        /// <inheritdoc/>
        public override void ConfigFromString(string config)
        {
            isUnique = JsonUtility.FromJson<JsonConfig>(config).IsUnique;
        }

        /// <inheritdoc/>
        public override byte[] ConfigToBytes()
        {
            var writer = new BGBinaryWriter(5);
            //version
            writer.AddInt(1);
            //toId
            writer.AddBool(isUnique);

            return writer.ToArray();
        }

        /// <inheritdoc/>
        public override void ConfigFromBytes(ArraySegment<byte> config)
        {
            var reader = new BGBinaryReader(config);
            var version = reader.ReadInt();
            switch (version)
            {
                case 1:
                {
                    isUnique = reader.ReadBool();
                    break;
                }
                default:
                {
                    throw new BGException("Unknown version: $", version);
                }
            }
        }

        [Serializable]
        private class JsonConfig
        {
            public bool IsUnique;
        }

        //=================================================================================================================
        //                      Callbacks
        //=================================================================================================================

        public void OnCreate()
        {
        }

        public void OnDelete()
        {
        }

        //=================================================================================================================
        //                      Fields
        //=================================================================================================================
        /// <summary>
        /// Find key's field using predicate as a filter
        /// </summary>
        public List<BGField> FindFields(List<BGField> result = null, Predicate<BGField> filter = null)
        {
            if (result == null) result = new List<BGField>();
            if (filter == null) result.AddRange(fields);
            else
                foreach (var field in fields)
                {
                    if (!filter(field)) continue;
                    result.Add(field);
                }

            return result;
        }

        /// <summary>
        /// Iterate over key's fields
        /// </summary>
        public void ForEachField(Action<BGField> action, Predicate<BGField> filter = null)
        {
            //do not convert to foreach 
            var fieldsCount = fields.Count;
            for (var i = 0; i < fieldsCount; i++)
            {
                var field = fields[i];
                if (filter != null && !filter(field)) continue;
                action(field);
            }
        }

        /// <summary>
        /// Get key's field by its index
        /// </summary>
        public BGField GetField(int index) => fields[index];

        /// <summary>
        /// Get key's field index
        /// </summary>
        public int GetFieldIndex(BGField field)
        {
            for (var i = 0; i < fields.Count; i++)
            {
                var f = fields[i];
                if (f.Id == field.Id) return i;
            }

            return -1;
        }

        /// <summary>
        /// Does the key have provided field?
        /// </summary>
        public bool HasField(BGField field) => GetFieldIndex(field) != -1;

        /// <summary>
        /// add a field to the key's fields
        /// </summary>
        public void AddField(int position, BGField field)
        {
            if (Meta.Id != field.MetaId) throw new BGException("Can not add field $ to key $: wrong meta, expected: $, found $", field.Name, Name, Meta.Name, field.MetaName);
            if (fields.Count >= MaxFieldsCount) throw new BGException("Fields count for a key can not exceed max=$", MaxFieldsCount);
            fields.Insert(position, field);
            Meta.Repo.Events.MetaWasChanged(Meta);
        }

        /// <summary>
        /// remove a field from the key's fields
        /// </summary>
        public void RemoveField(BGField field)
        {
            var index = GetFieldIndex(field);
            if (index == -1) throw new BGException("Unable to remove a field: field $ is not contained in key $", field.Name, Name);
            if (fields.Count == 1) throw new BGException("Unable to remove the last field $ from the key $", field.Name, Name);
            fields.Remove(field);
            Meta.Repo.Events.MetaWasChanged(Meta);
        }

        /// <summary>
        /// set fields
        /// </summary>
        public void SetFields(List<BGField> fields)
        {
            this.fields.Clear();
            this.fields.AddRange(fields);
        }

        //========================================================================================
        //              Binary
        //========================================================================================
        //reconstruct the key from binary stream
        internal static BGKey FromBinary(BGBinaryReader binder, BGMetaEntity meta)
        {
            var version = binder.ReadInt();
            switch (version)
            {
                case 1:
                case 2:
                {
                    var keyId = binder.ReadId();
                    var keyName = binder.ReadString();
                    var isUnique = binder.ReadBool();
                    var fieldIds = new List<BGId>();
                    binder.ReadArray(() =>
                    {
                        fieldIds.Add(binder.ReadId());
                    });

                    if (fieldIds.Count == 0) return null;
                    var fields = new BGField[fieldIds.Count];
                    for (var i = 0; i < fieldIds.Count; i++)
                    {
                        var fieldId = fieldIds[i];
                        var field = meta.GetField(fieldId, false);
                        if (field == null) return null;
                        fields[i] = field;
                    }

                    var key = Create(keyId, keyName, isUnique, fields);
                    
                    //v2
                    if (version >= 2)
                    {
                        key.Comment = binder.ReadString();
                        key.ControllerType = binder.ReadString();
                    }
                    
                    return key;
                }
                default:
                {
                    throw new BGException("Can not read key from binary array: unsupported version $", version);
                }
            }
        }

        //convert the key to binary array data
        internal static void ToBinary(BGBinaryWriter builder, BGKey key)
        {
            //version
            builder.AddInt(2);
            
            builder.AddId(key.Id);
            builder.AddString(key.Name);
            builder.AddBool(key.isUnique);
            builder.AddArray(() =>
            {
                key.ForEachField(field => builder.AddId(field.Id));
            }, key.CountFields);
            
            //v2
            builder.AddString(key.Comment);
            builder.AddString(key.ControllerType);
        }


        //=================================================================================================================
        //                      Entities
        //=================================================================================================================
        private BGKeyStorage store;
        private BGKeyStorage[] stores;

        /// <summary>
        /// Get the first entity with provided key values
        /// </summary>
        public BGEntity GetEntityByKey(params object[] keys)
        {
            CheckKeys(keys);

            if (keys.Length == fields.Count)
            {
                //full key
                Build();
                return store.GetEntity(keys);
            }

            var keysCountMinusOne = keys.Length - 1;
            EnsureStore(keysCountMinusOne);
            return stores[keysCountMinusOne].GetEntity(keys);
        }

        /// <summary>
        /// Get the first entity with provided key value
        /// </summary>
        public BGEntity GetEntityByKey<T0>(T0 t0)
        {
            const int keysCount = 1;
            const int keysCountMinusOne = 0;

            CheckKeysCount(keysCount);
            // CheckKey(0, t0);
            if (keysCount == fields.Count)
            {
                //full key
                Build();
                return store.GetEntity(t0);
            }

            EnsureStore(keysCountMinusOne);
            return stores[keysCountMinusOne].GetEntity(t0);
        }

        /// <summary>
        /// Get the first entity with provided key values
        /// </summary>
        public BGEntity GetEntityByKey<T0, T1>(T0 t0, T1 t1)
        {
            const int keysCount = 2;
            const int keysCountMinusOne = 1;

            CheckKeysCount(keysCount);
            // CheckKey(0, t0);
            // CheckKey(1, t1);

            if (keysCount == fields.Count)
            {
                //full key
                Build();
                return store.GetEntity(t0, t1);
            }

            EnsureStore(keysCountMinusOne);
            return stores[keysCountMinusOne].GetEntity(t0, t1);
        }

        /// <summary>
        /// Get the first entity with provided key values
        /// </summary>
        public BGEntity GetEntityByKey<T0, T1, T2>(T0 t0, T1 t1, T2 t2)
        {
            const int keysCount = 3;
            const int keysCountMinusOne = 2;

            CheckKeysCount(keysCount);
            // CheckKey(0, t0);
            // CheckKey(1, t1);
            // CheckKey(2, t2);

            if (keysCount == fields.Count)
            {
                //full key
                Build();
                return store.GetEntity(t0, t1, t2);
            }

            EnsureStore(keysCountMinusOne);
            return stores[keysCountMinusOne].GetEntity(t0, t1, t2);
        }

        /// <summary>
        /// Get the first entity with provided key values
        /// </summary>
        public BGEntity GetEntityByKey<T0, T1, T2, T3>(T0 t0, T1 t1, T2 t2, T3 t3)
        {
            const int keysCount = 4;
            const int keysCountMinusOne = 3;

            CheckKeysCount(keysCount);
            // CheckKey(0, t0);
            // CheckKey(1, t1);
            // CheckKey(2, t2);
            // CheckKey(3, t3);

            if (keysCount == fields.Count)
            {
                //full key
                Build();
                return store.GetEntity(t0, t1, t2, t3);
            }

            EnsureStore(keysCountMinusOne);
            return stores[keysCountMinusOne].GetEntity(t0, t1, t2, t3);
        }


        /// <summary>
        /// Get all entities with provided key values
        /// </summary>
        public List<BGEntity> GetEntitiesByKey(params object[] keys) => GetEntitiesByKey<BGEntity>(null, keys);

        /// <summary>
        /// Get all entities with provided key values
        /// </summary>
        public List<T> GetEntitiesByKey<T>(List<T> result, params object[] keys) where T : BGEntity
        {
            CheckKeys(keys);

            if (keys.Length == fields.Count)
            {
                //full key
                Build();
                return store.GetEntities(result, keys);
            }

            EnsureStore(keys.Length - 1);
            return stores[keys.Length - 1].GetEntities(result, keys);
        }

        /// <summary>
        /// Get all entities with provided key value
        /// </summary>
        public List<T> GetEntitiesByKey<T, T0>(List<T> result, T0 t0) where T : BGEntity
        {
            const int keysCount = 1;
            const int keysCountMinusOne = 0;

            CheckKeysCount(keysCount);
            // CheckKey<T0>(0, t0);
            if (keysCount == fields.Count)
            {
                //full key
                Build();
                return store.GetEntities(result, t0);
            }

            EnsureStore(keysCountMinusOne);
            return stores[keysCountMinusOne].GetEntities(result, t0);
        }

        /// <summary>
        /// Get all entities with provided key values
        /// </summary>
        public List<T> GetEntitiesByKey<T, T0, T1>(List<T> result, T0 t0, T1 t1) where T : BGEntity
        {
            const int keysCount = 2;
            const int keysCountMinusOne = 1;

            CheckKeysCount(keysCount);
            // CheckKey(0, t0);
            // CheckKey(1, t1);
            if (keysCount == fields.Count)
            {
                //full key
                Build();
                return store.GetEntities(result, t0, t1);
            }

            EnsureStore(keysCountMinusOne);
            return stores[keysCountMinusOne].GetEntities(result, t0, t1);
        }

        /// <summary>
        /// Get all entities with provided key values
        /// </summary>
        public List<T> GetEntitiesByKey<T, T0, T1, T2>(List<T> result, T0 t0, T1 t1, T2 t2) where T : BGEntity
        {
            const int keysCount = 3;
            const int keysCountMinusOne = 2;

            CheckKeysCount(keysCount);
            // CheckKey(0, t0);
            // CheckKey(1, t1);
            // CheckKey(2, t2);
            if (keysCount == fields.Count)
            {
                //full key
                Build();
                return store.GetEntities(result, t0, t1, t2);
            }

            EnsureStore(keysCountMinusOne);
            return stores[keysCountMinusOne].GetEntities(result, t0, t1, t2);
        }

        /// <summary>
        /// Get all entities with provided key values
        /// </summary>
        public List<T> GetEntitiesByKey<T, T0, T1, T2, T3>(List<T> result, T0 t0, T1 t1, T2 t2, T3 t3) where T : BGEntity
        {
            const int keysCount = 4;
            const int keysCountMinusOne = 3;

            CheckKeysCount(keysCount);
            // CheckKey(0, t0);
            // CheckKey(1, t1);
            // CheckKey(2, t2);
            // CheckKey(3, t3);
            if (keysCount == fields.Count)
            {
                //full key
                Build();
                return store.GetEntities(result, t0, t1, t2, t3);
            }

            EnsureStore(keysCountMinusOne);
            return stores[keysCountMinusOne].GetEntities(result, t0, t1, t2, t3);
        }

        //=================================================================================================================
        //                      Methods
        //=================================================================================================================
        public void MarkDirty()
        {
            store?.MarkDirty();
            if (stores == null) return;
            foreach (var storage in stores) storage?.MarkDirty();
        }

        /// <summary>
        /// Build default storage for full key
        /// </summary>
        public void Build()
        {
            if (store != null) return;
            store = new BGKeyStorage(this, fields.ToArray());
        }

        /// <summary>
        /// Build default storage for full key and all additional stores for not-full keys 
        /// </summary>
        public void BuildAll()
        {
            Build();
            for (var i = 0; i < fields.Count - 1; i++) EnsureStore(i);
        }

        //ensure the storage is initialized for the key with [index] values 
        private void EnsureStore(int index)
        {
            if (stores == null) stores = new BGKeyStorage[index + 1];
            else if (stores.Length <= index)
            {
                var oldArray = stores;
                stores = new BGKeyStorage[index + 1];
                Array.Copy(oldArray, stores, oldArray.Length);
            }

            if (stores[index] == null)
            {
                var storeFields = new BGField[index + 1];
                for (var i = 0; i < storeFields.Length; i++) storeFields[i] = fields[i];
                stores[index] = new BGKeyStorage(this, storeFields);
            }
        }

        //check if provided key's values comply to fields types
        private void CheckKeys(object[] keys)
        {
            CheckKeysCount(keys?.Length ?? 0);

            for (var i = 0; i < keys.Length; i++) CheckKey(i, keys[i]);
        }

        //check the number of provided key values
        private void CheckKeysCount(int keysCount)
        {
            if (keysCount == 0) throw new BGException("Keys are null or empty!");
            if (keysCount > fields.Count) throw new BGException("Keys count more than fields count! $ > $", keysCount, fields.Count);
        }

        //check one single key value
        private void CheckKey<T>(int index, T key)
        {
            var field = fields[index];
            if (key == null)
            {
                if (field.ConstantSize > 0) throw new BGException("Key $ is null, but field $ can not have null values!", index, field.Name);
            }
            else
                switch (field)
                {
                    case BGFieldEnumI enumI:
                    {
                        var targetType = enumI.EnumType;
                        if (key.GetType() != targetType)
                            throw new BGException("Key $ has incompatible type: required [$], actual [$] !",
                                index, GetValueType(targetType), GetValueType(key.GetType()));
                        break;
                    }
                    case BGFieldRelationSingle _ when !(key is BGEntity):
                        throw new BGException("Key $ has incompatible type: required [$], actual [$] !",
                            index, typeof(BGEntity).FullName, GetValueType(key.GetType()));
                    case BGFieldRelationSingle single:
                    {
                        var entity = key as BGEntity;
                        if (entity.MetaId != single.RelatedMeta.Id)
                            throw new BGException("Key $ has incompatible type: required entity of [$] meta, actual entity of [$] meta !",
                                index, single.RelatedMeta.Name, entity.Meta.Name);
                        break;
                    }
                    default:
                    {
                        var targetType = field.ValueType;
                        if (key.GetType() != targetType)
                            throw new BGException("Key $ has incompatible type: required [$], actual [$] !",
                                index, GetValueType(targetType), GetValueType(key.GetType()));
                        break;
                    }
                }
        }

        /// <inheritdoc/>
        public override void Delete()
        {
            if (IsDeleted) return;
            base.Delete();

            Meta.Unregister(this);
            Unload();
            // fields.Clear();
        }

        //=================================================================================================================
        //                      Factory
        //=================================================================================================================
        /// <summary>
        /// Create a key 
        /// </summary>
        public static BGKey Create(BGId id, string name, bool unique, BGField[] fields)
        {
            var key = new BGKey(id, name, fields) { isUnique = unique };
            return key;
        }

        //=================================================================================================================
        //                      Utility
        //=================================================================================================================
        public override string ToString() => "Key [id:" + Id + ", name:" + Name + ", fields count:" + CountFields + "]";

        //copy from BGCodeGeneratorA
        private static string GetValueType(Type valueType)
        {
            string type;
            if (valueType.IsGenericType)
            {
                type = valueType.GetGenericTypeDefinition().FullName;
                var iBacktick = type.IndexOf('`');
                if (iBacktick > 0) type = type.Remove(iBacktick);

                type += "<";
                var args = valueType.GetGenericArguments();
                for (var i = 0; i < args.Length; i++)
                {
                    var arg = args[i];
                    if (i != 0) type += ',';
                    type += arg.FullName;
                }

                type += ">";
            }
            else type = valueType.FullName;

            return type;
        }

        /// <summary>
        /// Clone this key to provided table
        /// </summary>
        public BGKey CloneTo(BGMetaEntity meta)
        {
            var cloneFields = new BGField[CountFields];
            for (var i = 0; i < cloneFields.Length; i++)
            {
                var field = meta.GetField(fields[i].Id, false);
                if (field == null) return null;
                cloneFields[i] = field;
            }

            var clone = new BGKey(Id, Name, cloneFields) { isUnique = isUnique, Comment = Comment, ControllerType = ControllerType };
            return clone;
        }

        /// <summary>
        /// compare keys parameters
        /// </summary>
        public bool DeepEqual(BGKey t2)
        {
            if (!string.Equals(ConfigToString(), t2.ConfigToString())) return false;
            if (!string.Equals(Name, t2.Name)) return false;
            if (!string.Equals(Comment, t2.Comment)) return false;
            if (!string.Equals(ControllerType, t2.ControllerType)) return false;
            if (isUnique != t2.isUnique) return false;
            if (CountFields != t2.CountFields) return false;

            for (var i = 0; i < CountFields; i++)
            {
                if (!Equals(fields[i], t2.fields[i])) return false;
            }

            return true;
        }
    }
}