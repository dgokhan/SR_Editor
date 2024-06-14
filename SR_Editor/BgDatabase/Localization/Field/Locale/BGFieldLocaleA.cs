/*
<copyright file="BGFieldLocaleA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// holds localized values
    /// </summary>
    public abstract partial class BGFieldLocaleA<T> : BGField<T>, BGFieldLocaleI
    {
        public override bool EmptyContent => true;

        //Should we add this?
        //public override bool SupportMultiThreadedLoading => false;
        
        public override string Name
        {
            get => base.Name;
            set => throw new BGException("Not supported");
        }

        protected bool IsMainRepo => BGRepo.DefaultRepo(Meta.Repo);


        //for new
        public BGFieldLocaleA(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing
        protected BGFieldLocaleA(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        public abstract void EnsureStore();

        public abstract void DestroyStore();
        //================================================================================================
        //                                              Callbacks
        //================================================================================================
    }
}