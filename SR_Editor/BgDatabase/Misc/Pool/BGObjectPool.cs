/*
<copyright file="BGObjectPool.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Concurrent;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// thread-safe objects pool
    /// https://docs.microsoft.com/en-us/dotnet/standard/collections/thread-safe/how-to-create-an-object-pool
    /// </summary>
    public abstract class BGObjectPool
    {
        /// <summary>
        /// Get pooled object
        /// </summary>
        public abstract object GetObject();
        /// <summary>
        /// Return an object to the pool
        /// </summary>
        public abstract void Return(object obj);
    }

    /// <summary>
    /// generic thread-safe objects pool (T is pooled object type)
    /// </summary>
    public class BGObjectPool<T> : BGObjectPool
    {
        //concurrent container
        private readonly ConcurrentBag<T> _objects;
        //object producer
        private readonly Func<T> _objectGenerator;
        //on object returned to pool
        private readonly Action<T> _dispose;

        public BGObjectPool(Func<T> objectGenerator, Action<T> dispose = null)
        {
            _objectGenerator = objectGenerator ?? throw new ArgumentNullException(nameof(objectGenerator));
            _objects = new ConcurrentBag<T>();
            _dispose = dispose;
        }

        /// <summary>
        /// Get pooled object
        /// </summary>
        public T Get() => _objects.TryTake(out T item) ? item : _objectGenerator();

        /// <summary>
        /// Return object to the pool
        /// </summary>
        public void Return(T item)
        {
            _dispose?.Invoke(item);
            _objects.Add(item);
        }

        /// <inheritdoc />
        public override object GetObject() => Get();

        /// <inheritdoc />
        public override void Return(object obj) => Return((T)obj);
    }
}