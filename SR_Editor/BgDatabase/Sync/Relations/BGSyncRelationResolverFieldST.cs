/*
<copyright file="BGSyncRelationResolverFieldST.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    public abstract class BGSyncRelationResolverFieldST : BGSyncRelationResolver
    {
        protected readonly BGSyncRowResolver rowResolver;
        protected readonly BGField relation;
        protected readonly BGRepo backupRepo;
        protected BGSyncRelationResolverFieldST(BGSyncRowResolver rowResolver, BGField relation, BGRepo backupRepo)
        {
            this.rowResolver = rowResolver;
            this.relation = relation;
            this.backupRepo = backupRepo;
        }

        public void ToDatabase(int index, string value)
        {
            if (string.IsNullOrEmpty(value)) return;
            value = value.Trim();
            ToDatabaseInternal(index, value);
        }

        protected abstract void ToDatabaseInternal(int index, string value);


        public string ToExternalFormat(int index)
        {
            var dbValue = relation.ToString(index);
            if (string.IsNullOrEmpty(dbValue)) return null;
            return ToExternalFormatInternal(dbValue);
        }

        protected abstract string ToExternalFormatInternal(string dbValue);
    }
}