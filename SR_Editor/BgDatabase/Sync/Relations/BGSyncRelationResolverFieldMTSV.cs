/*
<copyright file="BGSyncRelationResolverFieldMTSV.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    public class BGSyncRelationResolverFieldMTSV : BGSyncRelationResolverFieldMT
    {
        private BGFieldManyRelationsSingle relationField => (BGFieldManyRelationsSingle)relation;
        public BGSyncRelationResolverFieldMTSV(List<BGSyncRowResolver> rowResolvers, BGFieldManyRelationsSingle relation, BGRepo backupRepo) 
            : base(rowResolvers, relation, backupRepo)
        {
        }

        protected override void ToDatabaseInternal(int index, string value)
        {
            var rowRef = Resolve(value);
            // if (rowRef == null) throw new BGException("Can not resolve referenced entity for $ field, using $ as a reference.", relation.FullName, value);
            relation.FromString(index, BGFieldRelationMA<BGEntity, BGRowRef>.RowRefToString(rowRef));
        }

        public override string ToExternalFormat(int index)
        {
            var value = relationField.GetStoredValue(index);
            if (value == null) return null;
            return Resolve(value);
        }
    }
}