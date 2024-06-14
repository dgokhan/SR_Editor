using System;

namespace BansheeGz.BGDatabase
{
    public partial class BGAddonPartition
    {
        /// <summary>
        /// get partition file path for specified partition ID
        /// </summary>
        public static string[] GetPartitionPaths(BGId id) => new string[] { PartitionFilePathKey, ToFilePath(id) };

        /// <summary>
        /// Get partition table (DbPartition) for specified repo 
        /// </summary>
        public static BGMetaEntity GetPartitionMeta(BGRepo repo) => repo.GetMeta(PartitionMetaName);

        /// <summary>
        /// Create partition table (DbPartition) for specified repo 
        /// </summary>
        public static void CreatePartitionMeta(BGRepo repo) => new BGMetaRow(repo, PartitionMetaName).Addon = "Partition";        
                                                                                                                                  
        /// <summary>
        /// Create vertical partition tables  for specified repo 
        /// </summary>
        public static BGPartitionVerticalStructure CreatePartitionVerticalMetas(BGRepo repo) => BGPartitionVerticalStructure.Create(repo);

        /// <summary>
        /// Delete partition table (DbPartition) for specified repo 
        /// </summary>
        public static void DeletePartitionMeta(BGRepo repo) => repo.GetMeta(PartitionMetaName)?.Delete();       

        /// <summary>
        /// Delete vertical partition tables for specified repo 
        /// </summary>
        public static void DeletePartitionVerticalMetas(BGRepo repo) => BGPartitionVerticalStructure.Delete(repo);

        /// <summary>
        /// Get partition table (DbPartition) for specified repo 
        /// </summary>
        public static BGMetaEntity GetPartitionVerticalMeta(BGRepo repo) => repo.GetMeta(PartitionVerticalMetaName);


        /// <summary>
        /// Is specified table support partitioning 
        /// </summary>
        public static bool SupportPartitioningField(BGMetaEntity meta)
        {
            if (string.Equals(meta.Name, PartitionMetaName)) return false;
            if (string.Equals(meta.Name, PartitionVerticalMetaName)) return false;
            if (!meta.SupportPartitioningField) return false;
            if (!BGLocalizationUglyHacks.SupportPartitioning(meta)) return false;
            return true;
        }

        /// <summary>
        /// Merges source repo to target repo
        /// </summary>
        private static void Merge(BGRepo source, BGRepo target)
        {
            var settings = new BGMergeSettingsEntity()
            {
                //we do not need to update existing or remove orphaned
                AddMissing = true
            };
            foreach (var handler in LoadHandlers) handler.UpdateMergeSettings(settings);
            Merge(source, target, settings);
        }

        /// <summary>
        /// Merges source repo to target repo using provided merge settings
        /// </summary>
        public static void Merge(BGRepo source, BGRepo target, BGMergeSettingsEntity settings)
        {
            if (settings == null) throw new Exception("Settings can not be null!");
            new BGMergerEntity(null, source, target, settings).Merge();
        }

        /// <summary>
        /// Convert ID to safe file path representation
        /// </summary>
        public static string ToFilePath(BGId id) => id.ToString().Replace('/', '!');

        /// <summary>
        /// Is partitioning enabled for provided repo
        /// </summary>
        public static bool IsEnabled(BGRepo repo = null)
        {
            if (repo == null) repo = BGRepo.I;
            var addon = repo.Addons.Get<BGAddonPartition>();
            return addon != null && addon.Enabled;
        }
    }
}