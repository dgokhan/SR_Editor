/*
<copyright file="BGFieldLocalizedPrefab.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    [FieldDescriptor(Name = "localizedPrefab", Folder = "Localization", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerLocalized")]
    [BGLocalizedField(Name = "unityPrefab", TargetFieldType = typeof(BGFieldLocalePrefab))]
    public partial class BGFieldLocalizedPrefab : BGFieldLocalizedAssetA<GameObject>
    {
        public const ushort CodeType = 91;
        public override ushort TypeCode => CodeType;

        public BGFieldLocalizedPrefab(BGMetaEntity meta, string name, BGMetaLocalization to) : base(meta, name, to)
        {
        }

        internal BGFieldLocalizedPrefab(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory()
        {
            return (meta, id, name) => new BGFieldLocalizedPrefab(meta, id, name);
        }
    }
}