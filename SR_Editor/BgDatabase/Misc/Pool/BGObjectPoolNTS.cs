/*
<copyright file="BGObjectPoolNTS.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// not thread-safe (NTS) objects pool
    /// works faster than thread-safe pool
    /// </summary>
    public class BGObjectPoolNTS<T> : BGObjectPool
    {
        private readonly Queue<T> _objects;

        //object producer
        private readonly Func<T> _objectGenerator;

        //on object returned to pool
        private readonly Action<T> _dispose;

        public BGObjectPoolNTS(Func<T> objectGenerator, Action<T> dispose = null)
        {
            _objectGenerator = objectGenerator ?? throw new ArgumentNullException(nameof(objectGenerator));
            _objects = new Queue<T>();
            _dispose = dispose;
        }

        /// <summary>
        /// Get pooled object
        /// </summary>
        public T Get() => _objects.Count > 0 ? _objects.Dequeue() : _objectGenerator();

        /// <summary>
        /// Return object to the pool
        /// </summary>
        public void Return(T item)
        {
            _dispose?.Invoke(item);
            _objects.Enqueue(item);
        }

        /// <inheritdoc />
        public override object GetObject() => Get();

        /// <inheritdoc />
        public override void Return(object obj) => Return((T)obj);
    }
}