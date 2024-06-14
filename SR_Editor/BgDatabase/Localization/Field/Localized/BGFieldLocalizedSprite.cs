/*
<copyright file="BGFieldLocalizedSprite.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    [FieldDescriptor(Name = "localizedSprite", Folder = "Localization", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerLocalized")]
    [BGLocalizedField(Name = "unitySprite", TargetFieldType = typeof(BGFieldLocaleSprite))]
    public partial class BGFieldLocalizedSprite : BGFieldLocalizedAssetA<Sprite>
    {
        public const ushort CodeType = 92;
        public override ushort TypeCode => CodeType;

        public BGFieldLocalizedSprite(BGMetaEntity meta, string name, BGMetaLocalization to) : base(meta, name, to)
        {
        }

        internal BGFieldLocalizedSprite(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory()
        {
            return (meta, id, name) => new BGFieldLocalizedSprite(meta, id, name);
        }
    }
}