/*
<copyright file="BGMultiThreadedLoader.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Multi-threaded loader
    /// </summary>
    public class BGMultiThreadedLoader
    {
        private readonly ActionsListRunner[] loaders;
        private readonly ActionsListRunner mainThreadLoader;
        private readonly List<Exception> errorsList = new List<Exception>();

        private int currentLoader;

        public BGMultiThreadedLoader()
        {
            mainThreadLoader = new ActionsListRunner();
            var loadersCount = Mathf.Clamp(Environment.ProcessorCount, 1, 16);
            loaders = new ActionsListRunner[loadersCount];
            for (var i = 0; i < loadersCount; i++) loaders[i] = new ActionsListRunner();
        }

        /// <summary>
        /// add exception
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddException(Exception e) => errorsList.Add(e);

        /// <summary>
        /// print added exception
        /// </summary>
        public void PrintExceptions()
        {
            if (errorsList.Count == 0) return;
        }

        /// <summary>
        /// add a task
        /// </summary>
        public void AddAction(Action action, bool runOnMainThread)
        {
            if (runOnMainThread) mainThreadLoader.AddAction(action);
            else
            {
                var threadLoader = loaders[currentLoader];
                threadLoader.AddAction(action);
                currentLoader++;
                if (currentLoader == loaders.Length) currentLoader = 0;
            }
        }

        /// <summary>
        /// Load
        /// </summary>
        public void Load()
        {
            var threads = new Thread[loaders.Length];
            for (var i = 0; i < threads.Length; i++)
            {
                var runner = loaders[i];
                if (runner == null || !runner.HasActions) continue;

                var thread = new Thread(runner.Go);
                thread.Start();
                threads[i] = thread;
            }

            mainThreadLoader.Go();

            for (var i = 0; i < threads.Length; i++)
            {
                var thread = threads[i];
                thread?.Join();
            }

            PrintExceptions();
        }

        //runner for actions list
        private class ActionsListRunner
        {
            private readonly List<Action> actions = new List<Action>();

            public bool HasActions => actions.Count > 0;

            public void AddAction(Action action) => actions.Add(action);

            public void Go()
            {
                for (var i = 0; i < actions.Count; i++) actions[i]();
            }
        }
    }
}