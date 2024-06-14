/*
<copyright file="BGFieldLocalizedTexture.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    [FieldDescriptor(Name = "localizedTexture", Folder = "Localization", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerLocalized")]
    [BGLocalizedField(Name = "unityTexture", TargetFieldType = typeof(BGFieldLocaleTexture))]
    public partial class BGFieldLocalizedTexture : BGFieldLocalizedAssetA<Texture>
    {
        public const ushort CodeType = 95;
        public override ushort TypeCode => CodeType;

        public BGFieldLocalizedTexture(BGMetaEntity meta, string name, BGMetaLocalization to) : base(meta, name, to)
        {
        }

        internal BGFieldLocalizedTexture(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory()
        {
            return (meta, id, name) => new BGFieldLocalizedTexture(meta, id, name);
        }
    }
}