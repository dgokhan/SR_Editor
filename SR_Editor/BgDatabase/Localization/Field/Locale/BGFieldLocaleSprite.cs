/*
<copyright file="BGFieldLocaleSprite.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    [FieldDescriptor(ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerLocaleAsset")]
    [BGLocaleField(Name = "unitySprite", DelegateFieldType = typeof(BGFieldUnitySprite))]
    public partial class BGFieldLocaleSprite : BGFieldLocaleAssetA<Sprite>, BGAddressablesAssetCustomLoaderI
    {
        public const ushort CodeType = 84;
        public override ushort TypeCode => CodeType;

        public BGFieldLocaleSprite(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        internal BGFieldLocaleSprite(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory()
        {
            return (meta, id, name) => new BGFieldLocaleSprite(meta, id, name);
        }

        //================================================================================================
        //                                             BGAddressablesAssetCustomLoaderI
        //================================================================================================
        public BGAddressablesLoaderModel GetAddressablesLoaderModel(int entityIndex)
        {
            var delegateField = (BGFieldUnitySprite)DelegateField;
            var delegateIndex = FindDelegateIndex(entityIndex, delegateField);
            if (delegateIndex == -1) return null;
            return delegateField.GetAddressablesLoaderModel(delegateIndex);
        }

        public override string GetAddressablesAddress(int entityIndex)
        {
            var delegateField = (BGFieldUnitySprite)DelegateField;
            var delegateIndex = FindDelegateIndex(entityIndex, delegateField);
            if (delegateIndex == -1) return null;
            return delegateField.GetAddressablesAddress(delegateIndex);
        }
    }
}