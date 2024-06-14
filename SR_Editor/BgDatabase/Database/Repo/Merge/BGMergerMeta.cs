/*
<copyright file="BGMergerMeta.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// IS IT USED???
    /// this is merger for meta. This is experimental feature 
    /// </summary>
    public partial class BGMergerMeta : BGMergerA
    {
        private readonly BGMergeSettingsMeta settings;

        public BGMergerMeta(BGLogger logger, BGRepo from, BGRepo to, BGMergeSettingsMeta settings) : base(logger, from, to)
        {
            this.settings = settings;
        }

        public void Merge()
        {
            Section("Merging Metas", () =>
            {
                To.Events.Batch(() =>
                {
                    var mode = settings?.Mode ?? BGMergeModeEnum.Transfer;
                    switch (mode)
                    {
                        case BGMergeModeEnum.Transfer:
                            Transfer();
                            break;
                        case BGMergeModeEnum.Merge:
                            Combine();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                });
            });
        }

        //=================================================================================================================
        //                      Transfer
        //=================================================================================================================

        private void Transfer()
        {
            To.Clear();

            //switch every meta to new repo
            From.ForEachMeta(meta => meta.SwitchTo(To));
            From.Clear();
            AppendLine("$ metas was transfered.", To.CountMeta);

            To.Addons.Clear();
            To.Addons.AddFrom(From.Addons);
            From.Addons.Clear();
        }

        //=================================================================================================================
        //                      Merge
        //=================================================================================================================

        private void Combine()
        {
            if (settings.AddMissing || settings.UpdateMatching)
            {
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
                            if (settings.AddMissing)
                            {
                                metaTo = metaFrom.CloneTo(To, null, field => false, false);
                                AppendLine("Meta is not found in destination Repo and was added.");
                            }
                            else AppendLine("Meta is not found in destination Repo and was skipped due to settings");
                        }
                        else if (settings.UpdateMatching)
                        {
                            metaTo.Name = metaFrom.Name;
                            metaTo.Singleton = metaFrom.Singleton;
                            metaTo.EmptyName = metaFrom.EmptyName;
                            metaTo.UniqueName = metaFrom.UniqueName;
                            metaTo.Comment = metaFrom.Comment;
                            metaTo.ControllerType = metaFrom.ControllerType;
                            metaTo.UserDefinedReadonly = metaFrom.UserDefinedReadonly;

                            AppendLine("Meta was found and updated.");
                        }
                        else AppendLine("Meta was found but no action was taken due to settings.");

                        if (metaTo != null)
                        {
                            int added = 0, updated = 0, removed = 0, skipped = 0;
                            metaFrom.ForEachField(fieldFrom =>
                            {
                                if (!settings.IsFieldIncluded(fieldFrom))
                                {
                                    skipped++;
                                    AppendLine("field $ is skipped due to settings.", fieldFrom.Name);
                                    return;
                                }

                                var fieldTo = metaTo.GetField(fieldFrom.Id, false);
                                if (fieldTo == null)
                                {
                                    if (settings.AddMissing)
                                    {
                                        added++;
                                        AppendLine("field $ is added", fieldFrom.Name);
                                        fieldFrom.CloneTo(metaTo, false);
                                    }
                                    else
                                    {
                                        skipped++;
                                        AppendLine("field $ is not found in destination Repo and was skipped due to settings", fieldFrom.Name);
                                    }
                                }
                                else if (settings.UpdateMatching)
                                {
                                    fieldTo.Name = fieldFrom.Name;
                                    fieldTo.Required = fieldFrom.Required;
                                    fieldTo.UserDefinedReadonly = fieldFrom.UserDefinedReadonly;
                                    fieldTo.DefaultValue = fieldFrom.DefaultValue;
                                    fieldTo.Comment = fieldFrom.Comment;
                                    fieldTo.ControllerType = fieldFrom.ControllerType;
                                    updated++;
                                    AppendLine("field $ was updated", fieldFrom.Name);
                                }
                                else
                                {
                                    skipped++;
                                    AppendLine("field $ is skipped due to settings", fieldFrom.Name);
                                }
                            });

                            if (settings.RemoveOrphaned)
                                metaTo.ForEachField(field =>
                                {
                                    if (metaFrom.HasField(field.Id)) return;
                                    removed++;
                                    AppendLine("field $ is removed.", field.Name);
                                    field.Delete();
                                });
                            AppendLine("$ fields was added. $ fields was updated. $ fields was removed.. $ fields was skipped.", added, updated, removed, skipped);
                        }
                    }, "Processing meta $", metaFrom.Name);
                });

                From.Addons.ForEachAddon(addonFrom =>
                {
                    var has = To.Addons.Has(addonFrom.GetType());
                    if (has)
                    {
                        if (settings.UpdateMatching)
                        {
                            var addonTo = To.Addons.Get(addonFrom.GetType());
                            addonTo.ConfigFromString(addonFrom.ConfigToString());
                        }
                    }
                    else if (settings.AddMissing) addonFrom.CloneAndAddTo(To);
                });
            }

            if (settings.RemoveOrphaned)
            {
                To.ForEachMeta(metaTo =>
                {
                    if (!From.HasMeta(metaTo.Id)) metaTo.Delete();
                    else
                    {
                        var fields = new List<BGField>();
                        var metaFrom = From.GetMeta(metaTo.Id);
                        metaTo.ForEachField(field => fields.Add(field), field => !metaFrom.HasField(field.Id));

                        if (fields.Count > 0)
                            foreach (var field in fields)
                                field.Delete();
                    }
                });

                var toRemove = new List<Type>();
                To.Addons.ForEachAddon(addonTo =>
                {
                    var type = addonTo.GetType();
                    if (!From.Addons.Has(type)) toRemove.Add(type);
                });

                if (toRemove.Count > 0)
                    foreach (var type in toRemove)
                        To.Addons.Remove(type);
            }
        }
    }
}