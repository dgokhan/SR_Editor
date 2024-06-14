/*
<copyright file="BGRepo.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Data container for database.
    /// </summary>
    public partial class BGRepo
    {
        //================================================================================================
        //                                              Static
        //================================================================================================
        //Beta release version format -> "{Version}Beta MMdd", for example "1.7.9Beta 0818"
        //PLACE YOUR "TO DO" LIST HERE
        public const string Version = "1.8.8";
        public const string VersionBuild = "2023.12.17";
        
        //default repo
        private static readonly BGRepo instance = new BGRepo();

        /// <summary>
        /// This is a default database
        /// </summary>
        public static BGRepo I
        {
            get
            {
                if (!DefaultRepoLoaded) Load();
                return instance;
            }
        }

        /// <summary>
        /// This is a multi-threading service for default database
        /// </summary>
        public static BGMTService M
        {
            get
            {
                if (!DefaultRepoLoaded) Load();
                return instance.MTService;
            }
        }

        // all supported database loaders
        private static readonly BGLoaderForRepo[] Loaders = { new BGLoaderForRepoCustom(), new BGLoaderForRepoStreamingAssets(), new BGLoaderForRepoResources() };

        //================================================================================================
        //                                              Loading of default repo
        //================================================================================================
        //default reader (do not make readonly)
        public static RepoReaderI Reader = new BGRepoBinary();

        public static bool DefaultRepoLoaded;
        public static string DefaultRepoErrorOnLoad { get; set; }
        public static int DefaultRepoAssetId { get; set; }
        public static string DefaultRepoAssetPath { get; set; }

        public static bool Ok => DefaultRepoErrorOnLoad == null;

        public static bool IsFallbackRepo => DefaultRepoAssetPath != null && DefaultRepoAssetPath.Contains("default");

        //default database content
        private static BGRepoCustomLoaderModel defaultRepoCustomLoaderModel;
        private static BGLoaderForRepo defaultRepoLoader;
        private static bool isLoading;

        public static bool IsLoading => isLoading;

        //================================================================================================
        //                                              Events
        //================================================================================================
        public static event Action OnBeforeLoad;
        public static event Action<bool> OnLoad;

        private readonly BGRepoEvents repoEvents;

        /// <summary>
        /// events. turned off by default 
        /// </summary>
        public BGRepoEvents Events => repoEvents;

        /// <summary>
        /// events for default repo
        /// </summary>
        public static BGRepoEvents DefaultEvents => instance.Events;

        public static bool DefaultRepoResetEventsOnLoad;

        private static BGEventsHolder defaultRepoEventsHolder;

        //================================================================================================
        //                                              Addons
        //================================================================================================
        private readonly BGRepoAddons repoAddons;


        /// <summary>
        /// addons
        /// </summary>
        public BGRepoAddons Addons => repoAddons;

        //================================================================================================
        //                                              Binary version
        //================================================================================================
        /// <summary>
        /// If repo was loaded from binary stream, which version was used
        /// </summary>
        public int BinaryFormatVersion { get; set; }

        //================================================================================================
        //                                              Loader
        //================================================================================================
        /// <summary>
        /// Loaded which was used while database loading
        /// </summary>
        public BGLoaderForRepo RepoLoader { get; set; }

        /// <summary>
        /// Repository asset path (for Editor only)
        /// </summary>
        public string RepoAssetPath { get; set; }

        //================================================================================================
        //                                              MultiThreading
        //================================================================================================
        private BGMTService mtService;

        /// <summary>
        /// Multi-threading service providing multi-threading environment, based on multi-threading add-on settings.
        /// Return null if no add-on present
        /// </summary>
        public BGMTService MTService
        {
            get
            {
                if (mtService == null)
                {
                    BGMainThreadRunner.EnsureMainThread("Multi-threading service should be created on main thread");
                    var mtAddon = Addons.Get<BGAddonMT>();
                    if (mtAddon != null) mtService = mtAddon.CreateService();
                }

                return mtService;
            }
        }

        //================================================================================================
        //                                              Constructors
        //================================================================================================
        /// <summary>
        /// Creates new empty repository
        /// </summary>
        public BGRepo()
        {
            repoAddons = new BGRepoAddons(this);
            repoEvents = new BGRepoEvents(this);
        }

        /// <summary>
        /// Creates new empty repository and load data from content byte array
        /// </summary>
        public BGRepo(byte[] content) : this() => Load(content);

        //---------------------------------------- clone data from other repo
        /// <summary>
        /// Creates new empty repository and transfer data from other repository 
        /// </summary>
        public BGRepo(BGRepo other, bool copyValues = false) : this(other, null, null, copyValues, null)
        {
        }

        /// <summary>
        /// Creates new empty repository and transfer data from other repository using various filters
        /// </summary>
        public BGRepo(BGRepo other, Predicate<BGId> metaFilter, Predicate<BGField> fieldFilter, bool copyValues) : this(other, metaFilter, fieldFilter, copyValues, null)
        {
        }

        /// <summary>
        /// Creates new empty repository and transfer data from other repository using various filters
        /// </summary>
        public BGRepo(BGRepo other, Predicate<BGId> metaFilter, Predicate<BGField> fieldFilter, bool copyValues, Predicate<BGEntity> entityFilter) : this() =>
            other.CloneTo(this, metaFilter, fieldFilter, copyValues, entityFilter);

        //================================================================================================
        //                                              Load (Save is supported only from Editor)
        //================================================================================================
        /// <summary>
        /// load default database
        /// </summary>
        public static void Load() => Load((BGRepoLoadingContext)null);

        /// <summary>
        /// load default database
        /// </summary>
        public static void Load(BGRepoLoadingContext context)
        {
            if (isLoading) return;

            BGMainThreadRunner.EnsureMainThread("Database should be loaded on main thread");
            if (!DefaultRepoLoaded) instance.Events.On = true;
            DefaultRepoLoaded = true;
            DefaultRepoErrorOnLoad = null;
            isLoading = true;
            try
            {
                FireOnBeforeLoad();

                byte[] content = null;
                for (var i = 0; i < Loaders.Length; i++)
                {
                    var loaderForRepo = Loaders[i];
                    content = loaderForRepo.Load(defaultRepoCustomLoaderModel == null
                        ? null
                        : new BGLoaderForRepo.LoadRequest(defaultRepoCustomLoaderModel.MainDatabaseResource)
                    );
                    if (content == null) continue;
                    defaultRepoLoader = loaderForRepo;
                    break;
                }

                if (content == null) NoLuck(BGUtil.NoDatabaseFoundError, false);
                else
                {
                    //database content is loaded ok
                    instance.Load(content);
                    instance.RepoLoader = defaultRepoLoader;
                    instance.RepoAssetPath = DefaultRepoAssetPath;

                    var addons = instance.Addons.Addons;
                    addons.Sort((a1, a2) => a1.OnMainDatabaseLoadOrder.CompareTo(a2.OnMainDatabaseLoadOrder));
                    foreach (var addon in addons) addon.OnMainDatabaseLoad();
                }
            }
            catch (Exception e)
            {
                NoLuck(e.Message ?? e.GetType().FullName, true);
                Debug.LogException(e);
            }
            finally
            {
                context?.OnBeforeFiringOnLoad?.Invoke();
                try
                {
                    FireOnLoad();
                }
                finally
                {
                    isLoading = false;
                }
            }
        }

        private static void FireOnLoad()
        {
            if (OnLoad == null) return;
            try
            {
                OnLoad(DefaultRepoErrorOnLoad == null);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private static void FireOnBeforeLoad()
        {
            if (OnBeforeLoad == null) return;
            try
            {
                OnBeforeLoad();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private static void NoLuck(string message, bool includeLoadingInfo)
        {
            DefaultRepoErrorOnLoad = message ?? "unknown error!";
            if (includeLoadingInfo)
            {
                if (!string.IsNullOrEmpty(DefaultRepoAssetPath)) DefaultRepoErrorOnLoad += ", database path=" + DefaultRepoAssetPath;
                if (defaultRepoLoader != null) DefaultRepoErrorOnLoad += ", loader=" + defaultRepoLoader.Name;
            }

            DefaultRepoLoaded = false;
            DefaultRepoAssetId = 0;
            DefaultRepoAssetPath = null;
        }

        /// <summary>
        /// load from byte array
        /// </summary>
        public void Load(byte[] data)
        {
            repoAddons.Clear();
//            Events.Clear();

            var repo = Reader.Read(data);

            Events.WithEventsDisabled(() => repoAddons.AddFrom(repo.Addons));

            MergeOnLoad(repo);

#if !BG_SA
            //what the logic behind this check??????
            if (Application.isPlaying && repoAddons.Has<BGAddonMT>())
            {
                mtService = null;
                var service = MTService;
            }
#endif
            if (Events.On) Events.FireFullChange();
        }

        /// <summary>
        /// save database to byte array
        /// </summary>
        public byte[] Save()
        {
            return new BGRepoBinary().Write(this);
        }

        private void MergeOnLoad(BGRepo repo)
        {
            //data
            Events.WithEventsDisabled(() =>
            {
                Merge(repo, new BGMergeSettingsEntity { Mode = BGMergeModeEnum.Transfer });
            });

            //addons
            var addons = repoAddons.Addons;
            for (var i = 0; i < addons.Count; i++) addons[i].OnLoad();

            //events
            if (DefaultRepo(this) && defaultRepoEventsHolder != null)
            {
                ForEachMeta(meta =>
                {
                    defaultRepoEventsHolder.TransferEventsTo(meta);
                    if (!meta.LazyLoadingEnabledAndNotLoadedYet) meta.ForEachField(field => defaultRepoEventsHolder.TransferEventsTo(field));
                    else
                    {
                        var holder = defaultRepoEventsHolder;
                        meta.LazyLoader.AddAction(() => meta.ForEachField(field => holder.TransferEventsTo(field)));
                    }
                });
                defaultRepoEventsHolder = null;
            }
        }

        //=================================================================================================================
        //                      Custom loader
        //=================================================================================================================
        /// <summary>
        /// set default database content. If this content is set and is not null, it will be used during database loading  
        /// </summary>
        public static void SetDefaultRepoContent(byte[] defaultRepoContent)
        {
            if (defaultRepoContent == null) defaultRepoCustomLoaderModel = null;
            else defaultRepoCustomLoaderModel = new BGRepoCustomLoaderModel(new BGRepoCustomLoaderModel.DatabaseResource(defaultRepoContent));
        }

        /// <summary>
        /// set default database content with all additional resources.
        /// If this content is set and is not null, it will be used during database loading  (and during localization addon loading)
        /// </summary>
        public static void SetDefaultRepoContentModel(BGRepoCustomLoaderModel defaultRepoContent)
        {
            defaultRepoCustomLoaderModel = defaultRepoContent;
        }

        /// <summary>
        /// database content model if used
        /// </summary>
        public static BGRepoCustomLoaderModel DefaultRepoCustomLoaderModel => defaultRepoCustomLoaderModel;

        /// <summary>
        /// Default repo loader
        /// </summary>
        public static BGLoaderForRepo DefaultRepoLoader => defaultRepoLoader;

        //=================================================================================================================
        //                      Merge
        //=================================================================================================================
        /// <summary>
        /// merge with another database
        /// </summary>
        public void Merge(BGRepo repo, BGMergeSettingsEntity settings = null)
        {
            if (repo == this) throw new BGException("Can not merge with itself!");

            new BGMergerEntity(null, repo, this, settings).Merge();
        }

        //=================================================================================================================
        //                      Meta
        //=================================================================================================================
        private readonly BGIdDictionary<BGMetaEntity> id2Meta = new BGIdDictionary<BGMetaEntity>();
        private readonly Dictionary<string, BGMetaEntity> name2Meta = new Dictionary<string, BGMetaEntity>();
        private readonly List<BGMetaEntity> metas = new List<BGMetaEntity>();

        /// <summary>
        /// number of metas
        /// </summary>
        public int CountMeta => metas.Count;

        //do not remove!
        internal BGId NewMetaId
        {
            get
            {
                var id = BGId.NewId;
                while (id2Meta.ContainsKey(id))
                    //probably it's not possible- but just in case
                    id = BGId.NewId;

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
                var result = 0;
                ForEachMeta(meta => result += meta.CountFields);
                return result;
            }
        }

        //do not remove!!
        private void RebuildIndexes()
        {
            id2Meta.Clear();
            name2Meta.Clear();
            foreach (var meta in metas)
            {
                id2Meta[meta.Id] = meta;
                name2Meta[meta.Name] = meta;
            }
        }

        /// <summary>
        /// get meta by name
        /// </summary>
        public BGMetaEntity this[string metaName] => GetMeta(metaName);

        /// <summary>
        /// get meta by id
        /// </summary>
        public BGMetaEntity this[BGId metaId] => GetMeta(metaId);

        /// <summary>
        /// get meta by index
        /// </summary>
        public BGMetaEntity this[int index] => metas[index];

        /// <summary>
        /// find meta by filter
        /// </summary>
        public BGMetaEntity FindMeta(Predicate<BGMetaEntity> filter)
        {
            foreach (var meta in metas)
                if (filter(meta))
                    return meta;
            return null;
        }

        /// <summary>
        /// if meta with id exists
        /// </summary>
        public bool HasMeta(BGId metaId) => id2Meta.ContainsKey(metaId);

        /// <summary>
        /// if meta with name exists
        /// </summary>
        public bool HasMeta(string name) => name2Meta.ContainsKey(name);

        /// <summary>
        /// get meta id by name
        /// </summary>
        public BGId GetMetaId(string name) => BGUtil.Get(name2Meta, name).Id;

        /// <summary>
        /// get meta by name
        /// </summary>
        public BGMetaEntity GetMeta(string name) => BGUtil.Get(name2Meta, name);

        /// <summary>
        /// get meta by id
        /// </summary>
        public BGMetaEntity GetMeta(BGId id) => BGUtil.Get(id2Meta, id);

        /// <summary>
        /// get meta by index
        /// </summary>
        public BGMetaEntity GetMeta(int index) => this[index];

        /// <summary>
        /// get meta by id
        /// </summary>
        public T GetMeta<T>(BGId id) where T : BGMetaEntity => (T)BGUtil.Get(id2Meta, id);

        /// <summary>
        /// get meta by name
        /// </summary>
        public T GetMeta<T>(string name) where T : BGMetaEntity => (T)BGUtil.Get(name2Meta, name);

        /// <summary>
        /// invoke action for each meta
        /// </summary>
        public void ForEachMeta(Action<BGMetaEntity> action)
        {
            //do not replace with foreach
            for (var i = 0; i < metas.Count; i++) action(metas[i]);
        }

        /// <summary>
        /// invoke action for each meta, complying to filter
        /// </summary>
        public void ForEachMeta(Action<BGMetaEntity> action, Predicate<BGMetaEntity> filter)
        {
            //do not replace with foreach
            for (var i = 0; i < metas.Count; i++)
            {
                var meta = metas[i];
                if (filter != null && !filter(meta)) continue;
                action(meta);
            }
        }

        /// <summary>
        /// find all metas, which comply to given filter
        /// </summary>
        public List<BGMetaEntity> FindMetas(List<BGMetaEntity> result = null, Predicate<BGMetaEntity> filter = null)
        {
            if (result == null) result = new List<BGMetaEntity>();
            else result.Clear();

            var count = CountMeta;
            if (count == 0) return result;
            if (filter == null) result.AddRange(metas);
            else ForEachMeta(meta => result.Add(meta), filter);
            return result;
        }

        internal void Register(BGMetaEntity meta)
        {
            ErrorIfMetaNameIsNotUnique(meta.Name);
            id2Meta[meta.Id] = meta;
            name2Meta[meta.Name] = meta;
            metas.Add(meta);
            repoEvents.MetaWasAdded(meta);
        }

        internal void Unregister(BGMetaEntity meta)
        {
            if (!id2Meta.ContainsKey(meta.Id)) throw new BGException("Meta with id ($) not found!", meta.Id);

            id2Meta.Remove(meta.Id);
            name2Meta.Remove(meta.Name);
            metas.Remove(meta);

            repoEvents.MetaWasDeleted(meta);
        }

        internal void MetaNameWasChanged(string oldName, string newName)
        {
            var meta = name2Meta[oldName];
            name2Meta[newName] = meta;
            name2Meta.Remove(oldName);
            repoEvents.MetaWasChanged(meta);
        }

        /// <summary>
        /// Swap physical order for 2 metas   
        /// </summary>
        public void SwapMetas(int metaIndex1, int metaIndex2)
        {
            var count = CountMeta;
            if (metaIndex1 < 0 || metaIndex2 < 0 || metaIndex1 >= count || metaIndex2 >= count) throw new BGException("Invalid meta indexes for swap: $ and $ ", metaIndex1, metaIndex2);

            (metas[metaIndex1], metas[metaIndex2]) = (metas[metaIndex2], metas[metaIndex1]);

            if (Events.On) Events.FireAnyChange();
        }

        /// <summary>
        /// find meta index (-1 if not found)
        /// </summary>
        public int GetMetaIndex(BGId metaId)
        {
            for (var i = 0; i < metas.Count; i++)
            {
                if (metas[i].Id != metaId) continue;
                return i;
            }

            return -1;
        }

        //check meta/view name for uniqueness
        public void ErrorIfMetaNameIsNotUnique(string metaName)
        {
            if (name2Meta.ContainsKey(metaName)) throw new BGException("Meta with name ($) already exists! name should be unique", metaName);
            if (name2View != null && name2View.ContainsKey(metaName)) throw new BGException("View with name ($) already exists! name should be unique", metaName);
        }

        //=================================================================================================================
        //                      Entity
        //=================================================================================================================
        //do not remove! <- Probably not used anymore!
        [Obsolete]
        internal BGId NewEntityId => BGId.NewId;

        /// <summary>
        /// number of entities
        /// </summary>
        public int CountEntities
        {
            get
            {
                var count = 0;
                for (var i = 0; i < metas.Count; i++) count += metas[i].CountEntities;
                return count;
            }
        }

        /// <summary>
        /// if entity with id exists. this is slow method, use meta.HasEntity instead
        /// </summary>
        public bool HasEntity(BGId entityId)
        {
            for (var i = 0; i < metas.Count; i++)
                if (metas[i].HasEntity(entityId))
                    return true;

            return false;
        }

        /// <summary>
        /// get entity by id. this is slow method, use meta[id] instead
        /// </summary>
        public BGEntity GetEntity(BGId entityId)
        {
            for (var i = 0; i < metas.Count; i++)
            {
                var entity = metas[i].GetEntity(entityId);
                if (entity != null) return entity;
            }

            return null;
        }

        /// <summary>
        /// invoke action for each entity, that comply to filter
        /// </summary>
        public void ForEachEntity(Action<BGEntity> action, Predicate<BGEntity> filter)
        {
            ForEachMeta(meta => meta.ForEachEntity(action, filter));
        }

        //=================================================================================================================
        //                      Field
        //=================================================================================================================
        /// <summary>
        /// invoke action for each field, complying to the filter
        /// </summary>
        public void ForEachField(Action<BGField> action, Predicate<BGField> filter)
        {
            ForEachMeta(meta => meta.ForEachField(action, filter));
        }

        /// <summary>
        /// Get field by id
        /// </summary>
        public BGField GetField(BGId fieldId)
        {
            //do not replace with foreach
            for (var i = 0; i < metas.Count; i++)
            {
                var meta = metas[i];
                var fieldCount = meta.CountFields;
                if (fieldCount == 0) continue;
                for (var j = 0; j < fieldCount; j++)
                {
                    var field = meta.GetField(j);
                    if (field.Id == fieldId) return field;
                }
            }

            return null;
        }

        [Obsolete("Use BGMetaEntity.NewFieldId instead")]
        internal BGId NewFieldId(BGMetaEntity meta) => meta.NewFieldId;

        //=================================================================================================================
        //                      View
        //=================================================================================================================
        private Dictionary<BGId, BGMetaView> id2View;
        private Dictionary<string, BGMetaView> name2View;
        private List<BGMetaView> views;

        /// <summary>
        /// number of views
        /// </summary>
        public int CountViews => views?.Count ?? 0;

        //do not remove!
        internal BGId NewViewId
        {
            get
            {
                var id = BGId.NewId;
                if (id2View != null)
                {
                    while (id2View.ContainsKey(id))
                        //probably it's not possible- but just in case
                        id = BGId.NewId;
                }

                return id;
            }
        }

        /// <summary>
        /// find view by filter
        /// </summary>
        public BGMetaView FindView(Predicate<BGMetaView> filter)
        {
            if (views == null) return null;
            foreach (var view in views)
                if (filter(view))
                    return view;
            return null;
        }

        /// <summary>
        /// if view with id exists
        /// </summary>
        public bool HasView(BGId viewId)
        {
            if (views == null) return false;
            return id2View.ContainsKey(viewId);
        }

        /// <summary>
        /// if view with name exists
        /// </summary>
        public bool HasView(string name)
        {
            if (views == null) return false;
            return name2View.ContainsKey(name);
        }

        /// <summary>
        /// get view id by name
        /// </summary>
        public BGId GetViewId(string name)
        {
            if (views == null) return BGId.Empty;
            return BGUtil.Get(name2View, name).Id;
        }

        /// <summary>
        /// get view by name
        /// </summary>
        public BGMetaView GetView(string name)
        {
            if (views == null) return null;
            return BGUtil.Get(name2View, name);
        }

        /// <summary>
        /// get view by id
        /// </summary>
        public BGMetaView GetView(BGId id)
        {
            if (views == null) return null;
            return BGUtil.Get(id2View, id);
        }

        /// <summary>
        /// get view by index
        /// </summary>
        public BGMetaView GetView(int index)
        {
            if (views == null) throw new Exception($"Can not get a view with index {index}- there are no views in the repository");
            return views[index];
        }

        /// <summary>
        /// invoke action for each view
        /// </summary>
        public void ForEachView(Action<BGMetaView> action) => ForEachView(action, null);

        /// <summary>
        /// invoke action for each view, complying to filter
        /// </summary>
        public void ForEachView(Action<BGMetaView> action, Predicate<BGMetaView> filter)
        {
            if (views == null) return;
            //do not replace with foreach
            for (var i = 0; i < views.Count; i++)
            {
                var view = views[i];
                if (filter != null && !filter(view)) continue;
                action(view);
            }
        }

        /// <summary>
        /// find all views, which comply to provided filter
        /// </summary>
        public List<BGMetaView> FindViews(List<BGMetaView> result = null, Predicate<BGMetaView> filter = null)
        {
            if (result == null) result = new List<BGMetaView>();
            else result.Clear();

            var count = CountViews;
            if (count == 0) return result;
            if (filter == null) result.AddRange(views);
            else ForEachView(view => result.Add(view), filter);
            return result;
        }

        internal void Register(BGMetaView view)
        {
            ErrorIfMetaNameIsNotUnique(view.Name);
            EnsureViewContainers();

            id2View[view.Id] = view;
            name2View[view.Name] = view;
            views.Add(view);
            repoEvents.ViewWasAdded(view);
        }

        internal void Unregister(BGMetaView view)
        {
            if (views == null) return;
            if (!id2View.ContainsKey(view.Id)) throw new BGException("View with id ($) not found!", view.Id);

            id2View.Remove(view.Id);
            name2View.Remove(view.Name);
            views.Remove(view);

            repoEvents.ViewWasDeleted(view);
        }

        internal void ViewNameWasChanged(string oldName, string newName)
        {
            var view = name2View[oldName];
            name2View[newName] = view;
            name2View.Remove(oldName);
            repoEvents.ViewWasChanged(view);
        }

        /// <summary>
        /// Swap physical order for 2 views   
        /// </summary>
        public void SwapViews(int viewIndex1, int viewIndex2)
        {
            var count = CountViews;
            if (viewIndex1 < 0 || viewIndex2 < 0 || viewIndex1 >= count || viewIndex2 >= count) throw new BGException("Invalid view indexes for swap: $ and $ ", viewIndex1, viewIndex2);

            (views[viewIndex1], views[viewIndex2]) = (views[viewIndex2], views[viewIndex1]);

            if (Events.On) Events.FireAnyChange();
        }

        /// <summary>
        /// find view index (-1 if not found)
        /// </summary>
        public int GetViewIndex(BGId viewId)
        {
            if (views == null) return -1;
            for (var i = 0; i < views.Count; i++)
            {
                if (views[i].Id != viewId) continue;
                return i;
            }

            return -1;
        }

        private void EnsureViewContainers()
        {
            if (views != null) return;
            views = new List<BGMetaView>();
            id2View = new Dictionary<BGId, BGMetaView>();
            name2View = new Dictionary<string, BGMetaView>();
        }

        //=================================================================================================================
        //                      Misc
        //=================================================================================================================
        /// <summary>
        /// clear database
        /// </summary>
        public void Clear() => ClearInternal();

        /// <summary>
        /// clone to another database. it can copy values too. Filters are used to filter data.
        /// </summary>
        public void CloneTo(BGRepo repo, Predicate<BGId> metaFilter, Predicate<BGField> fieldFilter, bool copyValues) => CloneTo(repo, metaFilter, fieldFilter, copyValues, null);

        /// <summary>
        /// clone to another database. it can copy values too. Filters are used to filter data.
        /// </summary>
        public void CloneTo(BGRepo repo, Predicate<BGId> metaFilter, Predicate<BGField> fieldFilter, bool copyValues, Predicate<BGEntity> entityFilter)
        {
            repo.Addons.AddFrom(Addons);

            ForEachMeta(meta => meta.CloneTo(repo, metaFilter, fieldFilter, copyValues, entityFilter));
            ForEachView(view => view.CloneTo(repo));
        }

        /// <summary>
        /// Make sure, database state is restored if any error occurs.
        /// this is very expensive operation, cause it needs to create a full copy of database. 
        /// </summary>
        public void Transaction(Action action)
        {
            //save state
            var repo = new BGRepo(this, true);
            try
            {
                repoEvents.Batch(action);
            }
            catch (Exception e)
            {
                //restore state
                Merge(repo);
                Addons.Clear();
                Addons.AddFrom(repo.Addons);
                Debug.LogException(e);
                throw;
            }
        }

        private void ClearInternal()
        {
            //meta
            ForEachMeta(meta =>
            {
                if (!meta.LazyLoadingEnabledAndNotLoadedYet)
                {
                    meta.ForEachField(field =>
                    {
                        field.OnDelete();
                        field.Unload();
                    });
                    meta.ForEachKey(key =>
                    {
                        key.OnDelete();
                        key.Unload();
                    });
                    meta.ForEachIndex(index =>
                    {
                        index.OnDelete();
                        index.Unload();
                    });
                    meta.ClearEntities();
                }

                meta.Unload();
            }, meta => meta.Repo == this);

            //events
            if (DefaultRepo(this))
            {
                if (DefaultRepoResetEventsOnLoad)
                {
                    DefaultRepoResetEventsOnLoad = false;
                    defaultRepoEventsHolder = null;
                    // Events.Clear();
                }
                else
                {
                    if (defaultRepoEventsHolder == null)
                    {
                        defaultRepoEventsHolder = new BGEventsHolder();
                        ForEachMeta(meta =>
                        {
                            defaultRepoEventsHolder.TransferEventsFrom(meta);
                            if (!meta.LazyLoadingEnabledAndNotLoadedYet) meta.ForEachField(field => defaultRepoEventsHolder.TransferEventsFrom(field));
                        }, meta => meta.Repo == this);
                    }
                }
            }

            //views
            ForEachView(view => view.Unload(), view => view.Repo == this);

            id2Meta.Clear();
            name2Meta.Clear();
            metas.Clear();

            if (views != null)
            {
                views.Clear();
                id2View.Clear();
                name2View.Clear();
            }
        }

        /// <summary>
        /// Is repo default repo
        /// </summary>
        private bool IsDefaultRepo => DefaultRepo(this);

        /// <summary>
        /// Is repo default repo
        /// </summary>
        public static bool DefaultRepo(BGRepo repo)
        {
            return repo == instance;
        }


        /// <summary>
        /// interface for repo reader
        /// </summary>
        public partial interface RepoReaderI
        {
            BGRepo Read(byte[] dataBytes);
        }

        /// <summary>
        /// interface for repo writer
        /// </summary>
        public partial interface RepoWriterI
        {
            byte[] Write(BGRepo repo);
        }
    }
}