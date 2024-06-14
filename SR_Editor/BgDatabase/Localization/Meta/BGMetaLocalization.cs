/*
<copyright file="BGMetaLocalization.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    [MetaDescriptor(Name = "localization", ManagerType = "BansheeGz.BGDatabase.Editor.BGMetaManagerLocalization")]
    public partial class BGMetaLocalization : BGMetaLocalizationA
    {
        public const ushort CodeType = 4;

        public BGMetaLocalization(BGRepo repo, string name, Type fieldType) : base(repo, name, fieldType)
        {
        }

        internal BGMetaLocalization(BGRepo repo, BGId id, string name) : base(repo, id, name)
        {
        }

        //================================================================================================
        //                                              Delete
        //================================================================================================
        public override void Delete()
        {
            var repo = Repo;

            repo?.Events.Batch(() =>
            {
                var toDelete = new List<BGField>();
                repo.ForEachMeta(meta => meta.ForEachField(field => toDelete.Add(field), field => field is BGFieldLocalizedI localizedI && localizedI.ToId == Id));

                foreach (var field in toDelete) field.Delete();
                base.Delete();
            });
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        protected override Func<BGRepo, BGId, string, BGMetaEntity> CreateMetaFactory() => (repo, id, name) => new BGMetaLocalization(repo, id, name);

        public override ushort TypeCode => CodeType;

        //================================================================================================
        //                                              Misc
        //================================================================================================
        public override bool IsManagingItsOwnEntities => true;

        public override bool SupportPartitioningField => false;
    }
}