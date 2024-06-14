/*
<copyright file="BGFieldLocalizedAssetA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// base class for localized unity asset
    /// </summary>
    public abstract class BGFieldLocalizedAssetA<T> : BGFieldLocalizedA<T>, BGStorableString, BGAddressablesAssetI, BGAssetLoaderA.WithLoaderI
    {
        public BGFieldLocalizedAssetA(BGMetaEntity meta, string name, BGMetaLocalization to) : base(meta, name, to)
        {
        }

        protected BGFieldLocalizedAssetA(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        public virtual Type AssetType => ValueType;

        /// <inheritdoc />
        public override bool ReadOnly => true;

        public override bool StoredValueIsTheSameAsValueType => false;

        private BGStorable<string> GetRelatedField(BGMetaEntity relatedMeta)
        {
            if (relatedMeta == null) return null;
            var localizationLocale = BGAddonLocalization.DefaultRepoCurrentLocale;
            if (string.IsNullOrEmpty(localizationLocale)) return null;

            return relatedMeta.GetField(localizationLocale, false) as BGStorable<string>;
        }


        public void SetStoredValue(int entityIndex, string value)
        {
            var relatedEntity = GetTo(entityIndex);
            if (relatedEntity == null) return;

            var related = GetRelatedField(relatedEntity.Meta);
            related?.SetStoredValue(relatedEntity.Index, value);
        }

        //is this new modifier gonna be a problem??
        public new string GetStoredValue(int entityIndex)
        {
            var relatedEntity = GetTo(entityIndex);
            if (relatedEntity == null) return null;

            var related = GetRelatedField(relatedEntity.Meta);
            return related?.GetStoredValue(relatedEntity.Index);
        }

        protected override string ValueToSearchString(T val, int entityIndex)
        {
            return GetStoredValue(entityIndex);
        }

        //================================================================================================
        //                                              BGAssetLoaderA.WithLoaderI
        //================================================================================================
        public BGAssetLoaderA AssetLoader
        {
            get
            {
                var relatedField = GetRelatedField(To);
                var withLoader = (BGAssetLoaderA.WithLoaderI)relatedField;
                return withLoader?.AssetLoader;
            }
            set
            {
                //why would anyone call it?
            }
        }

        //================================================================================================
        //                                              BGFieldUnityAssetI
        //================================================================================================
        public string GetAssetPath(int entityIndex)
        {
            return GetStoredValue(entityIndex);
        }

        public void SetAssetPath(int entityIndex, string path)
        {
            SetStoredValue(entityIndex, path);
        }

        //================================================================================================
        //                                             BGAddressablesAssetI
        //================================================================================================
        public string GetAddressablesAddress(int entityIndex)
        {
            var relatedEntity = GetTo(entityIndex);
            if (relatedEntity == null) return null;

            var related = GetRelatedField(relatedEntity.Meta);
            return ((BGAddressablesAssetI)related)?.GetAddressablesAddress(relatedEntity.Index);
        }
    }
}