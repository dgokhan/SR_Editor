/*
<copyright file="BGMainThreadRunner.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// This component is used to make sure database accessed from main thread and run tasks on main thread
    /// </summary>
    public class BGMainThreadRunner : MonoBehaviour
    {
        
        //=============================== Static
        private static BGMainThreadRunner instance;
        private static volatile bool hasJobs;
        private static List<Action> jobs = new List<Action>(4);
        private static List<Action> jobsToRun = new List<Action>(4);

        /// <summary>
        /// Static singleton object
        /// </summary>
        public static BGMainThreadRunner Instance => instance;

        /// <summary>
        /// Adds the task to be executed on main thread
        /// </summary>
        public static void RunOnMainThread(Action action)
        {
            lock (jobs)
            {
                jobs.Add(action);
                hasJobs = true;
            }
        }

        /// <summary>
        /// Ensures the current thread is the main thread, otherwise throws an exception
        /// </summary>
        public static void EnsureMainThread(string error)
        {
            if (instance == null || instance.mainThread == null) return;
            if (instance.mainThread != Thread.CurrentThread) throw new BGException(error);
        }

        // [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            if (instance != null) return;
            instance = new GameObject("BGMainThreadRunner") { hideFlags = HideFlags.HideInInspector | HideFlags.HideInHierarchy }.AddComponent<BGMainThreadRunner>();
            DontDestroyOnLoad(instance.gameObject);
        }

        //=============================== Fields/Properties
        private Thread mainThread;
        public Thread MainThread => mainThread;

        //=============================== Unity Callbacks
        private void Start()
        {
            //stores the reference to the main thread
            mainThread = Thread.CurrentThread;
        }

        //using update method to run submitted tasks on the main thread
        private void Update()
        {
            if (!hasJobs) return;

            lock (jobs)
            {
                (jobsToRun, jobs) = (jobs, jobsToRun);
                hasJobs = false;
            }

            foreach (var action in jobsToRun)
                try
                {
                    action();
                }
                catch (Exception e)
                {
#if UNITY_EDITOR
                    //we can not let exception to propagate
                    Debug.LogException(e);
#endif
                }

            jobsToRun.Clear();
        }
    }
}