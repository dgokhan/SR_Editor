using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    public class BGPartitionSaveVerticalProvider
    {
        private readonly BGRepo referenceRepo;
        private readonly BGAddonPartition partitionAddon;
        private readonly Dictionary<BGId, Tuple<BGId, BGRepo>> metaId2Repo = new Dictionary<BGId, Tuple<BGId, BGRepo>>();
        private readonly List<Tuple<BGId, BGRepo>> repos = new List<Tuple<BGId, BGRepo>>();

        public BGPartitionSaveVerticalProvider(BGRepo referenceRepo, BGAddonPartition partitionAddon)
        {
            this.referenceRepo = referenceRepo;
            this.partitionAddon = partitionAddon;

            var structure = new BGPartitionVerticalStructure(referenceRepo);
            structure.ForEachPartition(partition =>
            {
                var tables = structure.GetMetas(partition);
                if (tables == null || tables.Count == 0) return;
                var repo = new BGRepo();
                foreach (var referenceMeta in tables) ProcessMeta(referenceMeta, repo, partition.Id);

                if (repo.CountMeta > 0) repos.Add(Tuple.Create(partition.Id, repo));
            });
        }

        private void ProcessMeta(BGMetaEntity referenceMeta, BGRepo repo, BGId partitionId)
        {
            var clonedMeta = referenceMeta.CloneTo(repo, null, null, false);
            metaId2Repo[clonedMeta.Id] = Tuple.Create(partitionId, repo);
            referenceMeta.ForEachField(field =>
            {
                var nestedField = (BGFieldNested)field;
                var referenceNested = nestedField.NestedMeta;
                ProcessMeta(referenceNested, repo, partitionId);
            }, field => field is BGFieldNested);

        }

        public void ForEachRepo(Action<BGId, BGRepo> action)
        {
            foreach (var (id, repo) in repos) action(id, repo);
        }

        public BGMetaEntity GetMeta(BGId metaId)
        {
            if (!metaId2Repo.TryGetValue(metaId, out var tuple)) return null;
            return tuple.Item2.GetMeta(metaId);
        }
    }
}