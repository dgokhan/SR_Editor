/*
<copyright file="BGSyncA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// abstract synchronization job
    /// </summary>
    public partial class BGSyncA
    {
        protected readonly BGLogger Logger;
        protected readonly BGRepo MainRepo;
        protected readonly BGMergeSettingsEntity EntitySettings;
        protected readonly BGMergeSettingsMeta MetaSettings;
        protected readonly BGSyncNameMapConfig NameMapConfig;
        protected readonly BGSyncIdConfig IdConfig;
        protected readonly BGSyncRelationsConfig RelationsConfig;
        protected readonly bool PrintWarnings;

        //do not remove it!! it's used by legacy code
        protected BGSyncA(BGLogger logger, BGRepo mainRepo, BGMergeSettingsEntity entitySettings, BGMergeSettingsMeta metaSettings) : this(logger, mainRepo, entitySettings, metaSettings, null)
        {
        }

        //do not remove it!! it's used by legacy code
        protected BGSyncA(BGLogger logger, BGRepo mainRepo, BGMergeSettingsEntity entitySettings, BGMergeSettingsMeta metaSettings, BGSyncNameMapConfig nameMapConfig) 
            : this(logger, mainRepo, entitySettings, metaSettings, nameMapConfig, null)
        {
        }

        protected BGSyncA(BGLogger logger, BGRepo mainRepo, BGMergeSettingsEntity entitySettings, BGMergeSettingsMeta metaSettings, BGSyncNameMapConfig nameMapConfig, BGSyncIdConfig idConfig)
            : this(logger, mainRepo, entitySettings, metaSettings, nameMapConfig, idConfig, null, false)
        {
            
        }
        protected BGSyncA(BGLogger logger, BGRepo mainRepo, BGMergeSettingsEntity entitySettings, BGMergeSettingsMeta metaSettings, BGSyncNameMapConfig nameMapConfig, BGSyncIdConfig idConfig, 
            BGSyncRelationsConfig relationsConfig, bool printWarnings)
        {
            Logger = logger ?? new BGLogger(false);
            MainRepo = mainRepo;
            EntitySettings = entitySettings;
            MetaSettings = metaSettings;
            NameMapConfig = nameMapConfig;
            IdConfig = idConfig;
            RelationsConfig = relationsConfig;
            PrintWarnings = printWarnings;
        }


        /// <summary>
        /// Run export job
        /// </summary>
        protected void Export(bool forceTransferMode, Action<BGRepo> setup, Func<BGRepo, bool, BGRepo> read, Action<bool, BGRepo, BGRepo> write)
        {
            var repo = EntitySettings.NewRepo(MainRepo, false);
            setup?.Invoke(repo);
            if (forceTransferMode || EntitySettings.Mode == BGMergeModeEnum.Transfer && (MetaSettings.Mode == BGMergeModeEnum.Transfer || !MetaSettings.HasAny))
            {
                //================================= full rewrite
                //entity
                new BGMergerEntity(Logger, new BGRepo(MainRepo, true), repo, EntitySettings).Merge();

                //meta
                BGRepo metaRepo = null;
                if (MetaSettings.HasAny)
                {
                    metaRepo = new BGRepo();
                    new BGMergerMeta(Logger, new BGRepo(MainRepo), metaRepo, MetaSettings).Merge();
                }

                write(true, repo, metaRepo);
            }
            else
            {
                //================================= merge
                //entity
                var mergeMeta = MetaSettings.HasAny;
                BGRepo metaRepo = null;
                Logger.Section("Reading repo", () => metaRepo = read(repo, mergeMeta));
                if (mergeMeta)
                {
                    if (MetaSettings.Mode == BGMergeModeEnum.Merge && metaRepo != null) new BGMergerMeta(Logger, MainRepo, metaRepo, MetaSettings).Merge();
                    else metaRepo = MainRepo;
                }

                new BGMergerEntity(Logger, MainRepo, repo, EntitySettings).Merge();

                write(false, repo, metaRepo);
            }
        }

        /// <summary>
        /// Run import job
        /// </summary>
        public void Import(bool updateNewIds, bool transferRowsOrder, Action<BGRepo> setup, Func<BGRepo, BGRepo> readMeta, Action<BGRepo> readEntity,
            Action write, Func<BGRepo, BGBookInfo> readerInfo)
        {
            MainRepo.Transaction(() =>
            {
                var repo = EntitySettings.Mode == BGMergeModeEnum.Transfer ? new BGRepo(MainRepo) : EntitySettings.NewRepo(MainRepo, false);

                setup?.Invoke(repo);

                var mergeMeta = MetaSettings.HasAny;
                if (mergeMeta)
                {
                    //---- meta
                    BGRepo metaRepo = null;
                    Logger.Section("Reading meta", () =>
                    {
                        metaRepo = readMeta(repo);
                        Logger.AppendLine(metaRepo == null ? "Can not read meta due to the errors above." : "Meta read ok");
                    });
                    if (metaRepo != null) new BGMergerMeta(Logger, metaRepo, MainRepo, MetaSettings).Merge();
                }

                //---- entities
                Logger.Section("Reading entities", () => readEntity(repo));

                if (updateNewIds && write != null) write();

                if (EntitySettings.Mode == BGMergeModeEnum.Transfer)
                {
                    //override
                    MainRepo.Merge(repo);
                    MainRepo.Addons.ForEachAddon(addon => addon.OnTransfer(repo));
                }
                else
                {
                    //merge
                    //we should exclude non existing metas and fields from settings
                    var settingsClone = (BGMergeSettingsEntity)EntitySettings.Clone();
                    settingsClone.RemoveNotExistent(repo, readerInfo(repo));
                    new BGMergerEntity(Logger, repo, MainRepo, settingsClone).Merge();

                    //sort
                    if (transferRowsOrder)
                        repo.ForEachMeta(meta =>
                        {
                            if (!settingsClone.IsMetaIncluded(meta.Id)) return;
                            var targetMeta = MainRepo.GetMeta(meta.Id);
                            if (targetMeta == null) return;

                            var order = new BGRowsOrder(Logger, meta, (index1, index2) => targetMeta.SwapEntities(index1, index2));
                            targetMeta.ForEachEntity(entity =>
                            {
                                var sourceEntity = meta.GetEntity(entity.Id);
                                if (sourceEntity == null) return;
                                order.Add(new BGRowsOrder.EntityOrderInfo(sourceEntity, entity, entity.Index));
                            });

                            order.Complete(null);
                        });
                }
            });
        }
    }
}