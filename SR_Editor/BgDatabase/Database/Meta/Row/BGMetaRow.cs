/*
<copyright file="BGMetaRow.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// meta for row. 
    /// </summary>
    [MetaDescriptor(Name = "row", ManagerType = "BansheeGz.BGDatabase.Editor.BGMetaManagerRow")]
    public partial class BGMetaRow : BGMetaEntity
    {
        public const ushort CodeType = 1;

        //for new models
        public BGMetaRow(BGRepo repo, string name) : base(repo, name)
        {
        }

        //for existing
        internal BGMetaRow(BGRepo repo, BGId id, string name) : base(repo, id, name)
        {
        }

        //================================================================================================
        //                                              Duplication
        //================================================================================================
        /// <summary>
        /// duplicate this table, optionally copy all data
        /// </summary>
        public BGMetaRow Duplicate(string newMetaName, bool copyData) => new BGMetaRowDuplication(this, newMetaName, copyData).Execute();

        //================================================================================================
        //                                              Factory
        //================================================================================================
        protected override Func<BGRepo, BGId, string, BGMetaEntity> CreateMetaFactory() => (repo, id, name) => new BGMetaRow(repo, id, name);

        public override ushort TypeCode => CodeType;
    }
}