using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    public class BGLazyLoadMetaLoader
    {
        private readonly BGMetaEntity meta;
        private readonly List<Action> loadActions = new List<Action>();
        private string error;

        public BGLazyLoadMetaLoader(BGMetaEntity meta) => this.meta = meta;

        public void AddAction(Action action) => loadActions.Add(action);

        public void Load()
        {
            if (error != null) throw new Exception(error);
            if (loadActions.Count == 0) throw new Exception("Can not load, cause load is already executed!");
            try
            {
                meta.Repo.Events.WithEventsDisabled(() =>
                {
                    foreach (var action in loadActions) action();
                });
            }
            catch (Exception e)
            {
                error = e.Message ?? "unknown error while lazy loading data: " + e.GetType().FullName;
                
                throw;
            }

            loadActions.Clear();
        }
    }
}