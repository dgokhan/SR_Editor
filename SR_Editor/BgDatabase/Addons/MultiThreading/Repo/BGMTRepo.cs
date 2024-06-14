/*
<copyright file="BGMTRepo.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Multi-threaded repo
    /// </summary>
    public class BGMTRepo
    {
        private readonly BGIdDictionary<BGMTMeta> id2Meta;
        private readonly Dictionary<string, BGMTMeta> name2Meta;
        private readonly BGMTMeta[] metas;

        internal BGMTRepo(BGMTMeta[] metaList)
        {
            id2Meta = new BGIdDictionary<BGMTMeta>();
            name2Meta = new Dictionary<string, BGMTMeta>();
            metas = metaList;
            foreach (var meta in metaList)
            {
                meta.Repo = this;
                id2Meta[meta.Id] = meta;
                name2Meta[meta.Name] = meta;
            }
        }

        private BGMTRepo(BGIdDictionary<BGMTMeta> id2Meta, Dictionary<string, BGMTMeta> name2Meta, BGMTMeta[] metas)
        {
            this.id2Meta = id2Meta;
            this.name2Meta = name2Meta;
            this.metas = metas;
            if (metas != null)
                for (var i = 0; i < this.metas.Length; i++)
                    this.metas[i].Repo = this;
        }

        //================================================================================================
        //                                              Meta
        //================================================================================================

        public BGMTMeta this[int index] => metas[index];

        public BGMTMeta this[BGId id] => id2Meta.TryGetValue(id, out var result) ? result : null;

        public BGMTMeta this[string name] => name2Meta.TryGetValue(name, out var result) ? result : null;

        public void ForEachMeta(Action<BGMTMeta> action)
        {
            for (var i = 0; i < metas.Length; i++) action(metas[i]);
        }

        //================================================================================================
        //                                              Readonly/Writable
        //================================================================================================

        internal BGMTRepo ToWritableRepo()
        {
            var id2Meta = new BGIdDictionary<BGMTMeta>();
            var name2Meta = new Dictionary<string, BGMTMeta>();

            var metaToCopy = this.metas;
            var metas = new BGMTMeta[metaToCopy.Length];
            for (var i = 0; i < metaToCopy.Length; i++)
            {
                var metaUpdatable = new BGMTMetaUpdatable(metaToCopy[i]);
                metas[i] = metaUpdatable;
                id2Meta[metaUpdatable.Id] = metaUpdatable;
                name2Meta[metaUpdatable.Name] = metaUpdatable;
            }

            return new BGMTRepo(id2Meta, name2Meta, metas);
        }

        internal BGMTRepo ToReadOnlyRepo()
        {
            var id2Meta = new BGIdDictionary<BGMTMeta>();
            var name2Meta = new Dictionary<string, BGMTMeta>();

            var metaToCopy = this.metas;
            var metas = new BGMTMeta[metaToCopy.Length];
            for (var i = 0; i < metaToCopy.Length; i++)
            {
                var updatable = metaToCopy[i];
                var meta = new BGMTMeta(updatable);
                updatable.Dispose();

                metas[i] = meta;
                id2Meta[meta.Id] = meta;
                name2Meta[meta.Name] = meta;
            }

            return new BGMTRepo(id2Meta, name2Meta, metas);
        }
    }
}