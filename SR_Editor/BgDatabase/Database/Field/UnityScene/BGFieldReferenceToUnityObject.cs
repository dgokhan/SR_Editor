/*
<copyright file="BGFieldReferenceToUnityObject.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// reference to BGWithId component
    /// </summary>
    [FieldDescriptor(Name = "objectReference", Folder = "Unity Scene", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerReferenceToUnityObject")]
    public partial class BGFieldReferenceToUnityObject : BGFieldReferenceSingleA<BGWithId>
    {
        public const ushort CodeType = 78;
        
        /// <inheritdoc />
        public override ushort TypeCode => CodeType;

        //for new field
        public BGFieldReferenceToUnityObject(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        protected internal BGFieldReferenceToUnityObject(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Value
        //================================================================================================
        /// <inheritdoc />
        protected override BGId IdProvider(BGWithId component) => component.Id;

        /// <inheritdoc />
        protected override BGWithId GetById(BGId id) => BGWithId.Get(id);

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc />
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldReferenceToUnityObject(meta, id, name);
    }
}