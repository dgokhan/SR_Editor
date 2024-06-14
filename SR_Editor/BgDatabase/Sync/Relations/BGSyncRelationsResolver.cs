/*
<copyright file="BGSyncRelationsResolver.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    public class BGSyncRelationsResolver
    {
        private readonly BGSyncRelationsConfig config;

        private readonly BGSyncIdConfig idConfig;
        private readonly BGLogger logger;
        private readonly bool printWarnings;
        
        //this is the repo, constructed from excel file
        private BGRepo repo;

        //this is the main repo
        private readonly BGRepo mainRepo;

        private readonly Dictionary<BGId, BGSyncRowResolver> metaId2Resolver = new Dictionary<BGId, BGSyncRowResolver>();

        public BGRepo Repo
        {
            // get => repo1;
            set => repo = value;
        }

        //do not remove just yet (remove it in version 1.8)
        public BGSyncRelationsResolver(BGSyncRelationsConfig config, BGSyncIdConfig idConfig, BGRepo mainRepo)
        {
            this.config = config;
            this.idConfig = idConfig;
            this.mainRepo = mainRepo;
        }
        
        public BGSyncRelationsResolver(BGSyncRelationsConfig config, BGSyncIdConfig idConfig, BGRepo mainRepo, BGLogger logger, bool printWarnings)
        {
            this.config = config;
            this.idConfig = idConfig;
            this.mainRepo = mainRepo;
            this.logger = logger;
            this.printWarnings = printWarnings;
        }

        public BGSyncRelationResolver GetResolver(BGField relation)
        {
            if (!(relation is BGAbstractRelationI)) throw new BGException("field $ is not relation", relation.FullName);
            if (config == null) return GetDefaultResolver(relation);
            BGSyncRelationResolver resolver = null;
            switch (relation)
            {
                case BGRelationI singleTableRelation:
                {
                    var metaId = singleTableRelation.ToId;
                    var rowResolver = GetRowResolver(metaId);
                    if (rowResolver is BGSyncRowResolverId) resolver = GetDefaultResolver(relation);
                    else
                    {
                        if (relation is BGFieldRelationMultiple relM) resolver = new BGSyncRelationResolverFieldSTMV(rowResolver, relM, mainRepo);
                        else resolver = new BGSyncRelationResolverFieldSTSV(rowResolver, relation, mainRepo);
                    }

                    break;
                }
                case BGManyTablesRelationI manyTableRelation:
                    var toIds = manyTableRelation.ToIds;
                    var resolvers = new List<BGSyncRowResolver>(toIds.Count);
                    foreach (var toId in toIds) resolvers.Add(GetRowResolver(toId));
                    switch (relation)
                    {
                        case BGFieldManyRelationsSingle relS:
                            resolver = new BGSyncRelationResolverFieldMTSV(resolvers, relS, mainRepo);
                            break;
                        case BGFieldManyRelationsMultiple relM:
                            resolver = new BGSyncRelationResolverFieldMTMV(resolvers, relM, mainRepo);
                            break;
                        default:
                            throw new Exception("Unknown relation type=" + relation.GetType().FullName);
                    }

                    break;
                default:
                    throw new Exception("Unknown relation type=" + relation.GetType().FullName);
            }

            return resolver;
        }


        private BGSyncRelationResolver GetDefaultResolver(BGField relation)
        {
            BGSyncRelationResolver resolver;
            switch (relation)
            {
                case BGFieldRelationMultiple relM:
                    resolver = new BGSyncRelationResolverByIdSTMV(relM, mainRepo);
                    break;
                case BGRelationI _:
                    resolver = new BGSyncRelationResolverByIdSTSV(relation, mainRepo);
                    break;
                case BGFieldManyRelationsMultiple rM:
                    resolver = new BGSyncRelationResolverByIdMTMV(rM, mainRepo);
                    break;
                case BGFieldManyRelationsSingle rS:
                    resolver = new BGSyncRelationResolverByIdMTSV(rS, mainRepo);
                    break;
                default:
                    throw new Exception("Unknown relation type " + relation.GetType().FullName);
            }

            return resolver;
        }

        private BGSyncRowResolver GetRowResolver(BGId metaId)
        {
            if (metaId2Resolver.TryGetValue(metaId, out var resolver)) return resolver;

            var metaConfig = config.GetMetaConfig(metaId);
            if (metaConfig == null)
            {
                switch (config.DefaultConfig)
                {
                    case BGSyncRelationsConfig.DefaultRelationConfigEnum.Name:
                        resolver = new BGSyncRowResolverField(GetMeta1(metaId), GetMeta2(metaId), GetNameFieldId(metaId), logger, printWarnings);
                        break;
                    case BGSyncRelationsConfig.DefaultRelationConfigEnum.IdConfig:
                        var idMetaConfig = idConfig?.GetMetaConfig(metaId);
                        if (idMetaConfig != null && idMetaConfig.configType == BGSyncIdConfig.IdConfigEnum.Field && HasField(metaId, idMetaConfig.FieldId))
                            resolver = new BGSyncRowResolverField(GetMeta1(metaId), GetMeta2(metaId), idMetaConfig.FieldId, logger, printWarnings);
                        break;
                }
            }
            else
            {
                if (metaConfig.configType == BGSyncRelationsConfig.RelationConfigEnum.Field && HasField(metaId, metaConfig.FieldId))
                    resolver = new BGSyncRowResolverField(GetMeta1(metaId), GetMeta2(metaId), metaConfig.FieldId, logger, printWarnings);
            }

            if (resolver == null) resolver = new BGSyncRowResolverId(metaId, mainRepo[metaId].Name);
            metaId2Resolver.Add(metaId, resolver);
            return resolver;
        }

        private bool HasField(BGId metaId, BGId fieldId)
        {
            var meta1 = GetMeta1(metaId);
            if (meta1 != null && meta1.HasField(fieldId)) return true;
            var meta2 = GetMeta2(metaId);
            if (meta2 != null && meta2.HasField(fieldId)) return true;
            return false;
        }

        private BGMetaEntity GetMeta1(BGId metaId) => repo?.GetMeta(metaId);
        private BGMetaEntity GetMeta2(BGId metaId) => mainRepo?.GetMeta(metaId);

        private BGId GetNameFieldId(BGId metaId)
        {
            BGId fieldId;
            var nameField = GetMeta1(metaId)?.NameField;
            if (nameField != null) fieldId = nameField.Id;
            else
            {
                nameField = GetMeta2(metaId)?.NameField;
                if (nameField != null) fieldId = nameField.Id;
                else throw new Exception("Unexpected error: both metas are null");
            }

            return fieldId;
        }
    }
}