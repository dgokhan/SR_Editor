/*
<copyright file="BGSyncRelationResolverByIdMTSV.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    public class BGSyncRelationResolverByIdMTSV : BGSyncRelationResolverByIdMT
    {

        public BGSyncRelationResolverByIdMTSV(BGFieldManyRelationsSingle relation, BGRepo backUpRepo): base(relation, backUpRepo)
        {
        }

        protected override string ToExternalFormatInternal(string value)
        {
            var rowRef = BGFieldRelationMA<BGEntity, BGRowRef>.StringToRowRef(value);
            if (rowRef == null) return value;
            //since it's an export- using backUpRepo works fine
            return BGFieldRelationMA<BGEntity, BGRowRef>.RowRefToString(rowRef, backUpRepo);
        }
    }
    
}