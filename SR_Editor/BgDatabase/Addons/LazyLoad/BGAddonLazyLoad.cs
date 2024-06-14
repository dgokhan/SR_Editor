/*
<copyright file="BGAddonLazyLoad.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Lazy load addon. With this addon enabled, database will not be loaded until the moment it's data really needed.
    /// See  <a href="http://www.bansheegz.com/BGDatabase/Addons/LazyLoad/">this link</a> for more details.
    /// </summary>
    [AddonDescriptor(Name = "LazyLoad", ManagerType = "BansheeGz.BGDatabase.Editor.BGAddonManagerLazyLoad")]
    public partial class BGAddonLazyLoad : BGAddon
    {
        private bool disconnectEntity;
        private bool enabledForPlayMode;

        /// <summary>
        /// Disconnect entity- means all BGEntityGo components (including subclasses) will not be connected to
        /// database in the Inspector
        /// </summary>
        public bool DisconnectEntity
        {
            get => disconnectEntity;
            set
            {
                if (disconnectEntity == value) return;
                disconnectEntity = value;
                FireChange();
            }
        }

        /// <summary>
        /// Enables lazy load for playmode. This is not recommended.
        /// </summary>
        public bool EnabledForPlayMode
        {
            get => enabledForPlayMode;
            set
            {
                if (enabledForPlayMode == value) return;
                enabledForPlayMode = value;
                FireChange();
            }
        }

        //================================================================================================
        //                                              Config
        //================================================================================================
        /// <inheritdoc />
        public override string ConfigToString()
        {
            var configToString = JsonUtility.ToJson(new Settings { DisconnectEntity = disconnectEntity, EnabledForPlayMode = enabledForPlayMode });
            return configToString;
        }

        /// <inheritdoc />
        public override void ConfigFromString(string config)
        {
            var settings = JsonUtility.FromJson<Settings>(config);
            disconnectEntity = settings.DisconnectEntity;
            enabledForPlayMode = settings.EnabledForPlayMode;
        }

        [Serializable]
        private class Settings
        {
            public bool DisconnectEntity;
            public bool EnabledForPlayMode;
        }

        public override byte[] ConfigToBytes()
        {
            var writer = new BGBinaryWriter(6);

            //version
            writer.AddInt(1);

            //fields
            writer.AddBool(disconnectEntity);
            writer.AddBool(enabledForPlayMode);

            return writer.ToArray();
        }

        public override void ConfigFromBytes(ArraySegment<byte> config)
        {
            var reader = new BGBinaryReader(config);
            var version = reader.ReadInt();
            switch (version)
            {
                case 1:
                {
                    disconnectEntity = reader.ReadBool();
                    enabledForPlayMode = reader.ReadBool();
                    break;
                }
                default:
                {
                    throw new BGException("Unknown version: $", version);
                }
            }
        }

        //================================================================================================
        //                                              Misc
        //================================================================================================
        /// <inheritdoc />
        public override BGAddon CloneTo(BGRepo repo)
        {
            return new BGAddonLazyLoad { Repo = repo, disconnectEntity = disconnectEntity, enabledForPlayMode = enabledForPlayMode };
        }

        /// <summary>
        /// Check if addon is enabled for specified repo
        /// </summary>
        public static bool Enabled(BGRepo repo)
        {
            if (!repo.Addons.Has<BGAddonLazyLoad>()) return false;
            
            //localization addon does not play well with lazyload currently- we need to disable it temporarily
            if (BGLocalizationUglyHacks.HasLocalizationAddon(repo)) return false;
                
            var addon = repo.Addons.Get<BGAddonLazyLoad>();

            return addon.EnabledForPlayMode;
        }
    }
}