/*
<copyright file="BGFieldLocalizedAudioClip.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    [FieldDescriptor(Name = "localizedAudioClip", Folder = "Localization", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerLocalized")]
    [BGLocalizedField(Name = "unityAudioClip", TargetFieldType = typeof(BGFieldLocaleAudioClip))]
    public partial class BGFieldLocalizedAudioClip : BGFieldLocalizedAssetA<AudioClip>
    {
        public const ushort CodeType = 88;
        public override ushort TypeCode => CodeType;

        public BGFieldLocalizedAudioClip(BGMetaEntity meta, string name, BGMetaLocalization to) : base(meta, name, to)
        {
        }

        internal BGFieldLocalizedAudioClip(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory()
        {
            return (meta, id, name) => new BGFieldLocalizedAudioClip(meta, id, name);
        }
    }
}