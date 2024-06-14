/*
<copyright file="BGFieldUnityTexture.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// texture field
    /// </summary>
    [FieldDescriptor(Name = "unityTexture", Folder = "Unity Asset", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerUnityTexture")]
    public partial class BGFieldUnityTexture : BGFieldUnityAssetA<Texture>
    {
        public const ushort CodeType = 57;

        /// <inheritdoc />
        public override ushort TypeCode => CodeType;

        //for new field
        public BGFieldUnityTexture(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldUnityTexture(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc />
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldUnityTexture(meta, id, name);
    }
}