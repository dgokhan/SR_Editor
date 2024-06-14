/*
<copyright file="BGAssetLoaderAddressables.cs" company="BansheeGz">
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
    /// loader for loading assets from Addressables system
    /// </summary>
    [AssetLoaderDescriptor(Name = "Addressables", ManagerType = "BansheeGz.BGDatabase.Editor.BGAssetLoaderManagerAddressables")]
    public class BGAssetLoaderAddressables : BGAssetLoaderA
    {
        public const string NoPluginWarning =
            "Important!! Addressables plug-in for BGDatabase is not installed or outdated. Please, download latest version here: www.bansheegz.com/BGDatabase/Downloads/";

        private static AddressablesLoader loaderDelegate;
        private static bool loaderDelegateLoadAttempted;

        /// <inheritdoc />
        public override string Name => "Addressables";

        /// <summary>
        /// delegate class implementation
        /// </summary>
        public static AddressablesLoader LoaderDelegate
        {
            get
            {
                InitDelegate();
                return loaderDelegate;
            }
        }

        /// <inheritdoc />
        public override T Load<T>(string path)
        {
            InitDelegate();
            if (loaderDelegate == null) return null;
            try
            {
                return loaderDelegate.Load<T>(path);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return null;
            }
        }

        /// <inheritdoc />
        public override T[] LoadAll<T>(string path)
        {
            InitDelegate();
            if (loaderDelegate == null) return null;
            IList<T> objects;
            try
            {
                objects = loaderDelegate.LoadAll<T>(path);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return null;
            }

            if (objects == null || objects.Count == 0) return null;
            var result = new T[objects.Count];
            for (var i = 0; i < objects.Count; i++) result[i] = objects[i];
            return result;
/*
            List<T> result = null;
            if (objects != null && objects.Count > 0)
            {
                result = new List<T>();
                foreach (var o in objects)
                {
                    var t = o as T;
                    if (t == null) continue;
                    result.Add(t);
                }
            }

            return result == null || result.Count == 0 ? null : result.ToArray();
*/
        }

        private static void InitDelegate()
        {
            if (loaderDelegate == null && !loaderDelegateLoadAttempted)
            {
                loaderDelegateLoadAttempted = true;
                try
                {
                    loaderDelegate = BGUtil.Create<AddressablesLoader>("BansheeGz.BGDatabase.BGAddressablesLoader", false);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }

                if (loaderDelegate == null) Debug.Log(NoPluginWarning);
            }
        }

        /// <summary>
        /// interface for addressables delegate implementation
        /// </summary>
        public interface AddressablesLoader
        {
            /// <summary>
            /// Load Unity asset using supplied key
            /// </summary>
            T Load<T>(string path) where T : Object;
            
            /// <summary>
            /// Load all Unity asset using supplied key
            /// </summary>
            IList<T> LoadAll<T>(string path) where T : Object;
        }
    }
}