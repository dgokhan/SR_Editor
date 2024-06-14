/*
<copyright file="BGCalcUnitLocalizationDelegate.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    public class BGCalcUnitLocalizationDelegate : BGCalcUnitLocalizationI
    {
        private static Dictionary<Type, Type> FieldType2ValueType = new Dictionary<Type, Type>()
        {
            { typeof(BGFieldLocaleAudioClip), typeof(AudioClip) },
            { typeof(BGFieldLocaleMaterial), typeof(Material) },
            { typeof(BGFieldLocaleObject), typeof(Object) },
            { typeof(BGFieldLocalePrefab), typeof(GameObject) },
            { typeof(BGFieldLocaleSprite), typeof(Sprite) },
            { typeof(BGFieldLocaleString), typeof(string) },
            { typeof(BGFieldLocaleText), typeof(string) },
            { typeof(BGFieldLocaleTexture), typeof(Texture) }
        };

        public string CurrentLocale
        {
            get => BGAddonLocalization.DefaultRepoCurrentLocale;
            set => BGAddonLocalization.DefaultRepoCurrentLocale = value;
        }

        public Type GetValueType(BGMetaEntity meta)
        {
            var singleValueMeta = GetMeta(meta);
            if (FieldType2ValueType.TryGetValue(singleValueMeta.FieldType, out var valueType)) return valueType;
            return typeof(Object);
        }

        public object GetValue(BGMetaEntity meta, BGEntity entity)
        {
            var singleValueMeta = GetMeta(meta);
            var currentLocale = BGAddonLocalization.DefaultRepoCurrentLocale;
            if (currentLocale == null) throw new Exception("Can not get a value- current locale is null!");
            var field = singleValueMeta.GetField(currentLocale);
            if (field == null) throw new Exception($"Can not get a value- current locale field {currentLocale} is null!");
            return field.GetValue(entity.Index);
        }

        private static BGMetaLocalizationSingleValue GetMeta(BGMetaEntity meta)
        {
            if (!(meta is BGMetaLocalizationSingleValue singleValueMeta)) throw new Exception($"Meta {meta.Name} has the wrong type- should be BGMetaLocalizationSingleValue!");
            return singleValueMeta;
        }
    }
}