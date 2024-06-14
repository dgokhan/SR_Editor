/*
<copyright file="BGDBLocaleFieldValueProvider.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    public class BGDBLocaleFieldValueProvider : BGDBField.FieldValueProvider
    {
        private static Dictionary<Type, Type> Field2Type = new Dictionary<Type, Type>
        {
            { typeof(BGFieldLocaleString), typeof(string) },
            { typeof(BGFieldLocaleText), typeof(string) },
            { typeof(BGFieldLocaleMaterial), typeof(Material) },
            { typeof(BGFieldLocaleObject), typeof(Object) },
            { typeof(BGFieldLocalePrefab), typeof(GameObject) },
            { typeof(BGFieldLocaleSprite), typeof(Sprite) },
            { typeof(BGFieldLocaleTexture), typeof(Texture) },
            { typeof(BGFieldLocaleAudioClip), typeof(AudioClip) }
        };

        public object GetValue(BGEntity entity)
        {
            var repo = entity.Repo;
            // var addon = repo.Addons.Get<BGAddonLocalization>();
            // if (addon == null) return null;
            // var locale = addon.CurrentLocale;
            var locale = BGAddonLocalization.DefaultRepoCurrentLocale;
            if (string.IsNullOrEmpty(locale)) return null;
            var field = entity.Meta.GetField(locale, false);
            return field?.GetValue(entity.Id);
        }

        public BGDBField.FieldValueProvider Create() => new BGDBLocaleFieldValueProvider();

        public Type GetValueType(BGMetaEntity meta)
        {
            if (!(meta is BGMetaLocalizationSingleValue singleValue)) return null;
            return GetValueTypeByField(singleValue);
        }

        public static Type GetValueTypeByField(BGMetaLocalizationA meta) => BGUtil.Get(Field2Type, meta.FieldType);
    }
}