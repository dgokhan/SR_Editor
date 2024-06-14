/*
<copyright file="BGMergerEntity.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Merger for merging entities
    /// </summary>
    public partial class BGMergerEntity : BGMergerA
    {
        private readonly BGMergeSettingsEntity settings;

        private readonly List<BGEntity> Missing = new List<BGEntity>();
        private readonly List<BGEntity> Orphaned = new List<BGEntity>();
        private readonly Dictionary<BGEntity, BGEntity> Other2Mine = new Dictionary<BGEntity, BGEntity>();
        private readonly BGIdDictionary<BGEntity> Id2EntityMine = new BGIdDictionary<BGEntity>();
        private readonly BGIdDictionary<BGEntity> Id2EntityOther = new BGIdDictionary<BGEntity>();

        public BGMergerEntity(BGLogger logger, BGRepo @from, BGRepo to, BGMergeSettingsEntity settings) : base(logger, from, to) => this.settings = settings;

        /// <summary>
        /// merge 'from' repo as source and 'to' repo as destination 
        /// </summary>
        public MergeResult Merge()
        {
            MergeResult result = null;
            Section("Merging Entities", () =>
            {
                To.Events.Batch(() =>
                {
                    var mode = settings?.Mode ?? BGMergeModeEnum.Transfer;
                    switch (mode)
                    {
                        case BGMergeModeEnum.Transfer:
                            result = Transfer();
                            break;
                        case BGMergeModeEnum.Merge:
                            result = Combine();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                });
            });
            return result;
        }

        //=================================================================================================================
        //                      Transfer
        //=================================================================================================================
        //this operation does not merge but simply transfer all data 
        private MergeResultTransfer Transfer()
        {
            To.Clear();
            To.BinaryFormatVersion = From.BinaryFormatVersion;

            //switch every meta to new repo
            From.ForEachMeta(meta => meta.SwitchTo(To));

            //switch every meta to new repo
            From.ForEachView(view => view.SwitchTo(To));

            From.Clear();
            AppendLine("$ meta transferred.", To.CountMeta);
            return new MergeResultTransfer { MetaCount = To.CountMeta };
        }

        //=================================================================================================================
        //                      Merge
        //=================================================================================================================
        //this operation merge 2 repos
        private MergeResultCombine Combine()
        {
            ClearTempLists();
            var result = new MergeResultCombine();


            //setup controller
            BGMergeSettingsEntity.IMergeReceiver mergeReceiver = null;
            BGMergeSettingsEntity.IAddMissingReceiver addMissingReceiver = null;
            var addMissingReceiverError = false;
            BGMergeSettingsEntity.IRemoveOrphanedReceiver removeOrphanedReceiver = null;
            var removeOrphanedReceiverError = false;
            BGMergeSettingsEntity.IUpdateMatchingReceiver updateMatchingReceiver = null;
            var updateMatchingReceiverError = false;
            BGMergeSettingsEntity.IUpdateMatchingFieldReceiver updateMatchingFieldReceiver = null;
            var updateMatchingFieldReceiverError = false;

            var controller = settings.NewController(logger);
            /*
             //moved to NewController method
            if (settings.Controller == null)
            {
                var controllerType = settings.ControllerType;
                if (!string.IsNullOrEmpty(controllerType))
                    try
                    {
                        controller = BGUtil.Create<object>(controllerType, false);
                    }
                    catch (Exception e)
                    {
                        Debug.Log($"[WARNING!] BGDatabase: Controller object can not be created using {controllerType} type! See the next line for error details!");
                        Debug.LogException(e);
                        AppendLine("Controller Type is set up, however the object can not be created (the error is $). Skipping..", e.Message);
                    }
            }
            else controller = settings.Controller;
            */

            if (controller != null)
            {
                result.Controller = controller;

                var implementedInterfaces = "";
                CheckInterface(controller, ref mergeReceiver, ref implementedInterfaces);
                CheckInterface(controller, ref addMissingReceiver, ref implementedInterfaces);
                CheckInterface(controller, ref removeOrphanedReceiver, ref implementedInterfaces);
                CheckInterface(controller, ref updateMatchingReceiver, ref implementedInterfaces);
                CheckInterface(controller, ref updateMatchingFieldReceiver, ref implementedInterfaces);

                if (string.IsNullOrEmpty(implementedInterfaces)) AppendLine("Controller was set up $, however controller does not implement any receiver interfaces.", settings.ControllerType);
                else AppendLine("Controller was set up $. The following receivers was assigned: $.", settings.ControllerType, implementedInterfaces);
            }


            if (mergeReceiver != null)
            {
                var cancelMerge = false;
                try
                {
                    cancelMerge = mergeReceiver.OnBeforeMerge(From, To);
                }
                catch (Exception e)
                {
                    AppendWarning("Error from mergeReceiver.OnBeforeMerge:" + e.Message);
                }

                if (cancelMerge)
                {
                    AppendLine("Attention!!! Merge was cancelled by a controller.");
                    return result;
                }
            }

            From.ForEachMeta(metaFrom =>
            {
                SubSection(() =>
                {
                    var metaId = metaFrom.Id;

                    if (!settings.IsMetaIncluded(metaId))
                    {
                        AppendLine("Meta is not included in settings. Skipping..");
                        return;
                    }

                    var metaTo = To.GetMeta(metaId);
                    if (metaTo == null)
                    {
                        AppendLine("Meta is not found in destination Repo. Skipping..");
                        return;
                    }

                    //get settings
                    var isAddingMissing = settings.IsAddingMissing(metaId);
                    var isRemovingOrphaned = settings.IsRemovingOrphaned(metaId);
                    var isUpdatingMatching = settings.IsUpdatingMatching(metaId);

                    //===============================  prepare data
                    // ClearTempLists();

                    //hash every entity by it's id
                    metaTo.ForEachEntity(myObject => Id2EntityMine[myObject.Id] = myObject);
                    metaFrom.ForEachEntity(otherObject => Id2EntityOther[otherObject.Id] = otherObject);

                    //======= sort it out
                    foreach (var pair in Id2EntityMine)
                    {
                        if (Id2EntityOther.TryGetValue(pair.Key, out var otherEntity)) Other2Mine[otherEntity] = pair.Value;
                        else Orphaned.Add(pair.Value);
                    }

                    foreach (var pair in Id2EntityOther)
                        if (!Id2EntityMine.ContainsKey(pair.Key))
                            Missing.Add(pair.Value);


                    //==================================== take actions
                    if (isAddingMissing)
                    {
                        var addedCount = 0;
                        foreach (var e in Missing)
                        {
                            var entity = e;
                            try
                            {
                                if (addMissingReceiver != null && addMissingReceiver.OnBeforeAdd(entity)) continue;
                            }
                            catch (Exception ex)
                            {
                                if (!addMissingReceiverError)
                                {
                                    addMissingReceiverError = true;
                                    AppendWarning("addMissingReceiver controller raised an error: $ ", ex.Message);
                                }
                            }


                            addedCount++;
                            metaTo.NewEntity(entity.Id);
                            metaTo.ForEachField(fieldTo =>
                            {
                                if (settings.AddMissingFieldFilter != null && !settings.AddMissingFieldFilter(fieldTo)) return;
                                var fieldFrom = metaFrom.GetField(fieldTo.Id, false);
                                if (fieldFrom == null) return;
                                fieldTo.CopyValue(fieldFrom, entity.Id, entity.Index, entity.Id);
                            });
                        }

                        result.AddedCount += addedCount;
                        result.AddedSkippedCount += Missing.Count - addedCount;
                        AppendLine("$ missing entities added. $ skipped. ", addedCount, Missing.Count - addedCount);
                    }
                    else AppendLine("$ missing entities found. No action was taken", Missing.Count);

                    if (isRemovingOrphaned)
                    {
                        var removedCount = 0;
                        foreach (var entity in Orphaned)
                        {
                            try
                            {
                                if (removeOrphanedReceiver != null && removeOrphanedReceiver.OnBeforeDelete(entity)) continue;
                            }
                            catch (Exception ex)
                            {
                                if (!removeOrphanedReceiverError)
                                {
                                    removeOrphanedReceiverError = true;
                                    AppendWarning("removeOrphanedReceiver controller raised an error: $ ", ex.Message);
                                }
                            }

                            removedCount++;
                            entity.Delete();
                        }

                        result.RemovedCount += removedCount;
                        result.RemovedSkippedCount += Orphaned.Count - removedCount;
                        AppendLine("$ orphaned entities removed. $ skipped.", removedCount, Orphaned.Count - removedCount);
                    }
                    else AppendLine("$ orphaned entities found. No action was taken", Orphaned.Count);

                    if (isUpdatingMatching)
                    {
                        var updatedCount = 0;
                        var updatedFieldsCount = 0;
                        var fieldUpdateCancelled = 0;
                        foreach (var pair in Other2Mine)
                        {
                            var fromEntity = pair.Key;
                            var toEntity = pair.Value;

                            try
                            {
                                if (updateMatchingReceiver != null && updateMatchingReceiver.OnBeforeUpdate(fromEntity, toEntity)) continue;
                            }
                            catch (Exception ex)
                            {
                                if (!updateMatchingReceiverError)
                                {
                                    updateMatchingReceiverError = true;
                                    AppendWarning("updateMatchingReceiverError controller raised an error: $ ", ex.Message);
                                }
                            }

                            updatedCount++;

                            metaTo.ForEachField(fieldTo =>
                            {
                                var fieldFrom = metaFrom.GetField(fieldTo.Id, false);
                                if (fieldFrom == null) return;
                                if (!settings.IsFieldIncluded(fieldFrom)) return;

                                try
                                {
                                    if (updateMatchingFieldReceiver != null && updateMatchingFieldReceiver.OnBeforeFieldUpdate(fieldFrom, fieldTo, fromEntity, toEntity))
                                    {
                                        fieldUpdateCancelled++;
                                        return;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    if (!updateMatchingFieldReceiverError)
                                    {
                                        updateMatchingFieldReceiverError = true;
                                        AppendWarning("updateMatchingFieldReceiverError controller raised an error: $ ", ex.Message);
                                    }
                                }

                                updatedFieldsCount++;
                                fieldTo.CopyValue(fieldFrom, fromEntity.Id, fromEntity.Index, fromEntity.Id);
                            });
                        }

                        result.UpdatedCount += updatedCount;
                        result.UpdatedSkippedCount += Other2Mine.Count - updatedCount;
                        result.UpdatedFieldsCount += updatedFieldsCount;
                        result.UpdatedFieldsSkippedCount += fieldUpdateCancelled;

                        AppendLine("$ matching entities updated. $ skipped. $ field update operations was cancelled.", updatedCount, Other2Mine.Count - updatedCount, fieldUpdateCancelled);
                    }
                    else AppendLine("$ matching entities found. No action was taken", Other2Mine.Count);

                    ClearTempLists();
                }, "Processing $ meta.", metaFrom.Name);
            });

            if (mergeReceiver != null) BGUtil.Catch(() => mergeReceiver.OnAfterMerge(From, To), exception => AppendWarning("Error from mergeReceiver.OnAfterMerge:" + exception.Message));

            AppendLine("$ metas processed.", From.CountMeta);
            return result;
        }

        private void CheckInterface<T>(object controller, ref T receiverInterface, ref string log) where T : class
        {
            if (!(controller is T controller1)) return;

            receiverInterface = controller1;
            log += typeof(T).Name + " ";
        }


        /*private void Split(Action<Action<BGEntity>, Predicate<BGEntity>, Comparison<BGEntity>> myObjectsIterator,
            Action<Action<BGEntity>, Predicate<BGEntity>, Comparison<BGEntity>> otherObjectsIterator,
            Action<BGEntity> missingAction, Action<BGEntity> orphanedAction, Action<BGEntity, BGEntity> matchingAction)
        {
            ClearTempLists();

            myObjectsIterator(myObject => Id2EntityMine.Add(myObject.Id, myObject), null, null);
            otherObjectsIterator(otherObject => Id2EntityOther.Add(otherObject.Id, otherObject), null, null);

            //======= sort out
            foreach (var pair in Id2EntityMine)
            {
                if (!Id2EntityOther.ContainsKey(pair.Key)) Orphaned.Add(pair.Value);
                else Other2Mine[Id2EntityOther[pair.Key]] = pair.Value;
            }

            foreach (var pair in Id2EntityOther)
                if (!Id2EntityMine.ContainsKey(pair.Key))
                    Missing.Add(pair.Value);


            //======= take actions
            if (missingAction != null)
            {
                foreach (var entity in Missing) missingAction(entity);
                AppendLine("$ missing entities added.", Missing.Count);
            }
            else AppendLine("$ missing entities found. No action was taken", Missing.Count);

            if (orphanedAction != null)
            {
                foreach (var entity in Orphaned) orphanedAction(entity);
                AppendLine("$ orphaned entities removed.", Orphaned.Count);
            }
            else AppendLine("$ orphaned entities found. No action was taken", Orphaned.Count);

            if (matchingAction != null)
            {
                foreach (var pair in Other2Mine) matchingAction(pair.Key, pair.Value);
                AppendLine("$ matching entities updated.", Other2Mine.Count);
            }
            else AppendLine("$ matching entities found. No action was taken", Other2Mine.Count);

            ClearTempLists();
        }*/

        //=================================================================================================================
        //                      Helper methods
        //=================================================================================================================

        private void ClearTempLists()
        {
            Missing.Clear();
            Orphaned.Clear();
            Other2Mine.Clear();
            Id2EntityMine.Clear();
            Id2EntityOther.Clear();
        }
        //=================================================================================================================
        //                      Nested classes
        //=================================================================================================================

        /// <summary>
        /// abstract data container for merge statistics 
        /// </summary>
        public abstract class MergeResult
        {
        }

        /// <summary>
        /// data container for merge statistics with mode=Transfer 
        /// </summary>
        public class MergeResultTransfer : MergeResult
        {
            public int MetaCount;
        }

        /// <summary>
        /// data container for merge statistics with mode=Merge
        /// </summary>
        public class MergeResultCombine : MergeResult
        {
            public int AddedCount;
            public int AddedSkippedCount;
            public int RemovedCount;
            public int RemovedSkippedCount;
            public int UpdatedCount;
            public int UpdatedSkippedCount;
            public int UpdatedFieldsCount;
            public int UpdatedFieldsSkippedCount;
            public object Controller;
        }

        /// <summary>
        /// result of parsing some data
        /// </summary>
        public interface ParseResultI
        {
            bool HasEntitySheet(BGId metaId);
            bool HasFieldInEntitySheet(BGId metaId, BGId fieldId);
        }
    }
}