/*
<copyright file="BGSyncRelationResolverByIdSTMV.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


using System;
using System.Text;

namespace BansheeGz.BGDatabase
{
    public class BGSyncRelationResolverByIdSTMV : BGSyncRelationResolverByIdST
    {
        private static readonly StringBuilder builder = new StringBuilder();

        public BGSyncRelationResolverByIdSTMV(BGFieldRelationMultiple relation, BGRepo backUpRepo) : base( relation, backUpRepo)
        {
        }

        protected override string ToExternalFormatInternal(string value)
        {
            var ids = value.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            if (ids.Length == 0) return value;
            try
            {
                builder.Length = 0;
                foreach (var idStr in ids)
                {
                    var id = BGFieldRelationSA<BGEntity, BGId>.IdFromString(idStr);
                    if (id.IsEmpty) return value;
                    if (builder.Length != 0) builder.Append('|');
                    builder.Append(BGFieldRelationMultiple.IdToString(id, backUpMeta[id]));
                }

                return builder.ToString();
            }
            catch
            {
                return value;
            }
            finally
            {
                builder.Length = 0;
            }
        }
    }
}