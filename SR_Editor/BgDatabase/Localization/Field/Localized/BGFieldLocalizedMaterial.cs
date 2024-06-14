/*
<copyright file="BGFieldLocalizedMaterial.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    [FieldDescriptor(Name = "localizedMaterial", Folder = "Localization", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerLocalized")]
    [BGLocalizedField(Name = "unityMaterial", TargetFieldType = typeof(BGFieldLocaleMaterial))]
    public partial class BGFieldLocalizedMaterial : BGFieldLocalizedAssetA<Material>
    {
        public const ushort CodeType = 89;
        public override ushort TypeCode => CodeType;

        public BGFieldLocalizedMaterial(BGMetaEntity meta, string name, BGMetaLocalization to) : base(meta, name, to)
        {
        }

        internal BGFieldLocalizedMaterial(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory()
        {
            return (meta, id, name) => new BGFieldLocalizedMaterial(meta, id, name);
        }
    }
}