using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    public partial class BGMetaEntity
    {
        private Dictionary<BGId, BGKey> id2Key;
        private Dictionary<string, BGKey> name2Key;
        private List<BGKey> keys;

        /// <summary>
        /// number of keys
        /// </summary>
        public int CountKeys
        {
            get
            {
                if (LazyLoader != null) LazyLoad();
                return keys?.Count ?? 0;
            }
        }

        /// <summary>
        /// Iterate all keys and call the action for each of them   
        /// </summary>
        public void ForEachKey(Action<BGKey> action)
        {
            if (LazyLoader != null) LazyLoad();
            if (keys == null) return;
            //do not convert to foreach 
            var keysCount = keys.Count;
            for (var i = 0; i < keysCount; i++)
            {
                var key = keys[i];
                action(key);
            }
        }

        /// <summary>
        /// Iterate all keys, which comply to the filter and call the action for each of them   
        /// </summary>
        public void ForEachKey(Action<BGKey> action, Predicate<BGKey> filter)
        {
            if (LazyLoader != null) LazyLoad();
            if (filter == null) ForEachKey(action);
            else
            {
                if (keys == null) return;
                //do not convert to foreach 
                var keysCount = keys.Count;
                for (var i = 0; i < keysCount; i++)
                {
                    var key = keys[i];
                    if (!filter(key)) continue;
                    action(key);
                }
            }
        }


        /// <summary>
        /// Find first key, which comply to the filter   
        /// </summary>
        public BGKey FindKey(Predicate<BGKey> filter)
        {
            if (LazyLoader != null) LazyLoad();
            if (keys == null) return null;
            //do not convert to foreach
            var keysCount = keys.Count;
            for (var i = 0; i < keysCount; i++)
            {
                var key = keys[i];
                if (filter(key)) return key;
            }

            return null;
        }

        /// <summary>
        /// Fill list of keys, which comply to the filter     
        /// </summary>
        public List<BGKey> FindKeys(List<BGKey> result = null, Predicate<BGKey> filter = null)
        {
            if (result == null) result = new List<BGKey>();
            else result.Clear();
            if (LazyLoader != null) LazyLoad();
            if (keys == null) return result;

            var count = CountKeys;
            if (count == 0) return result;
            if (filter == null) result.AddRange(keys);
            else ForEachKey(key => result.Add(key), filter);
            return result;
        }

        //-------getters
        /// <summary>
        /// Get the key by its id. Second argument shows if exception is thrown in case of key is not found   
        /// </summary>
        public BGKey GetKey(BGId keyID, bool errorIfNotFound = true)
        {
            if (LazyLoader != null) LazyLoad();
            if (id2Key == null) return null;

            if (id2Key.TryGetValue(keyID, out var key)) return key;
            if (errorIfNotFound) throw new BGException("No key with id ($) at meta ($)", keyID, Name);
            return null;
        }

        /// <summary>
        /// Get the key by its name. Second argument shows if exception is thrown in case of key is not found   
        /// </summary>
        public BGKey GetKey(string name, bool errorIfNotFound = true)
        {
            if (name == null)
            {
                if (errorIfNotFound) throw new BGException("Key name can not be null");
                return null;
            }
            if (LazyLoader != null) LazyLoad();
            if (name2Key == null) return null;

            if (name2Key.TryGetValue(name, out var key)) return key;

            if (name.Length == 0) throw new BGException("Key name can not be empty");
            if (errorIfNotFound) throw new BGException("No key with name ($) at meta ($)", name, Name);
            return null;
        }

        /// <summary>
        /// Get the key by its index. Second argument shows if exception is thrown in case of key is not found   
        /// </summary>
        public BGKey GetKey(int index)
        {
            if (LazyLoader != null) LazyLoad();
            if (keys == null) return null;

            return keys[index];
        }

        /// <summary>
        /// Get the key id by its name   
        /// </summary>
        public BGId GetKeyId(string name)
        {
            var key = GetKey(name);
            return key?.Id ?? BGId.Empty;
        }

        /// <summary>
        /// Find key index by its id   
        /// </summary>
        public int GetKeyIndex(BGId id)
        {
            if (LazyLoader != null) LazyLoad();
            if (keys == null) return -1;
            var keysCount = keys.Count;
            for (var i = 0; i < keysCount; i++)
            {
                var key = keys[i];
                if (key.Id != id) continue;
                return i;
            }

            return -1;
        }

        /// <summary>
        /// Swap physical order for 2 keys   
        /// </summary>
        public void SwapKeys(int keyIndex1, int keyIndex2)
        {
            if (LazyLoader != null) LazyLoad();
            if (keys == null) return;
            var count = CountKeys;
            if (keyIndex1 < 0 || keyIndex2 < 0 || keyIndex1 >= count || keyIndex2 >= count) throw new BGException("Invalid keys indexes for swap: $ and $ ", keyIndex1, keyIndex2);

            (keys[keyIndex1], keys[keyIndex2]) = (keys[keyIndex2], keys[keyIndex1]);

            if (events.On) events.FireAnyChange();
        }


        //-------has key
        /// <summary>
        /// Does this meta have a key with specified name?   
        /// </summary>
        public bool HasKey(string name)
        {
            if (LazyLoader != null) LazyLoad();
            return name2Key?.ContainsKey(name) ?? false;
        }

        /// <summary>
        /// Does this meta have a key with specified id?   
        /// </summary>
        public bool HasKey(BGId id)
        {
            if (LazyLoader != null) LazyLoad();
            return id2Key?.ContainsKey(id) ?? false;
        }

        //name of the key was changed
        internal void KeyNameWasChanged(BGKey key, string oldName)
        {
            if (LazyLoader != null) LazyLoad();
            if (name2Key == null) return;
            name2Key.Remove(oldName);
            name2Key.Add(key.Name, key);
            Repo.Events.MetaWasChanged(this);
        }

        //register (add) key
        internal void Register(BGKey key)
        {
            if (LazyLoader != null) LazyLoad();
            //Add
            CheckFieldName(key.Name);

            if (keys == null)
            {
                keys = new List<BGKey>();
                id2Key = new Dictionary<BGId, BGKey>();
                name2Key = new Dictionary<string, BGKey>();
            }

            id2Key.Add(key.Id, key);
            name2Key.Add(key.Name, key);
            keys.Add(key);
            key.OnCreate();
            Repo.Events.MetaWasChanged(this);
        }

        //unregister (remove) key
        internal void Unregister(BGKey key)
        {
            if (key == null) throw new Exception("Can not unregister the key cause the key is null!");
            if (key.Meta.Id != Id) throw new Exception("Can not unregister the key cause the key metaId is not matching metaId!");
            if (LazyLoader != null) LazyLoad();
            key.OnDelete();
            if (id2Key != null)
            {
                id2Key.Remove(key.Id);
                name2Key.Remove(key.Name);
                keys.Remove(key);
            }

            Repo.Events.MetaWasChanged(this);
        }
    }
}