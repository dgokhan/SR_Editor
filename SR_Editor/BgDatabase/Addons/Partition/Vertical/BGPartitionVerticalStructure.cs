using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    public class BGPartitionVerticalStructure
    {
        private readonly BGMetaEntity partitionMeta;
        private readonly BGFieldNested fieldNestedTables;
        private readonly BGFieldPartitionMetaReference fieldRef;

        public BGMetaEntity PartitionMeta => partitionMeta;

        public BGFieldNested FieldNestedTables => fieldNestedTables;

        public BGFieldPartitionMetaReference FieldRef => fieldRef;

        public BGPartitionVerticalStructure(BGRepo repo)
        {
            partitionMeta = repo.GetMeta(BGAddonPartition.PartitionVerticalMetaName);
            if (partitionMeta == null) throw new Exception($"Meta [{BGAddonPartition.PartitionVerticalMetaName}] not found");
            fieldNestedTables = (BGFieldNested)partitionMeta.GetField(BGAddonPartition.PartitionVerticalNestedField, false);
            if (fieldNestedTables == null) throw new Exception($"Field [{BGAddonPartition.PartitionVerticalNestedField}] not found");
            fieldRef = (BGFieldPartitionMetaReference)fieldNestedTables.NestedMeta.GetField(BGAddonPartition.PartitionVerticalMetaRefField, false);
            if (fieldRef == null) throw new Exception($"Field [{BGAddonPartition.PartitionVerticalMetaRefField}] not found");
        }

        public static BGPartitionVerticalStructure Create(BGRepo repo)
        {
            var meta = new BGMetaRow(repo, BGAddonPartition.PartitionVerticalMetaName)
            {
                Addon = "Partition"
            };
            var nestedField = new BGFieldNested(meta, BGAddonPartition.PartitionVerticalNestedField)
            {
                Addon = "Partition",
                NestedMeta =
                {
                    Addon = "Partition",
                    EmptyName = true
                }
            };
            new BGFieldPartitionMetaReference(nestedField.NestedMeta, BGAddonPartition.PartitionVerticalMetaRefField)
            {
                Addon = "Partition",
            };
            return new BGPartitionVerticalStructure(repo);
        }

        public void ForEachPartition(Action<BGEntity> action)
        {
            var count = partitionMeta.CountEntities;
            for (var i = 0; i < count; i++) action(partitionMeta.GetEntity(i));
        }

        public List<BGMetaEntity> GetMetas(BGEntity partition)
        {
            var metas = new List<BGMetaEntity>();
            ForEachMeta(partition, m => metas.Add(m));
            return metas;
        }

        public void ForEachMeta(BGEntity partition, Action<BGMetaEntity> action)
        {
            var tables = fieldNestedTables[partition.Index];
            if (tables == null || tables.Count == 0) return;
            for (var j = 0; j < tables.Count; j++)
            {
                var refMeta = fieldRef[tables[j].Index];
                
                if(!BGAddonPartition.IsSupportedForVerticalPartitioning(refMeta)) continue;
                action(refMeta);
            }
        }

        public BGEntity AddPartition(string name)
        {
            var entity = partitionMeta.NewEntity();
            entity.Name = name;
            return entity;
        }

        public BGEntity AddTable(BGEntity partition, BGMetaEntity meta)
        {
            var entity = fieldNestedTables.NestedMeta.NewEntity(partition);
            fieldRef[entity.Index] = meta;
            return entity;
        }

        public static void Delete(BGRepo repo) => repo.GetMeta(BGAddonPartition.PartitionVerticalMetaName)?.Delete();
    }
}