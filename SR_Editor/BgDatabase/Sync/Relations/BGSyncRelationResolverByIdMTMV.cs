/*
<copyright file="BGSyncRelationResolverByIdMTMV.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace BansheeGz.BGDatabase
{
    public class BGSyncRelationResolverByIdMTMV : BGSyncRelationResolverByIdMT
    {
        private static readonly StringBuilder builder = new StringBuilder();
        // private static readonly List<BGRowRef> refs = new List<BGRowRef>();
        public BGSyncRelationResolverByIdMTMV(BGFieldManyRelationsMultiple relation, BGRepo backUpRepo) : base(relation, backUpRepo)
        {
        }

        protected override string ToExternalFormatInternal(string value)
        {
            var ids = value.Split(new[]{'|'}, StringSplitOptions.RemoveEmptyEntries);
            if (ids.Length == 0) return value;
            try
            {
                builder.Length = 0;
                foreach (var rowRefStr in ids)
                {
                    var rowRef = BGFieldRelationMA<BGEntity, BGRowRef>.StringToRowRef(rowRefStr);
                    if (rowRef == null) return value;
                    if (builder.Length != 0) builder.Append('|');
                    //since it's an export- using backUpRepo works fine
                    builder.Append(BGFieldRelationMA<BGEntity, BGRowRef>.RowRefToString(rowRef, backUpRepo).Replace("|", ""));
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