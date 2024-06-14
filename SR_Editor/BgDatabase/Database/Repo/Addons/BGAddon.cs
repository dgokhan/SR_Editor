/*
<copyright file="BGAddon.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Addon can be enabled in database to extend it's functionality without loading any additional content
    /// </summary>
    public abstract partial class BGAddon : BGConfigurableI, BGConfigurableBinaryI
    {
        //================================================================================================
        //                                              Static
        //================================================================================================
        private static readonly List<Type> AllAddonTypes = new List<Type>();

        /// <summary>
        /// All possible addon types
        /// </summary>
        public static List<Type> AddonTypes
        {
            get
            {
                if (AllAddonTypes.Count != 0) return AllAddonTypes;

                var allSubTypes = BGUtil.GetAllSubTypes(typeof(BGAddon));
                foreach (var fieldType in allSubTypes) AllAddonTypes.Add(fieldType);
                return AllAddonTypes;
            }
        }

        /// <summary>
        /// get addon name by it's type
        /// </summary>
        public static string GetName(Type type) => BGUtil.GetAttribute<AddonDescriptor>(type).Name;

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <summary>
        /// Create addon by type and string config
        /// </summary>
        public static BGAddon Create(string type, string config)
        {
            var addon = BGUtil.Create<BGAddon>(type, false);
            addon.ConfigFromString(config);
            return addon;
        }

        /// <summary>
        /// Create addon by type and byte array config
        /// </summary>
        [Obsolete("This method is deprecated. Use FromBinary instead")]
        public static BGAddon Create(string type, ArraySegment<byte> config)
        {
            var addon = BGUtil.Create<BGAddon>(type, false);
            addon.ConfigFromBytes(config);
            return addon;
        }

        //================================================================================================
        //                                              Binary
        //================================================================================================
        //reconstruct addon from binary array
        internal static BGAddon FromBinary(BGBinaryReader binder)
        {
            var version = binder.ReadInt();
            switch (version)
            {
                case 1:
                {
                    var type = binder.ReadString();
                    var config = binder.ReadByteArray();
                    var addon = BGUtil.Create<BGAddon>(type, false);
                    addon.ConfigFromBytes(config);
                    return addon;
                }

                default:
                {
                    throw new BGException("Can not read addon from binary array: unsupported version $", version);
                }
            }
        }

        //convert addon to binary array
        internal static void ToBinary(BGBinaryWriter builder, BGAddon addon)
        {
            //version
            builder.AddInt(1);
            builder.AddString(addon.GetType().AssemblyQualifiedName);
            builder.AddByteArray(addon.ConfigToBytes());
        }

        //================================================================================================
        //                                              Fields
        //================================================================================================
        private BGRepo repo;

        /// <summary>
        /// addon's database
        /// </summary>
        public virtual BGRepo Repo
        {
            get => repo;
            protected set => repo = value;
        }

        /// <summary>
        /// addon's name
        /// </summary>
        public string Name => GetName(GetType());

        public virtual int OnMainDatabaseLoadOrder => 0;

        //================================================================================================
        //                                              Config
        //================================================================================================

        /// <inheritdoc />
        public virtual string ConfigToString() => null;

        /// <inheritdoc />
        public virtual void ConfigFromString(string config)
        {
        }

        /// <inheritdoc />
        public virtual byte[] ConfigToBytes() => null;

        /// <inheritdoc />
        public virtual void ConfigFromBytes(ArraySegment<byte> config)
        {
        }

        //================================================================================================
        //                                              Methods
        //================================================================================================

        /// <summary>
        /// Assign repo to addon 
        /// </summary>
        public void Init(BGRepo repo) => Repo = repo;

        /// <summary>
        /// Clone addon to another repo
        /// </summary>
        public abstract BGAddon CloneTo(BGRepo repo);

        /// <summary>
        /// Clone addon and add to specified repo
        /// </summary>
        public void CloneAndAddTo(BGRepo repo) => repo.Addons.Add(CloneTo(repo));

        /// <summary>
        /// the list of required addons. Addon may require another addon(s) to exist in repo
        /// </summary>
        public virtual List<Type> GetRequiredAddons() => null;

        /// <summary>
        /// addon data is changed
        /// </summary>
        public void FireChange()
        {
            if (Repo == null || !Repo.Events.On) return;
            Repo.Events.FireAddonChange();
        }

        /// <summary>
        /// called when main database is loaded in Editor
        /// </summary>
        public virtual void OnMainDatabaseLoad()
        {
        }
        //================================================================================================
        //                                              Descriptor
        //================================================================================================

        /// <summary>
        /// descriptor to use with addon type
        /// </summary>
        public class AddonDescriptor : BGAttributeWithManager
        {
        }

        /// <summary>
        /// on repo load callback
        /// </summary>
        public virtual void OnLoad()
        {
        }

        /// <summary>
        /// Called before addon is removed
        /// </summary>
        public virtual void OnDelete(BGRepo repo)
        {
        }

        /// <summary>
        /// Called before addon is added. Throws exception if addon can not be added
        /// </summary>
        public virtual void OnBeforeAdd(BGRepo repo)
        {
            var required = GetRequiredAddons();
            if (required == null || required.Count == 0) return;

            foreach (var requiredAddonType in required)
                if (!repo.Addons.Has(requiredAddonType))
                    throw new BGException("Addon $ is required, but it was not activated. Activate it first.", GetName(requiredAddonType));
        }

        /// <summary>
        /// Called before addon is transferred. Addons is transferred when repo is merged with transfer mode
        /// </summary>
        public virtual void OnTransfer(BGRepo repo)
        {
        }
    }
}