/*
<copyright file="BGSyncRelationResolverFieldMT.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    public abstract class BGSyncRelationResolverFieldMT : BGSyncRelationResolver
    {
        protected readonly Dictionary<BGId, BGSyncRowResolver> metaId2rowResolver = new Dictionary<BGId, BGSyncRowResolver>();
        protected readonly Dictionary<string, BGSyncRowResolver> metaName2rowResolver = new Dictionary<string, BGSyncRowResolver>();
        protected readonly BGField relation;
        protected readonly BGRepo backupRepo;

        public BGSyncRelationResolverFieldMT(List<BGSyncRowResolver> rowResolvers, BGField relation, BGRepo backupRepo)
        {
            foreach (var rowResolver in rowResolvers)
            {
                metaId2rowResolver[rowResolver.MetaId] = rowResolver;
                metaName2rowResolver[rowResolver.MetaName] = rowResolver;
            }
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

        public abstract string ToExternalFormat(int index);

        protected string Resolve(BGRowRef value)
        {
            if (!metaId2rowResolver.TryGetValue(value.MetaId, out var resolver)) 
                throw new BGException("Can not find a resolver for $ field, using $ meta ID", relation.FullName, value.MetaId);
            var idValue = resolver.ToString(value.EntityId);
            if (string.IsNullOrEmpty(idValue)) throw new BGException("Entity ID value is empty, entity id $ , entity resolver=[$]", value.EntityId, resolver);
            return resolver.MetaName + "." + idValue;
        }

        protected BGRowRef Resolve(string idFieldString)
        {
            var dotIndex = idFieldString.IndexOf('.');
            if (dotIndex > 0 && dotIndex < idFieldString.Length - 1)
            {
                var tableName = idFieldString.Substring(0, dotIndex);
                if (metaName2rowResolver.TryGetValue(tableName, out var resolver))
                {
                    var entityReference = idFieldString.Substring(dotIndex + 1, idFieldString.Length - dotIndex - 1);
                    var rowRef = resolver.FromString(entityReference);
                    if (rowRef == null) throw new BGException("Can not resolve entity for $ field, using $ value", relation.FullName, idFieldString);
                    return rowRef;
                }
            }
            //no table name or unknown name- fallback
            foreach (var pair in metaName2rowResolver)
            {
                var rowResolver = pair.Value;
                var rowRef = rowResolver.FromString(idFieldString);
                if (rowRef != null) return rowRef;
            }
            throw new BGException("Can not resolve entity for $ field, using $ value", relation.FullName, idFieldString);
        }
    }
}