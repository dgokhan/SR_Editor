/*
<copyright file="BGMetaPartitionModelLocalizationMainProvider.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// This is provider for localization tables in main repo!
    /// </summary>
    public class BGMetaPartitionModelLocalizationMainProvider : BGMetaPartitionModelProvider.BGMetaPartitionModelProviderDelegate
    {
        private static readonly BGMetaPartitionModelProvider provider = new BGMetaPartitionModelProvider();

        public BGMetaPartitionModelA Get(BGMetaEntity meta)
        {
            if (!(meta is BGMetaLocalization localization)) return null;
            var relations = BGMetaPartitionModelLocalized.GetInboundRelations(localization, provider.Get);
            if (relations.Count == 0) return null;

            return new BGMetaPartitionModelLocalization(relations);
        }
    }
}