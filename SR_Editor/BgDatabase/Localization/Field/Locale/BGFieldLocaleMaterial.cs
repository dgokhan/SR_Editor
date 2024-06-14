/*
<copyright file="BGFieldLocaleMaterial.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    [FieldDescriptor(ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerLocaleAsset")]
    [BGLocaleField(Name = "unityMaterial", DelegateFieldType = typeof(BGFieldUnityMaterial))]
    public partial class BGFieldLocaleMaterial : BGFieldLocaleAssetA<Material>
    {
        public const ushort CodeType = 81;
        public override ushort TypeCode => CodeType;

        public BGFieldLocaleMaterial(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        internal BGFieldLocaleMaterial(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory()
        {
            return (meta, id, name) => new BGFieldLocaleMaterial(meta, id, name);
        }
    }
}