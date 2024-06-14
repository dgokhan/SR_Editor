/*
<copyright file="BGMetaLocalizationSettings.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    [MetaDescriptor(ManagerType = "BansheeGz.BGDatabase.Editor.BGMetaManagerLocalizationSettings")]
    public partial class BGMetaLocalizationSettings : BGMetaEntity
    {
        public const ushort CodeType = 3;

        public BGMetaLocalizationSettings(BGRepo repo, string name) : base(repo, name)
        {
        }

        internal BGMetaLocalizationSettings(BGRepo repo, BGId id, string name) : base(repo, id, name)
        {
        }


        //================================================================================================
        //                                              Factory
        //================================================================================================
        protected override Func<BGRepo, BGId, string, BGMetaEntity> CreateMetaFactory()
        {
            return (repo, id, name) => new BGMetaLocalizationSettings(repo, id, name);
        }

        public override ushort TypeCode => CodeType;

        //================================================================================================
        //                                              Misc
        //================================================================================================
        public override bool IsManagingItsOwnEntities => true;

        public override bool SupportPartitioningField => false;
    }
}