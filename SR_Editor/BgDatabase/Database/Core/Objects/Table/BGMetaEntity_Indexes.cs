using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    public partial class BGMetaEntity
    {
        private Dictionary<BGId, BGIndex> id2Index;
        private Dictionary<string, BGIndex> name2Index;
        private List<BGIndex> indexes;

        /// <summary>
        /// number of indexes
        /// </summary>
        public int CountIndexes
        {
            get
            {
                if (LazyLoader != null) LazyLoad();
                return indexes?.Count ?? 0;
            }
        }

        /// <summary>
        /// Iterate all indexes, which comply to the filter and apply the action to each of them   
        /// </summary>
        public void ForEachIndex(Action<BGIndex> action)
        {
            if (LazyLoader != null) LazyLoad();
            if (indexes == null) return;
            //do not convert to foreach 
            var indexesCount = indexes.Count;
            for (var i = 0; i < indexesCount; i++)
            {
                var index = indexes[i];
                action(index);
            }
        }

        /// <summary>
        /// Iterate all indexes, which comply to the filter and apply the action to each of them   
        /// </summary>
        public void ForEachIndex(Action<BGIndex> action, Predicate<BGIndex> filter)
        {
            if (LazyLoader != null) LazyLoad();
            if (filter == null) ForEachIndex(action);
            else
            {
                if (indexes == null) return;
                //do not convert to foreach 
                var indexesCount = indexes.Count;
                for (var i = 0; i < indexesCount; i++)
                {
                    var index = indexes[i];
                    if (!filter(index)) continue;
                    action(index);
                }
            }
        }


        /// <summary>
        /// Find first index, which comply to the filter   
        /// </summary>
        public BGIndex FindIndex(Predicate<BGIndex> filter)
        {
            if (LazyLoader != null) LazyLoad();
            if (indexes == null) return null;
            //do not convert to foreach
            var indexesCount = indexes.Count;
            for (var i = 0; i < indexesCount; i++)
            {
                var index = indexes[i];
                if (filter(index)) return index;
            }

            return null;
        }

        /// <summary>
        /// Fill list of indexes, which comply to the filter     
        /// </summary>
        public List<BGIndex> FindIndexes(List<BGIndex> result = null, Predicate<BGIndex> filter = null)
        {
            if (LazyLoader != null) LazyLoad();
            if (result == null) result = new List<BGIndex>();
            else result.Clear();

            if (indexes == null) return result;

            var count = CountIndexes;
            if (count == 0) return result;
            if (filter == null) result.AddRange(indexes);
            else ForEachIndex(index => result.Add(index), filter);
            return result;
        }

        //-------getters
        /// <summary>
        /// Get the index by its id. Second argument shows if exception is thrown in case of index is not found   
        /// </summary>
        public BGIndex GetIndex(BGId indexId, bool errorIfNotFound = true)
        {
            if (LazyLoader != null) LazyLoad();
            if (id2Index == null) return null;

            if (id2Index.TryGetValue(indexId, out var index)) return index;
            if (errorIfNotFound) throw new BGException("No index with id ($) at meta ($)", indexId, Name);
            return null;
        }

        /// <summary>
        /// Get the index by its name. Second argument shows if exception is thrown in case of index is not found   
        /// </summary>
        public BGIndex GetIndex(string name, bool errorIfNotFound = true)
        {
            if (name == null)
            {
                if (errorIfNotFound) throw new BGException("Index name can not be null");
                return null;
            }
            if (LazyLoader != null) LazyLoad();
            if (name2Index == null) return null;

            if (name2Index.TryGetValue(name, out var index)) return index;

            if (name.Length == 0) throw new BGException("Index name can not be empty");
            if (errorIfNotFound) throw new BGException("No index with name ($) at meta ($)", name, Name);
            return null;
        }

        /// <summary>
        /// Get the index by its index. Second argument shows if exception is thrown in case of index is not found   
        /// </summary>
        public BGIndex GetIndex(int index)
        {
            if (LazyLoader != null) LazyLoad();
            if (indexes == null) return null;

            return indexes[index];
        }

        /// <summary>
        /// Get the index id by its name   
        /// </summary>
        public BGId GetIndexId(string name)
        {
            var index = GetIndex(name);
            return index?.Id ?? BGId.Empty;
        }

        /// <summary>
        /// Find index index by its id   
        /// </summary>
        public int GetIndexIndex(BGId id)
        {
            if (LazyLoader != null) LazyLoad();
            if (indexes == null) return -1;
            var indexesCount = indexes.Count;
            for (var i = 0; i < indexesCount; i++)
            {
                var index = indexes[i];
                if (index.Id != id) continue;
                return i;
            }

            return -1;
        }

        /// <summary>
        /// Swap physical order for 2 indexes   
        /// </summary>
        public void SwapIndexes(int indexIndex1, int indexIndex2)
        {
            if (LazyLoader != null) LazyLoad();
            if (indexes == null) return;
            var count = CountIndexes;
            if (indexIndex1 < 0 || indexIndex2 < 0 || indexIndex1 >= count || indexIndex2 >= count) throw new BGException("Invalid indexes for swap: $ and $ ", indexIndex1, indexIndex2);

            (indexes[indexIndex1], indexes[indexIndex2]) = (indexes[indexIndex2], indexes[indexIndex1]);

            if (events.On) events.FireAnyChange();
        }


        //-------has index
        /// <summary>
        /// Does this meta have a index with specified name?   
        /// </summary>
        public bool HasIndex(string name)
        {
            if (LazyLoader != null) LazyLoad();
            return name2Index?.ContainsKey(name) ?? false;
        }

        /// <summary>
        /// Does this meta have a index with specified id?   
        /// </summary>
        public bool HasIndex(BGId id)
        {
            if (LazyLoader != null) LazyLoad();
            return id2Index?.ContainsKey(id) ?? false;
        }

        //on index name changed
        internal void IndexNameWasChanged(BGIndex index, string oldName)
        {
            if (name2Index == null) return;
            name2Index.Remove(oldName);
            name2Index.Add(index.Name, index);
            Repo.Events.MetaWasChanged(this);
        }

        //register (add) index
        internal void Register(BGIndex index)
        {
            if (LazyLoader != null) LazyLoad();
            //Add
            CheckFieldName(index.Name);

            if (indexes == null)
            {
                indexes = new List<BGIndex>();
                id2Index = new Dictionary<BGId, BGIndex>();
                name2Index = new Dictionary<string, BGIndex>();
            }

            id2Index.Add(index.Id, index);
            name2Index.Add(index.Name, index);
            indexes.Add(index);
            index.OnCreate();
            Repo.Events.MetaWasChanged(this);
        }

        //unregister (remove) index
        internal void Unregister(BGIndex index)
        {
            if (LazyLoader != null) LazyLoad();
            if (index == null) throw new Exception("Can not unregister the index cause the index is null!");
            if (index.Meta.Id != Id) throw new Exception("Can not unregister the index cause the index metaId is not matching metaId!");

            index.OnDelete();
            if (id2Index != null)
            {
                id2Index.Remove(index.Id);
                name2Index.Remove(index.Name);
                indexes.Remove(index);
            }

            Repo.Events.MetaWasChanged(this);
        }
    }
}