/*
<copyright file="BGMTService.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using System.Threading;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// This service provide access to multi-threaded environment
    /// </summary>
    public class BGMTService
    {
        private BGMTRepo repo;
        private readonly UpdateWorker updateWorker;

        private BGMTRepo Repo
        {
            set => Interlocked.Exchange(ref repo, value);
        }

        public BGMTService(bool multithreadedUpdates, BGMTRepo repo)
        {
            this.repo = repo;
            if (!multithreadedUpdates) updateWorker = new UpdateWorker();
        }

        //================================================================================================
        //                                              Read
        //================================================================================================
        /// <summary>
        /// Fast, lightweight readonly transaction.
        /// You can also use Read method to start readonly transaction
        /// You can not change data inside this transaction
        /// </summary>
        public BGMTRepo RepoReadOnly => repo;

        /// <summary>
        /// Fast, lightweight readonly transaction.
        /// You can also use RepoReadOnly field to start readonly transaction
        /// You can not change data inside this transaction
        /// </summary>
        public void Read(Action<BGMTRepo> transaction)
        {
            transaction(RepoReadOnly);
        }

        //================================================================================================
        //                                              Write
        //================================================================================================
        /// <summary>
        /// Heavy write transaction.
        /// Make sure to use it sparingly, otherwise it can create a bottleneck.
        /// You can change data inside this transaction.
        /// Transaction can be executed asynchronously, so don't expect changes to take effect right after the method call.
        /// If you need to execute something after this transaction, use completedCallback Action parameter.
        /// completedCallback will not be executed if exception is thrown.
        /// </summary>
        public void Write(Action<BGMTRepo> transaction, Action completedCallback = null)
        {
            if (updateWorker == null) UpdateTask(transaction, completedCallback, false);
            else updateWorker.Add(() => UpdateTask(transaction, completedCallback, true));
        }

        private void UpdateTask(Action<BGMTRepo> transaction, Action completedCallback, bool async)
        {
            var writableRepo = RepoReadOnly.ToWritableRepo();

            transaction(writableRepo);

            //delete
            writableRepo.ForEachMeta(meta => meta.ApplyDelete());

            Repo = writableRepo.ToReadOnlyRepo();

            if (completedCallback != null)
            {
                if (async) ThreadPool.QueueUserWorkItem(state => completedCallback());
                else completedCallback();
            }
        }

        //================================================================================================
        //                                              Worker
        //================================================================================================
        private class UpdateWorker
        {
            private readonly BlockingQueue<Action> queue = new BlockingQueue<Action>();

            public UpdateWorker()
            {
                var updateThread = new Thread(Run) { IsBackground = true };
                updateThread.Start();
            }

            public void Add(Action action)
            {
                queue.Enqueue(action);
            }

            private void Run()
            {
                while (true)
                {
                    var action = queue.Dequeue();
                    action();
                }
            }
        }

        //================================================================================================
        //                                              Queue
        //================================================================================================
        private class BlockingQueue<T>
        {
            private int count;

            private readonly Queue<T> queue = new Queue<T>();

            public T Dequeue()
            {
                lock (queue)
                {
                    // If we have items remaining in the queue, skip over this. 
                    while (count <= 0)
                        // Release the lock and block on this line until someone
                        // adds something to the queue, resuming once they 
                        // release the lock again.
                        Monitor.Wait(queue);

                    count--;

                    return queue.Dequeue();
                }
            }

            public void Enqueue(T data)
            {
                lock (queue)
                {
                    queue.Enqueue(data);

                    count++;

                    // If the consumer thread is waiting for an item
                    // to be added to the queue, this will move it
                    // to a waiting list, to resume execution
                    // once we release our lock.
                    Monitor.Pulse(queue);
                }
            }
        }
    }
}