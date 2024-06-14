/*
<copyright file="BGLocalizationSaveManager.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    public static class BGLocalizationSaveManager
    {
        public static Dictionary<string, BGRepo> Save(BGRepo repo = null)
        {
            if (repo == null || BGRepo.DefaultRepo(repo)) repo = BGRepo.I;
            var structure = new BGLocalizationStructure(repo);

            var locales = structure.Locales;
            var result = new Dictionary<string, BGRepo>(locales.Count);
            if (locales.Count == 0) return result;

            foreach (var locale in locales)
            {
                var delegateRepo = new BGRepo();

                BGMetaLocalizationA.ForEachMeta(structure.Repo, meta =>
                {
                    var field = meta.GetField(locale.Name);
                    var fieldDelegate = Sync(delegateRepo, field);
                    var metaDelegate = fieldDelegate.Meta;
                    //sync entities
                    meta.ForEachEntity(entity =>
                    {
                        metaDelegate.NewEntity(entity.Id);

                        //sync values
                        fieldDelegate.CopyValue(field, entity.Id, entity.Index, entity.Id);
                    });
                });

                result[locale.Name] = delegateRepo;
            }

            return result;
        }

        public static BGField Sync(BGRepo delegateRepo, BGField field)
        {
            var meta = field.Meta;

            //sync meta
            var metaDelegate = BGMetaEntity.Create(delegateRepo, typeof(BGMetaRow).FullName, meta.Id, meta.Name, (string)null, true, null, false, false, true);

            //sync field
            var fieldDelegate = BGField.Create(metaDelegate, BGLocaleFieldAttribute.Get(field.GetType()).DelegateFieldType.FullName, field.Id, field.Name,
                (string)null, true, null, null, false);
            return fieldDelegate;
        }
    }
}