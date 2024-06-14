/*
<copyright file="BGFieldLocaleObject.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    [FieldDescriptor(ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerLocaleAsset")]
    [BGLocaleField(Name = "unityObject", DelegateFieldType = typeof(BGFieldUnityObject))]
    public partial class BGFieldLocaleObject : BGFieldLocaleAssetA<Object>, BGAddressablesAssetCustomLoaderI
    {
        public const ushort CodeType = 82;
        public override ushort TypeCode => CodeType;

        public BGFieldLocaleObject(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        internal BGFieldLocaleObject(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory()
        {
            return (meta, id, name) => new BGFieldLocaleObject(meta, id, name);
        }

        //================================================================================================
        //                                             BGAddressablesAssetCustomLoaderI
        //================================================================================================
        public BGAddressablesLoaderModel GetAddressablesLoaderModel(int entityIndex)
        {
            var delegateField = (BGFieldUnityObject)DelegateField;
            var delegateIndex = FindDelegateIndex(entityIndex, delegateField);
            if (delegateIndex == -1) return null;
            return delegateField.GetAddressablesLoaderModel(delegateIndex);
        }

        public override string GetAddressablesAddress(int entityIndex)
        {
            var delegateField = (BGFieldUnityObject)DelegateField;
            var delegateIndex = FindDelegateIndex(entityIndex, delegateField);
            if (delegateIndex == -1) return null;
            return delegateField.GetAddressablesAddress(delegateIndex);
        }
    }
}