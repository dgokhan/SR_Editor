/*
<copyright file="BGMetaPartitionModelLocale.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// for loaded locale repo localization-single meta!
    /// </summary>
    public class BGMetaPartitionModelLocale : BGMetaPartitionModelA
    {
        private readonly BGMetaPartitionModelDefault mainModel;

        //mainModel- are from main repo!
        public BGMetaPartitionModelLocale(BGMetaEntity meta, BGMetaPartitionModelDefault mainModel) : base(meta)
        {
            this.mainModel = mainModel;
        }

        public override int? GetPartitionIndex(BGEntity entity)
        {
            var mainEntity = mainModel.Meta.GetEntity(entity.Id);
            if (mainEntity == null) return null;
            return mainModel.GetPartitionIndex(mainEntity);
        }
    }
}