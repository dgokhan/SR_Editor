/*
<copyright file="BGSyncRelationResolverByIdST.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    public abstract class BGSyncRelationResolverByIdST : BGSyncRelationResolver
    {
        protected readonly BGField relation;
        protected readonly BGMetaEntity backUpMeta;
        protected BGSyncRelationResolverByIdST(BGField relation, BGRepo backUpRepo)
        {
            this.relation = relation;

            var rel = (BGRelationI)relation;
            if (rel.RelatedMeta == null && backUpRepo != null) backUpMeta = backUpRepo.GetMeta(rel.ToId);
        }

        public void ToDatabase(int index, string value) => BGUtil.FromString(relation, index, value);

        public string ToExternalFormat(int index)
        {
            if (backUpMeta == null) return BGUtil.ToString(relation, index);
            if (relation.CustomStringFormatSupported) return relation.ToCustomString(index);
            var value = relation.ToString(index);
            if (string.IsNullOrEmpty(value)) return null;
            return ToExternalFormatInternal(value);
        }

        protected abstract string ToExternalFormatInternal(string value);
    }
}