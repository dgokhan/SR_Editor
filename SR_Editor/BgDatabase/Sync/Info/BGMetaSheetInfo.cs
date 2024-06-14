/*
<copyright file="BGMetaSheetInfo.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    //================================================================================================
    //                                              Sheet with meta
    //================================================================================================

    /// <summary>
    /// NOT CURRENTLY USED. 
    /// </summary>
    public partial class BGMetaSheetInfo : BGSheetInfoA
    {
        public int IndexId = -1;
        public int IndexName = -1;
        public int IndexType = -1;
        public int IndexSystem = -1;
        public int IndexAddon = -1;
        public int IndexUniqueName = -1;
        public int IndexEmptyName = -1;
        public int IndexSingleton = -1;
        public int IndexComment = -1;
        public int IndexConfig = -1;

        public BGMetaSheetInfo(int sheetNumber) : base(sheetNumber)
        {
        }

        public override void Clear()
        {
            base.Clear();
            IndexId = -1;
            IndexName = -1;
            IndexType = -1;
            IndexSystem = -1;
            IndexAddon = -1;
            IndexUniqueName = -1;
            IndexEmptyName = -1;
            IndexSingleton = -1;
            IndexComment = -1;
            IndexConfig = -1;
        }

        public override object Clone()
        {
            var clone = new BGMetaSheetInfo(SheetNumber)
            {
                IndexId = IndexId,
                IndexName = IndexName,
                IndexType = IndexType,
                IndexSystem = IndexSystem,
                IndexAddon = IndexAddon,
                IndexUniqueName = IndexUniqueName,
                IndexEmptyName = IndexEmptyName,
                IndexSingleton = IndexSingleton,
                IndexComment = IndexComment,
                IndexConfig = IndexConfig
            };
            Clone(clone);
            return clone;
        }
    }
}