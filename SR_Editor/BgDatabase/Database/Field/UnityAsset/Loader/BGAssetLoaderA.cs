/*
<copyright file="BGAssetLoaderA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;
#if !BG_SA
using Object = UnityEngine.Object;
#endif

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// abstract loader for Unity asset
    /// </summary>
    public abstract partial class BGAssetLoaderA : BGConfigurableI, BGConfigurableBinaryI
    {
        //================================================================================================
        //                                              Static
        //================================================================================================
        //safe-to-use in multi-threaded environment
        //used from Editor code only
        private static readonly List<Type> AllLoadersTypes = new List<Type>();

        /// <summary>
        /// all loader types
        /// </summary>
        public static List<Type> LoaderTypes
        {
            get
            {
                if (AllLoadersTypes.Count != 0) return AllLoadersTypes;

                var allSubTypes = BGUtil.GetAllSubTypes(typeof(BGAssetLoaderA));
                foreach (var fieldType in allSubTypes) AllLoadersTypes.Add(fieldType);
                return AllLoadersTypes;
            }
        }

        //================================================================================================
        //                                              Load
        //================================================================================================
        /// <summary>
        /// loader name
        /// </summary>
        public abstract string Name { get; }


        /// <summary>
        /// Load asset at specified path, using specified type
        /// </summary>
        public abstract T Load<T>(string path) where T : Object;

        /// <summary>
        /// Load asset with subassets at specified path, using specified type
        /// </summary>
        public abstract T[] LoadAll<T>(string path) where T : Object;


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
        //                                              Descriptor
        //================================================================================================

        /// <summary>
        /// attribute to use with asset loader
        /// </summary>
        public class AssetLoaderDescriptor : BGAttributeWithManager
        {
            public static AssetLoaderDescriptor Get(Type type) => BGUtil.GetAttribute<AssetLoaderDescriptor>(type);
        }

        /// <summary>
        /// to mark a field with a loader
        /// </summary>
        public interface WithLoaderI
        {
            BGAssetLoaderA AssetLoader { get; set; }
        }
    }
}