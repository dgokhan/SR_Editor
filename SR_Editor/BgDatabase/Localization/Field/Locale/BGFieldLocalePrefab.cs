/*
<copyright file="BGFieldLocalePrefab.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    [FieldDescriptor(ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerLocaleAsset")]
    [BGLocaleField(Name = "unityPrefab", DelegateFieldType = typeof(BGFieldUnityPrefab))]
    public partial class BGFieldLocalePrefab : BGFieldLocaleAssetA<GameObject>
    {
        public const ushort CodeType = 83;
        public override ushort TypeCode => CodeType;

        public BGFieldLocalePrefab(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        internal BGFieldLocalePrefab(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory()
        {
            return (meta, id, name) => new BGFieldLocalePrefab(meta, id, name);
        }
    }
}