/*
<copyright file="BGSyncRelationResolverFieldSTSV.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


using System;

namespace BansheeGz.BGDatabase
{
    public class BGSyncRelationResolverFieldSTSV : BGSyncRelationResolverFieldST
    {
        public BGSyncRelationResolverFieldSTSV(BGSyncRowResolver rowResolver, BGField relation, BGRepo backupRepo) : base(rowResolver, relation, backupRepo)
        {
        }

        protected override void ToDatabaseInternal(int index, string value)
        {
            var rowRef = rowResolver.FromString(value);
            if (rowRef == null) throw new BGException("Can not resolve referenced entity, using $ as a reference and [$] row resolver.", value, rowResolver);
            var idStr = BGFieldRelationSingle.IdToString(rowRef.EntityId, null);
            relation.FromString(index, idStr);
        }

        protected override string ToExternalFormatInternal(string dbValue)
        {
            var id = BGFieldRelationSA<BGEntity, BGId>.IdFromString(dbValue);
            var idStr = rowResolver.ToString(id);
            if (string.IsNullOrEmpty(idStr)) throw new BGException("Entity ID value is empty, entity id $ , entity resolver=[$]", id, rowResolver );
            return idStr;
        }
    }
}