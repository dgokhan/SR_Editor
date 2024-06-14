/*
<copyright file="BGInterfaceFinder.cs" company="BansheeGz">
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
    //original from here http://wiki.unity3d.com/index.php/Interface_Finder
    /// <summary>
    /// interface finder
    /// </summary>
    public static partial class BGInterfaceFinder
    {
        //interface=>implementations
        private static readonly Dictionary<Type, HashSet<Type>> Type2ComponentList = new Dictionary<Type, HashSet<Type>>();
        //all mono behaviour script types
        private static readonly List<Type> AllComponents = new List<Type>();
        //interfaces to search for
        private static readonly List<Type> Interfaces = new List<Type>(new[] { typeof(BGAddonSaveLoad.BeforeSaveReciever) });

        private static bool inited;

        //this is super heavy method to be called once 
        private static void Init()
        {
            if (inited) return;
            inited = true;

            AllComponents.Clear();
            AllComponents.AddRange(BGUtil.GetAllSubTypes(typeof(MonoBehaviour)));

            foreach (var @interface in Interfaces) Process(@interface);
        }

        private static void Process(Type @interface)
        {
            var implementingTypes = new HashSet<Type>();

            for (var i = 0; i < AllComponents.Count; i++)
            {
                var component = AllComponents[i];
                if (!@interface.IsAssignableFrom(component)) continue;

                implementingTypes.Add(component);
            }

            if (implementingTypes.Count == 0) return;

            if (implementingTypes.Count > 0) Type2ComponentList.Add(@interface, implementingTypes);
        }

        /// <summary>
        /// Add interface to search for
        /// </summary>
        public static void AddInterface(Type interfaceType)
        {
            if (Interfaces.Contains(interfaceType)) return;
            Interfaces.Add(interfaceType);
            Process(interfaceType);
        }

        /// <summary>
        /// Find all unity objects, implementing interface T 
        /// </summary>
        public static List<T> FindObjects<T>(bool searchForInActive = false) where T : class
        {
            Init();

            var targetType = typeof(T);
            if (!Type2ComponentList.ContainsKey(targetType)) return null;

            var types = Type2ComponentList[targetType];

            if (types.Count == 0) return null;

            var result = new List<T>();
            foreach (var type in types)
            {
                var objects = searchForInActive ? Resources.FindObjectsOfTypeAll(type) : Object.FindObjectsOfType(type);

                if (objects == null || objects.Length == 0) continue;

                for (var i = 0; i < objects.Length; i++)
                {
                    var targetObj = objects[i] as T;
                    if (targetObj == null) continue;
                    result.Add(targetObj);
                }
            }

            return result;
        }
    }
}