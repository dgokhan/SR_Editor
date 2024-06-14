using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// struct with shared object for using() operator 
    /// </summary>
    public class BGSharedObjectValue<T> : IDisposable
    {
        private readonly BGObjectPool<T> pool;
        public readonly T Value;

        public BGSharedObjectValue(BGObjectPool<T> pool)
        {
            this.pool = pool;
            Value = pool.Get();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            pool.Return(Value);
        }
    }
}