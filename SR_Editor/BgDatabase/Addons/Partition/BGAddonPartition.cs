/*
<copyright file="BGAddonPartition.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Partitioning addon. With this addon enabled, database can be split up to several chunks (files)
    /// See  <a href="http://www.bansheegz.com/BGDatabase/Addons/Partition/">this link</a> for more details.
    /// </summary>
    [AddonDescriptor(Name = "Partition", ManagerType = "BansheeGz.BGDatabase.Editor.BGAddonManagerPartition")]
    public partial class BGAddonPartition : BGAddon
    {
        //================================================================================================
        //                                              static
        //================================================================================================


        //================================================================================================
        //                                              fields
        //================================================================================================

        //loaded partitions
        private readonly HashSet<BGId> loaded = new HashSet<BGId>();

        private bool disableTemporarily;

        /// <summary>
        /// Disable addon temporarily 
        /// </summary>
        public bool DisableTemporarily
        {
            get => disableTemporarily;
            set
            {
                if (disableTemporarily == value) return;
                disableTemporarily = value;
                FireChange();
            }
        }

        /// <inheritdoc />
        public override int OnMainDatabaseLoadOrder => 16;

        //================================================================================================
        //                                              enable/disable callbacks
        //================================================================================================
        /*
        /// <inheritdoc />
        public override void OnBeforeAdd(BGRepo repo)
        {
            base.OnBeforeAdd(repo);

            // if (repo.HasMeta(PartitionMetaName))
                // throw new BGException("Can not activate addon: meta with name $ already exists! Please, rename this meta before enabling partition addon", PartitionMetaName);

            // CreatePartitionMeta(repo);
        }
        */

        /// <inheritdoc />
        public override void OnDelete(BGRepo repo)
        {
            var hMeta = repo.GetMeta(PartitionMetaName);
            hMeta?.Delete();
            var vMeta = repo.GetMeta(PartitionVerticalMetaName);
            vMeta?.Delete();
            loaded.Clear();
        }

        /// <inheritdoc />
        public override BGAddon CloneTo(BGRepo repo)
        {
            return new BGAddonPartition
            {
                Repo = repo,
                disableTemporarily = disableTemporarily,
                disableHorizontalTemporarily = disableHorizontalTemporarily,
                disableVerticalTemporarily = disableVerticalTemporarily
            };
        }

        //================================================================================================
        //                                              callbacks
        //================================================================================================
        /// <inheritdoc />
        public override void OnMainDatabaseLoad()
        {
            LoadVertical();

        }

        //================================================================================================
        //                                              methods
        //================================================================================================
        /// <summary>
        /// Is partitioning enabled and has any meaningful setting
        /// </summary>
        public bool Enabled => EnabledHorizontal || EnabledVertical;

        /// <summary>
        /// Return partitioning model, which can be used to write data to files 
        /// </summary>
        public BGPartitionSaveModel Save(string basicPath)
        {
            return new BGPartitionSaveProcessor().Save(
                new BGPartitionSaveContext(
                    basicPath,
                    Repo,
                    this,
                    EnabledHorizontal ? new BGPartitionsModel(Repo) : null,
                    new BGMetaPartitionModelProvider(),
                    false
                )
            );
        }
    }
}