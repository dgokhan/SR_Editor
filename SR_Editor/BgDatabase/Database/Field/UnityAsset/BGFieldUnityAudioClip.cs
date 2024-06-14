/*
<copyright file="BGFieldUnityAudioClip.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// audioClip field
    /// </summary>
    [FieldDescriptor(Name = "unityAudioClip", Folder = "Unity Asset", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerUnityAudioClip")]
    public partial class BGFieldUnityAudioClip : BGFieldUnityAssetA<AudioClip>
    {
        public const ushort CodeType = 49;

        /// <inheritdoc />
        public override ushort TypeCode => CodeType;

        //for new field
        public BGFieldUnityAudioClip(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldUnityAudioClip(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc />
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldUnityAudioClip(meta, id, name);
    }
}