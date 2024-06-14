/*
<copyright file="BGFieldReferenceToEntityGoList.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// reference to list of BGEntityGo (or any inherited from BGEntityGo) components
    /// </summary>
    [FieldDescriptor(Name = "entityListReference", Folder = "Unity Scene", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerReferenceToEntityGoList", 
        DeprecatedNote="Use objectListReference field instead!")]
    public partial class BGFieldReferenceToEntityGoList : BGFieldReferenceListA<BGEntityGo>
    {
        public const ushort CodeType = 77;
        
        /// <inheritdoc />
        public override ushort TypeCode => CodeType;

        //for new field
        public BGFieldReferenceToEntityGoList(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldReferenceToEntityGoList(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Value
        //================================================================================================
        /// <inheritdoc />
        protected override BGId IdProvider(BGEntityGo component) => component.EntityId;

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc />
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldReferenceToEntityGoList(meta, id, name);
    }
}