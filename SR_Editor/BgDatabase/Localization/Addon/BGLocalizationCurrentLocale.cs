/*
<copyright file="BGLocalizationCurrentLocale.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    public class BGLocalizationCurrentLocale
    {
        private readonly BGLocalizationStructure structure;

        public BGLocalizationCurrentLocale(BGLocalizationStructure structure) => this.structure = structure;

        public string CurrentLocale
        {
            get
            {
                var currentLocale = structure.CurrentLocaleField;
                if (currentLocale == null) return null;

                var result = currentLocale[0];
                if (result != null) return result.Name;

                var locales = structure.Locales;
                return locales.Count > 0 ? locales[0].Name : null;

                // rexx, it's waaay too slow!! This method is performance critical!
//                var dna = Dna;
//                var locale = dna.CurrentLocale.Get(dna.Settings.Meta[0]);
//                if (locale != null) return locale.Name;

//                var locales = Locales;
//                return locales.Count > 0 ? locales[0].Name : null;
            }
            set
            {
                if (!Application.isPlaying)
                    throw new Exception("Can not change current locale: this method is meant to be called only at runtime, " +
                                        "while application is playing (Application.isPlaying)");

                var isDefaultRepo = BGRepo.DefaultRepo(structure.Repo);
                SetCurrentLocale(value, isDefaultRepo, isDefaultRepo, isDefaultRepo, isDefaultRepo);
                if (isDefaultRepo) BGAddonLocalization.FireLocaleChangedEvent();
            }
        }

        public void SetCurrentLocale(string value, bool unloadCache, bool loadLocale, bool unloadFields, bool updateBinders)
        {
            if (value == null) throw new Exception("Can not change current locale: value is null");

            var locales = structure.Locales;
            if (locales.Count == 0) throw new Exception("Can not change current locale: no locales found ");

            var oldLocale = CurrentLocale;
            if (string.Equals(oldLocale, value)) return;

            BGEntity targetEntity = null;
            foreach (var locale in locales)
            {
                if (!string.Equals(locale.Name, value)) continue;

                targetEntity = locale;
                break;
            }

            if (targetEntity == null) throw new BGException("Can not change current locale: can not find 'Locale' entity with name $", value);

            var currentField = structure.CurrentLocaleField;
            if (currentField == null) throw new BGException("Can not change current locale: can not find $ field in $ meta", BGLocalizationDna.FieldCurrent, BGLocalizationDna.FieldLocales);

            //actual changing value
            if (currentField.Meta.CountEntities == 0) throw new BGException("Can not change current locale: $ meta does not have any record!", BGLocalizationDna.FieldLocales);

            if (unloadCache) BGLocalizationReposCache.Unload();
            if (loadLocale)
                BGLocalizationReposCache.Load(structure, new BGLocalizationLoadingContext(targetEntity.Name)
                {
                    ErrorIfNotFound = true,
                    PushFieldsConfig = new BGLocalizationLoadingContext.PushFieldValuesConfig()
                    {
                        LoadPartitionsMode = BGLocalizationLoadingContext.LoadPartitionsModeEnum.LoadLoaded
                    }
                });

            //set locale                
            currentField[0] = targetEntity;

            //old fields
            if (unloadFields)
                if (!string.IsNullOrEmpty(oldLocale))
                    BGMetaLocalizationA.ForEachMeta(BGRepo.I, meta =>
                    {
                        var field = meta.GetField(oldLocale, false);
                        if (!(field is BGFieldLocaleI localeI)) return;
                        localeI.DestroyStore();
                    });


            if (!updateBinders) return;

            BGInterfaceFinder.AddInterface(typeof(BGAddonLocalization.LocaleChangeReceiver));
            var listeners = BGInterfaceFinder.FindObjects<BGAddonLocalization.LocaleChangeReceiver>(true);
            if (listeners != null && listeners.Count > 0)
                foreach (var listener in listeners)
                    try
                    {
                        listener.LocaleChanged();
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }

            //need to invoke on disabled as well!
            // var binders = UnityEngine.Object.FindObjectsOfType<BGDataBinderGoA>();
#if !BG_SA
            var binders = Resources.FindObjectsOfTypeAll<BGDataBinderGoA>();
            if (binders != null && binders.Length > 0)
                foreach (var binder in binders)
                {
                    //skip prefab
                    if (BGUtil.IsPrefab(binder.gameObject)) continue;
                    
                    if (HasError(binder)) continue;
                    switch (binder)
                    {
                        case BGDataBinderFieldGo fieldGo:
                        {
                            if (fieldGo.IsUsingSpecialField)
                            {
                                if (fieldGo.FieldIdString.StartsWith(BGLocalizationUglyHacks.DataBinderLocale)) fieldGo.Bind();
                            }
                            else
                            {
                                var field = fieldGo.Field;
                                if (!(field is BGFieldLocalizedI)) continue;
                                // var meta = Repo[fieldBinder.MetaId];
                                // if (meta == null || !(meta.GetField(fieldBinder.FieldId, false) is BGFieldLocalizedI)) continue;
                                fieldGo.Bind();
                            }

                            break;
                        }
                        case BGDataBinderTemplateGo templateGo:
                        {
                            var binderRoot = templateGo.Binder;
                            if (binderRoot == null) continue;
                            var fields = binderRoot.Fields;
                            foreach (var field in fields)
                            {
                                if (BGLocalizationUglyHacks.DataBinderLocale.Equals(field.SpecialField))
                                {
                                    templateGo.Bind();
                                    break;
                                }

                                var meta = structure.Repo[field.MetaId];
                                if (meta == null || !(meta.GetField(field.FieldId, false) is BGFieldLocalizedI)) continue;
                                templateGo.Bind();
                                break;
                            }

                            break;
                        }
                        case BGDataBinderGraphGo graphGo:
                        {
                            try
                            {
                                graphGo.Bind();
                            }
                            catch (Exception e)
                            {
                                Debug.LogException(e);
                            }

                            break;
                        }
                        case BGDataBinderComponentsGo componentsGo:
                        {
                            try
                            {
                                componentsGo.Bind();
                            }
                            catch (Exception e)
                            {
                                Debug.LogException(e);
                            }

                            break;
                        }
                        case BGDataBinderBatchGo batchGo:
                        {
                            var fieldBinders = batchGo.FieldBinders;
                            if (fieldBinders != null)
                                foreach (var fieldBinder in fieldBinders)
                                {
                                    if (HasError(batchGo, fieldBinder)) continue;
                                    if (fieldBinder.IsUsingSpecialField)
                                    {
                                        if (fieldBinder.FieldIdString != null && fieldBinder.FieldIdString.StartsWith(BGLocalizationUglyHacks.DataBinderLocale))
                                            BGDataBinderGoA.LogError(batchGo, fieldBinder.Bind());
                                    }
                                    else
                                    {
                                        var meta = structure.Repo[BGId.Parse(fieldBinder.MetaIdString)];
                                        if (meta == null || !(meta.GetField(BGId.Parse(fieldBinder.FieldIdString), false) is BGFieldLocalizedI)) continue;
                                        BGDataBinderGoA.LogError(batchGo, fieldBinder.Bind());
                                    }
                                }

                            var templateBinders = batchGo.TemplateBinders;
                            if (templateBinders != null)
                                foreach (var templateBinder in templateBinders)
                                {
                                    var binderRoot = templateBinder.Binder;
                                    if (binderRoot == null) continue;
                                    var fields = binderRoot.Fields;
                                    foreach (var field in fields)
                                    {
                                        if (BGLocalizationUglyHacks.DataBinderLocale.Equals(field.SpecialField))
                                        {
                                            BGDataBinderGoA.LogError(batchGo, templateBinder.Bind());
                                            break;
                                        }

                                        var meta = structure.Repo[field.MetaId];
                                        if (meta == null || !(meta.GetField(field.FieldId, false) is BGFieldLocalizedI)) continue;
                                        BGDataBinderGoA.LogError(batchGo, templateBinder.Bind());
                                        break;
                                    }
                                }

                            var graphBinders = batchGo.GraphBinders;
                            if (graphBinders != null)
                                foreach (var graphBinder in graphBinders)
                                    try
                                    {
                                        graphBinder.Bind();
                                    }
                                    catch (Exception e)
                                    {
                                        Debug.LogException(e);
                                    }

                            break;
                        }
                    }
                }
#endif
        }

        private bool HasError(BGDataBinderGoA binder)
        {
            if (binder.Error == null) return false;
            BGDataBinderGoA.LogError(binder, binder.Error);
            return true;
        }

        private bool HasError(BGDataBinderBatchGo batchBinder, BGDBA binder)
        {
            if (binder.Error == null) return false;
            BGDataBinderGoA.LogError(batchBinder, binder.Error);
            return true;
        }
    }
}