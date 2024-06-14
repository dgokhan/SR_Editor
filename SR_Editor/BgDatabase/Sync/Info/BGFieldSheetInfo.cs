/*
<copyright file="BGFieldSheetInfo.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    //================================================================================================
    //                                              Sheet with field
    //================================================================================================

    /// <summary>
    /// NOT CURRENTLY USED. 
    /// </summary>
    public partial class BGFieldSheetInfo : BGSheetInfoA
    {
        public int IndexId = -1;
        public int IndexName = -1;
        public int IndexMetaId = -1;
        public int IndexType = -1;
        public int IndexSystem = -1;
        public int IndexAddon = -1;
        public int IndexDefaultValue = -1;
        public int IndexRequired = -1;
        public int IndexConfig = -1;

        public BGFieldSheetInfo(int sheetNumber) : base(sheetNumber)
        {
        }

        public override void Clear()
        {
            base.Clear();
            IndexId = -1;
            IndexName = -1;
            IndexMetaId = -1;
            IndexType = -1;
            IndexSystem = -1;
            IndexAddon = -1;
            IndexDefaultValue = -1;
            IndexRequired = -1;
            IndexConfig = -1;
        }

        public override object Clone()
        {
            var clone = new BGFieldSheetInfo(SheetNumber)
            {
                IndexId = IndexId,
                IndexName = IndexName,
                IndexMetaId = IndexMetaId,
                IndexType = IndexType,
                IndexSystem = IndexSystem,
                IndexAddon = IndexAddon,
                IndexDefaultValue = IndexDefaultValue,
                IndexRequired = IndexRequired,
                IndexConfig = IndexConfig
            };
            Clone(clone);
            return clone;
        }
    }
}