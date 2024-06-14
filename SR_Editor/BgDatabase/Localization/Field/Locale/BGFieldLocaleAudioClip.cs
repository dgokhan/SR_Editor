/*
<copyright file="BGFieldLocaleAudioClip.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    [FieldDescriptor(ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerLocaleAsset")]
    [BGLocaleField(Name = "unityAudioClip", DelegateFieldType = typeof(BGFieldUnityAudioClip))]
    public partial class BGFieldLocaleAudioClip : BGFieldLocaleAssetA<AudioClip>
    {
        public const ushort CodeType = 80;
        public override ushort TypeCode => CodeType;

        public BGFieldLocaleAudioClip(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        internal BGFieldLocaleAudioClip(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory()
        {
            return (meta, id, name) => new BGFieldLocaleAudioClip(meta, id, name);
        }
    }
}