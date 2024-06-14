/*
<copyright file="BGFieldLocaleTexture.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    [FieldDescriptor(ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerLocaleAsset")]
    [BGLocaleField(Name = "unityTexture", DelegateFieldType = typeof(BGFieldUnityTexture))]
    public partial class BGFieldLocaleTexture : BGFieldLocaleAssetA<Texture>
    {
        public const ushort CodeType = 87;
        public override ushort TypeCode => CodeType;

        public BGFieldLocaleTexture(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        internal BGFieldLocaleTexture(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory()
        {
            return (meta, id, name) => new BGFieldLocaleTexture(meta, id, name);
        }
    }
}