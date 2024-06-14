/*
<copyright file="BGSyncRelationResolverByIdMT.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    public abstract class BGSyncRelationResolverByIdMT : BGSyncRelationResolver
    {
        protected readonly BGField relation;
        protected readonly BGRepo backUpRepo;
        // private readonly Dictionary<BGId, BGMetaEntity> metaId2Meta;
        protected bool someMetaIsMissing;
        protected BGSyncRelationResolverByIdMT(BGField relation, BGRepo backUpRepo)
        {
            this.relation = relation;
            this.backUpRepo = backUpRepo;
            var rel = (BGManyTablesRelationI)relation;
            var relToIds = rel.ToIds;
            someMetaIsMissing = relToIds.Count != rel.RelatedMetas.Count; 
        }

        public void ToDatabase(int index, string value) => BGUtil.FromString(relation, index, value);

        public string ToExternalFormat(int index)
        {
            if (!someMetaIsMissing) return BGUtil.ToString(relation, index);
            if (relation.CustomStringFormatSupported) return relation.ToCustomString(index);
            var value = relation.ToString(index);
            if (string.IsNullOrEmpty(value)) return null;
            return ToExternalFormatInternal(value);
        }

        protected abstract string ToExternalFormatInternal(string value);
    }
}