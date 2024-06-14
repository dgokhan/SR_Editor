/*
<copyright file="BGRepoDeltaDeleted.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    //wrapper class for matching members
    internal class BGRepoDeltaDeleted
    {
        private readonly Dictionary<BGId, List<BGId>> metaId2EntityIds = new Dictionary<BGId, List<BGId>>();

        //================================================================================================
        //                                              Methods
        //================================================================================================
        /// <summary>
        /// Find any deleted members and store them in internal state
        /// </summary>
        public void Match(BGRepo repo, BGRepo targetRepo)
        {
            metaId2EntityIds.Clear();

            BGRepoDeltaUtils.ForEachMatchingMeta(repo, targetRepo, (meta, targetMeta) =>
            {
                if (meta.CountEntities == 0) return;

                var deletedIds = BGRepoDeltaUtils.Except(meta, targetMeta);
                if (deletedIds.Count == 0) return;

                metaId2EntityIds[meta.Id] = deletedIds;
            });
        }

        /// <summary>
        /// Apply the changes to the repo
        /// </summary>
        public void ApplyTo(BGRepo repo, BGModdingRepoProtection repoProtection)
        {
            foreach (var metaId2EntityId in metaId2EntityIds)
            {
                var targetMeta = repo[metaId2EntityId.Key];
                if (targetMeta == null || metaId2EntityId.Value.Count == 0) continue;

                var set = new HashSet<BGEntity>();
                foreach (var entityId in metaId2EntityId.Value)
                {
                    var entity = targetMeta.GetEntity(entityId);
                    if (entity == null) continue;
                    if (repoProtection != null && repoProtection.IsDeleteDisabled(targetMeta.Id, entity.Id)) continue;
                    set.Add(entity);
                }

                if (set.Count == 0) continue;
                targetMeta.DeleteEntities(set);
            }
        }

        //================================================================================================
        //                                              Binary
        //================================================================================================
        private const int LastVersion = 1;

        public void ToBinary(BGBinaryWriter builder)
        {
            builder.AddInt(LastVersion);

            builder.AddArray(() =>
            {
                foreach (var metaId2EntityId in metaId2EntityIds)
                {
                    builder.AddId(metaId2EntityId.Key);
                    builder.AddArray(() =>
                    {
                        foreach (var entityId in metaId2EntityId.Value) builder.AddId(entityId);
                    }, metaId2EntityId.Value.Count);
                }
            }, metaId2EntityIds.Count);
        }

        public void FromBinary(BGBinaryReader reader)
        {
            metaId2EntityIds.Clear();


            var version = reader.ReadInt();
            switch (version)
            {
                case 1:
                {
                    reader.ReadArray(() =>
                    {
                        var metaId = reader.ReadId();
                        var entityIdList = new List<BGId>();
                        metaId2EntityIds[metaId] = entityIdList;
                        reader.ReadArray(() =>
                        {
                            entityIdList.Add(reader.ReadId());
                        });
                    });
                    break;
                }
                default:
                {
                    throw new BGException("Can not read repo delta deleted from binary array: unsupported version $", version);
                }
            }
        }
    }
}