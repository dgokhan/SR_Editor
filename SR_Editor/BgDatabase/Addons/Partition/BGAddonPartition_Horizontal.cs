using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    public partial class BGAddonPartition
    {
        public const string PartitionMetaName = "DbPartition";
        public const string PartitionFieldName = "dbPartition";
        public const string PartitionFilePathKey = "p";

        /// <summary>
        /// Is horizontal partitioning enabled and has at least 1 partition
        /// </summary>
        public bool EnabledHorizontal
        {
            get
            {
                if (disableTemporarily || disableHorizontalTemporarily) return false;
                var meta = PartitionMeta;
                if (meta == null) return false;
                if (meta.CountEntities == 0) return false;
                return true;
            }
        }
        private bool disableHorizontalTemporarily;

        /// <summary>
        /// Disable horizontal addon temporarily 
        /// </summary>
        public bool DisableHorizontalTemporarily
        {
            get => disableHorizontalTemporarily;
            set
            {
                if (disableHorizontalTemporarily == value) return;
                disableHorizontalTemporarily = value;
                FireChange();
            }
        }
        
        /// <summary>
        /// Get partitions table (DbPartition)
        /// </summary>
        public BGMetaEntity PartitionMeta => Repo.GetMeta(PartitionMetaName);

        //  Get partitions table (DbPartition) and checks it for null or emptiness
        private BGMetaEntity PartitionMetaWithCheck
        {
            get
            {
                var meta = PartitionMeta;

                if (meta == null) throw new BGException("Can not find $ meta!", PartitionMetaName);
                if (meta.CountEntities == 0) throw new BGException("$ meta does not have any entity!", PartitionMetaName);

                // if (partitionIndex < 0 || partitionIndex >= meta.CountEntities)
                //     throw new BGException("Can not load a partition with index < 0! Valid index value range is  $-$", 0, meta.CountEntities - 1);
                return meta;
            }
        }
        
        /// <summary>
        /// Load all partitions
        /// </summary>
        public void LoadAll()
        {
            if (disableTemporarily) return;
            var meta = Repo.GetMeta(PartitionMetaName);
            if (meta == null) return;
            if (meta.CountEntities == 0) return;

            //load all partitions
            // var partitionMeta = Repo.GetMeta(BGAddonPartition.PartitionMetaName);
            meta.ForEachEntity(entity => LoadNoEvent(entity, null));
        }


        /// <summary>
        /// Loads the partition with specified name 
        /// </summary>
        public void Load(string partitionName, BGPartitionLoadRequest request = null)
        {
            var meta = PartitionMetaWithCheck;
            var entity = meta.GetEntity(partitionName);
            if (entity == null) throw new BGException("Can not get a partition with name $", partitionName);
            Load(entity.Index, request);
        }

        /// <summary>
        /// Loads the partition with specified ID 
        /// </summary>
        public void Load(BGId partitionId, BGPartitionLoadRequest request = null)
        {
            var meta = PartitionMetaWithCheck;
            var entity = meta.GetEntity(partitionId);
            if (entity == null) throw new BGException("Can not get a partition with Id $", partitionId);
            Load(entity.Index, request);
        }

        /// <summary>
        /// Loads the partition with specified index 
        /// </summary>
        public void Load(int partitionIndex, BGPartitionLoadRequest request = null)
        {
            if (disableTemporarily) return;

            var partitionEntity = GetPartitionEntity(partitionIndex);
            LoadNoEvent(partitionEntity, request);
            OnAfterLoad(partitionEntity);
        }

        //calls callbacks on partition load
        private static void OnAfterLoad(BGEntity partitionEntity)
        {
            foreach (var handler in LoadHandlers) handler.OnLoad(partitionEntity);
        }

        // load specified partition without firing events
        private void LoadNoEvent(BGEntity partitionEntity, BGPartitionLoadRequest request)
        {
            var loader = Repo.RepoLoader ?? BGRepo.DefaultRepoLoader;
            byte[] content = null;
            BGLoaderForRepo.LoadRequest loadRequest = null;
            if (loader != null)
            {
                loadRequest = new BGLoaderForRepo.LoadRequest(Repo.RepoAssetPath ?? BGRepo.DefaultRepoAssetPath, GetPartitionPaths(partitionEntity.Id));
                content = loader.Load(loadRequest);
            }

            if (content == null)
            {
                var details = loadRequest == null ? "Partition ID is " + partitionEntity.Id : "File path is " + loadRequest.ToPath(loader);
                throw new BGException("Can not load partition file for $ partition! $", partitionEntity.Name, details);
            }

            var repo = new BGRepo(content);

            loaded.Add(partitionEntity.Id);

            if (request?.fireEvents == true) Merge(repo, Repo);
            else
            {
                var snapshot = new BGPartitionSnapshot(Repo);
                Repo.Events.WithEventsDisabled(() => Merge(repo, Repo));
                snapshot.MarkKeysAndIndexesDirty();
                Repo.Events.FireAnyChange();
            }
        }

        /// <summary>
        /// Unload the partition with specified name
        /// </summary>
        public void Unload(string partitionName, BGPartitionUnLoadRequest request = null)
        {
            var meta = PartitionMetaWithCheck;
            var entity = meta.GetEntity(partitionName);
            if (entity == null) throw new BGException("Can not get a partition with name $", partitionName);
            Unload(entity.Index, request);
        }

        /// <summary>
        /// Unload the partition with specified ID
        /// </summary>
        public void Unload(BGId partitionId, BGPartitionUnLoadRequest request = null)
        {
            var meta = PartitionMetaWithCheck;
            var entity = meta.GetEntity(partitionId);
            if (entity == null) throw new BGException("Can not get a partition with Id $", partitionId);
            Unload(entity.Index, request);
        }

        /// <summary>
        /// Unload the partition with specified index
        /// </summary>
        public void Unload(int partitionIndex, BGPartitionUnLoadRequest request = null)
        {
            if (disableTemporarily) return;


            var partitionEntity = GetPartitionEntity(partitionIndex);

            if (request?.fireEvents == true) Unload(partitionEntity);
            else
            {
                var snapshot = new BGPartitionSnapshot(Repo);
                Repo.Events.WithEventsDisabled(() => Unload(partitionEntity));
                snapshot.MarkKeysAndIndexesDirty();
                Repo.Events.FireAnyChange();
            }
        }

        //unload specified partition
        private void Unload(BGEntity partitionEntity)
        {
            var toDelete = new List<BGEntity>();
            var provider = new BGMetaPartitionModelProvider();
            provider.ForEachRootModel(Repo, model =>
            {
                var meta = model.Meta;
                toDelete.Clear();
                meta.ForEachEntity(entity =>
                {
                    var pIndex = model.GetPartitionIndex(entity);
                    if (pIndex == null) return;
                    if (pIndex.Value != partitionEntity.Index) return;
                    toDelete.Add(entity);
                });
                meta.DeleteEntities(toDelete);
                toDelete.Clear();
            });
            foreach (var handler in LoadHandlers) handler.OnUnload(partitionEntity);

            loaded.Remove(partitionEntity.Id);
        }

        //get  partition with specified index
        private BGEntity GetPartitionEntity(int partitionIndex)
        {
            var meta = PartitionMetaWithCheck;

            if (partitionIndex < 0 || partitionIndex >= meta.CountEntities)
                throw new BGException("Can not get partition entity with index $. Valid range is $ - $", partitionIndex, 0, meta.CountEntities - 1);

            var partitionEntity = meta.GetEntity(partitionIndex);
            return partitionEntity;
        }


        /// <summary>
        /// Is partition with ID=entityId is loaded
        /// </summary>
        public bool IsLoaded(BGId entityId) => loaded.Contains(entityId);

        /// <summary>
        /// Iterate all loaded partitions
        /// </summary>
        public void ForEachLoaded(Action<BGEntity> action)
        {
            var meta = PartitionMeta;
            if (meta == null) return;
            foreach (var id in loaded)
            {
                var entity = meta.GetEntity(id);
                if (entity == null) continue;
                action(entity);
            }
        }
        //================================================================================================
        //                                              Nested
        //================================================================================================

        /// <summary>
        /// load partition request
        /// </summary>
        public class BGPartitionLoadRequest
        {
            public bool fireEvents;
        }

        /// <summary>
        /// unload partition request
        /// </summary>
        public class BGPartitionUnLoadRequest
        {
            public bool fireEvents;
        }
    }
}