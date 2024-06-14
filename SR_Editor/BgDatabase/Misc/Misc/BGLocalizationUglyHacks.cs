/*
<copyright file="BGLocalizationUglyHacks.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// This class has all the code, which is meant to be in localization addon, not in database itself
    /// </summary>
    public static class BGLocalizationUglyHacks
    {
        public const string DataBinderLocale = "$locale";

        static BGLocalizationUglyHacks()
        {
            var localizationMetaFactoryType = BGUtil.GetType("BansheeGz.BGDatabase.BGMetaLocalizationTypeCodeFactory");
            if (localizationMetaFactoryType != null) LocalizationMetaFactory = Activator.CreateInstance(localizationMetaFactoryType) as BGMetaTypeCodeFactory.BGMetaTypeCodeFactoryI;

            var localizationFieldFactoryType = BGUtil.GetType("BansheeGz.BGDatabase.BGFieldLocalizationTypeCodeFactory");
            if (localizationFieldFactoryType != null) LocalizationFieldFactory = Activator.CreateInstance(localizationFieldFactoryType) as BGFieldTypeCodeFactory.BGFieldTypeCodeFactoryI;
        }

        //========================================================================
        //                 DataBinding
        //========================================================================
        private static BGDBField.FieldValueProvider localeFieldProvider;

        public static bool CanEnableDelta => !HasLocalizationAddon(BGRepo.I);

        public static BGDBField.FieldValueProvider DataBindingInitValueProvider(string fieldIdString)
        {
            //this is very intrusive way of doing things , but it's done for the sake of performance 
            if (fieldIdString?.StartsWith(DataBinderLocale) ?? false)
            {
                if (localeFieldProvider == null)
                    try
                    {
                        localeFieldProvider = BGUtil.Create<BGDBField.FieldValueProvider>("BansheeGz.BGDatabase.BGDBLocaleFieldValueProvider", false);
                    }
                    catch
                    {
                        return null;
                    }

                return localeFieldProvider.Create();
            }

            return null;
        }

        private static BGDBTextBinderField.BGDBTextBinderFieldSpecial LocaleBinder;

        public static bool DataBindingBind(string fieldName, BGDBTextBinderRoot root, BGDBTextBinderField.Pointer pointer)
        {
            if (!DataBinderLocale.Equals(fieldName)) return false;

            if (LocaleBinder == null)
                try
                {
                    LocaleBinder = BGUtil.Create<BGDBTextBinderField.BGDBTextBinderFieldSpecial>("BansheeGz.BGDatabase.BGDBLocaleFieldBinder", false);
                }
                catch (Exception e)
                {
                    root.Error = "Can not create locale binder:" + e.Message;
                    return true;
                }

            var fieldBinder = LocaleBinder.Create(pointer);
            root.Add(fieldBinder);
            return true;
        }

        //========================================================================
        //                 GoogleSheets
        //========================================================================
        public static readonly HashSet<string> GoogleSheetsSpecialFieldTypeNames = new HashSet<string>
        {
            "BansheeGz.BGDatabase.BGFieldLocalizedAudioClip",
            "BansheeGz.BGDatabase.BGFieldLocalizedMaterial",
            "BansheeGz.BGDatabase.BGFieldLocalizedObject",
            "BansheeGz.BGDatabase.BGFieldLocalizedPrefab",
            "BansheeGz.BGDatabase.BGFieldLocalizedSprite",
            "BansheeGz.BGDatabase.BGFieldLocalizedString",
            "BansheeGz.BGDatabase.BGFieldLocalizedText",
            "BansheeGz.BGDatabase.BGFieldLocalizedTexture"
        };

        public static bool GoogleSheetsHasField(string type)
        {
            return GoogleSheetsSpecialFieldTypeNames.Contains(type);
        }

        public static bool SupportPartitioning(BGMetaEntity meta)
        {
            return !"Locale".Equals(meta.Name);
        }
        //========================================================================
        //                 TypeCode factories
        //========================================================================

        public static BGMetaTypeCodeFactory.BGMetaTypeCodeFactoryI LocalizationMetaFactory { get; }
        public static BGFieldTypeCodeFactory.BGFieldTypeCodeFactoryI LocalizationFieldFactory { get; }

        //========================================================================
        //                 Misc
        //========================================================================
        public static bool HasLocaleField(BGMetaEntity meta)
        {
            var localizedType = BGUtil.GetType("BansheeGz.BGDatabase.BGFieldLocalizedI");
            if (localizedType == null) return false;
            return HasLocaleField(meta, localizedType);
        }

        private static bool HasLocaleField(BGMetaEntity meta, Type localizedType)
        {
            if (meta.FindField(localizedType.IsInstanceOfType) != null) return true;
            var nestedFields = meta.FindFields(null, f => f is BGFieldNested);
            foreach (var field in nestedFields)
            {
                var nestedField = (BGFieldNested)field;
                if (HasLocaleField(nestedField.RelatedMeta, localizedType)) return true;
            }

            return false;
        }

        public static bool IsLocaleField(BGField field)
        {
            var localizedType = BGUtil.GetType("BansheeGz.BGDatabase.BGFieldLocalizationI");
            if (localizedType == null) return false;
            return localizedType.IsInstanceOfType(field);
        }
        public static bool IsLocaleField(Type fieldType)
        {
            var localizedType = BGUtil.GetType("BansheeGz.BGDatabase.BGFieldLocalizationI");
            if (localizedType == null) return false;
            return localizedType.IsAssignableFrom(fieldType);
        }

        public static bool HasLocalizationAddon(BGRepo repo) => repo.Addons.Has("BansheeGz.BGDatabase.BGAddonLocalization");
        public static bool IsLocalizationSettings(BGMetaEntity meta) => meta.GetType().FullName == "BansheeGz.BGDatabase.BGMetaLocalizationSettings";

        public static bool IsLocalesTable(BGMetaEntity meta) => meta is BGMetaNested nested && IsLocalizationSettings(nested.Owner);
    }
}