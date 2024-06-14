/*
<copyright file="BGSyncRelationResolverFieldMTMV.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace BansheeGz.BGDatabase
{
    public class BGSyncRelationResolverFieldMTMV : BGSyncRelationResolverFieldMT
    {
        private static readonly StringBuilder builder = new StringBuilder();

        private BGFieldManyRelationsMultiple relationField => (BGFieldManyRelationsMultiple)relation;

        public BGSyncRelationResolverFieldMTMV(List<BGSyncRowResolver> rowResolvers, BGFieldManyRelationsMultiple relation, BGRepo backupRepo) : base(rowResolvers, relation,
            backupRepo)
        {
        }

        protected override void ToDatabaseInternal(int index, string value)
        {
            var ids = value.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            if (ids.Length == 0) return;
            var list = new List<BGRowRef>();
            foreach (var idStr in ids)
            {
                var id = Resolve(idStr);
                list.Add(id);
            }
            relationField.SetStoredValue(index, list);
        }

        public override string ToExternalFormat(int index)
        {
            var ids = relationField.GetStoredValue(index);
            if (ids == null || ids.Count == 0) return null;

            try
            {
                builder.Length = 0;
                foreach (var rowRef in ids)
                {
                    var rowRefStr = Resolve(rowRef);
                    if (rowRefStr == null) continue;
                    if (builder.Length != 0) builder.Append('|');
                    builder.Append(rowRefStr);
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