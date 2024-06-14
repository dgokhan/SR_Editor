/*
<copyright file="BGPartitionSaveContextLocalization.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// This is provider for loaded locale repo, not for main repo!
    /// </summary>
    public class BGMetaPartitionModelLocalizationProvider : BGMetaPartitionModelProvider
    {
        private readonly BGRepo mainRepo;

        public BGMetaPartitionModelLocalizationProvider(BGRepo mainRepo)
        {
            this.mainRepo = mainRepo;
        }

        public override BGMetaPartitionModelA Get(BGMetaEntity meta)
        {
            var mainMeta = mainRepo.GetMeta(meta.Id);
            switch (mainMeta)
            {
                case null:
                    return null;
                case BGMetaLocalizationSingleValue _:
                {
                    var model = base.Get(mainMeta);
                    if (!(model is BGMetaPartitionModelDefault modelDefault)) return null;
                    return new BGMetaPartitionModelLocale(meta, modelDefault);
                }
                case BGMetaLocalization localization:
                {
                    var relations = BGMetaPartitionModelLocalized.GetInboundRelations(localization, base.Get);
                    if (relations.Count == 0) return null;
                    return new BGMetaPartitionModelLocalized(meta, relations);
                }
                default:
                    return null;
            }
        }
    }
}