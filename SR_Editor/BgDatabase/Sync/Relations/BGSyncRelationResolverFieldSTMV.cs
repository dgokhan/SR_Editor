/*
<copyright file="BGSyncRelationResolverFieldSTMV.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


using System;
using System.Text;

namespace BansheeGz.BGDatabase
{
    public class BGSyncRelationResolverFieldSTMV : BGSyncRelationResolverFieldST
    {
        private static readonly StringBuilder builder = new StringBuilder();

        public BGSyncRelationResolverFieldSTMV(BGSyncRowResolver rowResolver, BGFieldRelationMultiple relation, BGRepo backupRepo) : base(rowResolver, relation, backupRepo)
        {
        }

        protected override void ToDatabaseInternal(int index, string value)
        {
            var ids = value.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            if (ids.Length == 0) return;
            builder.Length = 0;
            try
            {
                foreach (var rowRefStr in ids)
                {
                    var rowRef = rowResolver.FromString(rowRefStr);
                    if (rowRef == null) throw new BGException("Can not resolve referenced entity, using $ as a reference and [$] row resolver.", rowRefStr, rowResolver);
                    if (builder.Length != 0) builder.Append('|');
                    builder.Append(rowRef.EntityId);
                }

                relation.FromString(index, builder.ToString());
            }
            finally
            {
                builder.Length = 0;
            }
        }

        
        protected override string ToExternalFormatInternal(string value)
        {
            var ids = value.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            if (ids.Length == 0) return value;
            try
            {
                builder.Length = 0;
                foreach (var rowRefStr in ids)
                {
                    var entityId = BGFieldRelationSA<BGEntity, BGId>.IdFromString(rowRefStr);
                    if (entityId.IsEmpty) throw new BGException("Can not convert string value=[$] to row ID", rowRefStr);
                    var idStr = rowResolver.ToString(entityId);
                    if (string.IsNullOrEmpty(idStr)) throw new BGException("Entity ID value is empty, entity id $ , entity resolver=[$]", entityId, rowResolver);
                    if (builder.Length != 0) builder.Append('|');
                    builder.Append(idStr);
                }

                return builder.ToString();
            }
            finally
            {
                builder.Length = 0;
            }
        }
    }
}