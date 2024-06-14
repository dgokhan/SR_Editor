using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    public partial class BGAddonPartition
    {
        public const string PartitionVerticalMetaName = "DbPartitionVertical";
        public const string PartitionVerticalNestedField = "DbPartitionVerticalMetas";
        public const string PartitionVerticalMetaRefField = "metaRef";

        public const string PartitionVerticalFilePathKey = "v";

        private bool disableVerticalTemporarily;

        /// <summary>
        /// Disable horizontal addon temporarily 
        /// </summary>
        public bool DisableVerticalTemporarily
        {
            get => disableVerticalTemporarily;
            set
            {
                if (disableVerticalTemporarily == value) return;
                disableVerticalTemporarily = value;
                FireChange();
            }
        }

        /// <summary>
        /// Is vertical partitioning enabled and has at least 1 partition
        /// </summary>
        public bool EnabledVertical
        {
            get
            {
                if (disableTemporarily || disableVerticalTemporarily) return false;
                var meta = PartitionVerticalMeta;
                if (meta == null) return false;
                if (meta.CountEntities == 0) return false;
                return true;
            }
        }

        /// <summary>
        /// Get vertical partitions table (DbPartitionVertical)
        /// </summary>
        public BGMetaEntity PartitionVerticalMeta => Repo.GetMeta(PartitionVerticalMetaName);


        private void LoadVertical()
        {
            if (!EnabledVertical) return;
            var structure = new BGPartitionVerticalStructure(Repo);
            var loader = Repo.RepoLoader ?? BGRepo.DefaultRepoLoader;
            structure.ForEachPartition(partition =>
            {
                var metas = structure.GetMetas(partition);

                if (metas == null || metas.Count == 0) return;

                byte[] content = null;
                BGLoaderForRepo.LoadRequest loadRequest = null;
                if (loader != null)
                {
                    loadRequest = new BGLoaderForRepo.LoadRequest(Repo.RepoAssetPath ?? BGRepo.DefaultRepoAssetPath, new string[] { PartitionVerticalFilePathKey, ToFilePath(partition.Id) });
                    content = loader.Load(loadRequest);
                }

                if (content == null)
                {
                    var details = loadRequest == null ? "Partition name is " + partition.Name : "File path is " + loadRequest.ToPath(loader);
                    throw new BGException("Can not load vertical partition file for $ partition! $", partition.Name, details);
                }

                Merge(new BGRepo(content), Repo, new BGMergeSettingsEntity { AddMissing = true });
            });
        }

        public static bool IsSupportedForVerticalPartitioning(BGMetaEntity meta)
        {
            if (!(meta is BGMetaRow)) return false;
            if (!string.IsNullOrEmpty(meta.Addon)) return false;
            return true;
        }
    }
}