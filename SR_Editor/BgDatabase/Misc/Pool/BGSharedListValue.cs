using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// struct with shared list for using() operator
    /// </summary>
    public class BGSharedListValue<T> : IDisposable
    {
        private readonly BGListPoolDefault<T> pool;
        public readonly List<T> Value;

        public BGSharedListValue(BGListPoolDefault<T> pool)
        {
            this.pool = pool;
            Value = pool.Get();
            Value.Clear();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Value.Clear();
            pool.Return(Value);
        }
    }

}