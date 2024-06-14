using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    public partial class BGAddonPartition
    {
        private static List<OnLoadHandler> loadHandlers;

        // The list of partition listeners implementations
        private static List<OnLoadHandler> LoadHandlers
        {
            get
            {
                if (loadHandlers != null) return loadHandlers;
                loadHandlers = new List<OnLoadHandler>();
                var allImplementations = BGUtil.GetAllImplementations(typeof(OnLoadHandler));
                if (allImplementations != null && allImplementations.Count > 0)
                    foreach (var implementation in allImplementations)
                        loadHandlers.Add((OnLoadHandler)Activator.CreateInstance(implementation));

                return loadHandlers;
            }
        }
        
        //================================================================================================
        //                                              Nested
        //================================================================================================
        /// <summary>
        /// Partitions loading/unloading listeners
        /// </summary>
        public interface OnLoadHandler
        {
            /// <summary>
            /// Called on loading specified partition
            /// </summary>
            void OnLoad(BGEntity partitionEntity);
            /// <summary>
            /// Called on unloading specified partition
            /// </summary>
            void OnUnload(BGEntity partitionEntity);
            /// <summary>
            /// can be used to modify merge settings
            /// </summary>
            void UpdateMergeSettings(BGMergeSettingsEntity settings);
        }
    }
}