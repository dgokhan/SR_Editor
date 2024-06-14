/*
<copyright file="BGAddonModding.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Addon adds support for modding
    /// See  <a href="http://www.bansheegz.com/BGDatabase/Addons/Modding/">this link</a> for more details.
    /// </summary>
    [AddonDescriptor(Name = "Modding", ManagerType = "BansheeGz.BGDatabase.Editor.BGAddonManagerModding")]
    public class BGAddonModding : BGAddon
    {
        private bool inBuildsOnly;

        /// <summary>
        /// Should addon loads delta files data only in builds (In Unity Editor addon will not be executed) 
        /// </summary>
        public bool InBuildsOnly
        {
            get => inBuildsOnly;
            set
            {
                if (inBuildsOnly == value) return;
                inBuildsOnly = value;
                FireChange();
            }
        }

        private bool enableCellProtection;

        /// <summary>
        /// Enable cells protection. Cell protection can be used to prevent modders to change specific cells
        /// </summary>
        public bool EnableCellProtection
        {
            get => enableCellProtection;
            set
            {
                if (enableCellProtection == value) return;
                enableCellProtection = value;
                FireChange();
            }
        }

        private bool disableCellProtectionOnMerge;

        /// <summary>
        /// Not currently used?
        /// </summary>
        public bool DisableCellProtectionOnMerge
        {
            get => disableCellProtectionOnMerge;
            set
            {
                if (disableCellProtectionOnMerge == value) return;
                disableCellProtectionOnMerge = value;
                FireChange();
            }
        }

        private BGModdingRepoProtection repoProtection;

        /// <summary>
        /// Initialize data protection manager
        /// </summary>
        public BGModdingRepoProtection RepoProtection
        {
            get
            {
                if (!enableCellProtection) return null;
                if (repoProtection == null)
                {
                    repoProtection = new BGModdingRepoProtection(Repo);
                    AssignListeners();
                }

                return repoProtection;
            }
        }

        //fired when data protection parameters changed
        private void RepoProtectionChanged()
        {
            FireChange();
        }


        private List<ModdingSourceI> sources;

        // loads all delta files sources
        private List<ModdingSourceI> Sources
        {
            get
            {
                if (sources != null) return sources;
                sources = new List<ModdingSourceI>();
                var implementations = BGUtil.GetAllImplementations(typeof(ModdingSourceI));
                foreach (var type in implementations)
                    try
                    {
                        var instance = Activator.CreateInstance(type) as ModdingSourceI;
                        if (instance == null) continue;
                        sources.Add(instance);
                    }
                    catch (Exception e)
                    {
                    }

                sources.Sort((o1, o2) => o1.Order.CompareTo(o2.Order));
                return sources;
            }
        }

        /// <inheritdoc />
        public override void OnLoad()
        {
            if (!BGRepo.DefaultRepo(Repo)) return;
            var sources = Sources;

            foreach (var source in sources)
                try
                {
                    var deltas = source.Deltas;
                    if (deltas == null || deltas.Length == 0) continue;
                    foreach (var delta in deltas)
                    {
                        delta?.ApplyTo(Repo, repoProtection);
                    }
                }
                catch (Exception e)
                {
                }

            /*
            foreach (var source in sources)
            {
                try
                {
                    source.OnLoad(Repo);
                }
                catch (Exception e)
                {
                    Debug.LogError("Error while calling OnLoad method from " + source.GetType().FullName + " class. See exception below");
                    Debug.LogException(e);
                }
            }
        */
        }

        /// <inheritdoc />
        public override BGAddon CloneTo(BGRepo repo)
        {
            var clone = new BGAddonModding
            {
                Repo = repo,
                inBuildsOnly = inBuildsOnly,
                enableCellProtection = enableCellProtection,
                repoProtection = repoProtection?.CloneTo(repo)
            };
            clone.AssignListeners();
            return clone;
        }

        //attach listeners for monitoring data protection parameters
        private void AssignListeners()
        {
            if (repoProtection == null) return;
            repoProtection.Changed -= RepoProtectionChanged;
            repoProtection.Changed += RepoProtectionChanged;
        }


        //================================================================================================
        //                                              Config
        //================================================================================================

        /// <inheritdoc />
        public override string ConfigToString()
        {
            return JsonUtility.ToJson(new JsonConfig
            {
                InBuildsOnly = inBuildsOnly,
                EnableCellProtection = enableCellProtection,
                repoProtection = RepoProtection
            });
        }

        /// <inheritdoc />
        public override void ConfigFromString(string config)
        {
            var json = JsonUtility.FromJson<JsonConfig>(config);
            inBuildsOnly = json.InBuildsOnly;
            enableCellProtection = json.EnableCellProtection;
            if (enableCellProtection)
            {
                repoProtection = json.repoProtection;
                AssignListeners();
            }
        }

        [Serializable]
        private class JsonConfig
        {
            public bool InBuildsOnly;
            public bool EnableCellProtection;
            public BGModdingRepoProtection repoProtection;
        }

        /// <inheritdoc />
        public override byte[] ConfigToBytes()
        {
            var writer = new BGBinaryWriter(5);

            //version
            var version = 2;
            writer.AddInt(version);

            //fields
            writer.AddBool(inBuildsOnly);

            //version 2
            writer.AddBool(enableCellProtection);
            writer.AddBool(disableCellProtectionOnMerge);
            if (enableCellProtection) RepoProtection.ConfigToBytes(writer, version);
            return writer.ToArray();
        }

        /// <inheritdoc />
        public override void ConfigFromBytes(ArraySegment<byte> config)
        {
            var reader = new BGBinaryReader(config);
            var version = reader.ReadInt();
            switch (version)
            {
                case 1:
                {
                    inBuildsOnly = reader.ReadBool();
                    break;
                }
                case 2:
                {
                    inBuildsOnly = reader.ReadBool();
                    enableCellProtection = reader.ReadBool();
                    disableCellProtectionOnMerge = reader.ReadBool();
                    if (enableCellProtection) RepoProtection.ConfigFromBytes(reader, version);
                    break;
                }

                default:
                {
                    throw new BGException("Unknown version: $", version);
                }
            }
        }

        //================================================================================================
        //                                              Nested classes
        //================================================================================================
        //interface for delta files providers
        private interface ModdingSourceI
        {
            /// <summary>
            /// the order to process data. Less order- the earlier provider data will be processed  
            /// </summary>
            int Order { get; }
            /// <summary>
            /// The list of delta files data
            /// </summary>
            BGRepoDelta[] Deltas { get; }

            [Obsolete("Use Deltas property to provide delta content")]
            void OnLoad(BGRepo repo);
        }

        /// <summary>
        /// Default implementation for delta file provider
        /// </summary>
        public class ModdingSourceDefault : ModdingSourceI
        {
            /// <inheritdoc />
            public virtual int Order => 0;

            /// <inheritdoc />
            public virtual BGRepoDelta[] Deltas => null;

            [Obsolete("Use Deltas property to provide delta content")]
            public virtual void OnLoad(BGRepo repo)
            {
            }
        }
    }
}