/*
<copyright file="BGSyncRelationResolverByIdSTSV.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


namespace BansheeGz.BGDatabase
{
    public class BGSyncRelationResolverByIdSTSV : BGSyncRelationResolverByIdST
    {
        public BGSyncRelationResolverByIdSTSV(BGField relation, BGRepo backUpRepo) : base( relation, backUpRepo)
        {
        }

        protected override string ToExternalFormatInternal(string value)
        {
            if (value.Length != 22) return value;

            var toEntityId = new BGId(value);
            return BGFieldRelationSingle.IdToString(toEntityId, backUpMeta[toEntityId]);
        }
    }
}