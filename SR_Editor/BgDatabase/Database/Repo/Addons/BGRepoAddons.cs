/*
<copyright file="BGRepoAddons.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Data container for enabled database addons
    /// </summary>
    public partial class BGRepoAddons
    {
        private readonly Dictionary<Type, BGAddon> type2Addon = new Dictionary<Type, BGAddon>();
        private readonly BGRepo repo;

        public BGRepoAddons(BGRepo repo) => this.repo = repo;

        /// <summary>
        /// the number of enabled addons
        /// </summary>
        public int Count => type2Addon.Count;

        /// <summary>
        /// get all enabled addons
        /// </summary>
        public List<BGAddon> Addons => new List<BGAddon>(type2Addon.Values);

        /// <summary>
        /// Get enabled addon by its type
        /// </summary>
        public T Get<T>() where T : BGAddon => (T)BGUtil.Get(type2Addon, typeof(T));

        /// <summary>
        /// Get enabled addon by its type
        /// </summary>
        public BGAddon Get(Type addonType) => BGUtil.Get(type2Addon, addonType);

        /// <summary>
        /// Get enabled addon by its type full name
        /// </summary>
        public BGAddon Get(string type)
        {
            foreach (var pair in type2Addon)
                if (type.Equals(pair.Key.FullName))
                    return pair.Value;

            return null;
        }

        /// <summary>
        /// is provided addon T enabled?
        /// </summary>
        public bool Has<T>() where T : BGAddon => type2Addon.ContainsKey(typeof(T));

        /// <summary>
        /// is provided addon T enabled?
        /// </summary>
        public bool Has(Type type) => type2Addon.ContainsKey(type);

        /// <summary>
        /// is provided addon with type=typeFullName enabled?
        /// </summary>
        public bool Has(string typeFullName)
        {
            foreach (var addon in Addons)
                if (string.Equals(addon.GetType().FullName, typeFullName))
                    return true;
            return false;
        }

        /// <summary>
        /// add provided addon to enabled addons
        /// </summary>
        public void Add(BGAddon addon)
        {
            addon.Init(repo);
            type2Addon[addon.GetType()] = addon;
            repo.Events.FireAddonChange();
        }

        /// <summary>
        /// disable  addon with specified type 
        /// </summary>
        public void Remove(Type type)
        {
            if (!type2Addon.TryGetValue(type, out var addon)) return;
            addon.OnDelete(repo);
            type2Addon.Remove(type);
            repo.Events.FireAddonChange();
        }

        /// <summary>
        /// disable provided addon 
        /// </summary>
        public void Remove<T>() where T : BGAddon => Remove(typeof(T));

        /// <summary>
        /// Clear all addons
        /// </summary>
        public void Clear()
        {
            if (type2Addon.Count == 0) return;
            type2Addon.Clear();
            repo.Events.FireAddonChange();
        }

        /// <summary>
        /// add all addons from provided addons container
        /// </summary>
        public void AddFrom(BGRepoAddons addons)
        {
            //move all addons
            addons.ForEachAddon(addon => addon.CloneAndAddTo(repo));
            repo.Events.FireAddonChange();
        }

        /// <summary>
        /// iterate over all enabled addons
        /// </summary>
        public void ForEachAddon(Action<BGAddon> action)
        {
            foreach (var pair in type2Addon) action(pair.Value);
        }
    }
}