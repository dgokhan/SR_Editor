/*
<copyright file="BGAddressablesMonitor.cs" company="BansheeGz">
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
    /// class for monitoring how much the asset was loaded, so it can be unloaded
    /// </summary>
    public static class BGAddressablesMonitor
    {
        private static readonly Dictionary<Tuple<BGId, BGId>, int> Origin2Count = new Dictionary<Tuple<BGId, BGId>, int>();
        public static BGAddressablesMonitorDelegateI UnloadDelegate;

        public static bool Enabled => UnloadDelegate != null;
        public static bool UnloadOnRowDelete;
        public static bool DebugOn;

        /// <summary>
        /// this method is called once the asset was loaded
        /// </summary>
        public static void AssetWasLoaded(BGField field, BGId entityId)
        {
            var tuple = Tuple.Create(field.Id, entityId);
            int newCount;
            if (Origin2Count.TryGetValue(tuple, out var count)) newCount = count + 1;
            else newCount = 1;
            Origin2Count[tuple] = newCount;
            if (DebugOn) Debug.Log($"BGAddressablesMonitor debug: Asset for {field.FullName}[{entityId}] was loaded for {newCount} times");
        }

        /// <summary>
        /// unload the asset for provided cell
        /// </summary>
        public static void UnloadAsset<T>(BGFieldUnityAssetA<T> field, BGId entityId) where T : Object => UnloadAsset(field, entityId, field.Meta.FindEntityIndex(entityId));

        /// <summary>
        /// unload the asset for provided cell
        /// </summary>
        public static void UnloadAsset<T>(BGFieldUnityAssetA<T> field, BGId entityId, int entityIndex) where T : Object
        {
            var count = GetCount(field, entityId, entityIndex, out var address);
            if (count == 0) return;
            UnloadDelegate.Unload<T>(address, count);
        }

        /// <summary>
        /// unload the asset of type T for provided cell
        /// </summary>
        public static void UnloadAsset<T>(BGFieldUnityAssetArrayA<T> field, BGId entityId) where T : Object => UnloadAsset(field, entityId, field.Meta.FindEntityIndex(entityId));

        /// <summary>
        /// unload the asset of type T for provided cell
        /// </summary>
        public static void UnloadAsset<T>(BGFieldUnityAssetArrayA<T> field, BGId entityId, int entityIndex) where T : Object
        {
            var count = GetCount(field, entityId, entityIndex, out var address);
            if (count == 0) return;
            UnloadDelegate.UnloadAll<T>(address, count);
        }

        //how much asset was loaded
        private static int GetCount(BGField field, BGId entityId, int entityIndex, out string address)
        {
            address = null;
            if (entityIndex < 0) return 0;
            address = ((BGAddressablesAssetI)field).GetAddressablesAddress(entityIndex);
            if (string.IsNullOrEmpty(address)) return 0;
            var tuple = Tuple.Create(field.Id, entityId);
            if (!Origin2Count.TryGetValue(tuple, out var count)) return 0;
            if (DebugOn) Debug.Log($"BGAddressablesMonitor debug: Unloading asset for {field.FullName}[{entityId}] {count} times");
            Origin2Count.Remove(tuple);
            return count;
        }

        /// <summary>
        /// interface for delegate class, supporting asset unloading
        /// </summary>
        public interface BGAddressablesMonitorDelegateI
        {
            /// <summary>
            /// unload the asset with provided key
            /// </summary>
            void Unload<T>(string address, int times) where T : Object;
            
            /// <summary>
            /// unload all assets with provided key
            /// </summary>
            void UnloadAll<T>(string address, int times) where T : Object;
        }
    }
}