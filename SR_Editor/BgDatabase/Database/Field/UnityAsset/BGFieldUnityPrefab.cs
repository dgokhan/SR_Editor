/*
<copyright file="BGFieldUnityPrefab.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// prefab field
    /// </summary>
    [FieldDescriptor(Name = "unityPrefab", Folder = "Unity Asset", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerUnityPrefab")]
    public partial class BGFieldUnityPrefab : BGFieldUnityAssetA<GameObject>
    {
        public const ushort CodeType = 53;
        
        /// <inheritdoc />
        public override ushort TypeCode => CodeType;

        //for new field
        public BGFieldUnityPrefab(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldUnityPrefab(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc />
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldUnityPrefab(meta, id, name);
    }
}